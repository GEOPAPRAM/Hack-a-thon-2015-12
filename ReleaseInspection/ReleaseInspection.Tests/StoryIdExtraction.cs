using System;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NUnit.Framework;

namespace NewVoiceMedia.Tools.ReleaseInspection.Tests
{
    [TestFixture]
    public class StoryIdExtraction
    {
        [TestCase("12345678", "[#12345678]")]
        [TestCase("12345678", "[#12345678] blah ...")]
        [TestCase("12345678", "[#12345678] [#121] blah ...")]
        public void GivenPivotalIdPresentIdIsReturned(string storyId, string message)
        {
            var result = StoryIdExtractor.ExtractPivotalId(message);

            Assert.That(result, Is.EqualTo(storyId));
        }

        [TestCase("")]
        [TestCase("fdsafsdf")]
        [TestCase("12345678")]
        public void GivenNoPivotalIdNullIsReturned(string message)
        {
            Assert.AreEqual(StoryIdExtractor.ExtractPivotalId(message), String.Empty);
        }

        [TestCase("")]
        [TestCase("fdsafsdf")]
        [TestCase("12345678")]
        public void GivenNoJiraIdNullIsReturned(string message)
        {
            Assert.AreEqual(StoryIdExtractor.ExtractJiraId(message), String.Empty);
        }

        [TestCase("TA-126", "[#TA-126]")]
        [TestCase("TA-126", "TA-126 fdsafsdf")]
        [TestCase("TA-126", "[TA-126] [#123] [#TA-111] blah blah ...")]
        [TestCase("TA-126", "TA-126: [#123] [#TA-111] blah blah ...")]
        [TestCase("TA-126", "[TA-126]: [#123] [#TA-111] blah blah ...")]
        [TestCase("TA-126", "[#TA-126]: [#123] [#TA-111] blah blah ...")]
        [TestCase("P99-111", "[#P99-111]")]
        [TestCase("P99-111", "P99-111 fdsafsdf")]
        [TestCase("P99-111", "[P99-111] [#123] [#TA-111] blah blah ...")]
        [TestCase("P99-111", "P99-111: [#123] [#TA-111] blah blah ...")]
        [TestCase("P99-111", "[P99-111]: [#123] [#TA-111] blah blah ...")]
        [TestCase("P99-111", "[#P99-111]: [#123] [#TA-111] blah blah ...")]
        public void GivenJiraIdPresentIdIsReturned(string storyId, string message)
        {
            var result = StoryIdExtractor.ExtractJiraId(message);

            Assert.That(result, Is.EqualTo(storyId));
        }
    }
}
