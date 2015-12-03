using System;
using System.Collections.Generic;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class SourceChange
    {
        public SourceChange(DateTime dateTime, string author, string comments, IEnumerable<string> paths, long revision)
        {
            DateTime = dateTime;
            Author = author;
            Comments = comments;
            Paths = paths;
            Revision = revision;
        }

        public DateTime DateTime { get; private set; }
        public string Author { get; private set; }
        public string Comments { get; private set; }
        public IEnumerable<string> Paths { get; private set; }
        public long Revision { get; private set; } 
    }
}
