using System.Linq;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
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

        public Revision GetAppCookbookRevision()
        {
            if (Cookbook == null || Cookbook.AppCookbookName.Length==0 || Cookbook.Changes.Revisions.Count == 0) return null;

            return Cookbook.Changes.Revisions.Where(x => x.Module.Contains(Cookbook.AppCookbookName))
                                   .Select(x=>x)
                                   .FirstOrDefault();
        }
    }
}
