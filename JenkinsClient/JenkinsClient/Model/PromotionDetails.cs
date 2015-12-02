using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    public class PromotionDetails
    {
        [JsonProperty("lastSuccessfulBuild")]
        public Build LastSuccessfulBuild { get; set; }
    }
}
