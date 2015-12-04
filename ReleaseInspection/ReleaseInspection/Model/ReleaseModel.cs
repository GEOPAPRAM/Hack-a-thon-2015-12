namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class ReleaseModel : IReleaseModel
    {
        public bool ComparingToLive { get; set; }
        public string Version { get; set; }
        public string PreviousVersion { get; set; }
        public StoryWorkList AcceptedWork { get; set; }
        public StoryWorkList UnfinishedWork { get; set; }
        public UntrackedStoryWork UntrackedWork { get; set; }
        public ChoresModel Chores { get; set; }
        public DeploymentInfo DeploymentInfo { get; set; }
    }
}