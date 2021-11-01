using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TLDAG.Core.Xml
{
    public static class XmlExtensions
    {
        public static XmlDocument LoadXmlDocument(this FileInfo file)
            { XmlDocument document = new(); document.Load(file.FullName); return document; }

        public static IEnumerable<XmlNode> GetChildNodes(this XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;

            for (int i = 0, n = children.Count; i < n; ++i)
            {
                if (children[i] is XmlNode child) yield return child;
            }
        }

        public static IEnumerable<XmlElement> GetChildElements(this XmlNode node)
        {
            foreach (XmlNode child in GetChildNodes(node))
            {
                if (child is XmlElement element) yield return element;
            }
        }
    }
}
