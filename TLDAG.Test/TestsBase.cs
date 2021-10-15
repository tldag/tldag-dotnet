using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Test
{
    public class TestsBase
    {
        private string? solutionName;
        protected string SolutionName => solutionName ??= GetSolutionName();

        private string GetSolutionName()
        {
            throw NotYetImplemented();
        }
    }
}
