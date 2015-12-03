using System;
using System.Collections.Generic;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NUnit.Framework;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class PopulateReleaseStoryDetails
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private ReleaseModel _releaseModel;
        private IDeployableComponent _mockDeployableComponent;
        private IPathsAndAreasFactory _mockPathsAndAreasFactory;
        private IPathsAndAreas _mockPathsAndAreas;
       
        private IStoryRepositoryClient _mockStoryClient;
        private const string StoryId = "123";
        private const string StoryTitle = "title";
        private const string StoryStatus = "started";

        [SetUp]
        public void SetupRelease()
        {
            _mockDeployableComponent = MockRepository.GenerateStub<IDeployableComponent>();
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", string.Format("[#{0}] comments", StoryId), new [] {"path1", "path2"}, 45321)
                             });

            _mockPathsAndAreasFactory = MockRepository.GenerateStub<IPathsAndAreasFactory>();
            _mockPathsAndAreas = MockRepository.GenerateStub<IPathsAndAreas>();
            _mockPathsAndAreasFactory.Stub(m => m.Create(Arg<IProductPathAreaMap>.Is.Anything)).Return(_mockPathsAndAreas);


            _mockStoryClient = MockRepository.GenerateStub<IStoryRepositoryClient>();
            _mockStoryClient.Stub(sc => sc.GetStory(Arg<string>.Is.Anything))
                            .Return(new Story("123", StoryTitle, StoryStatus, null));

            var service = new ReleaseService(_mockDeployableComponent, new StoryWorkFactory(_mockPathsAndAreasFactory), _mockStoryClient);

            _releaseModel = service.PopulateContents("1", "2");
        }

        [Test]
        public void StoryDetailsAreRetrievedFromStoryRepository()
        {
            _mockStoryClient.AssertWasCalled( sc => sc.GetStory(StoryId));
        }
    }
}
