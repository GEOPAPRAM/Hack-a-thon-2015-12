using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Tests
{
    public class TestCaseFactoryForRevisionsModule
    {
        public static string[] ApplicationCases
        {
            get
            {
                return new[]
                {
                    WFM.Identifier,
                    CallCentre.Identifier,
                    IVR.Identifier
                };
            }
        }
    }

    public class TestCaseFactoryForReleaseModule
    {
        public static string[] ApplicationCases
        {
            get
            {
                return new[]
                {
                    WFM.Identifier,
                    CallCentre.Identifier,
                };
            }
        }
    }
}
