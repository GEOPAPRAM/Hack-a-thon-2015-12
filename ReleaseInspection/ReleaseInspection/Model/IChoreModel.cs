using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    interface IChoreModel
    {
        StoryWorkList AcceptedChores { get; set; }
        StoryWorkList UnfinishedChores { get; set; }
    }
}
