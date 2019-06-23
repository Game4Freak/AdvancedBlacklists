using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Game4Freak.AdvancedBlacklists
{
    public class Blacklist
    {
        [XmlAttribute("name")]
        public string name;
        [XmlArrayItem("itemID")]
        public List<ushort> itemIDs;

        public Blacklist()
        {
            name = "";
            itemIDs = new List<ushort>();
        }

        public Blacklist(string name)
        {
            this.name = name;
            itemIDs = new List<ushort>();
        }
    }
}
