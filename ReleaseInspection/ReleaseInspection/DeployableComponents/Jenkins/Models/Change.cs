using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class Change
    {
        [JsonProperty("changeSet")]
        public ChangeSet ChangeSet { get; set; }
    }
}
