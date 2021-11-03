using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace TLDAG.Core.Xml
{
    public static class XmlExtensions
    {
        public static XmlDocument LoadXmlDocument(this FileInfo file)
            { XmlDocument document = new(); document.Load(file.FullName); return document; }

        public static IEnumerable<XmlNode> ChildNodes(this XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;

            for (int i = 0, n = children.Count; i < n; ++i)
            {
                if (children[i] is XmlNode child) yield return child;
            }
        }

        public static IEnumerable<XmlElement> ChildElements(this XmlNode node)
        {
            foreach (XmlNode child in ChildNodes(node))
            {
                if (child is XmlElement element) yield return element;
            }
        }

        public static IEnumerable<XmlElement> AllElements(this XmlDocument document)
            => AllElements(document.DocumentElement);

        private static IEnumerable<XmlElement> AllElements(XmlElement? element)
            => element is null ? Array.Empty<XmlElement>() : element.ChildElements().SelectMany(AllElements).Prepend(element);
    }
}
