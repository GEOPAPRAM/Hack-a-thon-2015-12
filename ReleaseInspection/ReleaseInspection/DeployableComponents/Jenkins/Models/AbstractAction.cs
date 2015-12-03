using Newtonsoft.Json;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    [JsonObject(MemberSerialization.OptOut, ItemRequired = Required.Always)]
    public abstract class AbstractAction
    {
    }
}