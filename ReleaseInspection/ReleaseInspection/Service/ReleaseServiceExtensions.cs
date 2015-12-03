using System.Collections.Generic;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public static class ReleaseServiceExtensions
    {
        public static List<StoryWork> ExtractFeaturesFromChanges(this Dictionary<string, StoryWork> changesById)
        {
            return changesById.Select(item => item.Value)
                .Where(item => item.Story.StoryType.Name != "chore" && item.Story.StoryType.Name != "task")
                .ToList();
        }

        public static ChoresModel ExtractChoresFromChanges(this Dictionary<string, StoryWork> changesById)
        {
            var chores = changesById.Select(item => item.Value).Where(item => item.Story.StoryType.Name == "chore" || item.Story.StoryType.Name == "task").ToList();
            var acceptedChores = chores.FilterAcceptedWork();
            var unfinishedChores = chores.FilterWorkInProgress();

            return new ChoresModel
            {
                AcceptedChores = acceptedChores,
                UnfinishedChores = unfinishedChores
            };
        }

        public static StoryWorkList FilterAcceptedWork(this IEnumerable<StoryWork> work)
        {
            return work.Where(item => item.Story.Status.Name == "accepted" || item.Story.Status.Name.ToLower() == "done")
                .OrderBy(item => item.Story.StoryType.Id)
                .ToStoryWorkList();
        }

        public static StoryWorkList FilterWorkInProgress(this IEnumerable<StoryWork> work)
        {
            return work.Where(item => item.Story.Status.Name != "accepted" && item.Story.Status.Name.ToLower() != "done")
                .OrderBy(item => item.Story.StoryType.Id)
                .ToStoryWorkList();
        }
        public static void StoreUntrackedChanges(this SourceChange sourceChange,UntrackedStoryWork untrackedWork)
        {
            untrackedWork.MergeChanges(sourceChange);
        }

        public static void StoreStoryWork(this StoryWork storyWork, string storyId, SourceChange sourceChange)
        {
            storyWork.MergeChanges(sourceChange);
        }
    }
}