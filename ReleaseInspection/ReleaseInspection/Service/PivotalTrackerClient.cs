using System;
using System.Configuration;
using System.Diagnostics;
using EasyHttp.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class PivotalTrackerClient
    {
        public Story GetStory(string storyId)
        {
            var storyUrl = string.Format("https://www.pivotaltracker.com/services/v5/stories/{0}?fields=current_state,name,story_type,project_id", storyId);
            var retryPolicy = new RetryPolicy<PivotalTrackerErrorDetectionStrategy>(new FixedInterval());
            
            try
            {
                return retryPolicy.ExecuteAction(() => GetJson(storyUrl));
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
                throw;
            }
        }

        private Story GetJson(string url)
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var apiKey = ConfigurationManager.AppSettings["PivotalTrackerApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ConfigurationErrorsException(string.Format("Failed to load the PivotalTrackerApiKey. Value was: {0}", apiKey));
            }

            httpClient.Request.AddExtraHeader("X-TrackerToken", apiKey);
            
            //TODO: parse story_type here and set the id also to fix ordering issue
            //TODO: set the url for the link
            var response = httpClient.Get(url);
            dynamic storyResponse = response.DynamicBody;
            var story = new Story
            {
                Id = storyResponse.id.ToString(),
                Title = storyResponse.name,
                Status = new IssueStatus { Name = storyResponse.current_state.ToLower() },
                StoryType = new IssueType
                {
                    Name = storyResponse.story_type.ToLower(), 
                    Id = IssueType.TryParse(storyResponse.story_type.ToLower())
                },
                Team = new Team { Id = storyResponse.project_id },
                Url = string.Format("https://www.pivotaltracker.com/story/show/{0}", storyResponse.id.ToString())
            };

            return story;
        }
    }
}
