using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR
{
    public class IvrPathsByArea : IProductPathAreaMap
    {
        private const string Documentation = "Documentation";
        private const string Installers = "Installers";
        private const string ReleaseNotes = "Release Notes";
        private const string Source = "Source";
        private const string Test = "Test";
        private const string UnitTests = "Unit Tests";
        private const string Configuration = "Configuration";
        private const string AudioResources = "Audio Resource Files";

        public IDictionary<string, string[]> PathsByArea { get; private set; }
        public IEnumerable<string> IgnoredPaths { get; private set; }

        public IvrPathsByArea()
        {
            PathsByArea = new Dictionary<string, string[]>
                {
                    { 
                        UnitTests, new []
                        {
                            "Source/NewVoiceMedia.IVR.ContactWorldAccessTests",
                            "Source/NewVoiceMedia.IVR.Core.Tests",
                            "Source/NewVoiceMedia.IVR.Core.WebAPI.Tests",
                            "Source/NewVoiceMedia.IVR.EmailPaymentReportsTests",
                            "Source/NewVoiceMedia.IVR.ExternalDependenciesTests",
                            "Source/NewVoiceMedia.IVR.Payments.Tests",
                            "Source/NewVoiceMedia.IVR.PaymentWebAPI.Services.Tests",
                            "Source/NewVoiceMedia.IVR.PaymentWebAPI.Tests",
                            "Source/NewVoiceMedia.IVR.PaymentWebUITests",
                            "Source/NewVoiceMedia.IVR.PfwAps.Tests",
                            "Source/NewVoiceMedia.IVR.StandardAudio.Tests",
                            "Source/NewVoiceMedia.IVR.Tests.Common",
                            "Source/NewVoiceMedia.IVR.Web.Tests",
                            "Source/NewVoiceMedia.IVR.WebInstanceTests",
                            "Source/NewVoiceMedia.IVR.UnitTests.config"
                        }
                    },
                    {
                        AudioResources, new []
                        {
                            ".wav"
                        }
                    },
                    
                    {
                        Configuration, new []
                        {
                            "Source/Models"    
                        }
                    },
                    {
                        Documentation, new[]
                        {
                            "Documentation"
                        }
                    },
                    {
                        Installers, new[]
                        {
                            "Installers"
                        }
                    },
                    {
                        ReleaseNotes, new[]
                        {
                            "ReleaseNotes"
                        }
                    },
                    {
                        Source, new[]
                        {
                            "Source"
                        }
                    },
                    {
                        Test, new[]
                        {
                            "Test"
                        }
                    }
                };

            IgnoredPaths = new List<string>();
        }
    }
}
