using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface ICookbookService
    {
        CloudInfo PopulateCloudInfo(string cloudName, string cookbookName);
    }
}