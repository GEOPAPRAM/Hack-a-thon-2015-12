using System;
using System.Linq;
using EasyHttp.Http;
using HtmlAgilityPack;
using NewVoiceMedia.Tools.ReleaseInspection.Service;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF
{
    public class WafVersionRetrieverService : IVersionRetrieverService
    {
        private const string ChefCookbookVersionUri = "http://nvmchef01.general.newvoicemedia.com:8081/dashboard/";

        public Version GetVersionInformation()
        {
            var client = new HttpClient(string.Empty);
            var response = client.Get(ChefCookbookVersionUri);
            return new Version(GetVersionFromHtml(response.RawText));
        }

        public string GetVersionFromHtml(string html)
        { 
            var doc = new HtmlDocument();
              doc.LoadHtml(html);
            // find index of "Production Environment - excludes web farms" in columns
            var columnIndex = doc.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .First()
                .Descendants("td")
                .Select(
                    (td, i) =>
                        td.InnerText == "Production Environment - excludes web farms"
                            ? new {index = i}
                            : new {index = -1})
                .First(t => t.index > -1).index;


            // find value in row that holds "nvm_app_firewall"
            return doc.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .First(tr => tr.Descendants("td").Any(td => td.InnerText == "nvm_app_firewall"))
                .Descendants("td")
                .ToList()[columnIndex]
                .InnerText;
        }

    }
}