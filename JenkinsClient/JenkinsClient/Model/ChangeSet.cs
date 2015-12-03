using System.Collections.Generic;
using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class ChangeSet
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
        [JsonProperty("revisions")]
        public List<Revision> Revisions { get; set; }
    }
}

