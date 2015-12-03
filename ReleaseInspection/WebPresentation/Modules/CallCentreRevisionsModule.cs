using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class CallCentreRevisionsModule : NancyModule
    {
        public CallCentreRevisionsModule(IReleaseService releaseService)
            : base("/revisions/" + CallCentre.Identifier)
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