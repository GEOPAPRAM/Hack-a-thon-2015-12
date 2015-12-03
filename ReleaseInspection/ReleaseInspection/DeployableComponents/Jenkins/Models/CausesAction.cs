using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class CausesAction : AbstractAction
    {
        [JsonProperty("causes")]
        public IList<Cause> Causes { get; set; }
    }
}
