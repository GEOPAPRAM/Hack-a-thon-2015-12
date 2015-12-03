using System;
using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents
{
    public interface IDeployableComponent
    {
        IEnumerable<SourceChange> GetChanges(long fromRevision, long toRevision);
        string ComponentName { get; }
        long GetRevisionForVersion(string version);
        IEnumerable<string> ContributingPaths { get; }
        IProductPathAreaMap ProductAreaPathMap { get; }
        Version GetLiveVersionInformation();
    }
}
