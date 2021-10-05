using System;
using System.IO;

namespace TLDAG.Libraries.Core.CodeGen
{
    public partial class ParseData : RexData
    {
        public ParseData(RexData rex) : base(rex) { }

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
