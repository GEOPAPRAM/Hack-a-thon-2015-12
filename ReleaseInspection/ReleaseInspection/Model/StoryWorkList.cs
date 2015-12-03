using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class StoryWorkList : List<StoryWork>
    {
        public StoryWorkList()
        {
            
        }

        public StoryWorkList(IEnumerable<StoryWork> storyWorkItems) : base(storyWorkItems)
        {
            
        }
    }
}
