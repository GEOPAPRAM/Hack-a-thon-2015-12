using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class CookbookService : ICookbookService
    {
        public CloudInfoModel PopulateCloudInfo(string cloudName, string cookbookInfo)
        {
            return new CloudInfoModel(cloudName, cookbookInfo);
        }
    }
}
