using System;
using JenkinsClient.Converters;
using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class Build
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime StartedAt { get; set; }
    }
}
