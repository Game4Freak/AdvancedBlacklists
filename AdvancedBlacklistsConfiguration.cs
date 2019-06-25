using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Game4Freak.AdvancedBlacklists
{
    public class AdvancedBlacklistsConfiguration : IRocketPluginConfiguration
    {
        public string version;

        // permissons
        public string equipIgnoreBlacklistPermission;
        public string pickupIgnoreBlacklistPermission;
        public string vehicleIgnoreBlacklistPermission;
        public string driveIgnoreBlacklistPermission;

        // new blacklists
        [XmlArrayItem(ElementName = "equipBlacklist")]
        public List<Blacklist> equipBlacklists;
        [XmlArrayItem(ElementName = "pickupBlacklist")]
        public List<Blacklist> pickupBlacklists;
        [XmlArrayItem(ElementName = "vehicleBlacklist")]
        public List<Blacklist> vehicleBlacklists;
        [XmlArrayItem(ElementName = "driveBlacklist")]
        public List<Blacklist> driveBlacklists;

        // blocklists
        public List<string> equipBlocklistNames;
        public List<List<int>> equipBlocklist;
        public List<string> pickupBlocklistNames;
        public List<List<int>> pickupBlocklist;

        public void LoadDefaults()
        {
            version = "0.0.0.0";

            equipIgnoreBlacklistPermission = "advancedblacklists.ignore.equip";
            pickupIgnoreBlacklistPermission = "advancedblacklists.ignore.pickup";
            vehicleIgnoreBlacklistPermission = "advancedblacklists.ignore.vehicle";
            driveIgnoreBlacklistPermission = "advancedblacklists.ignore.drive";

            equipBlacklists = new List<Blacklist>();
            pickupBlacklists = new List<Blacklist>();
            vehicleBlacklists = new List<Blacklist>();
            driveBlacklists = new List<Blacklist>();

            equipBlocklistNames = new List<string>();
            equipBlocklist = new List<List<int>>();
            pickupBlocklistNames = new List<string>();
            pickupBlocklist = new List<List<int>>();
        }
    }
}
