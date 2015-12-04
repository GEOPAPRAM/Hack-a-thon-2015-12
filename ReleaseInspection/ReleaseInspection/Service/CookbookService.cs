using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class CookbookService : ICookbookService
    {
        private readonly IJenkinsClient _jenkinsClient;
        private readonly ISourceControl _sourceControl;
        private readonly IStoryWorkFactory _storyWorkFactory;
        private readonly IStoryRepositoryClient _storyRepositoryClient;
        public IProductPathAreaMap ProductPathAreaMap;

        public CookbookService(IJenkinsClient jenkinsClient, ISourceControl sourceControl, IStoryWorkFactory storyWorkFactory, IStoryRepositoryClient storyRepositoryClient)
        {
            _jenkinsClient = jenkinsClient;
            _sourceControl = sourceControl;
            _storyWorkFactory = storyWorkFactory;
            _storyRepositoryClient = storyRepositoryClient;
            ProductPathAreaMap = new WFMCookbookPathsByArea();
        }

        public CookbookChangesModel PopulateCloudInfo(string cloudName, string cookbookName)
        {
            var model = new CookbookChangesModel(cloudName, cookbookName);
            var changesByStory = new Dictionary<string, StoryWork>();
            var cloudInfo = _jenkinsClient.GetCloudInformation(cloudName, cookbookName);
            var untrackedWork = _storyWorkFactory.Create(ProductPathAreaMap);

            var revision = cloudInfo.GetAppCookbookRevision();

            model.Description = cloudInfo.Cookbook.Description;

            var changes = GetChanges(new[] { revision.Module }, revision.Number);

            foreach (var sourceChange in changes)
            {
                var storyId = sourceChange.Comments.TryExtractStoryId();

                if (storyId != String.Empty)
                {
                    var storyWork = changesByStory.ContainsKey(storyId) 
                                    ? changesByStory[storyId] 
                                    : changesByStory[storyId] = _storyWorkFactory.Create(new Story {Id = storyId}, ProductPathAreaMap);
                    
                    storyWork.StoreStoryWork(storyId, sourceChange);
                }
                else
                {
                    sourceChange.StoreUntrackedChanges(untrackedWork);
                }
            }

            Parallel.ForEach(changesByStory.Values.ToList(), GetStoryDetails);

            var stories = changesByStory.ExtractFeaturesFromChanges();
            var chores = changesByStory.ExtractChoresFromChanges();

            var acceptedStories = stories.FilterAcceptedWork();
            var unfinishedStories = stories.FilterWorkInProgress();

            model.Changes = new ReleaseModel
            {
                PreviousVersion = revision.Number.ToString(CultureInfo.InvariantCulture),
                Version = "HEAD",
                AcceptedWork = acceptedStories,
                UnfinishedWork = unfinishedStories,
                UntrackedWork = untrackedWork,
                Chores = chores,
            };

            return model;
        }

        private IEnumerable<SourceChange> GetChanges(IEnumerable<string> contributingPaths, long fromRevision)
        {
            var combinedChanges = new ConcurrentBag<SourceChange>();

            Task.WaitAll(contributingPaths.Select(path => Task.Factory.StartNew(() => GetChanges(path, fromRevision, combinedChanges))).ToArray());

            return combinedChanges.ToArray();
        }

        private void GetChanges(string path, long fromRevision, ConcurrentBag<SourceChange> combinedChanges)
        {
            var localChanges = _sourceControl.GetChangesSince(path, fromRevision);

            foreach (var sourceChange in localChanges.Where(c => c.Author != "jenkins.ci"))
            {
                combinedChanges.Add(sourceChange);
            }
        }

        private void GetStoryDetails(StoryWork storyWork)
        {
            var story = new Story
            {
                Id = storyWork.Story.Id,
                Status = new IssueStatus { Name = ReleaseService.PivotalFailureErrorMessage },
                Title = ReleaseService.PivotalFailureErrorMessage,
                StoryType = new IssueType { Id = (int)StoryType.PivotalError, Name = StoryType.PivotalError.ToString() }
            };

            try
            {
                story = _storyRepositoryClient.GetStory(storyWork.Story.Id);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
                storyWork.Story = story;
            }
        }
    }
}
