using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface IReleaseModel
    {
        string Version { get; }
        string PreviousVersion { get; }
        bool ComparingToLive { get; set; }
        StoryWorkList AcceptedWork { get; set; }
        StoryWorkList UnfinishedWork { get; set; }
        UntrackedStoryWork UntrackedWork { get; }
        ChoresModel Chores { get; set; }
    }
}