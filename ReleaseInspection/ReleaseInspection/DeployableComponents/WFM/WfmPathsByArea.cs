using System.Collections.Generic;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM
{
    public class WfmPathsByArea : IProductPathAreaMap
    {
        public const string BuildUtilities = "BuildUtilities";
        public const string Database = "Database";
        public const string MonitoringApp = "Monitoring App";
        public const string ThirdParty = "Third Party";
        public const string Tools = "Tools";
        public const string Model = "Model";
        public const string DevUi = "Dev Ui";
        public const string ServiceConsoleUi = "Service Console Ui";
        public const string RealTime = "Real time";
        public const string Forecast = "Forecast";
        public const string ShiftPatterns = "Shift Patterns";
        public const string Schedule = "Schedule";
        public const string Languages = "Languages";
        public const string Email = "Email";
        public const string ProjectFiles = "Project Files";
        public const string Agents = "Agents";
        public const string UnitTests = "Unit Tests";
        public const string IntegrationTests = "Integration Tests";

        public IDictionary<string, string[]> PathsByArea { get; private set; }
        public IEnumerable<string> IgnoredPaths { get; private set; }

        public WfmPathsByArea()
        {
            PathsByArea
                =
                new Dictionary<string, string[]>
                {
                    {
                        UnitTests,
                        new []
                        {
                            "Nostradamus.Ui.Test",
                            "Nostradamus.Presentation.Test",
                            "Nostradamus.Data.Nhibernate.Test",
                            "Nostradamus.Common.Test",
                            "Nostradamus.Areas.Test"
                        }
                    },
                    {
                        IntegrationTests,
                        new []
                        {
                            "IntegrationTests",
                            "Nostradamus.Selenium",
                            "SeleniumTestConfiguration",
                            "TestSuiteCommon",
                            "Nostradamus.Specifications",
                            "Nostradamus.Workers.Specifications"
                        }
                    },
                    {
                        Agents,
                        new []
                        {
                            "agent"
                        }
                    },
                    {
                        Email,
                        new []
                        {
                            "email"
                        }
                    },
                    {
                        Languages,
                        new []
                        {
                            "language"
                        }
                    },
                    {
                        Schedule,
                        new []
                        {
                            "schedule"
                        }
                    },
                    {
                        ShiftPatterns,
                        new []
                        {
                            "shiftpattern"
                        }
                    },
                    {
                        RealTime,
                        new []
                        {
                            "realtime"
                        }
                    },
                    {
                        Forecast,
                        new []
                        {
                            "forecast/",
                            "forecastcontroller"
                        }
                    },
                    {
                        BuildUtilities,
                        new[]
                        {
                            "buildutilities/"
                        }
                    },
                    {
                        Database,
                        new []
                        {
                            "database/",
                            "databasetestscripts/",
                            "nostradamus/nostradamus.databasemigration/"
                        }
                    },
                    {
                        MonitoringApp,
                        new []
                        {
                            "serviceconsole.monitoring/"
                        }
                    },
                    {
                        ThirdParty,
                        new []
                        {
                            "thirdparty/"
                        }
                    },
                    {
                        Tools,
                        new []
                        {
                            "tools/"
                        }
                    },
                    {
                        DevUi,
                        new []
                        {
                            "nostradamus/nostradamus.devui/",
                            "nostradamus/nostradamus.devui.test/"
                        }
                    },
                    {
                        Model,
                        new []
                        {
                            "Nostradamus/Nostradamus.Model/",
                            "Nostradamus/Nostradamus.Model.Test/"
                        }
                    },
                    {
                        ServiceConsoleUi,
                        new []
                        {
                            "Nostradamus/ServiceConsole.Ui/",
                        }
                    },
                    {
                        ProjectFiles,
                        new []{".csproj"}
                    }
                };

            IgnoredPaths = new[]
            {
               ".sln" 
            };
        }
    }
}