using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Build.Workspace.Internal
{
    internal class SolutionInfo
    {
        private List<ProjectInfo> projects;

        public IReadOnlyList<ProjectInfo> ProjectsInOrder => projects;

        public SolutionInfo(IEnumerable<ProjectInfo> projects)
        {
            this.projects = new(projects);
        }
    }
}
