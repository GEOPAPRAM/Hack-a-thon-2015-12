using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM
{
    // ReSharper disable once InconsistentNaming
    public class WFMCookbookPathsByArea : IProductPathAreaMap
    {
        public const string Attributes = "Attributes";
        public const string Recipes = "Recipes";
        public const string Templates = "Templates";
        public const string Tests = "Tests";

        public IDictionary<string, string[]> PathsByArea { get; private set; }
        public IEnumerable<string> IgnoredPaths { get; private set; }

        public WFMCookbookPathsByArea()
        {
            PathsByArea =
                new Dictionary<string, string[]>
                {
                    {
                        Attributes,
                        new []
                        {
                            "attributes"
                        }
                    },
                    {
                        Recipes,
                        new []
                        {
                            "recipes"
                        }
                    },
                    {
                        Templates,
                        new []
                        {
                            "templates"
                        }
                    },
                    {
                        Tests,
                        new []
                        {
                            "tests"
                        }
                    }
                };
            IgnoredPaths = new List<string>
            {
                ".kitchen",
                ".kitchen.local.yml",
                "/metdata.rb"
            };
        }
    }
}
