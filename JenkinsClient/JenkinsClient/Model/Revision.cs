using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class Revision
    {
        [JsonProperty("module")]
        public string Module { get; set; }
        [JsonProperty("revision")]
        public long Number { get; set; }
    }
}