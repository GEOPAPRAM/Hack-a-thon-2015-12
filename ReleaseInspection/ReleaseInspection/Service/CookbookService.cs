using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class CookbookService : ICookbookService
    {
        private readonly IJenkinsClient _jenkinsClient;
        private ISourceControl _sourceControl;
        private IStoryWorkFactory _storyWorkFactory;
        private IStoryRepositoryClient _storyRepositoryClient;

        public CookbookService(IJenkinsClient jenkinsClient, ISourceControl sourceControl, IStoryWorkFactory storyWorkFactory, IStoryRepositoryClient storyRepositoryClient)
        {
            _jenkinsClient = jenkinsClient;
            _sourceControl = sourceControl;
            _storyWorkFactory = storyWorkFactory;
            _storyRepositoryClient = storyRepositoryClient;
        }

        public CloudInfo PopulateCloudInfo(string cloudName, string cookbookName)
        {
            var changesByStory = new Dictionary<string, StoryWork>();
            var cloudInfo = _jenkinsClient.GetCloudInformation(cloudName, cookbookName);

            var revision = cloudInfo.GetAppCookbookRevision();

            var changes = GetChanges(new[] { revision.Module }, revision.Number);

            return new CloudInfo("", "");
        }

        private IEnumerable<SourceChange> GetChanges(IEnumerable<string> contributingPaths, long fromRevision)
        {
            var combinedChanges = new ConcurrentBag<SourceChange>();

            Task.WaitAll(contributingPaths.Select(path => Task.Factory.StartNew(() => GetChanges(path, fromRevision, combinedChanges))).ToArray());

            return combinedChanges.ToArray();
        }

        private void GetChanges(string path, long fromRevision, ConcurrentBag<SourceChange> combinedChanges)
        {
            var localChanges = _sourceControl.GetChangesSince(path, fromRevision);

            foreach (var sourceChange in localChanges.Where(c => c.Author != "jenkins.deploy"))
            {
                combinedChanges.Add(sourceChange);
            }
        }

    }
}
