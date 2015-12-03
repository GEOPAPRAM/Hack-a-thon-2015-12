using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class WfmRevisionsModule : NancyModule
    {
        public WfmRevisionsModule(IReleaseService releaseService)
            : base("/revisions/" + WFM.Identifier)
        {
            Get["/"] = parameters =>
            {
                var startRevision = Request.Query["start"];
                var endRevision = Request.Query["end"];

                var model = releaseService.PopulateContentsForRevisions(startRevision, endRevision);

                return View["Release", model];
            };
        }
    }
}