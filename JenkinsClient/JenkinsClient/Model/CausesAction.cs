using System.Collections.Generic;
using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class CausesAction : AbstractAction
    {
        [JsonProperty("causes")]
        public IList<Cause> Causes { get; set; }
    }
}
