using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface ISolution
    {
        string Name { get; }
        void LoadFromSourceControl();
        IEnumerable<Project> Projects { get; }
    }
}
