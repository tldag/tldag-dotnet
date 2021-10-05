using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public partial class RexData
    {
        public StringSet Names { get; }

        public RexData(StringSet names) { Names = names; }
    }

    public class RexState
    {
    }

    public class RexCompiler
    {
        public RexCompiler(RexNode root)
        {
        }

        public RexData Compile()
        {
            CreateStates();
            CreateTransitions();
            CreateAccepting();

            throw new NotImplementedException();
        }

        private void CreateStates()
        {
            throw new NotImplementedException();
        }

        private void CreateTransitions()
        {
            throw new NotImplementedException();
        }

        private void CreateAccepting()
        {
            throw new NotImplementedException();
        }

        public static RexCompiler Create(RexNode root) => new(root);
        public static RexData Compile(RexNode root) => Create(root).Compile();
    }
}
