using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM
{
// ReSharper disable once InconsistentNaming
    public class WFMCookbooks: DeployableComponentBase
    {
        private string _svnRootUrl;
        public const string Identifier = "wfm";

        public WFMCookbooks(string componentName, ISourceControl sourceControl, IVersionRetrieverService versionRetrieverService) : base(componentName, sourceControl, versionRetrieverService)
        {
            _svnRootUrl = ConfigurationManager.AppSettings["SvnRootUrl"];
        }

        public override IEnumerable<string> ContributingPaths
        {
            get { return Enumerable.Empty<string>(); }
        }

        public override IProductPathAreaMap ProductAreaPathMap
        {
            get { return new WFMCookbookPathsByArea(); }
        }

        protected override void GetChanges(string contributingPath, long fromRevision, long toRevision, System.Collections.Concurrent.ConcurrentBag<SourceChange> mergedChanges)
        {
            var localChanges = SourceControl.GetChanges(contributingPath, fromRevision, toRevision);

            foreach (var sourceChange in localChanges.Where(c => c.Author != "jenkins.deploy"))
            {
                mergedChanges.Add(sourceChange);
            }
        }
    }
}
