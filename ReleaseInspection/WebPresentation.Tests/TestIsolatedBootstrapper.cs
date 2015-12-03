using System;
using System.IO;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    public class TestIsolatedBootstrapper : ReleaseNotesBootStrapper
    {
        private static readonly DateTime Now = DateTime.UtcNow;
        private static readonly CallCentrePathsByArea CallCentrePathsByArea = new CallCentrePathsByArea();
        private static string[] _knownPaths;

        public static readonly string UnmappedPath = Path.GetRandomFileName();

        public ISourceControl SourceControl { get; private set; }
        public IStoryRepositoryClient StoryRepositoryClient { get; private set; }
        public IDeployableComponent DeployableComponent { get; private set; }
        public IVersionRetrieverService VersionRetrieverService { get; private set; }

        public TestIsolatedBootstrapper()
        {
            SourceControl = MockRepository.GenerateMock<ISourceControl>();
            VersionRetrieverService = MockRepository.GenerateMock<IVersionRetrieverService>();
            StoryRepositoryClient = MockRepository.GenerateMock<IStoryRepositoryClient>();
            var callCentrePaths = CallCentrePathsByArea.PathsByArea.Values.ToArray();
            _knownPaths = new[] { callCentrePaths[0][0], callCentrePaths[1][0], callCentrePaths[2][0] };
        }
        
        protected override void ConfigureApplicationContainer(Ninject.IKernel container)
        {
            base.ConfigureApplicationContainer(container);
            ApplicationContainer.Rebind<ISourceControl>().ToConstant(SourceControl);
            ApplicationContainer.Rebind<IStoryRepositoryClient>().ToConstant(StoryRepositoryClient);
            ApplicationContainer.Unbind<IVersionRetrieverService>();
            ApplicationContainer.Rebind<IVersionRetrieverService>().ToConstant(VersionRetrieverService);
        }
        
        public void ConfigureReleaseService(string storyState, Version liveVersion)
        {
            StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id= "1", 
                    Title = "Some Title", 
                    Status = new IssueStatus { Name = storyState }, 
                    StoryType = new IssueType { Id = (int) StoryType.Feature, Name = StoryType.Feature.ToString() },
                    Team = new Team()
                });
       
            SourceControl.Stub(sc => sc.GetChanges(null, long.MinValue, long.MinValue))
                .IgnoreArguments()
                .Return(new[] { new SourceChange(Now, "Author", "[#123]", new[] { _knownPaths[0], UnmappedPath }, 54678)});
            VersionRetrieverService.Stub(v => v.GetVersionInformation()).Return(liveVersion);

        }
        
    }
}