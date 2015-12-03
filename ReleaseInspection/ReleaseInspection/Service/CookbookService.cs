using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using CloudInfo = NewVoiceMedia.Tools.ReleaseInspection.Model.CloudInfo;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class CookbookService : ICookbookService
    {
        private readonly IDeployableComponent _deployableComponent;

        public CookbookService(IDeployableComponent deployableComponent)
        {
            _deployableComponent = deployableComponent;
        }

        public CloudInfo PopulateCloudInfo(string cloudName, string cookbookName)
        {

            //return GetCloudInformation(cloudName, cookbookName);
            return new CloudInfo("","");
        }

        

        
    }
}
