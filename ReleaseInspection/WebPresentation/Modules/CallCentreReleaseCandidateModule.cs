using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class CallCentreReleaseCandidateModule : NancyModule
    {
        public CallCentreReleaseCandidateModule(IReleaseService releaseService)
            : base(CallCentre.Identifier)
        {
            Get["/{version}"] = parameters =>
            {
                var model = releaseService.PopulateContentsForCurrentLive(parameters.version);

                return View[model];
            };
        }
    }
}