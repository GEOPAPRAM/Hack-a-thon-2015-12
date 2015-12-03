using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class Project
    {
        private readonly ISourceControl _sourceControl;

        public static bool IsProjectDefintionLine(string line)
        {
            return line.Contains(".csproj")
                   || line.Contains(".vbproj");
        }

        public Project(Uri solutionPath, string projectDefinition, ISourceControl sourceControl)
        {
            _svnRootUrl = ConfigurationManager.AppSettings["SvnRootUrl"];

            _sourceControl = sourceControl;

            // projectDefinition format is Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "ServiceControlSuite.UI", "NewVoiceMedia.ServiceControlSuite\UI\ServiceControlSuite.UI.csproj", "{58379533-98CE-4E3A-A2F5-904DA9E584A8}"

            var segments = projectDefinition.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);

            var name = segments[3];
            Name = name;

            var relativePath = segments[5];

            var solutionFileName = solutionPath.Segments[solutionPath.Segments.Length - 1];
            var rootedProjectPath = solutionPath.OriginalString.Replace(solutionFileName, relativePath);

            var projectPathSegments = rootedProjectPath.Split(new [] {Path.DirectorySeparatorChar},
                                                          StringSplitOptions.RemoveEmptyEntries);
            var projectFolder = rootedProjectPath.Replace(projectPathSegments[projectPathSegments.Length - 1], String.Empty);

            FolderPath = new Uri(projectFolder);
        }

        public string Name { get; private set; }
        public Uri FolderPath { get; private set; }
        public long CommitsOverTime { get; private set; }
        public readonly static double HistoryDays = 180.0;
        private readonly string _svnRootUrl;

        public void GetCommitHistory()
        {
            try
            {
                var changes = _sourceControl.GetChangesSince(FolderPath.ToString(), DateTime.Today.AddDays(-HistoryDays));
                CommitsOverTime = changes.ToList().LongCount();
            }
            catch (Exception)
            {
                CommitsOverTime = -1; 
            }
        }

        public override string ToString()
        {
            var shortenedPath =
                FolderPath.ToString()
                    .Replace(string.Format("{0}/nvm/components/applications/call_centre/trunk/source/", _svnRootUrl), "../");

            return String.Format("{0}, {1}", Name, shortenedPath);
        }
    }
}