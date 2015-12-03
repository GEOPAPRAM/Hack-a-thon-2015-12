using Newtonsoft.Json;

namespace JenkinsClient.Converters
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
