using Nancy.Testing;
﻿using NUnit.Framework;
﻿using System;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    [TestFixture]
    public class DisplayLiveVersion
    {
        private Browser _browser;
        private Version _releaseCandidateVersion;
        private Version _contactWorldVersion;

        [SetUp]
        public void SetupInstances()
        {
            var randomRevision = new Randomizer();
            _releaseCandidateVersion = new Version(1, 2, 3, randomRevision.Next(12345, 99999));
            _contactWorldVersion = new Version(1, 2, 2, randomRevision.Next(12345, 99999));
            var testBootstrapper = new TestIsolatedBootstrapper();
           _browser = new Browser(testBootstrapper);
            testBootstrapper.ConfigureReleaseService("accepted", _contactWorldVersion);
        }

        [TestCaseSource(typeof(TestCaseFactoryForReleaseModule),"ApplicationCases")]
        public void GivenVersionIsDisplayedInTheView(string application)
        {
            var result = _browser.Get(
                application + "/" + _releaseCandidateVersion,
                with => with.HttpRequest()
                );

            Assert.That(result.Body.AsString(), Is.StringContaining(_contactWorldVersion.ToString()));
        }
    }
}
