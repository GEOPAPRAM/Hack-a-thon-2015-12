using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public static class StoryIdExtractor
    {
        public static string ExtractPivotalId(string comments)
        {
            var storyNumberRegex = new Regex(@"\[#(\d+)\]");

            var firstMatch = storyNumberRegex.Match(comments);

            if (firstMatch.Success && firstMatch.Groups.Count >= 2)
            {
                var firstMatchedValue = firstMatch.Groups[1].Value;

                var parsedId = 0;

                if (int.TryParse(firstMatchedValue, out parsedId))
                {
                    return parsedId.ToString(CultureInfo.InvariantCulture);
                }
            }

            return String.Empty;
        }

        public static string ExtractJiraId(string comments)
        {
            var storyNumberRegex = new Regex(@"^\[?#?[a-zA-Z0-9]+[-][1-9][0-9]*\]?");

            var firstMatch = storyNumberRegex.Match(comments);

            if (firstMatch.Success && firstMatch.Groups.Count >= 1)
            {
                var tag = firstMatch.Groups[0].Value;
                tag = tag.Replace("#", string.Empty);
                tag = tag.Replace("[", string.Empty);
                tag = tag.Replace("]", string.Empty);

                return tag;
            }

            return String.Empty;
        }

        public static string TryExtractStoryId(this string comments)
        {
            var storyId = ExtractPivotalId(comments);
            if (storyId == string.Empty)
                storyId = ExtractJiraId(comments);
            return storyId;
        }
    }
}
