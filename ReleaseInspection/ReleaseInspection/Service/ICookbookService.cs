using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface ICookbookService
    {
        CookbookChangesModel PopulateCloudInfo(string cloudName, string cookbookName);
    }
}