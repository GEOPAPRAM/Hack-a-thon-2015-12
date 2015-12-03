using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class StoryWork
    {
        public Story Story { get; set; }
        public IList<string> Authors { get; private set; }
        public IPathsAndAreas PathsAndAreas { get; private set; }
        public IList<string> Comments { get; private set; }

        private StoryWork(IPathsAndAreas pathsAndAreas)
        {
            PathsAndAreas = pathsAndAreas;
            Authors = new List<string>();
            Comments = new List<string>();
        }

        public StoryWork(Story story, IPathsAndAreas pathsAndAreas)
            : this(pathsAndAreas)
        {
            Story = story;
        }

        public void MergeChanges(SourceChange sourceChange)
        {
            if (!Authors.Contains(sourceChange.Author))
            {
                Authors.Add(sourceChange.Author);
            }

            PathsAndAreas.ExtractPathsAndAreas(sourceChange);

            Comments.Add(sourceChange.Comments);
        }
    }
}