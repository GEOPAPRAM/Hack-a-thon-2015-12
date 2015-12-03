using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface IStoryRepositoryClient
    {
        Story GetStory(string storyId);
    }
}
