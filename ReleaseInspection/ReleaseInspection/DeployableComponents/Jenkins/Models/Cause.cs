using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Cause
    {
        [JsonProperty("upstreamBuild")]
        public int UpstreamBuild { get; set; }
        [JsonProperty("upstreamProject")]
        public string UpstreamProject { get; set; }
        [JsonProperty("upstreamUrl")]
        public string UpstreamUrl { get; set; }
    }
}