using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection
{
    public static class StoryWorkExtensions
    {
        public static StoryWorkList ToStoryWorkList(this IEnumerable<StoryWork> storyWorkItems)
        {
            return new StoryWorkList(storyWorkItems);
        }
    }
}
