using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;
using UnityEngine;

/// General Settings
/// Static Variables
/// 

namespace Assets.WoWEditSettings
{
    public static class Settings
    {
        private static string filePath = "settings.ini";

        // Data Settings //
        public static string ApplicationPath;
        public static List<INISection> Sections = new List<INISection>();

        // World Settings //
        public static float WorldScale = 10.0f;

        // LoD Settings //
        public static float terrainMaterialDistance = 300;
        public static float highMipMapBias = 0.5f; // negative = sharper, 0 = default, positive = blurryer
        public static float lowMipMapBias = 0f; // negative = sharper, 0 = default, positive = blurryer

        // Terrain Settings //
        public static bool ShowVertexColors = true;

        public static void Load()
        {
            var currentSection = string.Empty;

            if (!File.Exists(filePath))
            {
                //File.Create(filePath);
                string defaultSettingsFile =
                "[path]\n" +
                "cachepath=\n" +
                "selectedpath=\n" +
                "[misc]\n" +
                "wowsource=\n" +
                "localproduct=\n" +
                "onlineproduct=";
                File.WriteAllText(filePath, defaultSettingsFile);
            }

            using (var sr = File.OpenText(filePath))
            {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (currentSection != string.Empty)
                    {
                        if (line.Contains("="))
                        {
                            var keyValue = line.Split('=');
                            var section = Sections.Find(x => x.Name == currentSection);
                            try { section.AddKeyValue(keyValue[0], keyValue[1]); }
                            catch
                            {
                                section = new INISection() { Name = currentSection };
                                section.AddKeyValue(keyValue[0], keyValue[1]);
                                Sections.Add(section);
                            }
                        }
                        else if (line.StartsWith("["))
                            currentSection = string.Empty;
                    }

                    if (line.StartsWith("#")) continue;
                    if (line.StartsWith("["))
                        currentSection = line;
                }
            }
        }

        public static void Save()
        {
            using (var sw = new StreamWriter(filePath))
            {
                foreach (var section in Sections)
                {
                    sw.WriteLine(section.Name);

                    var keyValues = section.GetKeyValues();
                    foreach (var pair in keyValues)
                    {
                        sw.WriteLine($"{pair.Key}={pair.Value.ToLower()}");
                    }
                }
            }
        }

        public static INISection GetSection(string name)
        {
            name = $"[{name}]";
            return Sections.Find(x => x.Name == name);
        }

        public enum WoWSource : uint
        {
            Game = 0,
            Online = 1,
            Extracted = 2
        }
    }
}