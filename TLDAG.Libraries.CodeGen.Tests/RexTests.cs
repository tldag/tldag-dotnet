using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TLDAG.Libraries.CodeGen.Tests
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
            RexForest forest = RexForestBuilder.Create().AddTree(RexTrees.Figure_3_41()).Build();

            foreach (RexNode node in forest.Nodes)
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
        }
    }
}
