using Newtonsoft.Json;

namespace JenkinsClient.Model
{
    [JsonObject(MemberSerialization.OptOut, ItemRequired = Required.Always)]
    public abstract class AbstractAction
    {
    }
}