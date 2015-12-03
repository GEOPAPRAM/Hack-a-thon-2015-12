using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class Revision
    {
        [JsonProperty("module")]
        public string Module { get; set; }
        [JsonProperty("revision")]
        public long Number { get; set; }
    }
}