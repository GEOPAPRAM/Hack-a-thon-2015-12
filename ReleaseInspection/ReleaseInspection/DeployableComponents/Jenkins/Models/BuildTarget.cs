using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class BuildTarget
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<AbstractAction> Actions { get; set; }
    }
}
