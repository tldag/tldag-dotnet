using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core.Reflection
{
    public class Executable
    {
        public readonly string Path;

        public Executable(string path)
        {
            Path = path;
        }
    }
}
