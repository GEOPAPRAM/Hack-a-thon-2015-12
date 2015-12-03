using System.Collections.Generic;
using Newtonsoft.Json;

namespace JenkinsClient.Converters
{
    public class ParametersAction: AbstractAction
    {
        [JsonProperty("parameters")]
        public IList<Parameter> Parameters { get; set; }
    }
}
