using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NUnit.Framework;
using Rhino.Mocks;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class GetChangesFromSourceControl
    {
        const string SvnRootUrl = "https://svn.nvminternal.net/svn";

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["SvnUsername"] = "jenkins.ci";
            ConfigurationManager.AppSettings["SvnPassword"] = "$j3nkinsCI";
            ConfigurationManager.AppSettings["SvnRootUrl"] = SvnRootUrl;
        }
        [Test]
        public void GetChangesUsingSetCredentialsAndRevsionsGetsChanges()
        {           
            var sourceControl = new SourceControl();

            var path = string.Format("{0}/nvm/components/applications/call_centre/trunk", SvnRootUrl);
            const long from = 37813;
            const long to = 38274;

            var sourceChanges = sourceControl.GetChanges(path, from, to);

            Assert.That(sourceChanges, Is.Not.Empty);
            Assert.That(sourceChanges.First().Paths.First(), Is.StringStarting(string.Format("{0}/nvm", SvnRootUrl)));
        }

        [Test]
        public void GetChangesUsingSetCredentialsAndDatesGetsChanges()
        {
            var sourceControl = new SourceControl();

            var path = string.Format("{0}/nvm/components/applications/call_centre/trunk", SvnRootUrl);
            var from = new DateTime(2015, 02, 02);
            var to = new DateTime(2015, 02, 03);

            var sourceChanges = sourceControl.GetChanges(path, from, to);

            Assert.That(sourceChanges, Is.Not.Empty);
            Assert.That(sourceChanges.First().Paths.First(), Is.StringStarting(string.Format("{0}/nvm", SvnRootUrl)));
        }


        [Test]
        public void GetChangeSincesUsingSetCredentialsGetsChanges()
        {
            var sourceControl = new SourceControl();

            var path = string.Format("{0}/chef-repo", SvnRootUrl);
            var from = DateTime.Today.Subtract(TimeSpan.FromDays(1));

            var sourceChanges = sourceControl.GetChangesSince(path, from);

            Assert.That(sourceChanges, Is.Not.Empty);
            Assert.That(sourceChanges.First().Paths.First(), Is.StringStarting(string.Format("{0}/chef-repo", SvnRootUrl)));
        }

        [Test]
        public void GetChangesForEachSignificantPath()
        {
            var versionRetriever = MockRepository.GenerateMock<IVersionRetrieverService>();
            var sourceControl = MockRepository.GenerateStub<ISourceControl>();
            sourceControl.Stub(sc => sc.GetChanges(Arg<string>.Is.Anything, Arg<long>.Is.Anything, Arg<long>.Is.Anything))
                         .Return(new List<SourceChange>
                             {
                                 new SourceChange(DateTime.UtcNow, "author", "comments", new [] {"path1", "path2"}, 45321)
                             });
            var product = new CallCentre(sourceControl, versionRetriever);
            const int fromRevision = 1;
            const int toRevision = 2;

            product.GetChanges(fromRevision, toRevision);

            sourceControl.AssertWasCalled(sc => sc.GetChanges(Arg<string>.Is.Anything, Arg<long>.Is.Anything, Arg<long>.Is.Anything), opt => opt.Repeat.Times(product.ContributingPaths.Count()));
        }
    }
}
