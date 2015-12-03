using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre
{
    public class CallCentrePathsByArea : IProductPathAreaMap
    {
        public const string Administration = "NVM Only Administration";
        public const string AgentInteractions = "Agent Interactions";
        public const string CallCentreCore = "CallCentre Core";
        public const string CallCentreInstaller = " CallCentre Installer";
        public const string CallControl = "Call Control";
        public const string CallRecording = "Call Recording";
        public const string CallPlanArchitect = "Call Plan Architect";
        public const string CallPlanRunTime = "Call Plan Run-Time";
        public const string ContactPad = "Contact Pad";
        public const string CrmObjectRouting = "CRM Object Routing";
        public const string DatabaseInstaller = " Database Installer";
        public const string EventsAndQueueManagerService = "Events and QueueManager Services";
        public const string IntegrationTests = "Integration Tests";
        public const string InternalWebApi = "Internal Web API";
        public const string LanguageFiles = "User interface text (Language files)";
        public const string Model = "Data/Domain Model";
        public const string PublicWebApi = "Public Web Api";
        public const string Root = "Root";
        public const string SalesForceIntegration = "SalesForce Integration";
        public const string SupervisorAdministration = "Supervisor Administration (new UI)";
        public const string ServiceControlSuite = "Service Control Suite";
        public const string Statistics = "Statistics";
        public const string ThirdPartyServiceIntegration = "Third Party Service Integration";
        public const string UnitTests = "Unit Tests";
        public const string UserAdministration = "User Administration";
        public const string WebApiCallControl = "Call Control Web API";
        public const string WebApiChat = "Chat Web API";

        public IDictionary<string, string[]> PathsByArea { get; private set; }
        public IEnumerable<string> IgnoredPaths { get; private set; }
        
        public CallCentrePathsByArea()
        {

            PathsByArea
                =
                new Dictionary<string, string[]>
                {
                    {
                        IntegrationTests,
                        new[]
                        {
                            "/components/applications/call_centre/trunk/test",
                            "ModelTest/IntegrationTests"
                        }
                    }
                    ,
                    {
                        UnitTests,
                        new[]
                        {
                            "source/AgentTelephonyBaseTest/",
                            "callcentre.tests.common/",
                            "source/ServicesTest/",
                            "/Tests/",
                            ".Tests/",
                            ".Test/",
                            "Test/",
                            "source/ModelTest/",
                            "source/NewVoiceMedia.Services/WebServicesTest/"
                        }
                    }
                    ,
                    {
                        AgentInteractions,
                        new[]
                        {
                            "source/AgentTelephonyBase/",
                            "/source2/AgentTelephony/"
                        }
                    }
                    ,
                    {
                        CallCentreCore,
                        new[] {"trunk/source/CallCentre.Core"}
                    }
                    ,
                    {
                        CallControl,
                        new[] {"source/NewVoiceMedia/CallControl", "Source2/AgentDesktop1"}
                    }
                    ,
                    {
                        CallPlanArchitect,
                        new[] {"trunk/source/CallPlanArchitect"}
                    }
                    ,
                    {
                        CallPlanRunTime,
                        new[]
                        {
                            "source/NewVoiceMedia.CallPlanRuntime.DataProviders",
                            "Source2/VoiceXML",
                            "CallPlanRuntime.Service/"
                        }
                    }
                    ,
                    {
                        CallRecording,
                        new []
                        {
                            "callanalytics/callrecording",
                            "/source2/callanalytics/"
                        }
                    }
                    ,
                    {
                        ContactPad,
                        new[] {"trunk/source/AgentInterface"}
                    }
                    ,
                    {
                        CrmObjectRouting,
                        new[] {"source/NewVoiceMedia.CrmObjectRouting/"}
                    }
                    ,
                  
                    {
                        Model,
                        new[]
                        {
                            "CallCentreModel"
                        }
                    }
                    ,
                    {
                        EventsAndQueueManagerService,
                        new[]
                        {
                            "source/NewVoiceMedia.Services/WebServices",
                            "source/NewVoiceMedia.Services/WebServices.ServiceContract"
                        }
                    }
                    ,
                    {
                        CallCentreInstaller,
                        new[]
                        {
                            "CallCentre Installer.aip",
                            "Source2/scripts"
                        }
                    }
                    ,
                    
                    {
                        LanguageFiles,
                        new[]
                        {
                            "/language/"
                        }
                    }
                    ,
                    {
                        ServiceControlSuite,
                        new[] {"Source2/wildcardVCM"}
                    }
                    ,
                    {
                        SalesForceIntegration,
                        new[] {"source/Services/SalesForce", "source/Salesforce"}
                    }
                    ,
                    {
                        Statistics,
                        new[]
                        {
                            "NewVoiceMedia/Statistics",
                            "Source2/stats"
                        }
                    }
                    ,
                    {
                        SupervisorAdministration,
                        new[]
                        {
                            "/CallCentreAdministration.UI/"
                        }
                    }
                    ,
                    {
                        ThirdPartyServiceIntegration,
                        new[] {"source/Services/Services.csproj"}
                    }
                    ,
                    
                    {
                        WebApiCallControl,
                        new[] {"source/PublicAPIs/ExternalCallHandler.WebApi/"}
                    }
                    ,
                    {
                        WebApiChat,
                        new[] {"source/PublicAPIs/ChatRouting.WebApi/"}
                    }
                    ,
                    {
                        DatabaseInstaller,
                        new[]
                        {
                            "Upgrade scripts/",
                            "ccdbinstaller.aip"
                        }
                    }
                    ,
                    {
                        UserAdministration,
                        new[]
                        {
                            "source/Administration/UserAdministration.UI",
                            "source/Administration/UserAdministration.Service",
                            "source/Administration/UserAdministration.Service.Tests",
                            "source/Administration/UserAdministration.UI.Tests"
                        }
                    }
                    ,
                    {
                        Administration,
                        new[]
                        {
                            "source/Administration/AdministrationUI",
                            "source/Administration/Administration.Service",
                            "source/Administration/Administration.Service.Tests",
                            "source/Administration/AdministrationUI.Tests"
                        }
                    }
                    ,
                    {
                        Root,
                        new[]
                        {
                            "source/Source2/Root"
                        }
                    }
                    ,
                    {
                        InternalWebApi,
                        new[]
                        {
                            "source/WebApi.Services"
                        }
                    }
                       ,
                    {
                        PublicWebApi,
                        new[]
                        {
                            "source/WebApi"
                        }
                    }
                };

            IgnoredPaths = new[]
            {
                "/components/installers/call_centre/trunk/ReleaseNotes/ReleaseNotes.txt",
                "source/CallCentre.sln"
            };
        }
    }
}

