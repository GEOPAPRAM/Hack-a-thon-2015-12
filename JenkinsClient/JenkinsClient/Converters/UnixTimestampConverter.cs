using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JenkinsClient.Converters
{
    public class UnixTimestampConverter : DateTimeConverterBase
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var unixTimestamp = (DateTime.UtcNow.Ticks - (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks)/10000;

            writer.WriteValue(unixTimestamp);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var ms = long.Parse(reader.Value.ToString());
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms);
        }
    }
}
