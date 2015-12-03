using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF
{
    public class WAF : DeployableComponentBase
    {
        private readonly string _svnRootUrl;
        public const string Identifier = "application-firewall";

        public WAF(ISourceControl sourceControl, IVersionRetrieverService versionRetrieverService)
            : base(Identifier, sourceControl, versionRetrieverService)
        {
            _svnRootUrl = ConfigurationManager.AppSettings["SvnRootUrl"];
        }

        public override IEnumerable<string> ContributingPaths
        {
            get
            {
                return new[]
                {
                    string.Format("{0}/chef-repo/trunk/cookbooks/nvm_app_firewall", _svnRootUrl),
                    string.Format("{0}/chef-repo/trunk/cookbooks/nvm_linux", _svnRootUrl ),
                    string.Format("{0}/chef-repo/trunk/cookbooks/nvm_clouds", _svnRootUrl ),
                    string.Format("{0}/nvm/components/3rdparty/modsecurity/etc/httpd/modsecurity.d", _svnRootUrl ) // TODO: remove this once we've released all these changes that were made here
                };
            }
        }

        public override IProductPathAreaMap ProductAreaPathMap
        {
            get { return new WafPathsByArea(); }
        }

        private string VersionFilePath
        {
            get { return string.Format("{0}/chef-repo/trunk//cookbooks/nvm_app_firewall/metadata.rb", _svnRootUrl); }
        }

        public override long GetRevisionForVersion(string version)
        {
            var actualVersion = string.Empty;
            var splitVersion = version.Split('.');

            for (int i = 0; i < splitVersion.Length; i++)
            {
                var divider = string.Empty;
                if (i == 1 && splitVersion[i].Length == 1)
                {
                    divider = ".0";

                }
                else if (i > 0)
                {
                    divider = ".";

                }
                actualVersion = string.Concat(actualVersion, divider, splitVersion[i]);
            }

            var earliestDate = DateTimeOffset.Now.AddDays(-100);
            var fromDate = DateTimeOffset.Now.AddDays(-20);
            var toDate = DateTimeOffset.Now;

            while (toDate > earliestDate)
            {
                var changes = SourceControl.GetChanges(VersionFilePath, fromDate, toDate);
                var versionRevision = changes.FirstOrDefault(v => v.Comments.Contains(actualVersion));
                if (versionRevision != null)
                {
                    return versionRevision.Revision;
                }

                toDate = fromDate;
                fromDate = fromDate.AddDays(-20);
            }

            return -1;
        }

        protected override void GetChanges(string contributingPath, long fromRevision, long toRevision, ConcurrentBag<SourceChange> mergedChanges)
        {
            var localChanges = SourceControl.GetChanges(contributingPath, fromRevision, toRevision);

            foreach (var sourceChange in localChanges.Where(c => c.Author != "jenkins.deploy"))
            {
                mergedChanges.Add(sourceChange);
            }
        }
    }
}