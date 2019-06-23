using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Game4Freak.AdvancedBlacklists
{
    public class AdvancedBlacklists : RocketPlugin<AdvancedBlacklistsConfiguration>
    {
        public static AdvancedBlacklists Instance;
        public const string VERSION = "1.1.0.0";
        int frame;

        protected override void Load()
        {
            Instance = this;
            frame = 0;
            Logger.Log("AdvancedBlacklist v" + VERSION);

            UpdateConfig();

            ItemManager.onTakeItemRequested += onItemPickup;
        }

        public void UpdateConfig()
        {
            if (compareVersion(VERSION, Configuration.Instance.version))
            {
                Logger.Log("Updating config");
                for (int i = 0; i < Configuration.Instance.equipBlocklistNames.Count; i++)
                {
                    Blacklist b = new Blacklist(Configuration.Instance.equipBlocklistNames[i]);
                    if (Configuration.Instance.equipBlocklist[i] != null)
                    {
                        foreach (var id in Configuration.Instance.equipBlocklist[i])
                        {
                            b.itemIDs.Add((ushort)id);
                        }
                    }
                    Configuration.Instance.equipBlacklists.Add(b);
                }
                for (int i = 0; i < Configuration.Instance.pickupBlocklistNames.Count; i++)
                {
                    Blacklist b = new Blacklist(Configuration.Instance.pickupBlocklistNames[i]);
                    if (Configuration.Instance.pickupBlocklist[i] != null)
                    {
                        foreach (var id in Configuration.Instance.pickupBlocklist[i])
                        {
                            b.itemIDs.Add((ushort)id);
                        }
                    }
                    Configuration.Instance.pickupBlacklists.Add(b);
                }
                Configuration.Instance.pickupBlocklistNames.Clear();
                Configuration.Instance.pickupBlocklist.Clear();
                Configuration.Instance.equipBlocklistNames.Clear();
                Configuration.Instance.equipBlocklist.Clear();
            }
            Configuration.Instance.version = VERSION;
            Configuration.Save();
        }

        protected override void Unload()
        {
            ItemManager.onTakeItemRequested -= onItemPickup;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"noAllow", "This item is blacklisted!" },
            {"invalid", "Invalid! Try {0}" },
            {"exists", "The blacklist: {0} already exists" },
            {"addblacklist", "Added the blacklist: {0}" },
            {"invalidnumber", "Invalid! {0} is not a number" },
            {"has", "The blacklist: {0} already has the ID: {1}" },
            {"additem", "Added the ID: {0} to the blacklist: {1}" },
            {"removeblacklist", "Removed the blacklist: {0}" },
            {"noexists", "The blacklist: {0} does not exist" },
            {"nohas", "The blacklist: {0} does not have the ID: {1}" },
            {"removeitem", "Removed the ID: {0} from the blacklist: {1}" },
            {"help1", "These are all commands of the {0}" },
            {"help2", "Check out the {0} wiki for more information with {1}" },
            {"wiki", "Need help? Take a look at the AdvancedBlacklist wiki" }

        };

        private void Update()
        {
            frame++;
            if (frame % 10 != 0) return;

            foreach (var splayer in Provider.clients)
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(splayer);
                if (player.Player.equipment.isSelected)
                {
                    onItemEquip(player, player.Player.equipment);
                }
                if (player.IsInVehicle)
                {
                    onVehicleEnter(player, player.CurrentVehicle);
                }
            }
        }

        private void onItemPickup(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            if (uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower()) || uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower() + ".item." + itemData.item.id))
            {
                return;
            }
            foreach (var blacklist in Configuration.Instance.pickupBlacklists)
            {
                if (!uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower() + ".blacklist." + blacklist.name.ToLower()) && blacklist.itemIDs.Contains(itemData.item.id))
                {
                    UnturnedChat.Say(uPlayer, Instance.Translations.Instance.Translate("noAllow"), Color.red);
                    shouldAllow = false;
                }
            }
        }

        private void onItemEquip(UnturnedPlayer uPlayer, PlayerEquipment equipment)
        {
            if (uPlayer.HasPermission(Configuration.Instance.equipIgnoreBlacklistPermission.ToLower()) || uPlayer.HasPermission(Configuration.Instance.equipIgnoreBlacklistPermission.ToLower() + ".item." + equipment.itemID))
            {
                return;
            }
            foreach (var blacklist in Configuration.Instance.equipBlacklists)
            {
                if (!uPlayer.HasPermission(Configuration.Instance.equipIgnoreBlacklistPermission.ToLower() + ".blacklist." + blacklist.name.ToLower()) && blacklist.itemIDs.Contains(equipment.itemID))
                {
                    UnturnedChat.Say(uPlayer, Instance.Translations.Instance.Translate("noAllow"), Color.red);
                    equipment.dequip();
                }
            }
        }

        private void onVehicleEnter(UnturnedPlayer player, InteractableVehicle vehicle)
        {
            if (player.HasPermission(Configuration.Instance.vehicleIgnoreBlacklistPermission.ToLower()) || player.HasPermission(Configuration.Instance.vehicleIgnoreBlacklistPermission.ToLower() + ".item." + vehicle.id))
            {
                return;
            }
            foreach (var blacklist in Configuration.Instance.vehicleBlacklists)
            {
                if (!player.HasPermission(Configuration.Instance.vehicleIgnoreBlacklistPermission.ToLower() + ".blacklist." + blacklist.name.ToLower()) && blacklist.itemIDs.Contains(vehicle.id))
                {
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("noAllow"), Color.red);
                    VehicleManager.forceRemovePlayer(vehicle, player.CSteamID);
                }
            }
        }

        private bool compareVersion(string version1, string version2)
        {
            return int.Parse(version1.Replace(".", "")) > int.Parse(version2.Replace(".", ""));
        }

        public Blacklist getEquipBlacklistByName(string name)
        {
            foreach (var blacklist in Configuration.Instance.equipBlacklists)
            {
                if (blacklist.name.ToLower() == name.ToLower())
                {
                    return blacklist;
                }
            }
            return null;
        }

        public Blacklist getPickupBlacklistByName(string name)
        {
            foreach (var blacklist in Configuration.Instance.pickupBlacklists)
            {
                if (blacklist.name.ToLower() == name.ToLower())
                {
                    return blacklist;
                }
            }
            return null;
        }

        public Blacklist getVehicleBlacklistByName(string name)
        {
            foreach (var blacklist in Configuration.Instance.vehicleBlacklists)
            {
                if (blacklist.name.ToLower() == name.ToLower())
                {
                    return blacklist;
                }
            }
            return null;
        }
    }
}