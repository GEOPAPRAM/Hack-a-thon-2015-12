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
    public class ReleaseContentsByStory
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private ReleaseService _releaseService;
        private IDeployableComponent _mockDeployableComponent;
        private IPathsAndAreasFactory _pathsAndAreasFactory;
        private IPathsAndAreas _pathsAndAreas;
        private IStoryRepositoryClient _mockStoryClient;
        private IProductPathAreaMap _mockProductPathAndAreas;
        private const string StoryTitle = "title";

        [SetUp]
        public void SetupRelease()
        {
            _mockProductPathAndAreas = MockRepository.GenerateStub<IProductPathAreaMap>();
            _mockProductPathAndAreas.Stub(p => p.IgnoredPaths).Return(new[] {"path5"});
            _mockProductPathAndAreas.Stub(p => p.PathsByArea).Return(new Dictionary<string, string[]>
            {
                {"Path 1", new[]{"path1", "path2" }}
            });
            _mockDeployableComponent = MockRepository.GenerateStub<IDeployableComponent>();
            _mockDeployableComponent.Stub(d => d.ProductAreaPathMap).Return(_mockProductPathAndAreas);
            _pathsAndAreas = new PathsAndAreas(_mockProductPathAndAreas);
            _pathsAndAreasFactory = new PathsAndAreasFactory();

            _mockStoryClient = MockRepository.GenerateStub<IStoryRepositoryClient>();
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("123")))
                            .Return(new Story("123", StoryTitle, "started", StoryType.Bug));
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("345")))
                            .Return(new Story("345", StoryTitle, "accepted", StoryType.Feature));
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("567")))
                            .Return(new Story("345", StoryTitle, "delivered", StoryType.Feature));
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("789")))
                            .Return(new Story("345", StoryTitle, "accepted", StoryType.Bug));
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("222")))
                            .Return(new Story("345", StoryTitle, "accepted", StoryType.Chore));
            _mockStoryClient.Stub(sc => sc.GetStory(Arg.Is("111")))
                            .Return(new Story("345", StoryTitle, "accepted", StoryType.Chore));




            _releaseService = new ReleaseService(_mockDeployableComponent, new StoryWorkFactory(_pathsAndAreasFactory), _mockStoryClient);
        }

        [Test]
        public void ReleaseRevisionsArePopulated()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321)
                             });

            const string from = "1";
            const string to = "2";
            var model = _releaseService.PopulateContents(from, to);

            Assert.That(model.PreviousVersion, Is.EqualTo(from));
            Assert.That(model.Version, Is.EqualTo(to));
        }

        [Test]
        public void GivenStoriesReleaseContentsIsPopulatedInTheCorrectOrder()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#789] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#567] comments", new [] {"path1", "path2"}, 45321)
                             });
            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.AcceptedWork, Is.Not.Empty);
            Assert.That(model.AcceptedWork.ElementAt(0).Story.StoryType.Name, Is.EqualTo(StoryType.Feature.ToString().ToLower()));
            Assert.That(model.AcceptedWork.ElementAt(1).Story.StoryType.Name, Is.EqualTo(StoryType.Bug.ToString().ToLower()));

            Assert.That(model.UnfinishedWork, Is.Not.Empty);
            Assert.That(model.UnfinishedWork.ElementAt(0).Story.StoryType.Name, Is.EqualTo(StoryType.Feature.ToString().ToLower()));
            Assert.That(model.UnfinishedWork.ElementAt(1).Story.StoryType.Name, Is.EqualTo(StoryType.Bug.ToString().ToLower()));
        }

        [Test]
        public void GivenCommentsForUniqueStoriesReleaseContentsAreGroupedByStatus()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path1", "path2"}, 45321)
                             });

            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.AcceptedWork.Select(s => s.Story.Id), Contains.Item("345"));
            Assert.That(model.UnfinishedWork.Select(s => s.Story.Id), Contains.Item("123"));
        }

        [Test]
        public void GivenCommentsForDuplicateStoriesReleaseContentsAreGroupedByStoryAndStatus()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path3", "path4"}, 45321)
                             });

            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.AcceptedWork, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenMixtureOfStoryIdsAndNoStoryIdReleaseContentsAreGroupedByStory()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "comments", new [] {"path3", "path4"}, 45321),
                                 new SourceChange(UtcNow, "author", "More comments", new [] {"path3", "path4"}, 45321)
                             });

            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.UntrackedWork, Is.Not.Null);
        }

        [Test]
        public void GivenOneNoStoryIdChangeThenReleaseContentsContainOneRowUntrackedWork()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "comments", new [] {"path1", "path2"}, 45321),
                             });

            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.UntrackedWork, Is.Not.Null);
            Assert.That(model.UntrackedWork.Items.Count, Is.EqualTo(1));
            Assert.That(model.UntrackedWork.Items[0].Author, Is.EqualTo("author"));
            Assert.That(model.UntrackedWork.Items[0].PathsAndAreas.Paths.Contains("path1"));
            Assert.That(model.UntrackedWork.Items[0].PathsAndAreas.Paths.Contains("path2"));
        }

        [Test]
        public void GivenTwoNoStoryIdChangesThenReleaseContentsContainTwoRowsUntrackedWork()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author1", "comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author2", "More comments", new [] {"path3", "path4"}, 45322)
                             });

            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.UntrackedWork, Is.Not.Null);
            Assert.That(model.UntrackedWork.Items.Count, Is.EqualTo(2));
            Assert.That(model.UntrackedWork.Items[0].Author, Is.EqualTo("author1"));
            Assert.That(model.UntrackedWork.Items[0].PathsAndAreas.Paths.Contains("path1"));
            Assert.That(model.UntrackedWork.Items[0].PathsAndAreas.Paths.Contains("path2"));
            Assert.That(model.UntrackedWork.Items[1].Author, Is.EqualTo("author2"));
            Assert.That(model.UntrackedWork.Items[1].PathsAndAreas.Paths.Contains("path3"));
            Assert.That(model.UntrackedWork.Items[1].PathsAndAreas.Paths.Contains("path4"));
        }

        [Test]
        public void GivenOnlyChoresTheseAreCorrectlySeparatedFromStories()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#111] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#222] comments", new [] {"path1", "path2"}, 45321)
                             });

            var model = _releaseService.PopulateContents("1", "2");
            
            Assert.That(model.AcceptedWork, Is.Empty);
            Assert.That(model.UnfinishedWork, Is.Empty);
            Assert.That(model.Chores.AcceptedChores, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenAMixtureOfStoriesAndChoresTheseAreCorrectlySeparated()
        {
            _mockDeployableComponent.Stub(dc => dc.GetChanges(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(UtcNow, "author", "[#111] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#222] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#123] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#789] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#345] comments", new [] {"path1", "path2"}, 45321),
                                 new SourceChange(UtcNow, "author", "[#567] comments", new [] {"path1", "path2"}, 45321)

                             });
            var model = _releaseService.PopulateContents("1", "2");

            Assert.That(model.AcceptedWork, Has.Count.EqualTo(2));
            Assert.That(model.UnfinishedWork, Has.Count.EqualTo(2));
            Assert.That(model.Chores.AcceptedChores, Has.Count.EqualTo(2));            
        }
    }
}
