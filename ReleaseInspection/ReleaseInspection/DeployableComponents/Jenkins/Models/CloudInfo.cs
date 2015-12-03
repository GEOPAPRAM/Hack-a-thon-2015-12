using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class CloudInfo
    {
        private readonly List<CookbookInfo> _cookbooks;

        public CloudInfo(string cloudName)
        {
            CloudName = cloudName;
            _cookbooks = new List<CookbookInfo>();
        }

        public string CloudName { get; private set; }

        public void AddCookbookInfo(CookbookInfo cookbookInfo)
        {
            _cookbooks.Add(cookbookInfo);
        }

        public ReadOnlyCollection<CookbookInfo> Cookbooks
        {
            get { return _cookbooks.AsReadOnly(); }
        }
    }
}
