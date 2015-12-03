using System;
using Nancy.Testing;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
   
    [TestFixture]
    class ModuleResponses
    {
        private TestIsolatedBootstrapper _bootstrapper;
        private static readonly DateTime Now = DateTime.UtcNow;

        [SetUp]
        public void SetUp()
        {
            _bootstrapper = new TestIsolatedBootstrapper();
        }

        [TestCaseSource(typeof (TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenValidRevisionRequestTheResponseHasUtf8Encoding(string application)
        {
            const string storyTitle = "Some Story Title";

            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id = "1", 
                    Title = storyTitle,
                    Status = new IssueStatus { Name = "Accepted" }, 
                    StoryType = new IssueType { Id = (int) StoryType.Feature, Name = StoryType.Feature.ToString() },
                    Team = new Team()
                });

            var response = GetResponse(application);

            Assert.That(response.ContentType.Contains("utf-8"));
        }

        private BrowserResponse GetResponse(string application)
        {
            _bootstrapper.SourceControl.Stub(d => d.GetChanges(Arg<string>.Is.Anything, Arg<long>.Is.Equal(1), Arg<long>.Is.Equal(2)))
               .Return(
               new[]
                {
                    new SourceChange(Now, "Author", "[#123]", new[] { "path 1" }, 12345),
                    new SourceChange(Now, "Another Author", "Commit Message", new[] { "path 2" }, 12346)
                });

            var browser = new Browser(_bootstrapper);

            var response = browser.Get(
                "/revisions/" + application,
                with =>
                {
                    with.HttpRequest();
                    with.Query("start", "1");
                    with.Query("end", "2");
                }
            );

            return response;
        }
    }

    
    
}
