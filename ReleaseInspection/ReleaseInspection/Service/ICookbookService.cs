using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface ICookbookService
    {
        CloudInfoModel PopulateCloudInfo(string cloudName, string cookbookName);
    }
}