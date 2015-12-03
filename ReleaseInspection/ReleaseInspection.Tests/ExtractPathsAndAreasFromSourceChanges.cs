using System.IO;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class ExtractPathsAndAreasFromSourceChanges
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private static readonly CallCentrePathsByArea CallCentrePathsByArea = new CallCentrePathsByArea();
        private static string[] _knownTestPaths;
        private static string[] _knownProductPaths;
        private PathsAndAreas _pathsAndAreas;

        [TestFixtureSetUp]
        public void InitialisePaths()
        {
            var callCentrePaths = CallCentrePathsByArea.PathsByArea.Values.ToArray();
            _knownTestPaths = new[] { callCentrePaths[0][0], callCentrePaths[1][0] };
            _knownProductPaths = new[] { callCentrePaths[2][0] };
        }

        [SetUp]
        public void InitialiseMocks()
        {
            _pathsAndAreas = new PathsAndAreas(CallCentrePathsByArea);
        }

        [Test]
        public void PathsAreGrouped()
        {
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownTestPaths[0], _knownTestPaths[1] }, 45321);

            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            var secondChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownTestPaths[0], _knownProductPaths[0] }, 45321);

            _pathsAndAreas.ExtractPathsAndAreas(secondChange);

            Assert.That(_pathsAndAreas.Paths, Has.Count.EqualTo(3));
            Assert.That(_pathsAndAreas.Paths, Contains.Item(_knownTestPaths[0]));
            Assert.That(_pathsAndAreas.Paths, Contains.Item(_knownTestPaths[1]));
            Assert.That(_pathsAndAreas.Paths, Contains.Item(_knownProductPaths[0]));
        }

        [Test]
        public void TestPathsAreTranslatedToAreas()
        {
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownTestPaths[0] }, 45321);

            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            Assert.That(_pathsAndAreas.TestAreas, Is.Not.Empty);
        }

        [Test]
        public void ProductPathsAreTranslatedToAreas()
        {
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownProductPaths[0] }, 45321);

            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            Assert.That(_pathsAndAreas.Areas, Is.Not.Empty);
        }

        [Test]
        public void AreaCountIsIncremented()
        {
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownProductPaths[0], _knownProductPaths[0] + "wibble.cs" }, 45321);

            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            Assert.That(_pathsAndAreas.Areas.First().Value.Count, Is.EqualTo(2));
            Assert.That(_pathsAndAreas.TestAreas.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAreaCountIsIncremented()
        {
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { _knownTestPaths[0], _knownTestPaths[0] + "/wibble.cs" }, 45321);
            
            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            Assert.That(_pathsAndAreas.TestAreas.First().Value.Count, Is.EqualTo(2));
            Assert.That(_pathsAndAreas.Areas.Count, Is.EqualTo(0));
        }

        [Test]
        public void UnMappedPathsAreExposed()
        {
            var unmappedPath = Path.GetRandomFileName();
            var firstChange = new SourceChange(UtcNow, "author", "[#123] comments", new[] { unmappedPath }, 45321);
            
            _pathsAndAreas.ExtractPathsAndAreas(firstChange);

            Assert.That(_pathsAndAreas.UnmappedPaths, Is.Not.Empty);
        }
    }
}
