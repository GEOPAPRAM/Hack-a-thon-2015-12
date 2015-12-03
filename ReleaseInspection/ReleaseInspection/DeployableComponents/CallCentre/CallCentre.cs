using System.Collections.Generic;
using System.Configuration;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre
{
    public class CallCentre : DeployableComponentBase
    {
        private readonly string _svnRootUrl;
        public const string Identifier = "call-centre";

        public CallCentre(ISourceControl sourceControl, IVersionRetrieverService versionRetrieverService)
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
                    string.Format("{0}/nvm/components/applications/call_centre/trunk/", _svnRootUrl),
                    string.Format("{0}/nvm/components/applications/common/trunk/", _svnRootUrl),
                    string.Format("{0}/nvm/components/installers/call_centre/trunk/", _svnRootUrl),
                    string.Format("{0}/nvm/components/installers/database/trunk/", _svnRootUrl)
                };
            }
        }

        public override IProductPathAreaMap ProductAreaPathMap
        {
            get { return new CallCentrePathsByArea(); }
        }
    }
}