using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface IPathsAndAreas
    {
        IDictionary<string, IList<string>> Areas { get; }
        IDictionary<string, IList<string>> TestAreas { get; }
        IList<string> Paths { get; }
        IList<string> UnmappedPaths { get; }

        void ExtractPathsAndAreas(SourceChange sourceChange);
        IPathsAndAreas GetCleanCopy();
    }
}