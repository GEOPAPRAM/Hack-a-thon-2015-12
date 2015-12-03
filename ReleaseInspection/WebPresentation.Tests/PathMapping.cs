
using System;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class RevisionsModuleTests
    {
        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void StoryRequestRoutesToRevisionsModule(string application)
        {
            var result = GenerateResponseForReleaseContainingStoryOfState("anything", application);

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void UnmappedPathsAreDisplayedForStoriesInProgress(string application)
        {
            var response = GenerateResponseForReleaseContainingStoryOfState("started", application);

            Assert.That(response.Body.AsString(), Is.StringContaining(TestIsolatedBootstrapper.UnmappedPath));
        }

        [TestCaseSource(typeof(TestCaseFactoryForRevisionsModule), "ApplicationCases")]
        public void UnmappedPathsAreDisplayedForAcceptedStories(string application)
        {
            var response = GenerateResponseForReleaseContainingStoryOfState("accepted", application);

            Assert.That(response.Body.AsString(), Is.StringContaining(TestIsolatedBootstrapper.UnmappedPath));
        }

        [TestCaseSource(typeof(TestCaseFactoryForReleaseModule), "ApplicationCases")]
        public void RequestForChangesToApplicationRoutesToApplicationComponents(string application)
        {
            var response = GenerateResponseForLiveVersionContainingStoryOfState("accepted", application);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private BrowserResponse GenerateResponseForReleaseContainingStoryOfState(string storyState, string application)
        {
            var bootstrapper = new TestIsolatedBootstrapper();
          
            var browser = new Browser(bootstrapper);
            bootstrapper.ConfigureReleaseService(storyState, new Version(1,2,3,4));
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

        private BrowserResponse GenerateResponseForLiveVersionContainingStoryOfState(string storyState, string application)
        {
            const string liveVersion = "1.2.3.4";
            var bootstrapper = new TestIsolatedBootstrapper();

            bootstrapper.ConfigureReleaseService(storyState, new Version(liveVersion));

            var browser = new Browser(bootstrapper);

            var response = browser.Get(
                "/" + application + "/1.2.3.5",
                with => with.HttpRequest()
                );

            return response;
        }
    }
}
