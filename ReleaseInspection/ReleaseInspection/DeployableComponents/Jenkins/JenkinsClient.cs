using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EasyHttp.Http;
using Newtonsoft.Json;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Converters;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using CloudInfo = NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models.CloudInfo;

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

            var promotionStatus = GetCookbookPromotionStatus(JenkinsUrl, cookbookName);

            var promotion = GetPromotionForCloud(promotionStatus, cloudName);

            if(promotion==null) throw new Exception("No promotions for specified cloud!");

            var promotionDetails = GetPromotionDetails(promotion);

            var build = GetBuildInfo(promotionDetails);

            //Get build version for Environment and Application cookbooks
            //var parameters = build.Actions.OfType<ParametersAction>().First();
            var appCookbookCause = GetAppCookbookCause(build);

            var changes = Get<Change>(string.Concat(JenkinsUrl, "/", appCookbookCause.UpstreamUrl, appCookbookCause.UpstreamBuild, "/api/json?tree=changeSet[items[*],revisions[*]]"));

            var variables = GetBuildEnvVariables(build);

            cloudInfo.Cookbook.AppCookbook = appCookbookCause;
            cloudInfo.Cookbook.Description = build.DisplayName;
            cloudInfo.Cookbook.EnvBuildVersion = variables.Variables["BUILD_VERSION"];
            cloudInfo.Cookbook.AppBuildVersion = variables.Variables["COOKBOOK_VERSION"];
            cloudInfo.Cookbook.AppCookbookName = variables.Variables["CHILD_COOKBOOK"];
            cloudInfo.Cookbook.Changes = changes.ChangeSet;

            return cloudInfo;
        }

        public DeploymentInfo GetDeploymentInfoForClouds(IList<string> clouds, IEnumerable<string> cookbooks, string identifier)
        {
            var deploymentInfo = new DeploymentInfo(identifier);

            foreach (var cookbook in cookbooks)
            {
                var promotionStatus = GetCookbookPromotionStatus(JenkinsUrl, cookbook);
                foreach (var cloud in clouds)
                {
                    var promotion = GetPromotionForCloud(promotionStatus, cloud);

                    if (promotion == null) continue;
                    try
                    {
                        var promotionDetails = GetPromotionDetails(promotion);
                        var build = GetBuildInfo(promotionDetails);
                        var variables = GetBuildEnvVariables(build);

                        var cookbookInfoBase = new CookbookInfoBase(cookbook)
                        {
                            Description = build.DisplayName,
                            EnvBuildVersion = variables.Variables["BUILD_VERSION"],
                            AppBuildVersion = variables.Variables["COOKBOOK_VERSION"],
                            AppCookbookName = variables.Variables["CHILD_COOKBOOK"]
                        };

                        deploymentInfo.AddCloudCookbookInfo(cloud, cookbookInfoBase);
                    }
                    catch
                    {
                        
                    }
                }
            }

            return deploymentInfo;
        }

        private static PromotionStatus GetCookbookPromotionStatus(string jenkinsUrl, string cookbookName)
        {
            //get all promotions
            return Get<PromotionStatus>(string.Format("{0}/job/EnvironmentCookbooks/job/{1}Environment/job/{1}/promotion/api/json", jenkinsUrl, cookbookName));
        }

        private static Promotion GetPromotionForCloud(PromotionStatus promotionStatus, string cloudName)
        {
            //for every cloud get the promotion history
            return promotionStatus.Promotions.FirstOrDefault(x => x.Name.EndsWith(cloudName));
        }

        private static EnvironmentVariables GetBuildEnvVariables(BuildTarget build)
        {
            var variables = Get<EnvironmentVariables>(string.Concat(build.Url, "injectedEnvVars/api/json"));
            return variables;
        }

        private static Cause GetAppCookbookCause(BuildTarget build)
        {
            //Get causes that trigger the build
            var causes = build.Actions.OfType<CausesAction>().First();

            //Try to find our Application cookbook link and build number
            return causes.Causes.First(c => c.UpstreamProject.Contains("ApplicationCookbooks"));
        }

        private static BuildTarget GetBuildInfo(PromotionDetails promotionDetails)
        {
            //get this build details
            return Get<BuildTarget>(string.Concat(promotionDetails.LastSuccessfulBuild.Url, "target/api/json"), new ActionJsonConverter());
        }

        private static PromotionDetails GetPromotionDetails(Promotion promotion)
        {
            //get the latest succesful build promoted to the cloud
            return Get<PromotionDetails>(string.Concat(promotion.Url, "api/json?tree=lastSuccessfulBuild[url,number,timestamp]"));
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
