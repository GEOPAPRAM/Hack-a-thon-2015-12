using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class DeploymentInfo
    {
        public string CookbookDetailsBaseUrl { get; set; }
        private readonly Dictionary<string, IDictionary<string, CookbookInfoBase>> _cloudCookbooksInfo;

        public DeploymentInfo(string identifier)
        {
            _cloudCookbooksInfo = new Dictionary<string, IDictionary<string, CookbookInfoBase>>();
            CookbookDetailsBaseUrl = string.Format("cookbooks/{0}", identifier);
        }

        public void AddCloudCookbookInfo(string cloudName, CookbookInfoBase cookbookInfo)
        {
            if (!_cloudCookbooksInfo.ContainsKey(cloudName))
            {
                _cloudCookbooksInfo[cloudName] = new Dictionary<string, CookbookInfoBase> { { cookbookInfo.CookbookName, cookbookInfo } };
            }
            else
            {
                _cloudCookbooksInfo[cloudName].Add(cookbookInfo.CookbookName, cookbookInfo);
            }
        }

        public IReadOnlyDictionary<string, IDictionary<string,CookbookInfoBase>> CloudCookbooksInfo
        {
            get { return _cloudCookbooksInfo; }
        }
    }
}