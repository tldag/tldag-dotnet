﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Passes
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("pass")]
        public List<Pass> Entries { get; set; } = new();

        public bool Contains(int id) => Entries.Where(p => p.Id == id).Any();
        public Pass Get(int id) => Entries.Where(p => p.Id == id).First();

        public void Add(int id)
        {
            if (!Contains(id) && id >= 0)
            {
                Entries.Add(new(id));
                Entries.Sort();
            }
        }
    }
}
