using NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class RoutingForGivenReleaseCandidate
    {
        private Browser _browser;
        private IReleaseCandidateService _mockReleaseCandidateService;

        [SetUp]
        public void SetupInstances()
        {
            _mockReleaseCandidateService = MockRepository.GenerateStub<IReleaseCandidateService>();
            _mockReleaseCandidateService.Stub(r => r.PopulateContents(Arg<string>.Is.Anything))
                .Return(new ReleaseModel("1.2.3.45678", "1.2.2.231321"));

            _browser = new Browser(with =>
            {
                with.Module<ReleaseCandidateModule>();
                with.Dependencies(_mockReleaseCandidateService);
            });
        }

        [Test]
        public void ReleaseCandidateRequestRoutesToCorrectModule()
        {
            var result = _browser.Get(
                "/call-centre/1.2.3.1234",
                with => with.HttpRequest()
                );

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));            
        }
    }
}
