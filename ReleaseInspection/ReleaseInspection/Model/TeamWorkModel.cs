namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class TeamWorkModel
    {
        public string Name { get; set; }
        public StoryWorkList AcceptedWork { get; set; }
        public StoryWorkList UnfinishedWork { get; set; }
    }
}