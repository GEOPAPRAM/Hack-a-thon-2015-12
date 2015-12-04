using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class WfmRevisionsModule : NancyModule
    {
        private readonly IJenkinsClient _jenkinsClient;

        public WfmRevisionsModule(IReleaseService releaseService, IJenkinsClient jenkinsClient)
            : base("/revisions/" + WFM.Identifier)
        {
            _jenkinsClient = jenkinsClient;
            Get["/"] = parameters =>
            {
                var startRevision = Request.Query["start"];
                var endRevision = Request.Query["end"];

                var model = releaseService.PopulateContentsForRevisions(startRevision, endRevision);
                if (model != null)
                {
                    model.DeploymentInfo = _jenkinsClient.GetDeploymentInfoForClouds(new[] {"Cloud 4", "Cloud 11", "Cloud 12", "Cloud 17"}, new[] {"PerformWebServer", "PerformWorkerServer"}, WFM.Identifier);
                }


                return View["Release", model];
            };
        }
    }
}