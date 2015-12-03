using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class IvrReleaseCandidateModule : NancyModule
    {
        public IvrReleaseCandidateModule(IReleaseService releaseService)
            : base(IVR.Identifier)
        {
            Get["/{version}"] = parameters =>
            {
                var model = releaseService.PopulateContentsForCurrentLive(parameters.version);

                return View[model];
            };
        }
    }
}