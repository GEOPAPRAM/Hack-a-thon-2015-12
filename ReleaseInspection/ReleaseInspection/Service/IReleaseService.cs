using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface IReleaseService
    {
        ReleaseModel PopulateContents(string fromVersion, string toVersion);
        ReleaseModel PopulateContentsForCurrentLive(string toVersion);
        ReleaseModel PopulateContentsForRevisions(long fromRevision, long toRevision);
    }
}