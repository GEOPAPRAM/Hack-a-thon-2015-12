using System;
using System.CodeDom;
using System.Linq;
using System.Net;
using EasyHttp.Http;
using Newtonsoft.Json;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Converters;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;
using CloudInfo = NewVoiceMedia.Tools.ReleaseInspection.Model.CloudInfo;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins
{
    public class JenkinsClient : IJenkinsClient
    {
        private string JenkinsUrl {get; set;}

        public JenkinsClient(string jenkinsUrl)
        {
            JenkinsUrl = jenkinsUrl;
        }

        public CloudInfo GetCloudInformation(string cloudName, string cookbookName)
        {
            var cloudInfo = new CloudInfo(cloudName, cookbookName);

            //get all promotions
            var promotionStatus = Get<PromotionStatus>(string.Format("{0}/job/EnvironmentCookbooks/job/{1}Environment/job/{1}/promotion/api/json",JenkinsUrl,cookbookName));

            //for every cloud get the promotion history
            var promotion = promotionStatus.Promotions.FirstOrDefault(x => x.Name.EndsWith(cloudInfo.CloudName));

            if(promotion==null) throw new Exception("No promotions for specified cloud!");

            //get the latest succesful build promoted to the cloud
            var promotionDetails = Get<PromotionDetails>(string.Concat(promotion.Url, "api/json?tree=lastSuccessfulBuild[url,number,timestamp]"));

            //get this build details
            var build = Get<BuildTarget>(string.Concat(promotionDetails.LastSuccessfulBuild.Url, "target/api/json"), new ActionJsonConverter());

            //Get build version for Environment and Application cookbooks
            //var parameters = build.Actions.OfType<ParametersAction>().First();
            //Get causes that trigger the build
            var causes = build.Actions.OfType<CausesAction>().First();

            //Try to find our Application cookbook link and build number
            var appCookbookCause = causes.Causes.First(c => c.UpstreamProject.Contains("ApplicationCookbooks"));

            var changes = Get<Change>(string.Concat(JenkinsUrl, "/", appCookbookCause.UpstreamUrl, appCookbookCause.UpstreamBuild, "/api/json?tree=changeSet[items[*],revisions[*]]"));

            var variables = Get<EnvironmentVariables>(string.Concat(build.Url, "injectedEnvVars/api/json"));

            cloudInfo.Cookbook.AppCookbook = appCookbookCause;
            cloudInfo.Cookbook.Description = build.DisplayName;
            cloudInfo.Cookbook.EnvBuildVersion = variables.Variables["BUILD_VERSION"];
            cloudInfo.Cookbook.AppBuildVersion = variables.Variables["COOKBOOK_VERSION"];
            cloudInfo.Cookbook.AppCookbook.Name = variables.Variables["CHILD_COOKBOOK"];
            cloudInfo.Cookbook.Changes = changes.ChangeSet;

            return cloudInfo;
        }

        private static T Get<T>(string url, params JsonConverter[] converters) where T : class
        {
            var client = new HttpClient();

            var response = client.Get(url);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unsuccessful request");
            }

            return JsonConvert.DeserializeObject<T>(response.RawText, converters);
        }
    }
}
