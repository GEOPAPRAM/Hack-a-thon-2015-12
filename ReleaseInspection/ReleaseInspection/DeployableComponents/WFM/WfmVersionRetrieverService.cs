using System;
using System.Text.RegularExpressions;
using EasyHttp.Http;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM
{
    public class WfmVersionRetrieverService : IVersionRetrieverService
    {
        public Version GetVersionInformation()
        {
            var httpClient = new HttpClient();
            var response = httpClient.Get(@"https://wfm4.contact-world.net/");
            var responseString = response.RawText;
            var regex = new Regex(@"Version: ([\d\.]+)", RegexOptions.None);
            var match = regex.Match(responseString);

            var version = string.Empty;

            if (match.Success)
            {
                version = match.Groups[1].Value;
            }

            var versionInformation = new Version(version);

            return versionInformation;
        }
    }
}
