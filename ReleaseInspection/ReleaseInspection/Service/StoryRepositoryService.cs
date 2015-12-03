using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class StoryRepositoryService : IStoryRepositoryClient
    {
        private readonly JiraClient _jiraClient = new JiraClient();
        private readonly PivotalTrackerClient _pivotalTracker = new PivotalTrackerClient();

        public Story GetStory(string storyTag)
        {
            int id;
            var story = int.TryParse(storyTag, out id) ? _pivotalTracker.GetStory(storyTag) : _jiraClient.GetStory(storyTag);
            return story;
        }
    }
}
