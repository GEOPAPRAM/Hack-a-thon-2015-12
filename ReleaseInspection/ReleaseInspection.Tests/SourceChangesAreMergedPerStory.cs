using System;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class SourceChangesAreMergedPerStory
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private IPathsAndAreasFactory _mockPathsAndAreasFactory;
        private IPathsAndAreas _mockPathsAndAreas;
        private IProductPathAreaMap _mockPathsAndAreasMap;

        [SetUp]
        public void InitialiseMocks()
        {
            _mockPathsAndAreasFactory = MockRepository.GenerateStub<IPathsAndAreasFactory>();
            _mockPathsAndAreasMap = MockRepository.GenerateStub<IProductPathAreaMap>();
            _mockPathsAndAreas = MockRepository.GenerateStub<IPathsAndAreas>();
            _mockPathsAndAreasFactory.Stub(m => m.Create(_mockPathsAndAreasMap)).Return(_mockPathsAndAreas);
        }

        [Test]
        public void AuthorsAreGrouped()
        {
            var storyWork = new StoryWork(new Story("123"), _mockPathsAndAreasFactory.Create(_mockPathsAndAreasMap));

            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { "path1", "path2" }, 45321);
            storyWork.MergeChanges(firstChange);
            var secondChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { "path1", "path3" }, 45321);

            storyWork.MergeChanges(secondChange);

            Assert.That(storyWork.Authors, Has.Count.EqualTo(1));
        }

        [Test]
        public void CommentsAreListed()
        {
            var storyWork = new StoryWork(new Story("123"), _mockPathsAndAreasFactory.Create(_mockPathsAndAreasMap));

            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { "path1", "path2" }, 45321);
            storyWork.MergeChanges(firstChange);
            var secondChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { "path1", "path3" }, 45321);

            storyWork.MergeChanges(secondChange);

            Assert.That(storyWork.Comments, Has.Count.EqualTo(2));
        }

        [Test]
        public void RevisionsAreSavedInSeparateUntrackedStoryWorkItems()
        {
            var storyWork = new UntrackedStoryWork(new PathsAndAreas(new CallCentrePathsByArea()));

            var firstChange = new SourceChange(UtcNow, "author1", "[#123] comments", new[] { "path1", "path2" }, 45321);
            storyWork.MergeChanges(firstChange);
            var secondChange = new SourceChange(UtcNow, "author2", "[#123] comments", new[] { "path1", "path3" }, 45322);
            storyWork.MergeChanges(secondChange);

            Assert.That(storyWork.Items, Has.Count.EqualTo(2));
            Assert.That(storyWork.Items[0].Revision, Is.EqualTo(45321));
            Assert.That(storyWork.Items[1].Revision, Is.EqualTo(45322));
        }

        [Test]
        public void AuthorsAreSavedInSeparateUntrackedStoryWorkItems()
        {
            var storyWork = new UntrackedStoryWork(new PathsAndAreas(new CallCentrePathsByArea()));

            var firstChange = new SourceChange(UtcNow, "author1", "[#123] comments", new[] { "path1", "path2" }, 45321);
            storyWork.MergeChanges(firstChange);
            var secondChange = new SourceChange(UtcNow, "author2", "[#123] comments", new[] { "path1", "path3" }, 45322);
            storyWork.MergeChanges(secondChange);

            Assert.That(storyWork.Items, Has.Count.EqualTo(2));
            Assert.That(storyWork.Items[0].Author, Is.EqualTo("author1"));
            Assert.That(storyWork.Items[1].Author, Is.EqualTo("author2"));
        }

        [Test]
        public void CommentsAreSavedInSeparateUntrackedStoryWorkItems()
        {
            var storyWork = new UntrackedStoryWork(new PathsAndAreas(new CallCentrePathsByArea()));

            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments1", new[] { "path1", "path2" }, 45321);
            storyWork.MergeChanges(firstChange);
            var secondChange = new SourceChange(UtcNow, "author", "[#123] comments2", new[] { "path1", "path3" }, 45322);

            storyWork.MergeChanges(secondChange);

            Assert.That(storyWork.Items, Has.Count.EqualTo(2));
            Assert.That(storyWork.Items[0].Comment, Is.EqualTo("[#123] comments1"));
            Assert.That(storyWork.Items[1].Comment, Is.EqualTo("[#123] comments2"));
        }
    }
}
