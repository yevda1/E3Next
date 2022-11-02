﻿using E3Core.Data;
using E3Core.Settings;
using E3Core.Settings.FeatureSettings;
using E3Core.Utility;
using MonoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E3Core.Processors
{
    /// <summary>
    /// A catch all for ancillary commands and functions.
    /// </summary>
    public static class Basics
    {
        public static SavedGroupDataFile SavedGroupData = new SavedGroupDataFile();
        public static Logging Log = E3.Log;
        private static IMQ _mq = E3.Mq;
        private static ISpawns _spawns = E3.Spawns;
        public static bool IsPaused = false;
        public static List<int> GroupMembers = new List<int>();
        private static long _nextGroupCheck = 0;
        private static long _nextGroupCheckInterval = 1000;
        private static long _nextResourceCheck = 0;
        private static long _nextResourceCheckInterval = 1000;
        private static long _nextAutoMedCheck = 0;
        private static long _nextAutoMedCheckInterval = 1000;
        private static long _nextFoodCheck = 0;
        private static long _nextFoodCheckInterval = 1000;
        private static long _nextCursorCheck = 0;
        private static long _nextCursorCheckInterval = 1000;
        private static DateTime? _cursorOccupiedSince;
        private static TimeSpan _cursorOccupiedTime;
        private static TimeSpan _cursorOccupiedThreshold = new TimeSpan(0, 0, 0, 30);

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [SubSystemInit]
        public static void Init()
        {
            RegisterEvents();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        public static void RegisterEvents()
        {
            EventProcessor.RegisterEvent("InviteToGroup", "(.+) invites you to join a group.", (x) =>
            {
                _mq.Cmd("/invite");
                _mq.Delay(300);
            });
            EventProcessor.RegisterEvent("InviteToRaid", "(.+) invites you to join a raid.", (x) =>
            {
                _mq.Delay(500);
                _mq.Cmd("/raidaccept");
            });

            EventProcessor.RegisterEvent("InviteToDZ", "(.+) tells you, 'dzadd'", (x) =>
            {
                if (x.match.Groups.Count > 1)
                {
                    _mq.Cmd($"/dzadd {x.match.Groups[1].Value}");
                }
            });

            EventProcessor.RegisterEvent("Zoned", @"You have entered (.+)\.", (x) =>
            {
                //means we have zoned.
                _spawns.RefreshList();//make sure we get a new refresh of this zone.
                Loot.Reset();
                Movement.ResetKeepFollow();
                Assist.Reset();
                Pets.Reset();
            });
            EventProcessor.RegisterEvent("Summoned", @"You have been summoned!", (x) =>
            {
                _spawns.RefreshList();//make sure we get a new refresh of this zone.
                Loot.Reset();
                Movement.Reset();
                Assist.Reset();
            });
            //
            EventProcessor.RegisterEvent("InviteToDZ", "(.+) tells you, 'raidadd'", (x) =>
            {
                if (x.match.Groups.Count > 1)
                {
                    _mq.Cmd($"/raidinvite {x.match.Groups[1].Value}");
                }
            });

            EventProcessor.RegisterCommand("/dropinvis", (x) =>
            {
                E3.Bots.BroadcastCommandToGroup("/makemevisible");
                _mq.Cmd("/makemevisible");
            });
            EventProcessor.RegisterCommand("/debug", (x) =>
            {
                if (Logging._minLogLevelTolog == Logging.LogLevels.Error)
                {
                    Logging._minLogLevelTolog = Logging.LogLevels.Debug;
                    Logging._traceLogLevel = Logging.LogLevels.Trace;
                    MainProcessor._processDelay = 1000;
                }
                else
                {
                    Logging._minLogLevelTolog = Logging.LogLevels.Error;
                    Logging._traceLogLevel = Logging.LogLevels.None;
                    MainProcessor._processDelay = E3.ProcessDelay;
                }
            });

            EventProcessor.RegisterCommand("/pizza", (x) =>
            {
                if (E3.CurrentName == "Reek")
                {
                    System.Diagnostics.Process.Start("https://ordering.orders2.me/menu/pontillos-pizzeria-hudson-ridge");
                }
                else
                {
                    System.Diagnostics.Process.Start("https://www.dominos.com/en/restaurants?type=Delivery");
                }
            });

            EventProcessor.RegisterCommand("/yes", (x) =>
            {
                if (x.args.Count == 0)
                {
                    E3.Bots.BroadcastCommandToGroup("/yes all");
                }
                ClickYesNo(true);
            });
            EventProcessor.RegisterCommand("/no", (x) =>
            {
                if (x.args.Count == 0)
                {
                    E3.Bots.BroadcastCommandToGroup("/no all");
                }
                ClickYesNo(false);
            });

            EventProcessor.RegisterCommand("/bark", (x) =>
            {
                //rebuild the bark message, and do a /say
                if (x.args.Count > 0)
                {
                    int targetid = _mq.Query<int>("${Target.ID}");
                    if (targetid > 0)
                    {
                        Spawn s;
                        if (_spawns.TryByID(targetid, out s))
                        {
                            e3util.TryMoveToLoc(s.X, s.Y);
                            System.Text.StringBuilder sb = new StringBuilder();
                            bool first = true;
                            foreach (string arg in x.args)
                            {
                                if (!first) sb.Append(" ");
                                sb.Append(arg);
                                first = false;
                            }
                            string message = sb.ToString();
                            E3.Bots.BroadcastCommandToGroup($"/bark-send {targetid} \"{message}\"");
                            int currentZone = E3.ZoneID;

                            for (int i = 0; i < 5; i++)
                            {
                                _mq.Cmd($"/say {message}");
                                _mq.Delay(1500);
                                int tzone = _mq.Query<int>("${Zone.ID}");
                                if (tzone != currentZone)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            });

            EventProcessor.RegisterCommand("/bark-send", (x) =>
            {
                if (x.args.Count > 1)
                {
                    int targetid;
                    if (int.TryParse(x.args[0], out targetid))
                    {
                        if (targetid > 0)
                        {
                            Spawn s;
                            if (_spawns.TryByID(targetid, out s))
                            {
                                Casting.TrueTarget(targetid);
                                _mq.Delay(100);
                                e3util.TryMoveToLoc(s.X, s.Y);

                                string message = x.args[1];
                                int currentZone = E3.ZoneID;
                                for (int i = 0; i < 5; i++)
                                {
                                    _mq.Cmd($"/say {message}");
                                    _mq.Delay(1000);
                                    int tzone = _mq.Query<int>("${Zone.ID}");
                                    if (tzone != currentZone)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            });

            EventProcessor.RegisterCommand("/evac", (x) =>
            {
                if (x.args.Count > 0)
                {
                    //someone told us to gate
                    Spell s;
                    if (!Spell._loadedSpellsByName.TryGetValue("Exodus", out s))
                    {
                        s = new Spell("Exodus");
                    }
                    if (Casting.CheckReady(s))
                    {
                        Casting.Cast(0, s);
                    }
                    else
                    {
                        //lets try and do evac spell?
                        string spellToCheck = string.Empty;
                        if (E3.CurrentClass == Class.Wizard)
                        {
                            spellToCheck = "Evacuate";
                        }
                        else if (E3.CurrentClass == Class.Druid)
                        {
                            spellToCheck = "Succor";
                        }

                        if (spellToCheck != string.Empty && _mq.Query<bool>($"${{Me.Book[{spellToCheck}]}}"))
                        {
                            if (!Spell._loadedSpellsByName.TryGetValue(spellToCheck, out s))
                            {
                                s = new Spell(spellToCheck);
                            }
                            if (Casting.CheckReady(s) && Casting.CheckMana(s))
                            {
                                Casting.Cast(0, s);
                            }
                        }
                    }
                }
                else
                {
                    E3.Bots.BroadcastCommandToGroup("/evac me");
                }
            });

            EventProcessor.RegisterCommand("/e3p", (x) =>
            {
                //swap them
                IsPaused = IsPaused ? false : true;
                if (IsPaused) _mq.Write("\arPAUSING E3!");
                if (!IsPaused) _mq.Write("\agRunning E3 again!");
            });

            EventProcessor.RegisterCommand("/savegroup", (x) =>
            {
                var args = x.args;
                if (args.Count == 0)
                    return;

                _mq.Write($"\agCreating new saved group by the name of {args[0]}");
                SavedGroupData.SaveData(args[0]);
                _mq.Write($"\agSuccessfully created {args[0]}");
            });

            EventProcessor.RegisterCommand("/group", (x) =>
            {
                var args = x.args;
                if (args.Count == 0)
                    return;

                var server = _mq.Query<string>("${MacroQuest.Server}");
                var groupKey = server + "_" + args[0];
                var savedGroups = SavedGroupData.GetData();
                if (!savedGroups.TryGetValue(groupKey, out var groupMembers))
                {
                    _mq.Write($"\arNo group with the name of {args[0]} found in Saved Groups.ini. Use /savegroup groupName to create one");
                }
                _mq.Cmd("/disband");
                _mq.Cmd("/raiddisband");
                E3.Bots.BroadcastCommand("/raiddisband");
                E3.Bots.BroadcastCommand("/disband");

                if (_mq.Query<int>("${Group}") > 0)
                {
                    _mq.Delay(2000);
                }

                foreach (var member in groupMembers)
                {
                    _mq.Cmd($"/invite {member}");
                }
            });
        }
        private static void ClickYesNo(bool YesClick)
        {
            string TypeToClick = "Yes";
            if (!YesClick)
            {
                TypeToClick = "No";
            }

            bool windowOpen = _mq.Query<bool>("${Window[ConfirmationDialogBox].Open}");
            if (windowOpen)
            {
                _mq.Cmd($"/notify ConfirmationDialogBox {TypeToClick}_Button leftmouseup");
            }
            else
            {
                windowOpen = _mq.Query<bool>("${Window[LargeDialogWindow].Open}");
                if (windowOpen)
                {
                    _mq.Cmd($"/notify LargeDialogWindow LDW_{TypeToClick}Button leftmouseup");
                }
            }
        }

        /// <summary>
        /// Refreshes the group member cache.
        /// </summary>
        public static void RefreshGroupMembers()
        {
            if (!e3util.ShouldCheck(ref _nextGroupCheck, _nextGroupCheckInterval)) return;

            int groupCount = _mq.Query<int>("${Group}");
            groupCount++;
            if (groupCount != GroupMembers.Count)
            {
                GroupMembers.Clear();
                //refresh group members.
                //see if any  of our members have it.
                for (int i = 0; i < groupCount; i++)
                {
                    int id = _mq.Query<int>($"${{Group.Member[{i}].ID}}");
                    GroupMembers.Add(id);
                }
            }
        }

        /// <summary>
        /// Am I dead?
        /// </summary>
        /// <returns>Returns a bool indicating whether or not you're dead.</returns>
        public static bool AmIDead()
        {
            //scan through our inventory looking for a container.
            for (int i = 1; i <= 10; i++)
            {
                bool SlotExists = _mq.Query<bool>($"${{Me.Inventory[pack{i}]}}");
                if (SlotExists)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Am I in combat?
        /// </summary>
        /// <returns>Returns a bool indicating whether or not you're in combat</returns>
        public static bool InCombat()
        {
            bool inCombat = Assist._isAssisting || _mq.Query<bool>("${Me.Combat}") || _mq.Query<bool>("${Me.CombatState.Equal[Combat]}");
            return inCombat;
        }

        /// <summary>
        /// Checks the mana resources, and does actions to regenerate mana during combat.
        /// </summary>
        [ClassInvoke(Data.Class.ManaUsers)]
        public static void CheckManaResources()
        {
            if (!e3util.ShouldCheck(ref _nextResourceCheck, _nextResourceCheckInterval)) return;

            using (Log.Trace())
            {
                if (E3.IsInvis) return;
                if (Basics.AmIDead()) return;

                int minMana = 40;
                int minHP = 60;
                int maxMana = 75;
                int maxLoop = 25;

                int totalClicksToTry = 40;
                //Int32 minManaToTryAndHeal = 1000;

                if (!InCombat())
                {
                    minMana = 85;
                    maxMana = 95;
                }

                int pctMana = _mq.Query<int>("${Me.PctMana}");
                int currentHps = _mq.Query<int>("${Me.CurrentHPs}");

                if (E3.CurrentClass == Data.Class.Enchanter)
                {
                    bool manaDrawBuff = _mq.Query<bool>("${Bool[${Me.Buff[Mana Draw]}]}") || _mq.Query<bool>("${Bool[${Me.Song[Mana Draw]}]}");
                    if (manaDrawBuff)
                    {
                        if (pctMana > 50)
                        {
                            return;
                        }
                    }
                }

                if (E3.CurrentClass == Data.Class.Necromancer)
                {
                    bool deathBloom = _mq.Query<bool>("${Bool[${Me.Buff[Death Bloom]}]}") || _mq.Query<bool>("${Bool[${Me.Song[Death Bloom]}]}");
                    if (deathBloom)
                    {
                        return;
                    }
                }

                if (E3.CurrentClass == Data.Class.Shaman)
                {
                    bool canniReady = _mq.Query<bool>("${Me.AltAbilityReady[Cannibalization]}");

                    if (canniReady && currentHps > 7000 && _mq.Query<double>("${Math.Calc[${Me.MaxMana} - ${Me.CurrentMana}]}") > 4500)
                    {
                        Spell s;
                        if (!Spell._loadedSpellsByName.TryGetValue("Cannibalization", out s))
                        {
                            s = new Spell("Cannibalization");
                        }
                        if (s.CastType != CastType.None)
                        {
                            Casting.Cast(0, s);
                            return;
                        }
                    }
                }

                if (_mq.Query<bool>("${Me.ItemReady[Summoned: Large Modulation Shard]}"))
                {
                    if (_mq.Query<double>("${Math.Calc[${Me.MaxMana} - ${Me.CurrentMana}]}") > 3500 && currentHps > 6000)
                    {
                        Spell s;
                        if (!Spell._loadedSpellsByName.TryGetValue("Summoned: Large Modulation Shard", out s))
                        {
                            s = new Spell("Summoned: Large Modulation Shard");
                        }
                        if (s.CastType != CastType.None)
                        {
                            Casting.Cast(0, s);
                            return;
                        }
                    }
                }
                if (_mq.Query<bool>("${Me.ItemReady[Azure Mind Crystal III]}"))
                {
                    if (_mq.Query<double>("${Math.Calc[${Me.MaxMana} - ${Me.CurrentMana}]}") > 3500)
                    {
                        Spell s;
                        if (!Spell._loadedSpellsByName.TryGetValue("Azure Mind Crystal III", out s))
                        {
                            s = new Spell("Azure Mind Crystal III");
                        }
                        if (s.CastType != CastType.None)
                        {
                            Casting.Cast(0, s);
                            return;
                        }
                    }
                }

                if (E3.CurrentClass == Data.Class.Necromancer && pctMana < 50)
                {
                    bool deathBloomReady = _mq.Query<bool>("${Me.AltAbilityReady[Death Bloom]}");
                    if (deathBloomReady && currentHps > 8000)
                    {
                        Spell s;
                        if (!Spell._loadedSpellsByName.TryGetValue("Death Bloom", out s))
                        {
                            s = new Spell("Death Bloom");
                        }
                        if (s.CastType != CastType.None)
                        {
                            Casting.Cast(0, s);
                            return;
                        }
                    }
                }
                if (E3.CurrentClass == Data.Class.Enchanter && pctMana < 50)
                {
                    bool manaDrawReady = _mq.Query<bool>("${Me.AltAbilityReady[Mana Draw]}");
                    if (manaDrawReady)
                    {
                        Spell s;
                        if (!Spell._loadedSpellsByName.TryGetValue("Mana Draw", out s))
                        {
                            s = new Spell("Mana Draw");
                        }
                        if (s.CastType != CastType.None)
                        {
                            Casting.Cast(0, s);
                            return;
                        }
                    }
                }
                if (pctMana > minMana) return;
                //no manastone in pok
                bool pok = _mq.Query<bool>("${Zone.ShortName.Equal[poknowledge]}");
                if (pok) return;

                bool hasManaStone = _mq.Query<bool>("${Bool[${FindItem[=Manastone]}]}");

                if (hasManaStone)
                {
                    _mq.Write("\agUsing Manastone...");
                    int pctHps = _mq.Query<int>("${Me.PctHPs}");
                    pctMana = _mq.Query<int>("${Me.PctMana}");
                    int currentLoop = 0;
                    while (pctHps > minHP && pctMana < maxMana)
                    {
                        currentLoop++;
                        int currentMana = _mq.Query<int>("${Me.CurrentMana}");

                        for (int i = 0; i < totalClicksToTry; i++)
                        {
                            _mq.Cmd("/useitem \"Manastone\"");
                        }
                        //allow mq to have the commands sent to the server
                        _mq.Delay(50);
                        if ((E3.CurrentClass & Class.Priest) == E3.CurrentClass)
                        {
                            if (Heals.SomeoneNeedsHealing(currentMana, pctMana))
                            {
                                return;
                            }
                        }
                        if (currentLoop > maxLoop)
                        {
                            return;
                        }

                        pctHps = _mq.Query<int>("${Me.PctHPs}");
                        pctMana = _mq.Query<int>("${Me.PctMana}");
                    }
                }
            }
        }

        /// <summary>
        /// Do I need to med?
        /// </summary>
        [ClassInvoke(Data.Class.All)]
        public static void CheckAutoMed()
        {
            if (!e3util.ShouldCheck(ref _nextAutoMedCheck, _nextAutoMedCheckInterval)) return;
            int autoMedPct = E3.GeneralSettings.General_AutoMedBreakPctMana;
            if (autoMedPct == 0) return;
            if (!E3.CharacterSettings.Misc_AutoMedBreak) return;
            using (Log.Trace())
            {
                if (Movement._following || InCombat()) return;

                bool amIStanding = _mq.Query<bool>("${Me.Standing}");

                if (amIStanding && autoMedPct > 0)
                {
                    int pctMana = _mq.Query<int>("${Me.PctMana}");
                    int pctEndurance = _mq.Query<int>("${Me.PctEndurance}");

                    if (pctMana < autoMedPct && (E3.CurrentClass & Class.ManaUsers) == E3.CurrentClass)
                    {
                        _mq.Cmd("/sit");
                    }
                    if (pctEndurance < autoMedPct)
                    {
                        _mq.Cmd("/sit");
                    }
                }
            }
        }

        /// <summary>
        /// Checks hunger and thirst levels, and eats the configured food and drink in order to save stat food.
        /// </summary>
        [ClassInvoke(Class.All)]
        public static void CheckFood()
        {
            if (!e3util.ShouldCheck(ref _nextFoodCheck, _nextFoodCheckInterval)) return;

            if (!E3.CharacterSettings.Misc_AutoFoodEnabled || Assist._isAssisting) return;
            using (Log.Trace())
            {
                var toEat = E3.CharacterSettings.Misc_AutoFood;
                var toDrink = E3.CharacterSettings.Misc_AutoDrink;

                if (_mq.Query<bool>($"${{FindItem[{toEat}].ID}}") && _mq.Query<int>("${Me.Hunger}") < 4500)
                {
                    _mq.Cmd($"/useitem \"{toEat}\"");
                }

                if (_mq.Query<bool>($"${{FindItem[{toDrink}].ID}}") && _mq.Query<int>("${Me.Thirst}") < 4500)
                {
                    _mq.Cmd($"/useitem \"{toDrink}\"");
                }
            }
        }

        /// <summary>
        /// Checks the cursor and clears it if necessary.
        /// </summary>
        [ClassInvoke(Class.All)]
        public static void CheckCursor()
        {
            if (!e3util.ShouldCheck(ref _nextCursorCheck, _nextCursorCheckInterval)) return;
            using (Log.Trace())
            {
                bool itemOnCursor = _mq.Query<bool>("${Bool[${Cursor.ID}]}");
                if (itemOnCursor)
                {
                    if (_cursorOccupiedSince == null)
                    {
                        _cursorOccupiedSince = DateTime.Now;
                    }

                    bool regenItem = _mq.Query<bool>("${Cursor.Name.Equal[Azure Mind Crystal III]}") || _mq.Query<bool>("${Cursor.Name.Equal[Summoned: Large Modulation Shard]}") || _mq.Query<bool>("${Cursor.Name.Equal[Sanguine Mind Crystal III]}");

                    if (regenItem)
                    {
                        int charges = _mq.Query<int>("${Cursor.Charges}");
                        if (charges == 3)
                        {
                            e3util.ClearCursor();
                            _cursorOccupiedSince = null;
                        }
                    }
                    else
                    {
                        bool orb = _mq.Query<bool>("${Cursor.Name.Equal[Molten orb]}") || _mq.Query<bool>("${Cursor.Name.Equal[Lava orb]}");
                        if (orb)
                        {
                            int charges = _mq.Query<int>("${Cursor.Charges}");
                            if (charges == 10)
                            {
                                e3util.ClearCursor();
                                _cursorOccupiedSince = null;
                            }
                        }
                        else
                        {
                            _cursorOccupiedTime = DateTime.Now - _cursorOccupiedSince.GetValueOrDefault();
                            // if there's a thing on our cursor for > 30 seconds, inventory it
                            if (_cursorOccupiedTime > _cursorOccupiedThreshold)
                            {
                                _cursorOccupiedTime = new TimeSpan();
                                e3util.ClearCursor();
                                _cursorOccupiedSince = null;
                            }
                        }
                    }
                }
                else
                {
                    _cursorOccupiedSince = null;
                }
            }
        }
    }
}
