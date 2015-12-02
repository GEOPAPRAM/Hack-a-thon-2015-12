using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class Promotion
    {
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}