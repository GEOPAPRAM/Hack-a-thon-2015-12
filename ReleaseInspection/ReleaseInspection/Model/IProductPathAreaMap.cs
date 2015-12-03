using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface IProductPathAreaMap
    {
        IDictionary<string, string[]> PathsByArea { get; }
        IEnumerable<string> IgnoredPaths { get; }
    }
}