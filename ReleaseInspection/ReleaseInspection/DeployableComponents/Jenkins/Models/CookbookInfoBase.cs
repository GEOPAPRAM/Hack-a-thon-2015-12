namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class CookbookInfoBase
    {
        public CookbookInfoBase(string cookbookName)
        {
            CookbookName = cookbookName;
        }

        public CookbookInfoBase(){}

        public string AppCookbookName { get; set; }
        public string CookbookName { get; private set; }
        public string Description { get; set; }
        public string EnvBuildVersion { get; set; }
        public string AppBuildVersion { get; set; }
    }
}