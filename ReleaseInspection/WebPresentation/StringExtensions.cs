using System;
using System.Configuration;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation
{
    public static class DisplayHelpers
    {
        private static readonly string SvnRootUrl;

        static DisplayHelpers()
        {
            SvnRootUrl = ConfigurationManager.AppSettings["SvnRootUrl"];
        }

        public static string ToDisplayPath(this string path)
        {
            return path
                .Replace(SvnRootUrl, string.Empty)
                .Replace(@"/components/applications/call_centre/trunk/source", @"...")
                .Replace(@"/components/applications/call_centre/trunk/test/source", @"...")
                .Replace(@"/components/installers/database/trunk",@"...");
        }

        public static string ToViewVcPath(this string path)
        {
            return path.Replace("/svn/", "/viewvc/");
        }

        public static string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString().Substring(1, 32);
        }
    }
}