using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using SharpSvn;
using SharpSvn.Security;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface ISourceControl
    {
        IEnumerable<string> GetFileContents(Uri uri);
        IEnumerable<SourceChange> GetChanges(string path, long fromRevision, long toRevision);
        IEnumerable<SourceChange> GetChanges(string path, DateTimeOffset from, DateTimeOffset to);
        IEnumerable<SourceChange> GetChangesSince(string path, DateTime fromDate);
        IEnumerable<SourceChange> GetChangesSince(string path, long fromRevision);
    }

    public class SourceControl : ISourceControl
    {
        public IEnumerable<string> GetFileContents(Uri uri)
        {
            var fileName = uri.Segments[uri.Segments.Length - 1];

            var fileFolder = uri.ToString().Replace(fileName, string.Empty);
            var checkOutFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (var client = new SvnClient())
            {
                client.LoadConfiguration(Path.Combine(Path.GetTempPath(), "Svn"), true);
                client.Authentication.Clear();
                client.Authentication.DefaultCredentials = new NetworkCredential(ConfigurationManager.AppSettings["SvnUsername"],
                                                                                    ConfigurationManager.AppSettings["SvnPassword"]);
                client.Authentication.SslServerTrustHandlers += InternalCertificateTrustHandler;

                client.CheckOut(new SvnUriTarget(fileFolder), checkOutFolder,
                    new SvnCheckOutArgs() { AllowObstructions = false, Depth = SvnDepth.Files });

                var fileContents = File.ReadAllLines(Path.Combine(checkOutFolder, fileName));

                return fileContents;
            }

        }

        public IEnumerable<SourceChange> GetChanges(string path, long fromRevision, long toRevision)
        {
            var pathUri = new Uri(path);
            var args = new SvnLogArgs { Range = new SvnRevisionRange(fromRevision, toRevision) };

            return GetSourceControlHistory(pathUri, args);
        }

        public IEnumerable<SourceChange> GetChangesSince(string path, DateTime fromDate)
        {
            var pathUri = new Uri(path);
            var args = new SvnLogArgs { Range = new SvnRevisionRange(new SvnRevision(fromDate), new SvnRevision(DateTime.Today)) };


            return GetSourceControlHistory(pathUri, args);
        }

        public IEnumerable<SourceChange> GetChangesSince(string path, long fromRevision)
        {
            var pathUri = new Uri(path);
            var args = new SvnLogArgs { Range = new SvnRevisionRange(new SvnRevision(fromRevision), new SvnRevision(SvnRevisionType.Head)) };


            return GetSourceControlHistory(pathUri, args);
        }

        public IEnumerable<SourceChange> GetChanges(string path, DateTimeOffset from, DateTimeOffset to)
        {
            var pathUri = new Uri(path);
            var args = new SvnLogArgs { Range = new SvnRevisionRange(new SvnRevision(from.DateTime), new SvnRevision(to.DateTime)) };

            return GetSourceControlHistory(pathUri, args);
        }

        private IEnumerable<SourceChange> GetSourceControlHistory(Uri pathUri, SvnLogArgs args)
        {
            Collection<SvnLogEventArgs> logItems;
            string repositoryRoot;

            using (var client = new SvnClient())
            {
                client.LoadConfiguration(Path.Combine(Path.GetTempPath(), "Svn"), true);
                client.Authentication.Clear();
                client.Authentication.DefaultCredentials =
                    new NetworkCredential(ConfigurationManager.AppSettings["SvnUsername"],
                        ConfigurationManager.AppSettings["SvnPassword"]);

                client.Authentication.SslServerTrustHandlers += InternalCertificateTrustHandler;
                repositoryRoot = client.GetRepositoryRoot(pathUri).ToString();
                if (repositoryRoot.EndsWith("/"))
                {
                    repositoryRoot = repositoryRoot.Remove(repositoryRoot.Length - 1);
                }

                client.GetLog(pathUri, args, out logItems);
            }

            var changes =
                logItems.Select(li => new SourceChange(li.Time, li.Author, li.LogMessage, li.ChangedPaths.Select(cp => string.Format("{0}{1}", repositoryRoot, cp.Path)).ToList(), li.Revision))
                        .ToList();
            return changes;
        }

        private void InternalCertificateTrustHandler(object sender, SvnSslServerTrustEventArgs actualCertificateDetails)
        {
            if (TheExpectedCertificateDetailMatch(actualCertificateDetails))
            {
                actualCertificateDetails.AcceptedFailures = actualCertificateDetails.Failures;
                actualCertificateDetails.Save = true;
            }
        }

        private bool TheExpectedCertificateDetailMatch(SvnSslServerTrustEventArgs e)
        {
            return e.CommonName == "*.nvminternal.net" &&
                   e.Failures == SvnCertificateTrustFailures.UnknownCertificateAuthority &&
                   e.Fingerprint == "FA:8F:6D:DF:95:63:D4:B3:C7:4F:31:CB:1A:26:26:4F:50:10:4D:E8" &&
                   e.Issuer == "http://certs.godaddy.com/repository/, GoDaddy.com, Inc., Scottsdale, Arizona, US" &&
                   e.ValidFrom == "Jan  9 13:25:38 2015 GMT" &&
                   e.ValidUntil == "Jan 10 17:59:24 2018 GMT";
        }
    }
}
