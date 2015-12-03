using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class Solution : ISolution
    {
        private readonly ISourceControl _sourceControl;
        private readonly Uri _solutionPath;

        public Solution(string name, Uri solutionPath, ISourceControl sourceControl)
        {
            Name = name;
            _solutionPath = solutionPath;
            _sourceControl = sourceControl;
        }

        public string Name { get; private set; }

        public IEnumerable<Project> Projects { get; private set; }

        public void LoadFromSourceControl()
        {
            if (Projects != null)
            {
                return;
            }

            var solutionContents = _sourceControl.GetFileContents(_solutionPath);

            var projects = (from line in solutionContents where Project.IsProjectDefintionLine(line) select ParseProject(line)).ToList();

            Projects = projects.OrderBy(p => p.ToString());

            Parallel.ForEach(Projects, GetProjectCommitHistory);
        }

        private Project ParseProject(string line)
        {
            var project = new Project(_solutionPath, line, _sourceControl);

            return project;
        }

        private static void GetProjectCommitHistory(Project project)
        {
            project.GetCommitHistory();
        }
    }
}