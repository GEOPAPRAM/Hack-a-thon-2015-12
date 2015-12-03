using Nancy.Testing;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NUnit.Framework;
using Rhino.Mocks;
using System;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class DisplayStoryTitleAndStateFromPivotal
    {
        private static readonly DateTime Now = DateTime.UtcNow;
        private TestIsolatedBootstrapper _bootstrapper;


        [SetUp]
        public void SetupMock()
        {
            _bootstrapper = new TestIsolatedBootstrapper();
        }

        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenChangesWithValidPivotalIdThenStoryTitleIsDisplayed(string application)
        {
            const string storyTitle = "Some Story Title";

            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id = "1", 
                    Title = storyTitle, 
                    Status = new IssueStatus { Name = "Accepted" }, 
                    StoryType = new IssueType { Id = (int)StoryType.Feature, Name = StoryType.Feature.ToString() },
                    Team = new Team()
                });

            var response = GenerateRelease(application);

            Assert.That(response, Is.StringContaining(storyTitle));
        }

        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenChangesWithValidPivotalIdThenStoryStateIsDisplayed(string application)
        {
            const string storyState = "Some Story State";

            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id = "1", 
                    Title = "story Title", 
                    Status = new IssueStatus { Name = storyState }, 
                    StoryType = new IssueType { Id = (int) StoryType.Feature, Name = StoryType.Feature.ToString() },
                    Team = new Team()
                });

            var response = GenerateRelease(application);

            Assert.That(response, Is.StringContaining(storyState));
        }

        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenChangesWithValidPivotalIdThenStoryTypeIsDisplayed(string application)
        {
            const StoryType storyType = StoryType.Bug;

            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id = "1", 
                    Title = "story Title", 
                    Status = new IssueStatus { Name = "accepted" },
                    StoryType = new IssueType { Id = (int) storyType, Name = storyType.ToString().ToLower() },
                    Team = new Team()
                });

            var response = GenerateRelease(application);

            Assert.That(response, Is.StringContaining(storyType.ToString()));
        }

        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenChangesWithoutPivotalIdThenWebResponseContainsNotTraceableSection(string application)
        {
            var NotTraceableSection =
                "   <h2>Changes not traceable to a story/defect/chore</h2>" +
                "   <table class=\"table table-striped table-condensed\">" +
                "   <thead>" +
                "       <th>Revision</th>" +
                "       <th>Comment</th>" +
                "       <th>Authors</th>" +
                "       <th>Areas or Paths</th>" +
                "   </thead>";

            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return(new Story
                {
                    Id = "1", 
                    Title = "story Title", 
                    Status = new IssueStatus { Name = "accepted" }, 
                    StoryType = new IssueType { Id = (int) StoryType.Chore, Name = StoryType.Chore.ToString() },
                    Team = new Team()
                });

            var response = GenerateRelease(application);

            Assert.That(GetTrimmedString(response), Is.StringContaining(GetTrimmedString(NotTraceableSection)));
        }

        [Ignore]
        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void GivenChangesWithAnInvalidPivotalIdThenStoryStateIsDisplayed(string application)
        {
            _bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything)).Throw(new Exception());

            var response = GenerateRelease(application);

            Assert.That(response, Is.StringContaining(ReleaseService.PivotalFailureErrorMessage));
        }

        private string GenerateRelease(string application)
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

            return response.Body.AsString();
        }

        private string GetTrimmedString(string input)
        {
            return input.Replace(" ", "").Replace(Environment.NewLine, "");
        }
    }
}
