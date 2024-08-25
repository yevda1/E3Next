using E3Core.Utility;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E3Core.Settings.FeatureSettings
{
    public class HoardDataFile : BaseSettings
    {
        public static HashSet<string> Coins = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Gems = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Ore = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Silks = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Hides = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Food = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Powder = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Venom = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Alchemy = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Tinkerer = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> Other = new HashSet<string>(10000, StringComparer.OrdinalIgnoreCase);
        public static bool _isDirty = false;

        private static string _fileName = @"Hoard Settings.ini";

        public static void Init()
        {



        }
        private static void RegisterEvents()
        {

        }
        public static void LoadData()
        {
            string fileNameFullPath = GetSettingsFilePath(_fileName);
         

            if (!System.IO.File.Exists(fileNameFullPath))
            {
                if (!System.IO.Directory.Exists(_configFolder + _settingsFolder))
                {
                    System.IO.Directory.CreateDirectory(_configFolder + _settingsFolder);
                }
                 //file straight up doesn't exist, lets create it
                System.IO.File.CreateText(fileNameFullPath);
            }
            else
            {
                //File already exists, may need to merge in new settings lets check
               

                FileIniDataParser fileIniData = e3util.CreateIniParser();
                _log.Write($"Reading Hoard Settings:{fileNameFullPath}");
                var parsedData = fileIniData.ReadFile(fileNameFullPath);
                
                //because of the old Loot system, need to do 4 regex, and get more specific as we go down 
                //before we go non specific on regex4

                //plat/copper exist
                System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"(.+?)\d+p\d+c", System.Text.RegularExpressions.RegexOptions.Compiled);
                //only plat exists
                System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@"(.+?)\d+p", System.Text.RegularExpressions.RegexOptions.Compiled);
                //only copper exists
                System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@"(.+?)\d+c", System.Text.RegularExpressions.RegexOptions.Compiled);
                //no money exists, safely to look for digits in the text.
                System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"([a-zA-Z-\s':;`.\d\(\)]+)", System.Text.RegularExpressions.RegexOptions.Compiled);

                //(.+?)\d+p\d+c
                //(.+?)\d+p
                //(.+?)\d+c
                //([a-zA-Z\s':;`.\d]+)
                foreach (var section in parsedData.Sections)
                {
                    foreach (var key in section.Keys)
                    {
                        if(key.Value.StartsWith("Coins", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Coins, match); }
                            if(!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Coins, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Coins, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Coins, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Gems", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Gems, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Gems, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Gems, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Gems, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Ore", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Ore, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Ore, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Ore, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Ore, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Silks", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Silks, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Silks, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Silks, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Silks, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Hides", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Hides, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Hides, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Hides, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Hides, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Food", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Food, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Food, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Food, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Food, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Powder", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Powder, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Powder, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Powder, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Powder, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Venom", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Venom, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Venom, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Venom, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Venom, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Alchemy", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Alchemy, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Alchemy, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Alchemy, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Alchemy, match); }
                            }
                        }
                        else if (key.Value.StartsWith("Tinkerer", StringComparison.OrdinalIgnoreCase))
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Tinkerer, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Tinkerer, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Tinkerer, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Tinkerer, match); }
                            }
                        }
                        else
                        {
                            //lets get the data out of the old format. 
                            string keyname = key.KeyName;//lets get rid of the junk
                            var match = regex1.Match(keyname);
                            if (match.Success) { MatchSuccess(Other, match); }
                            if (!match.Success)
                            {
                                match = regex2.Match(keyname);
                                if (match.Success) { MatchSuccess(Other, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex3.Match(keyname);
                                if (match.Success) { MatchSuccess(Other, match); }
                            }
                            if (!match.Success)
                            {
                                match = regex4.Match(keyname);
                                if (match.Success) { MatchSuccess(Other, match); }
                            }
                        }
                    }
                }
            }

          
        }
        private static void MatchSuccess(HashSet<string> hash, Match match)
        {
            if (match.Groups.Count > 1)
            {
                string matchValue = match.Groups[1].Value.Trim();
                matchValue = matchValue.Replace(";", ":");
                if (!hash.Contains(matchValue))
                {
                    hash.Add(matchValue);
                }
            }
        }
        public static void SaveData()
        {

            IniParser.FileIniDataParser parser = e3util.CreateIniParser();
            IniData newFile = new IniData();
            //lets create all the sections in alpha order.

            //create sorted lists

            List<string> _coinsSorted = Coins.OrderBy(x => x).ToList();
            List<string> _gemsSorted = Gems.OrderBy(x => x).ToList();
            List<string> _oreSorted = Ore.OrderBy(x => x).ToList();
            List<string> _silksSorted = Silks.OrderBy(x => x).ToList();
            List<string> _hidesSorted = Hides.OrderBy(x => x).ToList();
            List<string> _foodSorted = Food.OrderBy(x => x).ToList();
            List<string> _powderSorted = Powder.OrderBy(x => x).ToList();
            List<string> _venomSorted = Venom.OrderBy(x => x).ToList();
            List<string> _alchemySorted = Alchemy.OrderBy(x => x).ToList();
            List<string> _tinkererSorted = Tinkerer.OrderBy(x => x).ToList();
			List<string> _otherSorted = Other.OrderBy(x => x).ToList();

			for (char c = 'A'; c <= 'Z'; c++)
            {
                string tc = c.ToString();
                newFile.Sections.AddSection(tc);
                var section = newFile.Sections.GetSectionData(tc);

                foreach (string hashvalue in _coinsSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Coins");
                    }
                }
                foreach (string hashvalue in _gemsSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Gems");
                    }
                }
                foreach (string hashvalue in _oreSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Ore");
                    }
                }
                foreach (string hashvalue in _silksSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Silks");
                    }
                }
                foreach (string hashvalue in _hidesSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Hides");
                    }
                }
                foreach (string hashvalue in _foodSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Food");
                    }
                }
                foreach (string hashvalue in _powderSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Powder");
                    }
                }
                foreach (string hashvalue in _venomSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Venom");
                    }
                }
                foreach (string hashvalue in _alchemySorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Alchemy");
                    }
                }
                foreach (string hashvalue in _tinkererSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Tinkerer");
                    }
                }
                foreach (string hashvalue in _otherSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Other");
                    }
                }

            }
            for (char c = '0'; c <= '9'; c++)
            {
                string tc = c.ToString();
                newFile.Sections.AddSection(tc);
                var section = newFile.Sections.GetSectionData(tc);

                foreach (string hashvalue in _coinsSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Coins");
                    }
                }
                foreach (string hashvalue in _gemsSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Gems");
                    }
                }
                foreach (string hashvalue in _oreSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Ore");
                    }
                }
                foreach (string hashvalue in _silksSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Silks");
                    }
                }
                foreach (string hashvalue in _hidesSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Hides");
                    }
                }
                foreach (string hashvalue in _foodSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Food");
                    }
                }
                foreach (string hashvalue in _powderSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Powder");
                    }
                }
                foreach (string hashvalue in _venomSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Venom");
                    }
                }
                foreach (string hashvalue in _alchemySorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Alchemy");
                    }
                }
                foreach (string hashvalue in _tinkererSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Tinkerer");
                    }
                }
                foreach (string hashvalue in _otherSorted)
                {
                    if (hashvalue.StartsWith(tc))
                    {
                        section.Keys.AddKey(hashvalue, "Other");
                    }
                }

            }
            string fileNameFullPath = GetSettingsFilePath(_fileName);


            //Parse the ini file
            //Create an instance of a ini file parser
            FileIniDataParser fileIniData = e3util.CreateIniParser();
            System.IO.File.Delete(fileNameFullPath);
            parser.WriteFile(fileNameFullPath, newFile);

        }
    }
}
