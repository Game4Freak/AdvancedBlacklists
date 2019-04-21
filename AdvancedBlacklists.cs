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
        public const string VERSION = "0.1.0.0";
        int frame;

        protected override void Load()
        {
            Instance = this;
            frame = 0;
            Logger.Log("AdvancedBlacklist v" + VERSION);

            ItemManager.onTakeItemRequested += onItemPickup;
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
            {"wiki", "Need help? Take a look at the AdvancedZones wiki" }

        };

        private void Update()
        {
            frame++;
            if (frame % 10 != 0) return;

            foreach (var splayer in Provider.clients)
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(splayer);
                // Player Equip
                if (player.Player.equipment.isSelected)
                {
                    onItemEquip(player, player.Player.equipment);
                }
            }
        }

        private void onItemPickup(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            if (uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower() + ".item." + itemData.item.id))
            {
                return;
            }
            foreach (var blacklistname in Configuration.Instance.pickupBlocklistNames)
            {
                if (!uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower() + ".blacklist." + blacklistname.ToLower()) && Configuration.Instance.pickupBlocklist.ElementAt(Configuration.Instance.pickupBlocklistNames.IndexOf(blacklistname)).Contains(itemData.item.id))
                {
                    UnturnedChat.Say(uPlayer, Instance.Translations.Instance.Translate("noAllow"), Color.red);
                    shouldAllow = false;
                }
            }
        }

        private void onItemEquip(UnturnedPlayer uPlayer, PlayerEquipment equipment)
        {
            if (uPlayer.HasPermission(Configuration.Instance.equipIgnoreBlacklistPermission.ToLower() + ".item." + equipment.itemID))
            {
                return;
            }
            foreach (var blacklistname in Configuration.Instance.equipBlocklistNames)
            {
                if (!uPlayer.HasPermission(Configuration.Instance.pickupIgnoreBlacklistPermission.ToLower() + ".blacklist." + blacklistname.ToLower()) && Configuration.Instance.equipBlocklist.ElementAt(Configuration.Instance.equipBlocklistNames.IndexOf(blacklistname)).Contains(equipment.itemID))
                {
                    UnturnedChat.Say(uPlayer, Instance.Translations.Instance.Translate("noAllow"), Color.red);
                    equipment.dequip();
                }
            }
        }
    }
}