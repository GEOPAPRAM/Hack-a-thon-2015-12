﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Converters;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class Item
    {
        [JsonProperty("revision")]
        public long Revision { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("date", ItemConverterType = typeof(UnixTimestampConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("commitId")]
        public string CommitId { get; set; }
        [JsonProperty("affectedPaths")]
        public IList<string> AffectedPaths { get; set; }
    }
}