using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class WfmeReleaseCandidateModule : NancyModule
    {
        public WfmeReleaseCandidateModule(IReleaseService releaseService)
            : base(WFM.Identifier)
        {
            Get["/{version}"] = parameters =>
            {
                var model = releaseService.PopulateContentsForCurrentLive(parameters.version);

                return View[model];
            };
        }
    }
}