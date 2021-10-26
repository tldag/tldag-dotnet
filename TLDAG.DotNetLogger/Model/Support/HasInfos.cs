using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model.Support
{
    [Serializable]
    public class HasInfos
    {
        [XmlElement("infos")]
        public Infos? Infos { get; set; } = null;

        public void AddInfo(string info) { (Infos ??= new()).Add(info); }
    }
}
