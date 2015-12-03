
using System;
using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NUnit.Framework;
using Nancy.Testing;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class Caching
    {
        private static readonly DateTime Now = DateTime.UtcNow;

        [Test]
        public void ChangesAreIncorrectlyCachedBetweenResponses()
        {
            var bootstrapper = new TestIsolatedBootstrapper();
            bootstrapper.StoryRepositoryClient.Stub(c => c.GetStory(Arg<string>.Is.Anything))
                .Return( new Story
                {
                    Id = "521", 
                    Title = "Some Title", 
                    Status = new IssueStatus{ Name = "accepted" }, 
                    StoryType = new IssueType { Id = (int) StoryType.Feature, Name = StoryType.Feature.ToString() },
                    Team = new Team()
                });
            
            bootstrapper.SourceControl.Stub(d => d.GetChanges(Arg<string>.Is.Anything, Arg<long>.Is.Equal(1), Arg<long>.Is.Equal(2))).Return(new[] { new SourceChange(Now, "Author", "[#123]", new [] {"/path1"}, 5434) });
            bootstrapper.SourceControl.Stub(d => d.GetChanges(Arg<string>.Is.Anything, Arg<long>.Is.Equal(3), Arg<long>.Is.Equal(4))).Return(new[] { new SourceChange(Now, "Author", "[#345]", new[] { "/path2" }, 2345) });
          
            bootstrapper.VersionRetrieverService.Stub(d => d.GetVersionInformation()).Return(new Version(1, 2, 3, 4));
          
            var browser = new Browser(bootstrapper);

            var responseForRevisionAtoB = browser.Get(
                "/revisions/call-centre",
                with =>
                {
                    with.HttpRequest();
                    with.Query("start", "1");
                    with.Query("end", "2");
                }
            );

            Assert.That(responseForRevisionAtoB.Body.AsString(), Is.StringContaining("path1"));

            var responseForRevisionCtoD = browser.Get(
                "/revisions/call-centre",
                with =>
                {
                    with.HttpRequest();
                    with.Query("start", "3");
                    with.Query("end", "4");
                }
            );

            Assert.That(responseForRevisionCtoD.Body.AsString(), Is.Not.StringContaining("path1"));
        }
    }
}
