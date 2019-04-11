using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Assets.WoWEditSettings;

/// General Settings
/// Static Variables
/// 

namespace Assets.WoWEditSettings
{
    public static class Settings
    {
        // Path //
        public const string SettingsPath = "Settings.json";
        // Data Settings //
        public static string ApplicationPath, CachePath, WoWPath, ExtractedPath;
        /*
         * [0] = Cache Folder Location
         * [1] = First Run? 0/1
         * [2] = WoWSource: 0-game/1-online/2-extracted
         * [3] = WoWGameSelected
         * [4] = WoWLivePath
         * [5] = WoWPTRPath
         * [6] = WoWAlphaPath
         * [7] = WoWOnlineSelected
         * [8] = WoWExtractedPath
         * [9] = WoWCustomGameSource
         * [10]= SelectedDB2Definitions
         * 
         */
        public static TerrainImport terrainImport;
        public static WorldSettings WorldSettings;
        public static List<string> DropdownGameList;
        public static WoWSource WoWSource;
        public static bool LoadWMOs, LoadM2s;

        public static void LoadConfig()
        {
            if (!File.Exists(SettingsPath))
                DefaultSettings();

            SettingsManager<Configuration>.Initialise(SettingsPath);
        }

        public static void SaveFile()
        {
            Configuration config = new Configuration()
            {
                CachePath       = CachePath,
                WoWPath         = WoWPath,
                ApplicationPath = ApplicationPath,
                ExtractedPath   = ExtractedPath,
                WoWSource       = WoWSource,
                TerrainImport   = terrainImport,
                WorldSettings   = WorldSettings,
            };

            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private static void DefaultSettings()
        {
            Configuration config = new Configuration()
            {
                CachePath       = string.Empty,
                WoWPath         = string.Empty,
                ApplicationPath = string.Empty,
                ExtractedPath   = string.Empty,
                WoWSource       = WoWSource.Null,
                TerrainImport   = new TerrainImport()
                {
                    LoadWMOs    = true,
                    LoadM2s     = true,
                    LoadLights  = false,
                    LoadSoundEmitters   = false,
                    LoadShadowMaps      = false,
                    ShowVertexColors    = true,
                },
                WorldSettings   = new WorldSettings()
                {
                    WorldScale      = 10.0f,
                    terrainMaterialDistance = 300.0f,
                    highMipMapBias  = 0.5f,
                    lowMipMapBias   = 0f
                },
            };

            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }
}