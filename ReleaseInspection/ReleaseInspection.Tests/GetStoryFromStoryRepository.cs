using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NUnit.Framework;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class GetStoryFromStoryRepository
    {
        [Ignore("Ignored because pivotal client has been decommisioned")]
        protected class UsingPivotalTrackerClient : GetStoryFromStoryRepository
        {
            private static Story _story;
           
            [TestFixtureSetUp]
            public void GetStory()
            {
                var client = new PivotalTrackerClient();
                _story = client.GetStory("56941570");
            }

            [Test]
            public void StoryIsNotNull()
            {
                Assert.That(_story, Is.Not.Null);
            }

            [Test]
            public void TitleIsPopulated()
            {
                Assert.That(_story.Title, Is.Not.Empty);
            }

            [Test]
            public void StatusIsPopulated()
            {
                Assert.That(_story.Status, Is.Not.Null);
            }

            [Test]
            public void TypeIsPopulated()
            {
                Assert.That(_story.StoryType, Is.Not.Null);
            }
        }

        protected class UsingJiraClient : GetStoryFromStoryRepository
        {
            private static Story _story;

            [TestFixtureSetUp]
            public void GetStory()
            {
                var client = new JiraClient();
                _story = client.GetStory("14801");
            }

            [Test]
            public void StoryIsNotNull()
            {
                Assert.That(_story, Is.Not.Null);
            }

            [Test]
            public void TitleIsPopulated()
            {
                Assert.That(_story.Title, Is.Not.Empty);
            }

            [Test]
            public void StatusIsPopulated()
            {
                Assert.That(_story.Status, Is.Not.Null);
            }
        }
        
    }
}
