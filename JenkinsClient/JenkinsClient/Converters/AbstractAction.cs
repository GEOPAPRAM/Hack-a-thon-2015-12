using Newtonsoft.Json;

namespace JenkinsClient.Converters
{
    [JsonObject(MemberSerialization.OptOut, ItemRequired = Required.Always)]
    public abstract class AbstractAction
    {
    }
}