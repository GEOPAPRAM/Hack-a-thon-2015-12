using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JenkinsClient.Model;
using Newtonsoft.Json;
using RestSharp;

namespace JenkinsClient
{
    class Program
    {
        static readonly IList<string> Clouds = new List<string>{"Cloud 4", "Cloud 17", "Cloud 11", "Cloud 12"};

        static void Main()
        {
            //get all promotions
            var promotionStatus = Get<PromotionStatus>("https://jenkins.nvminternal.net/job/EnvironmentCookbooks/job/PerformWorkerServerEnvironment/job/PerformWorkerServer/promotion/api/json");
            //foreach (var cloud in Clouds)
            //{
                
            //}

            //for every cloud get the promotion history
            var promotion = promotionStatus.Promotions.FirstOrDefault(x => x.Name.EndsWith("Cloud 4"));

            if(promotion==null) return;

            //get the latest succesful build promoted to the cloud
            var promotionDetails = Get<PromotionDetails>(string.Concat(promotion.Url, "api/json?tree=lastSuccessfulBuild[url,number,timestamp]"));

            if(promotionDetails==null) return;

            //get this build details
            var build = Get<BuildTarget>(string.Concat(promotionDetails.LastSuccessfulBuild.Url, "target/api/json"));


            var variables = Get<EnvironmentVariables>(string.Concat(build.Url, "injectedEnvVars/api/json"));
            
            Console.WriteLine();
        }

        private static T Get<T>(string url) where T:class
        {
            var client = new RestClient(url);

            var request = new RestRequest(Method.GET);
            
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unsuccessful request");
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }


    }
}
