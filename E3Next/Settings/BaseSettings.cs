﻿using E3Core.Processors;
using IniParser.Model;
using MonoCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E3Core.Settings
{
    public abstract class BaseSettings
    {

        public static Logging _log = E3.Log;
        protected static IMQ MQ = E3.Mq;

        protected static string _macroFolder = MQ.Query<string>("${MacroQuest.Path[macros]}");
        protected static string _configFolder = MQ.Query<string>("${MacroQuest.Path[config]}");

        protected static string _settingsFolder = @"\e3 Macro Inis\";
        protected static string _botFolder = @"\e3 Bot Inis\";

  
        static BaseSettings()
        {



        }
        public static string GetBoTFilePath(string fileName)
        {
            string macroFile = _macroFolder + _botFolder + fileName;
            string configFile = _configFolder + _botFolder + fileName;
            string fullPathToUse = macroFile;

            if (!System.IO.File.Exists(macroFile) && !System.IO.File.Exists(configFile))
            {

                fullPathToUse = configFile;
            }
            else
            {
                fullPathToUse = macroFile;
                if (System.IO.File.Exists(configFile)) fullPathToUse = configFile;
            }
            return fullPathToUse;
        }
        public static string GetSettingsFilePath(string fileName)
        {
            string macroFile = _macroFolder + _settingsFolder + fileName;
            string configFile = _configFolder + _settingsFolder + fileName;
            string fullPathToUse = macroFile;

            if (!System.IO.File.Exists(macroFile) && !System.IO.File.Exists(configFile))
            {

                fullPathToUse = configFile;
            }
            else
            {
                fullPathToUse = macroFile;
                if (System.IO.File.Exists(configFile)) fullPathToUse = configFile;
            }
            return fullPathToUse;
        }
        public String ToStringFields()
        {
            String output = "";

            var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                int count = 0;
                foreach (String value in (IEnumerable<string>)field.GetValue(this))
                {
                    count++;
                    output += "C" + count + ": " + value + System.Environment.NewLine;
                }
            }
            return output;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.Append(this.GetType().Name);
            sb.AppendLine();
            sb.AppendLine("==============");
            foreach (FieldInfo property in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                var value = property.GetValue(this);
                if (value is System.Collections.IEnumerable && !(value is string))
                {
                    sb.AppendLine();
                    sb.Append("Collection:" + property.Name);
                    sb.AppendLine();
                    sb.AppendLine("==============");

                    foreach (var listitem in value as System.Collections.IEnumerable)
                    {
                        sb.Append("Item: " + listitem.ToString());
                    }
                }
                else
                {
                    sb.Append(property.Name);
                    sb.Append(": ");
                    sb.Append(property.GetValue(this));

                    sb.Append(System.Environment.NewLine);
                }

               
            }

            return sb.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="k1">Key1</param>
        /// <param name="k2">Key2</param>
        /// <param name="k3">Key3</param>
        /// <param name="whatToWrite"></param>
        /// <param name="overwrite">to overwrite the value in the ini</param>
        public void WriteToIni(string k1,string k2,string k3, string whatToWrite, bool overwrite = false)
        {
            using(_log.Trace())
            {
                //| By switching ':'s to '*'s in arguemnt 3, to avoid issues when reading variables from the inis.
                k3 = k3.Replace(":", ";");
                string combinedKey = $"{k1},{k2},{k3}";
                if(!MQ.Query<bool>($"${{Ini[{combinedKey}].Length}}") || overwrite)
                {
                    if (!string.IsNullOrWhiteSpace(whatToWrite))
                    {
                        MQ.Cmd($"/ini \"{k1}\" \"{k2}\" \"{k3}\" \"{whatToWrite}\"");
                    }
                    else
                    {
                        MQ.Cmd($"/ini \"{k1}\" \"{k2}\" \"{k3}\"");
                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, ref string valueToSet)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            valueToSet = data;
                        }
                    }
                }
            }
        }
        public static void LoadKeyData<K, V>(string sectionKey, string Key, IniData parsedData, Dictionary<K, V> dictionary)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            var splits = data.Split('/');
                            if (!(splits.Length > 1)) continue;
                            dictionary.Add((K)(object)splits[0], (V)(object)splits[1]);
                        }
                    }
                }
            }
        }
        public static string LoadKeyData(string sectionKey, string Key, IniData parsedData)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            return data;
                        }
                    }
                }
            }
            return String.Empty;
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, ref Boolean valueToSet)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            if (data.Equals("Off") || data.Equals("False"))
                            {
                                valueToSet = false;
                            }
                            else if (data.Equals("On") || data.Equals("True"))
                            {
                                valueToSet = true;
                            }
                            else
                            {
                                valueToSet = false;
                            }

                        }
                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, ref Int32 valueToSet)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                           valueToSet= Int32.Parse(data);

                        }
                    }
                }
            }
        }

        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, List<String> collectionToAddTo)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            collectionToAddTo.Add(data);
                        }

                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, List<Data.Spell> collectionToAddTo)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            if (!string.Equals(sectionKey, "Cures"))
                            {
                                CheckFor(data);
                            }

                            collectionToAddTo.Add(new Data.Spell(data, parsedData));
                        }

                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, Queue<Data.Spell> collectionToAddTo)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            if (!string.Equals(sectionKey, "Cures"))
                            {
                                CheckFor(data);
                            }

                            collectionToAddTo.Enqueue(new Data.Spell(data, parsedData));
                        }

                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, List<Data.MelodyIfs> collectionToAddTo)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            collectionToAddTo.Add(new Data.MelodyIfs(data, parsedData));
                        }

                    }
                }
            }
        }
        public static void LoadKeyData(string sectionKey, string Key, IniData parsedData, out Data.Spell spellToLoad)
        {
            _log.Write($"{sectionKey} {Key}");
            var section = parsedData.Sections[sectionKey];
            if (section != null)
            {
                var keyData = section.GetKeyData(Key);
                if (keyData != null)
                {
                    foreach (var data in keyData.ValueList)
                    {
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            spellToLoad=new Data.Spell(data, parsedData);
                            return;
                        }

                    }
                }
            }
            spellToLoad = null;
        }

        /// <summary>
        /// Checks if i have a thing and broadcasts a warning message that i don't.
        /// </summary>
        /// <param name="thingToCheckFor">The thing.</param>
        public static void CheckFor(string thingToCheckFor)
        {
            string thing = thingToCheckFor;
            if (thingToCheckFor.Contains('/'))
            {
                thing = thingToCheckFor.Split('/')[0];
            }

            if (!MQ.Query<bool>($"${{Me.Book[{thing}]}}") && !MQ.Query<bool>($"${{Me.AltAbility[{thing}]}}") &&
                !MQ.Query<bool>($"${{Me.CombatAbility[{thing}]}}") && !MQ.Query<bool>($"${{Me.Ability[{thing}]}}") &&
                !MQ.Query<bool>($"${{FindItem[={thing}]}}"))
            {
                E3.Bots.Broadcast($"\ayI do not have {thing} that is configured in bot ini.");
            }
        }
    }
    interface IBaseSettings
    {
        IniData CreateSettings();
    }
}
