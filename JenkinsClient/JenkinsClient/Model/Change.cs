using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class Change
    {
        [JsonProperty("changeSet")]
        public ChangeSet ChangeSet { get; set; }
    }
}
