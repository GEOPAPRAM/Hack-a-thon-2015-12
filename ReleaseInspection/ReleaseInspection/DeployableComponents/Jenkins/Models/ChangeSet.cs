using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class ChangeSet
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
        [JsonProperty("revisions")]
        public List<Revision> Revisions { get; set; }
    }
}

