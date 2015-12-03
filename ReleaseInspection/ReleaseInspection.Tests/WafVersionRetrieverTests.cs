using System.IO;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF;
using NUnit.Framework;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class WafVersionRetrieverTests
    {
        [Test]
        public void WafVersionRetrieverGetsRightVersion()
        {
            var versionRetriever = new WafVersionRetrieverService();
            var html = File.ReadAllText("ChefCookbookVersions.html");

            var version = versionRetriever.GetVersionFromHtml(html);
            Assert.AreEqual("18.2.11", version);
        }

    }
}
