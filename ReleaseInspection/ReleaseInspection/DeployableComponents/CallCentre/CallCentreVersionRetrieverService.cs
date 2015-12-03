using System;
using System.Text.RegularExpressions;
using EasyHttp.Http;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre
{
    public class CallCentreVersionRetrieverService : IVersionRetrieverService
    {
        public Version GetVersionInformation()
        {
            var httpClient = new HttpClient();
            var response = httpClient.Get(@"http://cloud11.contact-world.net/callcentre/contacthub/login");
            var responseString = response.RawText;

            var regex = new Regex(@"\(V([\d\.]+)", RegexOptions.None);
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
