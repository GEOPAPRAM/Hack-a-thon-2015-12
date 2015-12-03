using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class IvrRevisionsModule : NancyModule
    {
        public IvrRevisionsModule(IReleaseService releaseService)
            : base("/revisions/" + IVR.Identifier)
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