﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EnvironmentVariables
    {
        [JsonExtensionData]
        private IDictionary<string, JToken> _envMap;

        public EnvironmentVariables()
        {
            _envMap = new Dictionary<string, JToken>();
        }

        public IDictionary<string, string> Variables { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Variables = JsonConvert.DeserializeObject<Dictionary<string, string>>(_envMap["envMap"].ToString());
        }
    }
}
