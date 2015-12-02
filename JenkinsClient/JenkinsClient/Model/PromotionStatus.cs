using System.Collections.Generic;
using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class PromotionStatus
    {
        [JsonProperty("processes")]
        public IList<Promotion> Promotions { get; set; }
    }
}
