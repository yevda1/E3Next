using E3Core.Data;
using E3Core.Settings;
using E3Core.Settings.FeatureSettings;
using E3Core.Utility;
using MonoCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.ServiceModel.PeerResolvers;
using System.Windows.Forms;

namespace E3Core.Processors
{
    public static class Hoard
    {
        public static Logging _log = E3.Log;
        private static IMQ MQ = E3.MQ;
        private static ISpawns _spawns = E3.Spawns;
      
        private static bool _fullInventoryAlert = false;
        private static Int64 _nextHoardCheck = 0;
        private static Int64 _nextHoardCheckInterval = 1000;

		[SubSystemInit]
        public static void Hoard_Init()
        {
            RegisterEvents();
            try
            {
                HoardDataFile.LoadData();

            }
            catch (Exception ex)
            {
                MQ.Write("Exception loading Hoard Data file. Hoard data is not available. Error:"+ex.Message + " stack:" + ex.StackTrace);
               
            }
        }
        public static void Reset()
        {

        }
        private static void RegisterEvents()
        {
            EventProcessor.RegisterCommand("/Hoardcoins", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Coins.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Coins");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Coins");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardgems", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Gems.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Gems");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Gems");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardore", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Ore.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Ore");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Ore");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardsilks", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Silks.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Silks");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Silks");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardhides", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Hides.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Hides");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Hides");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardfood", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Food.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Food");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Food");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardpowder", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Powder.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Powder");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Powder");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardvenom", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Venom.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Venom");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Venom");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardalchemy", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Alchemy.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Alchemy");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Alchemy");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardtinkerer", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Tinkerer.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Tinkerer");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Tinkerer");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoardother", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);
                HoardDataFile.Other.Add(cursorItem);

                MQ.Write($"\aoSetting {cursorItem} to Other");
                E3.Bots.BroadcastCommand($"/E3HoardAdd \"{cursorItem}\" Other");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/Hoarddelete", (x) =>
            {
                string cursorItem = MQ.Query<string>("${Cursor.Name}");

                if (cursorItem.Equals("NULL", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(cursorItem))
                {
                    MQ.Write("You don't have an item on your cursor, cannot modify the Hoard file.");
                    MQ.Write("Place an item on your cursor and then give the proper /Hoardcoins, /Hoardgems, /Hoardore, /Hoardsilks, /Hoardhides, /Hoardfood, /Hoardpowder, /Hoardvenom, /Hoardalchemy, /Hoardtinkerer, /Hoardother command");
                    return;
                }

                HoardDataFile.Coins.Remove(cursorItem);
                HoardDataFile.Gems.Remove(cursorItem);
                HoardDataFile.Ore.Remove(cursorItem);
                HoardDataFile.Silks.Remove(cursorItem);
                HoardDataFile.Hides.Remove(cursorItem);
                HoardDataFile.Food.Remove(cursorItem);
                HoardDataFile.Powder.Remove(cursorItem);
                HoardDataFile.Venom.Remove(cursorItem);
                HoardDataFile.Alchemy.Remove(cursorItem);
                HoardDataFile.Tinkerer.Remove(cursorItem);
                HoardDataFile.Other.Remove(cursorItem);

                MQ.Write($"\aoDeleted {cursorItem} from Hoard");
                HoardDataFile.SaveData();

                e3util.ClearCursor();
            });
            EventProcessor.RegisterCommand("/OrganizeHoard", (x) =>
            {
                OrganizeHoard();
            });
        }

        public static void Process()
        {

            if (E3.IsInvis) return;
            if (!e3util.ShouldCheck(ref _nextHoardCheck, _nextHoardCheckInterval)) return;

            if(!Assist.IsAssisting)
            {
 				long currentTimestamp = Core.StopWatch.ElapsedMilliseconds;

				if (!E3.CharacterSettings.Misc_AutoHoardEnabled) return;

				if(Basics.AmIDead())
				{
					E3.Bots.Broadcast("I am dead, turning off autoHoard");
					E3.CharacterSettings.Misc_AutoHoardEnabled = false;
					return;
				}
            }
        }
        private static void OrganizeHoard()
        {
            try
            {
                Organize("Coins");
                Organize("Gems");
                Organize("Ore");
                Organize("Silks");
                Organize("Hides");
                Organize("Food");
                Organize("Powder");
                Organize("Venom");
                Organize("Alchemy");
                Organize("Tinkerer");
                Organize("Other");
            }
            finally
            {
                MQ.Write("\agFinished organizing the hoard");
            }
        }

        private static void Organize(string hoardGroup)
        {
            E3.Bots.Broadcast($"\ag Organize {hoardGroup}");
            String bot;
            HashSet<string> hoardItems;
            //Get name of Bot
            switch (hoardGroup)
            {
                case "Coins":
                    bot = E3.GeneralSettings.Hoard_HoarderofCoins;
                    hoardItems = HoardDataFile.Coins;
                    break;
                case "Gems":
                    bot = E3.GeneralSettings.Hoard_HoarderofGems;
                    hoardItems = HoardDataFile.Gems;
                    break;
                case "Ore":
                    bot = E3.GeneralSettings.Hoard_HoarderofOre;
                    hoardItems = HoardDataFile.Ore;
                    break;
                case "Silks":
                    bot = E3.GeneralSettings.Hoard_HoarderofSilks;
                    hoardItems = HoardDataFile.Silks;
                    break;
                case "Hides":
                    bot = E3.GeneralSettings.Hoard_HoarderofHides;
                    hoardItems = HoardDataFile.Hides;
                    break;
                case "Food":
                    bot = E3.GeneralSettings.Hoard_HoarderofFood;
                    hoardItems = HoardDataFile.Food;
                    break;
                case "Powder":
                    bot = E3.GeneralSettings.Hoard_HoarderofPowder;
                    hoardItems = HoardDataFile.Powder;
                    break;
                case "Venom":
                    bot = E3.GeneralSettings.Hoard_HoarderofVenom;
                    hoardItems = HoardDataFile.Venom;
                    break;
                case "Alchemy":
                    bot = E3.GeneralSettings.Hoard_HoarderofAlchemy;
                    hoardItems = HoardDataFile.Alchemy;
                    break;
                case "Tinkerer":
                    bot = E3.GeneralSettings.Hoard_HoarderofTinkerer;
                    hoardItems = HoardDataFile.Tinkerer;
                    break;
                case "Other":
                    bot = E3.GeneralSettings.Hoard_HoarderofOther;
                    hoardItems = HoardDataFile.Other;
                    break;
                default:
                    E3.Bots.Broadcast($"\arERROR: Hoard switch statement not configured for {hoardGroup}");
                    throw new Exception("ERROR: Hoard switch statement not configured for {hoardGroup}");
            }

            _spawns.TryByID(MQ.Query<int>("${Me.ID}"), out var botMe);

            //if not me
            if (bot != botMe.Name)
            {
                if (_spawns.TryByName(bot, out var botSpawn))
                {
                    var targetId = botSpawn.ID;

                    //scan through our inventory looking for items
                    for (Int32 i = 1; i <= 10; i++)
                    {
                        bool SlotExists = MQ.Query<bool>($"${{Me.Inventory[pack{i}]}}");
                        if (SlotExists)
                        {
                            Int32 ContainerSlots = MQ.Query<Int32>($"${{Me.Inventory[pack{i}].Container}}");

                            if (ContainerSlots > 0)
                            {
                                for (Int32 e = 1; e <= ContainerSlots; e++)
                                {
                                    String itemName = MQ.Query<String>($"${{Me.Inventory[pack{i}].Item[{e}]}}");
                                    if (itemName == "NULL")
                                    {
                                        continue;
                                    }

                                    //Hand over valid items
                                    if (hoardItems.Contains(itemName))
                                    {
                                        //Pickup item
                                        MQ.Cmd($"/nomodkey /itemnotify in pack{i} {e} leftmouseup", 500);
                                        if (MQ.Query<bool>("${Window[QuantityWnd].Open}"))
                                        {
                                            MQ.Cmd("/nomodkey /notify QuantityWnd QTYW_Accept_Button leftmouseup", 500);
                                        }
                                        E3.Bots.Broadcast($"\arItem {itemName}");

                                        //Trade item to bot
                                        if (Casting.TrueTarget(targetId))
                                        {
                                            e3util.GiveItemOnCursorToTarget(false, false);
                                        }

                                        MQ.Delay(500);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    E3.Bots.Broadcast($"\arERROR: Cannot find bot {bot}");
                }
            }
        }
    }
}
