using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Rex
    {
        public partial class Data
        {
            public virtual void Save(Stream stream)
            {
                throw new NotImplementedException();
            }

            public static Data Load(Stream stream)
            {
                throw new NotImplementedException();
            }
        }
    }
}
