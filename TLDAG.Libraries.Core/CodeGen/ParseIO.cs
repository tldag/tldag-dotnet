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
        public partial class ParseData : RexData
        {
            public ParseData(RexData rex) : base(rex.Names) { }

            public override void Save(Stream stream)
            {
                base.Save(stream);

                throw new NotImplementedException();
            }

            public static new ParseData Load(Stream stream)
            {
                RexData rex = RexData.Load(stream);

                throw new NotImplementedException();
            }
        }
    }
}
