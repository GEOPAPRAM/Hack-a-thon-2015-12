using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF
{
    public class WafPathsByArea : IProductPathAreaMap
    {
        private const string AppFirewallRules = "App Firewall Rules";
        private const string AppFirewallCookbook = "App Firewall Cookbook";
        private const string LinuxCookbook = "Linux cookbook";
        private const string CloudsCookbook = "Clouds cookbook";

        public IDictionary<string, string[]> PathsByArea { get; private set; }
        public IEnumerable<string> IgnoredPaths { get; private set; }

        public WafPathsByArea()
        {
            PathsByArea = new Dictionary<string, string[]>
            {
                {
                    AppFirewallRules, 
                    new[]
                    {
                        "activated_rules"
                    }
                },
                {
                    AppFirewallCookbook, 
                    new[]
                    {
                        "nvm_app_firewall"
                    }
                },
                {
                    LinuxCookbook, 
                    new[]
                    {
                        "nvm_linux"
                    }
                },
                {
                    CloudsCookbook, 
                    new[]
                    {
                        "nvm_clouds"
                    }
                }
            };

            IgnoredPaths = new[]
            {
                "/metdata.rb"
            };
        }

        public IPathsAndAreas Create()
        {
            return new PathsAndAreas(this);
        }
    }
}