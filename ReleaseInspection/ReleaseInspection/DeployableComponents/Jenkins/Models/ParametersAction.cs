using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class ParametersAction: AbstractAction
    {
        [JsonProperty("parameters")]
        public IList<Parameter> Parameters { get; set; }
    }
}
