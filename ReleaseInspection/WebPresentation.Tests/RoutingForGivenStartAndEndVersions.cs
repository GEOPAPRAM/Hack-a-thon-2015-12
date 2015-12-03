using System.Globalization;
using NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class RoutingForGivenStartAndEndVersions
    {
        private IReleaseService _mockReleaseService;
        private Browser _browser;
        const string Start = "1234";
        const string End = "5678";

        [SetUp]
        public void SetupInstances()
        {
            _mockReleaseService = MockRepository.GenerateStub<IReleaseService>();
            _mockReleaseService.Stub(r => r.PopulateContents(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
            
            _browser = new Browser(with =>
            {
                with.Module<StoriesModule>();
                with.Dependencies(_mockReleaseService);
            });
        }

        [Test]
        public void StoryRequestRoutesToStoryModule()
        {
            var result = MakeGetRequest(Start, End);

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));            
        }

        [Test]
        public void StartRevisionIsRequired()
        {
            MakeGetRequest(Start, End);

            _mockReleaseService.AssertWasCalled(r => r.PopulateContents(Arg<string>.Is.Equal(Start), Arg<string>.Is.Anything));
        }

        [Test]
        public void EndRevisionIsRequired()
        {
            MakeGetRequest(Start, End);

            _mockReleaseService.AssertWasCalled(r => r.PopulateContents(Arg<string>.Is.Anything, Arg<string>.Is.Equal(End)));
        }

        private BrowserResponse MakeGetRequest(string start, string end)
        {
            return _browser.Get(
                "/stories/",
                with =>
                {
                    with.HttpRequest();
                    with.Query("start", start);
                    with.Query("end", end);
                }
            );
        }
    }
}
