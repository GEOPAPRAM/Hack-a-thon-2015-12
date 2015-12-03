using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Converters
{
    public class ActionJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var properties = jsonObject.Properties();

            if(!properties.Any()) return null;

            JToken propertyToken;

            if(jsonObject.TryGetValue("parameters", out propertyToken))
            {
                return jsonObject.ToObject<ParametersAction>(serializer);
            }

            if (jsonObject.TryGetValue("causes", out propertyToken))
            {
                return jsonObject.ToObject<CausesAction>(serializer);
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (AbstractAction));
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
