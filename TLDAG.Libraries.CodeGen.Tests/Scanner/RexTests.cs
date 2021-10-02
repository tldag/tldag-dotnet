using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TLDAG.Libraries.CodeGen.Scanner
{
    [TestClass]
    public class RexTests
    {
        [TestMethod]
        public void TreeNames()
        {
            Regex rex = RexTreeBuilder.NameRex;

            Assert.IsTrue(rex.IsMatch("PERCS_2"));
        }

        [TestMethod]
        public void Figure_3_41()
        {
            RexTree tree = RexTreeBuilder.Create("FIGURE_3_41")
                .AddSymbol('a').AddSymbol('b').AddChoose()
                .AddKleene()
                .AddSymbol('a').AddConcat()
                .AddSymbol('b').AddConcat()
                .AddSymbol('b').AddConcat()
                .Build();

            RexForest forest = RexForestBuilder.Create().AddTree(tree).Build();

            foreach (RexNode node in tree.Nodes)
            {
                string content = "";

                if (node is RexNode.LeafNode) { content += node.Id + ": "; }
                else if (node is RexNode.Choose) { content += "|: "; }
                else if (node is RexNode.Concat) { content += ".: "; }
                else if (node is RexNode.Kleene) { content += "*: "; }

                content += node.Nullable + ", {" + node.Firstpos + "}, {" + node.Lastpos + "}";

                if (node is RexNode.LeafNode leaf2)
                {
                    content += ", {" + leaf2.Followpos + "}";
                }

                Debug.WriteLine(content);
            }

            string symbols = string.Join(", ", forest.Symbols.Select(c => $"'{c}'"));

            Debug.WriteLine(symbols);
        }
    }
}
