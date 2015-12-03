using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class ReleaseService : IReleaseService
    {
        public const string PivotalFailureErrorMessage = "Call to Pivotal Failed!!";

        private readonly IDeployableComponent _deployableComponent;
        private readonly IStoryRepositoryClient _storyRepositoryClient;

        private readonly IStoryWorkFactory _storyWorkFactory;

        public ReleaseService(IDeployableComponent deployableComponent,
                              IStoryWorkFactory storyWorkFactory,
                              IStoryRepositoryClient storyRepositoryClient)
        {
            _deployableComponent = deployableComponent;
            _storyWorkFactory = storyWorkFactory;
            _storyRepositoryClient = storyRepositoryClient;
        }

        public ReleaseModel PopulateContentsForRevisions(long fromRevision, long toRevision)
        {
            var changesByStory = new Dictionary<string, StoryWork>();
            var changes = _deployableComponent.GetChanges(fromRevision, toRevision);
            var untrackedWork = _storyWorkFactory.Create(_deployableComponent.ProductAreaPathMap);

            foreach (var sourceChange in changes)
            {
                var storyId = sourceChange.Comments.TryExtractStoryId();

                if (storyId != String.Empty)
                {
                    var storyWork = changesByStory.ContainsKey(storyId)
                                    ? changesByStory[storyId]
                                    : changesByStory[storyId] = _storyWorkFactory.Create(new Story { Id = storyId }, _deployableComponent.ProductAreaPathMap);

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

            return new ReleaseModel
            {
                PreviousVersion = fromRevision.ToString(CultureInfo.InvariantCulture),
                Version = toRevision.ToString(CultureInfo.InvariantCulture),
                AcceptedWork = acceptedStories,
                UnfinishedWork = unfinishedStories,
                UntrackedWork = untrackedWork,
                Chores = chores,
            };
        }

        public ReleaseModel PopulateContents(string fromVersion, string toVersion)
        {
            var fromRevision = _deployableComponent.GetRevisionForVersion(fromVersion);
            var toRevision = _deployableComponent.GetRevisionForVersion(toVersion);
            var releaseModel = PopulateContentsForRevisions(fromRevision, toRevision);

            releaseModel.PreviousVersion = fromVersion;
            releaseModel.Version = toVersion;

            return releaseModel;
        }

        public ReleaseModel PopulateContentsForCurrentLive(string toVersion)
        {
            var liveVersion = _deployableComponent.GetLiveVersionInformation();
            var model = PopulateContents(liveVersion.ToString(), toVersion);
            model.ComparingToLive = true;
            return model;
        }

        private void GetStoryDetails(StoryWork storyWork)
        {
            var story = new Story
                {
                    Id = storyWork.Story.Id,
                    Status = new IssueStatus { Name = PivotalFailureErrorMessage },
                    Title = PivotalFailureErrorMessage,
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