using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class PromotionDetails
    {
        [JsonProperty("lastSuccessfulBuild")]
        public Build LastSuccessfulBuild { get; set; }
    }
}
