using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins
{
    public interface IJenkinsClient
    {
        CloudInfo GetCloudInformation(string cloudName, string cookbookName);
        DeploymentInfo GetDeploymentInfoForClouds(IList<string> clouds, IEnumerable<string> cookbooks, string identifier);
    }
}