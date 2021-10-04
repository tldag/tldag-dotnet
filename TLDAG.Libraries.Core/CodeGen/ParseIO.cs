using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Parse
    {
        public partial class Data : Rex.Data
        {
            public override void Save(Stream stream)
            {
                base.Save(stream);

                throw new NotImplementedException();
            }

            public static new Data Load(Stream stream)
            {
                Rex.Data rexData = Rex.Data.Load(stream);

                throw new NotImplementedException();
            }
        }
    }
}
