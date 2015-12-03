using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents
{
    public abstract class DeployableComponentBase : IDeployableComponent
    {
        protected DeployableComponentBase(string componentName, ISourceControl sourceControl, IVersionRetrieverService versionRetrieverService)
        {
            SourceControl = sourceControl;
            VersionRetrieverService = versionRetrieverService;
            ComponentName = componentName;
        }

        public IEnumerable<SourceChange> GetChanges(long fromRevision, long toRevision)
        {
            var combinedChanges = new ConcurrentBag<SourceChange>();

            var tasks = new List<Task>();

            foreach (var contributingPath in ContributingPaths)
            {
                var path = contributingPath;
                tasks.Add(
                    Task.Factory.StartNew(() => GetChanges(path, fromRevision, toRevision, combinedChanges))
                    );
            }

            Task.WaitAll(tasks.ToArray());

            return combinedChanges.ToArray();
        }
        
        public string ComponentName { get; private set; }

        protected ISourceControl SourceControl
        {
            get;private set;
        }

        protected IVersionRetrieverService VersionRetrieverService
        {
            get;
            private set;
        }

        public abstract IEnumerable<string> ContributingPaths { get; }
        public abstract IProductPathAreaMap ProductAreaPathMap { get; }

        public virtual long GetRevisionForVersion(string version)
        {
            var revision = 0;

            if (Int32.TryParse(version, out revision))
            {
                return revision;
            }

            var systemVersion = new Version(version);

            return systemVersion.Revision;
        }

        public virtual Version GetLiveVersionInformation()
        {
            return VersionRetrieverService.GetVersionInformation();
        }

        protected virtual void GetChanges(string contributingPath, long fromRevision, long toRevision, ConcurrentBag<SourceChange> mergedChanges)
        {
            var localChanges = SourceControl.GetChanges(contributingPath, fromRevision, toRevision);
            
            foreach (var sourceChange in localChanges)
            {
                mergedChanges.Add(sourceChange);
            }
        }
    }
}
