using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class PromotionStatus
    {
        [JsonProperty("processes")]
        public IList<Promotion> Promotions { get; set; }
    }
}
