using System;
using System.Configuration;
using System.Diagnostics;
using EasyHttp.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public class JiraClient 
    {
        public Story GetStory(string storyId)
        {
            var issueUrl = string.Format("https://newvoicemedia.atlassian.net/rest/api/2/issue/{0}", storyId);
            var retryPolicy = new RetryPolicy<PivotalTrackerErrorDetectionStrategy>(new FixedInterval());

            try
            {
                return retryPolicy.ExecuteAction(() => GetJson(issueUrl));
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
                throw;
            }
        }

        //TODO: replace this with proper authentication, use basic auth for now
        private static Story GetJson(string url)
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var username = ConfigurationManager.AppSettings["JiraUserName"];
            var pass = ConfigurationManager.AppSettings["JiraPass"];

            var basicAuthKey = GeneratedBasicAuthKey(username, pass);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(pass))
            {
                throw new ConfigurationErrorsException(string.Format("Failed to load the Jira username and password. Value was: {0}:{1}", username, pass));
            }

            httpClient.Request.AddExtraHeader("Authorization", String.Format("Basic {0}", basicAuthKey));

            var response = httpClient.Get(url);
            dynamic storyResponse = response.DynamicBody;
            
            var story = new Story
            {
                Id = storyResponse.id,
                Key = storyResponse.key,
                Title = storyResponse.fields.summary,
                Status = new IssueStatus
                {
                    Id = Convert.ToInt32(storyResponse.fields.status.id), 
                    Name = storyResponse.fields.status.name.ToLower()
                },
                StoryType = new IssueType
                {
                    Id = Convert.ToInt32(storyResponse.fields.issuetype.id), 
                    Name = storyResponse.fields.issuetype.name.ToLower()
                },
                Team = new Team
                {
                    Id = Convert.ToInt32(storyResponse.fields.project.id), 
                    Name = storyResponse.fields.project.name
                },
                Url = String.Format("https://newvoicemedia.atlassian.net/browse/{0}", storyResponse.key)
            };
            
            return story;
        }

        private static object GeneratedBasicAuthKey(string username, string pass)
        {
            var userAndPass = String.Format("{0}:{1}", username, pass);
            byte[] toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(userAndPass);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            
            return returnValue;
        }
    }
}
