using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class UntrackedStoryWork
    {
        public IPathsAndAreas PathsAndAreas { get; private set; }
        public List<UntrackedStoryWorkItem> Items = new List<UntrackedStoryWorkItem>();

        public UntrackedStoryWork(IPathsAndAreas pathsAndAreas)
        {
            PathsAndAreas = pathsAndAreas;
        }

        public void MergeChanges(SourceChange sourceChange)
        {
            IPathsAndAreas pathsAndAreas = PathsAndAreas.GetCleanCopy();
            pathsAndAreas.ExtractPathsAndAreas(sourceChange);
            var item = new UntrackedStoryWorkItem(sourceChange.Revision, sourceChange.Author, sourceChange.Comments, pathsAndAreas);
            Items.Add(item);
        }

        public class UntrackedStoryWorkItem
        {
            public UntrackedStoryWorkItem(long revision, string author, string comment, IPathsAndAreas pathsAndAreas)
            {
                Revision = revision;
                Author = author;
                Comment = comment;
                PathsAndAreas = pathsAndAreas;
            }

            public long Revision { get; private set; }
            public string Author { get; private set; }
            public string Comment { get; private set; }
            public IPathsAndAreas PathsAndAreas { get; private set; }
        }
    }
}
