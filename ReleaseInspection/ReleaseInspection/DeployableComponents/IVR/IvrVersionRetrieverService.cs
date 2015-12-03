using System;
using System.Text.RegularExpressions;
using EasyHttp.Http;
using NewVoiceMedia.Tools.ReleaseInspection.Service;


namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR
{
    public class IvrVersionRetrieverService : IVersionRetrieverService
    {
        public Version GetVersionInformation()
        {
            var httpClient = new HttpClient();
            var response = httpClient.Get(@"https://paymentapi.contact-world.net/version");
            var versionInformation = new Version(GetVersion(response.RawText));

            return versionInformation;
        }

        private static string GetVersion(string responseString)
        {
            var version = "1.0.0.0";
            var regex = new Regex(string.Format(@"{0}([\d\.]+)", '"'), RegexOptions.None);
            var match = regex.Match(responseString);

            if (match.Success)
            {
                version = match.Groups[1].Value;
            }

            return version;
        }
    }
}
