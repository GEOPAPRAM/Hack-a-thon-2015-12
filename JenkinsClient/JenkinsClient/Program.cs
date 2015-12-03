using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JenkinsClient.Converters;
using JenkinsClient.Model;
using Newtonsoft.Json;
using RestSharp;

namespace JenkinsClient
{
    class Program
    {
        static readonly IList<string> Clouds = new List<string> {"Cloud 4" };
        static readonly IList<string> EnvironmentCookbooks = new List<string> { "PerformWebServer", "PerformWorkerServer" };
        const string JenkinsUrl = "https://jenkins.nvminternal.net/";

        static void Main()
        {
            var cloudInformation = new List<CloudInfo>();

            foreach (var cloud in Clouds)
            {
                var cloudInfo = new CloudInfo(cloud);
                foreach (var cookbook in EnvironmentCookbooks)
                {
                    GetCloudInfo(cloudInfo, cookbook);
                }
                cloudInformation.Add(cloudInfo);
            }


            Console.WriteLine();
        }

        private static T Get<T>(string url, params JsonConverter[] converters) where T : class
        {
            var client = new RestClient(url);

            var request = new RestRequest(Method.GET);

            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unsuccessful request");
            }

            return JsonConvert.DeserializeObject<T>(response.Content, converters);
        }

        private static void GetCloudInfo(CloudInfo cloudInfo, string cookbookName)
        {
            var cookbookInfo = new CookbookInfo(cookbookName);
            
            //get all promotions
            var promotionStatus = Get<PromotionStatus>(string.Format("https://jenkins.nvminternal.net/job/EnvironmentCookbooks/job/{0}Environment/job/{0}/promotion/api/json", cookbookName));

            //for every cloud get the promotion history
            var promotion = promotionStatus.Promotions.FirstOrDefault(x => x.Name.EndsWith(cloudInfo.CloudName));

            //get the latest succesful build promoted to the cloud
            var promotionDetails = Get<PromotionDetails>(string.Concat(promotion.Url, "api/json?tree=lastSuccessfulBuild[url,number,timestamp]"));

            //get this build details
            var build = Get<BuildTarget>(string.Concat(promotionDetails.LastSuccessfulBuild.Url, "target/api/json"), new ActionJsonConverter());

            //Get build version for Environment and Application cookbooks
            var parameters = build.Actions.OfType<ParametersAction>().First();
            //Get causes that trigger the build
            var causes = build.Actions.OfType<CausesAction>().First();

            //Try to find our Application cookbook link and build number
            var appCookbookCause = causes.Causes.First(c => c.UpstreamProject.Contains("ApplicationCookbooks"));

            var changes = Get<Change>(string.Concat(JenkinsUrl, appCookbookCause.UpstreamUrl, appCookbookCause.UpstreamBuild, "/api/json?tree=changeSet[items[*],revisions[*]]"));

            var variables = Get<EnvironmentVariables>(string.Concat(build.Url, "injectedEnvVars/api/json"));

            cookbookInfo.Description = build.DisplayName;
            cookbookInfo.EnvBuildVersion = variables.Variables["BUILD_VERSION"];
            cookbookInfo.AppBuildVersion = variables.Variables["COOKBOOK_VERSION"];
            cookbookInfo.Changes = changes.ChangeSet;

            cloudInfo.AddCookbookInfo(cookbookInfo);

        }
    }
}
