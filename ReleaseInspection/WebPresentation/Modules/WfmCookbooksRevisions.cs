using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class WfmCookbooksRevisions : NancyModule
    {
        public WfmCookbooksRevisions(ICookbookService cookbookService) : base("/cookbooks/"+ WFM.Identifier)
        {
            Get["/"] = parameters =>
            {
                var cloud = Request.Query["cloud"];
                var cookbook = Request.Query["cookbook"]; 

                var model = cookbookService.PopulateCloudInfo(cloud, cookbook);

                return View["Cookbooks", model];
            };
        }
    }
}