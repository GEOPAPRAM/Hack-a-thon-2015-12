using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins
{
    public interface IJenkinsClient
    {
        CloudInfo GetCloudInformation(string cloudName, string cookbookName);
    }
}