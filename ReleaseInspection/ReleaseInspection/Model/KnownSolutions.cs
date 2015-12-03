using System;
using System.Collections.Generic;
using System.Configuration;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class KnownSolutions : IKnownSolutions
    {
        private readonly ISourceControl _sourceControl;

        private readonly IDictionary<string, Uri> _knownSolutions = new Dictionary<string, Uri>
            {
                {"call-centre", new Uri(string.Format("{0}/nvm/components/applications/call_centre/trunk/source/CallCentre.sln", ConfigurationManager.AppSettings["SvnRootUrl"]))}
            };

        public KnownSolutions(ISourceControl sourceControl)
        {
            _sourceControl = sourceControl;
        }

        public bool Contains(string name)
        {
            return _knownSolutions.ContainsKey(name);
        }

        public Solution Get(string name)
        {
            var uri = _knownSolutions[name];

            return new Solution(name, uri, _sourceControl);
        }
    }
}