using Nancy;
using NewVoiceMedia.Tools.ReleaseInspection.Model;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class SourceControlModule : NancyModule
    {
        public SourceControlModule(IKnownSolutions knownSolutions)
            : base("/source")
        {
            Get["/{solution}"] = parameters =>
                {
                    var solutionName = parameters.solution;

                    if (!knownSolutions.Contains(solutionName))
                    {
                        return new NotFoundResponse();
                    }

                    var solution = knownSolutions.Get(solutionName);

                    solution.LoadFromSourceControl();

                    return View[solution];
                };
        }
    }
}