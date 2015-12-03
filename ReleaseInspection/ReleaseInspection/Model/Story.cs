namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class Story
    {
        public Story()
        {
        }

        public Story(string storyId)
        {
            Id = storyId;
            StoryType = new IssueType();
            Status = new IssueStatus();
        }

        public Story(string storyId, string storyTitle, string status, StoryType? typeEnum)
        {
            Id = storyId;
            Title = storyTitle;
            Status = new IssueStatus
            {
                Name = status
            };

            if (typeEnum.HasValue)
            {
                StoryType = new IssueType
                {
                    Id = (int) typeEnum,
                    Name = typeEnum.ToString().ToLower()
                };
            }
            else
            {
                StoryType = new IssueType();
            }
            
            Team = new Team();

        }

        public string Id { get; set; }
        public string Title { get; set; }
        public IssueType StoryType { get; set; }
        public Team Team { get; set; }
        public IssueStatus Status { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
    }

    public enum StoryType
    {
        Unknown,
        Feature,
        Bug,
        Chore,
        Release,
        PivotalError
    }
}