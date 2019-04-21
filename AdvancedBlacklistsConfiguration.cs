using Rocket.API;
using System.Collections.Generic;

namespace Game4Freak.AdvancedBlacklists
{
    public class AdvancedBlacklistsConfiguration : IRocketPluginConfiguration
    {
        // permissons
        public string equipIgnoreBlacklistPermission;
        public string pickupIgnoreBlacklistPermission;

        // blocklists
        public List<string> equipBlocklistNames;
        public List<List<int>> equipBlocklist;
        public List<string> pickupBlocklistNames;
        public List<List<int>> pickupBlocklist;

        public void LoadDefaults()
        {
            equipIgnoreBlacklistPermission = "advancedblacklists.ignore.equip";
            pickupIgnoreBlacklistPermission = "advancedblacklists.ignore.pickup";

            equipBlocklistNames = new List<string>();
            equipBlocklist = new List<List<int>>();
            pickupBlocklistNames = new List<string>();
            pickupBlocklist = new List<List<int>>();
        }
    }
}
