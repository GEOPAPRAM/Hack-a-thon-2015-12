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
                var storyId = StoryIdExtractor.ExtractPivotalId(sourceChange.Comments);
                
                if (storyId != String.Empty)
                {
                    StoreStoryWork(changesByStory, storyId, sourceChange);
                }
                else
                {
                    var jiraStoryId = StoryIdExtractor.ExtractJiraId(sourceChange.Comments);
                    if (jiraStoryId != String.Empty)
                    {
                        StoreStoryWork(changesByStory, jiraStoryId, sourceChange);
                    }
                    else
                    {
                        StoreUntrackedChanges(untrackedWork, sourceChange);
                    }
                }
            }

            Parallel.ForEach(changesByStory.Values.ToList(), GetStoryDetails);

            var stories = ExtractFeaturesFromChanges(changesByStory);
            var chores = ExtractChoresFromChanges(changesByStory);
            
            var acceptedStories = FilterAcceptedWork(stories);
            var unfinishedStories = FilterWorkInProgress(stories);

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

        private static StoryWorkList FilterWorkInProgress(IEnumerable<StoryWork> work)
        {
            return work.Where(item => item.Story.Status.Name != "accepted" && item.Story.Status.Name.ToLower() != "done")
                .OrderBy(item => item.Story.StoryType.Id)
                .ToStoryWorkList();
        }

        private static StoryWorkList FilterAcceptedWork(IEnumerable<StoryWork> work)
        {
            return work.Where(item => item.Story.Status.Name == "accepted" || item.Story.Status.Name.ToLower() == "done")
                .OrderBy(item => item.Story.StoryType.Id)
                .ToStoryWorkList();
        }

        private static List<StoryWork> ExtractFeaturesFromChanges(Dictionary<string, StoryWork> changesById)
        {
            return changesById.Select(item => item.Value)
                .Where(item => item.Story.StoryType.Name != "chore" && item.Story.StoryType.Name != "task")
                .ToList();
        }

        private static ChoresModel ExtractChoresFromChanges(Dictionary<string, StoryWork> changesById)
        {
            var chores = changesById.Select(item => item.Value).Where(item => item.Story.StoryType.Name == "chore" || item.Story.StoryType.Name == "task").ToList();
            var acceptedChores = FilterAcceptedWork(chores);
            var unfinishedChores = FilterWorkInProgress(chores);

            return new ChoresModel
            {
                AcceptedChores = acceptedChores,
                UnfinishedChores = unfinishedChores
            };
        }

        private void StoreStoryWork(IDictionary<string, StoryWork> changesByStory, string storyId, SourceChange sourceChange)
        {
            if (!changesByStory.ContainsKey(storyId))
            {
                changesByStory[storyId] = _storyWorkFactory.Create(new Story { Id = storyId }, _deployableComponent.ProductAreaPathMap);
            }

            changesByStory[storyId].MergeChanges(sourceChange);
        }

        private void StoreUntrackedChanges(UntrackedStoryWork untrackedWork, SourceChange sourceChange)
        {
            untrackedWork.MergeChanges(sourceChange);
        }

        private void GetStoryDetails(StoryWork storyWork)
        {
            var story = new Story
                {
                    Id = storyWork.Story.Id,
                    Status = new IssueStatus { Name = PivotalFailureErrorMessage },
                    Title = PivotalFailureErrorMessage,
                    StoryType = new IssueType { Id = (int) StoryType.PivotalError, Name = StoryType.PivotalError.ToString() }
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