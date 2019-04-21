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
                        if (AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Add(command[2].ToLower());
                        AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.Add(new List<int>());
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Contains(command[2].ToLower()))
                        { 
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("exists", command[2].ToLower()), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Add(command[2].ToLower());
                        AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.Add(new List<int>());
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("addblacklist", command[2]), Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist add equip|pickup <blacklist>"), Color.red);
                        return;
                    }
                }
                else if (command.Length > 3)
                {
                    int ID;
                    if (!int.TryParse(command[3], out ID))
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalidnumber", command[3]), Color.red);
                        return;
                    }
                    if (command[1].ToLower() == "equip")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower()));
                        if (blocklistIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower())).Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower()));
                        if (blocklistIDs.Contains(ID))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("has", command[2].ToLower(), ID), Color.red);
                            return;
                        }
                        AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower())).Add(ID);
                        AdvancedBlacklists.Instance.Configuration.Save();
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("additem", ID, command[2].ToLower()), Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist add equip|pickup <blacklist> <itemID>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist add equip|pickup <blacklist> <itemID>"), Color.red);
                    return;
                }
            }
            else if (command[0].ToLower() == "remove")
            {
                if (command.Length == 3)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        if (AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Contains(command[2].ToLower()))
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.RemoveAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower()));
                            AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Remove(command[2].ToLower());
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Contains(command[2].ToLower()))
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.RemoveAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower()));
                            AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Remove(command[2].ToLower());
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeblacklist", command[2]), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist remove equip|pickup <blacklist>"), Color.red);
                        return;
                    }
                }
                else if (command.Length > 3)
                {
                    int ID;
                    if (!int.TryParse(command[3], out ID))
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalidnumber", command[3]), Color.red);
                        return;
                    }
                    if (command[1].ToLower() == "equip")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower()));
                        if (blocklistIDs.Contains(ID))
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower())).Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower()));
                        if (blocklistIDs.Contains(ID))
                        {
                            AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower())).Remove(ID);
                            AdvancedBlacklists.Instance.Configuration.Save();
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("removeitem", ID, command[2].ToLower()), Color.cyan);
                            return;
                        }
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("nohas", command[2].ToLower(), ID), Color.red);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist remove equip|pickup <blacklist> <itemID>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist remove equip|pickup <blacklist> <itemID>"), Color.red);
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
                        foreach (var blacklistname in AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames)
                        {
                            message = message + blacklistname + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        string message = "Pickup blocklists: ";
                        foreach (var blacklistname in AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames)
                        {
                            message = message + blacklistname + ", ";
                        }
                        UnturnedChat.Say(caller, message, Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist list equip|pickup"), Color.red);
                        return;
                    }
                }
                else if (command.Length == 3)
                {
                    if (command[1].ToLower() == "equip")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.equipBlocklistNames.IndexOf(command[2].ToLower()));
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in blocklistIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else if (command[1].ToLower() == "pickup")
                    {
                        if (!AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.Contains(command[2].ToLower()))
                        {
                            UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("noexists", command[2].ToLower()), Color.red);
                            return;
                        }
                        List<int> blocklistIDs = AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklist.ElementAt(AdvancedBlacklists.Instance.Configuration.Instance.pickupBlocklistNames.IndexOf(command[2].ToLower()));
                        string message = "Blocklist: " + command[2].ToLower() + " { ";
                        foreach (var id in blocklistIDs)
                        {
                            message = message + id + ", ";
                        }
                        UnturnedChat.Say(caller, message + "}", Color.cyan);
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist list equip|pickup <blacklist>"), Color.red);
                        return;
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, AdvancedBlacklists.Instance.Translations.Instance.Translate("invalid", "/ help or / blacklist list equip|pickup <blacklist>"), Color.red);
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
                UnturnedChat.Say(caller, "(3) /blacklist add equip|pickup <blacklist> <itemID>", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(4) /blacklist remove equip|pickup <blacklist> <itemID>", UnityEngine.Color.cyan);
                UnturnedChat.Say(caller, "(5) /blacklist list equip|pickup <blacklist>", UnityEngine.Color.cyan);
            }
        }
    }
}
