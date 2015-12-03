using System;
using Newtonsoft.Json;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Converters;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
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
