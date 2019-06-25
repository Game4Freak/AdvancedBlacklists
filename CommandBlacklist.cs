using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Core;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using UnityEngine;

namespace Game4Freak.AdvancedBlacklists
{
    public class CommandBlacklist : IRocketCommand
    {
        public string Name
        {
            get { return "blacklist"; }
        }
        public string Help
        {
            get { return "administrate blacklists"; }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public string Syntax
        {
            get { return "wiki"; }
        }

        public List<string> Aliases
        {
            get { return new List<string> { "ablacklist" }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "advancedblacklists" };
            }
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length < 1 || command.Length > 4)
            {
                UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist " + Syntax), Color.red);
                return;
            }
            if (command[0].ToLower() == "add")
            {
                if (command.Length == 3)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        if (AdvancedBlacklists.Instance.getEquipBlacklistByName(command[2].ToLower()) != null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.equipBlacklists.Add(new Blacklist(command[2].ToLower()));
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (AdvancedBlacklists.Instance.getPickupBlacklistByName(command[2].ToLower()) != null)
                        { 
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.pickupBlacklists.Add(new Blacklist(command[2].ToLower()));
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        if (AdvancedBlacklists.Instance.getVehicleBlacklistByName(command[2].ToLower()) != null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.vehicleBlacklists.Add(new Blacklist(command[2].ToLower()));
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        if (AdvancedBlacklists.Instance.getDriveBlacklistByName(command[2].ToLower()) != null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.driveBlacklists.Add(new Blacklist(command[2].ToLower()));
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist add equip|pickup|vehicle|drive <blacklist>"), Color.red);
                        return;
                    }
                }
                else if (command.Length > 3)
                {
                    ushort ID;
                    if (!ushort.TryParse(command[3], out ID))
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalidnumber", command[3]), Color.red);
                        return;
                    }
                    if (command[1].ToLower() == "equip")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getEquipBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        currentBlacklist.itemIDs.Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getPickupBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        currentBlacklist.itemIDs.Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getVehicleBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        currentBlacklist.itemIDs.Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getDriveBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        currentBlacklist.itemIDs.Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist add equip|pickup|vehicle|drive <blacklist> <itemID>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist add equip|pickup|vehicle|drive <blacklist> <itemID>"), Color.red);
                    return;
                }
            }
            else if (command[0].ToLower() == "remove")
            {
                if (command.Length == 3)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getEquipBlacklistByName(command[2]);
                        if (currentBlacklist != null)
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.equipBlacklists.Remove(currentBlacklist);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getPickupBlacklistByName(command[2]);
                        if (currentBlacklist != null)
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.pickupBlacklists.Remove(currentBlacklist);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getVehicleBlacklistByName(command[2]);
                        if (currentBlacklist != null)
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.vehicleBlacklists.Remove(currentBlacklist);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getDriveBlacklistByName(command[2]);
                        if (currentBlacklist != null)
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.driveBlacklists.Remove(currentBlacklist);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist remove equip|pickup|vehicle|drive <blacklist>"), Color.red);
                        return;
                    }
                }
                else if (command.Length > 3)
                {
                    ushort ID;
                    if (!ushort.TryParse(command[3], out ID))
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalidnumber", command[3]), Color.red);
                        return;
                    }
                    if (command[1].ToLower() == "equip")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getEquipBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            currentBlacklist.itemIDs.Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getPickupBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            currentBlacklist.itemIDs.Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getVehicleBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            currentBlacklist.itemIDs.Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getDriveBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        if (currentBlacklist.itemIDs.Contains(ID))
                        {
                            currentBlacklist.itemIDs.Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist remove equip|pickup|vehicle|drive <blacklist> <itemID>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist remove equip|pickup|vehicle|drive <blacklist> <itemID>"), Color.red);
                    return;
                }
            }
            else if (command[0].ToLower() == "list")
            {
                if (command.Length == 2)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        string message = "Equip blocklists: ";
                        foreach (var blacklist in AdvancedBlacklists.Instance.Configuration.Instance.equipBlacklists)
                        {
                            message = message + blacklist.name + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        string message = "Pickup blocklists: ";
                        foreach (var blacklist in AdvancedBlacklists.Instance.Configuration.Instance.pickupBlacklists)
                        {
                            message = message + blacklist.name + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        string message = "Vehicle blocklists: ";
                        foreach (var blacklist in AdvancedBlacklists.Instance.Configuration.Instance.vehicleBlacklists)
                        {
                            message = message + blacklist.name + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        string message = "Drive blocklists: ";
                        foreach (var blacklist in AdvancedBlacklists.Instance.Configuration.Instance.driveBlacklists)
                        {
                            message = message + blacklist.name + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist list equip|pickup|vehicle|drive"), Color.red);
                        return;
                    }
                }
                else if (command.Length == 3)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getEquipBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in currentBlacklist.itemIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getPickupBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in currentBlacklist.itemIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "vehicle")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getVehicleBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in currentBlacklist.itemIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "drive")
                    {
                        Blacklist currentBlacklist = AdvancedBlacklists.Instance.getDriveBlacklistByName(command[2]);
                        if (currentBlacklist == null)
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in currentBlacklist.itemIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist list equip|pickup|vehicle|drive <blacklist>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/help or /blacklist list equip|pickup|vehicle|drive <blacklist>"), Color.red);
                    return;
                }
            }
            else if (command[0].ToLower() == "wiki")
            {
                player.Player.channel.send("askBrowserRequest", player.CSteamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, AdvancedBlacklists.Instance.Translations.Instance.Translate("wiki"), "https://github.com/Game4Freak/AdvancedBlacklist/wiki");
            }
            else if (command[0].ToLower() == "help")
            {
                UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("help1", "AdvancedBlacklists-Plugin"), UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("help2", "AdvancedBlacklists", "/blacklist wiki"), UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(1) /blacklist help", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(2) /blacklist wiki", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(3) /blacklist add equip|pickup|vehicle|drive <blacklist> <itemID>", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(4) /blacklist remove equip|pickup|vehicle|drive <blacklist> <itemID>", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(5) /blacklist list equip|pickup|vehicle|drive <blacklist>", UnityEngine.Color.cyan);
            }
        }
    }
}
