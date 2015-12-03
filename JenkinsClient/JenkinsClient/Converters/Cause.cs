using Newtonsoft.Json;

namespace JenkinsClient.Converters
{
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