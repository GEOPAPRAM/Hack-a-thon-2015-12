using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class WafReleaseCandidateModule : NancyModule
    {
        public WafReleaseCandidateModule(IReleaseService releaseService)
            : base(WAF.Identifier)
        {
            Get["/{version}"] = parameters =>
            {
                var model = releaseService.PopulateContentsForCurrentLive(parameters.version);

                return View[model];
            };
        }


    }
}