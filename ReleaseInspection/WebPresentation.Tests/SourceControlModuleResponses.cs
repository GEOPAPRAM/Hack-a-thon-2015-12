using System;
using System.IO;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class SourceControlModuleResponses
    {
        private Browser _browser;
        private const string SolutionName = "call-centre";
        private ISourceControl _mockSourceControl;
        private static string[] _solutionContents;

        [TestFixtureSetUp]
        public void LoadSolutionFile()
        {
            _solutionContents =
                File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CallCentre.sln.txt"));
        }

        [SetUp]
        public void SetupInstances()
        {
            _mockSourceControl = MockRepository.GenerateStub<ISourceControl>();
            _mockSourceControl.Stub(a => a.GetFileContents(Arg<Uri>.Is.Anything)).Return(_solutionContents);

            _browser = new Browser( with =>
                {
                    with.Module<SourceControlModule>();
                    with.Dependencies(new KnownSolutions(_mockSourceControl));
                }
            );
        }

        [Test]
        public void SourceRequestRoutesToCorrectModule()
        {
            var result = _browser.Get(
                "/source/" + SolutionName,
                with => with.HttpRequest()
                );

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void SourceRequestForUnknownSolutionGivesNotFound()
        {
            var result = _browser.Get(
                "/source/" + Path.GetRandomFileName(),
                with => with.HttpRequest()
                );

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void CallCentreSourceRequestGetsProjectsList()
        {
            var result = _browser.Get(
                "/source/" + SolutionName,
                with => with.HttpRequest()
                );

            Assert.That(result.Body.AsString(), Is.StringContaining(SolutionName));
        }

        [Test]
        public void ProjectHistoryIsRetrievedForBaseFolder()
        {
            const string projectPath = @"NewVoiceMedia.ServiceControlSuite\UI\ServiceControlSuite.UI.csproj";
            const string projectLine = "Project(\"{Guid}\") = \"ProjectName\", \"" + projectPath + "\", \"{Guid}";

            _mockSourceControl = MockRepository.GenerateStub<ISourceControl>();
            _mockSourceControl.Stub(a => a.GetFileContents(Arg<Uri>.Is.Anything)).Return( new [] { projectLine });

            _browser = new Browser(with =>
            {
                with.Module<SourceControlModule>();
                with.Dependencies(new KnownSolutions(_mockSourceControl));
            }
            );

            _browser.Get(
                "/source/" + SolutionName,
                with => with.HttpRequest()
                );

            _mockSourceControl.AssertWasCalled( o => o.GetChangesSince(Arg<string>.Is.Anything, Arg<DateTime>.Is.Anything));

            var arguments = _mockSourceControl.GetArgumentsForCallsMadeOn(s => s.GetChangesSince(Arg<string>.Is.Anything, Arg<DateTime>.Is.Anything));

            var givenProjectPath = arguments[0][0];

            Assert.That(givenProjectPath, Is.StringEnding("NewVoiceMedia.ServiceControlSuite/UI/"));
        }
    }
}
