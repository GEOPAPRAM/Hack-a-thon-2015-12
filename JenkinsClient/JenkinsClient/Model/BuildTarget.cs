using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class BuildTarget
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
