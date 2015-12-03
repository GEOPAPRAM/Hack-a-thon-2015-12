using System.Collections.Generic;
using System.Configuration;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR
{
    public class IVR : DeployableComponentBase
    {
        private readonly string _svnRootUrl;
        public const string Identifier = "ivr";

        public IVR(ISourceControl sourceControl, IVersionRetrieverService versionRetrieverService)
            : base(Identifier, sourceControl, versionRetrieverService)
        {
            _svnRootUrl = ConfigurationManager.AppSettings["SvnRootUrl"];
        }

        public override IEnumerable<string> ContributingPaths
        {
            get
            {
                return new[]
                {
                    string.Format("{0}/nvm/components/applications/NewVoiceMedia.IVR/trunk", _svnRootUrl)
                };
            }
        }

        public override IProductPathAreaMap ProductAreaPathMap
        {
            get { return new IvrPathsByArea(); }
        }
    }
}
