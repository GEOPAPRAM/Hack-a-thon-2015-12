using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class CloudInfo
    {
        public string CloudName { get; set; }
        public CookbookInfo Cookbook { get; set; }


        public CloudInfo(string cloudName, string cookbookName)
        {
            CloudName = cloudName;
            Cookbook = new CookbookInfo(cookbookName);
        }
    }
}
