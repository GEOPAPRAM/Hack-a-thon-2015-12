using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation
{
    class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "NewVoiceMedia.Tools.ReleaseInspection";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "System.Linq";
            yield return "System.Collections.Generic";
            yield return "System.Collections";
            yield return "NewVoiceMedia.Tools.ReleaseInspection";
            yield return "NewVoiceMedia.Tools.ReleaseInspection.Model";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
