using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

/// General Settings
/// Static Variables
/// 

namespace Assets.WoWEditSettings
{
    public static class Settings
    {
        // Path //
        public const string SettingsPath = "Settings.json";

        // Settings Variables //
        public static string ApplicationPath, CachePath, ExtractedPath, SelectedPath;
        public static List<string> WoWPath;
        public static TerrainImport terrainImport;
        public static WorldSettings WorldSettings;
        public static List<string> DropdownGameList;
        public static WoWSource WoWSource;
        public static bool LoadWMOs, LoadM2s;

        public static void LoadConfig()
        {
            terrainImport = new TerrainImport();
            WorldSettings = new WorldSettings();
            DropdownGameList = new List<string>();
            WoWPath = new List<string>();

            if (!File.Exists(SettingsPath))
                DefaultSettings();

            SettingsManager<Configuration>.Initialise(SettingsPath);
            terrainImport = SettingsManager<Configuration>.Config.TerrainImport;
            WorldSettings = SettingsManager<Configuration>.Config.WorldSettings;
            CachePath = SettingsManager<Configuration>.Config.CachePath;
            WoWPath = SettingsManager<Configuration>.Config.WoWPath;
            SelectedPath = SettingsManager<Configuration>.Config.SelectedPath;
        }

        public static void SaveFile()
        {
            Configuration config = new Configuration()
            {
                CachePath       = CachePath,
                WoWPath         = WoWPath,
                SelectedPath    = SelectedPath,
                ApplicationPath = ApplicationPath,
                ExtractedPath   = ExtractedPath,
                WoWSource       = WoWSource,
                TerrainImport   = terrainImport,
                WorldSettings   = WorldSettings
            };

            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private static void DefaultSettings()
        {
            Configuration config = new Configuration()
            {
                CachePath       = string.Empty,
                WoWPath         = new List<string>(),
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

        public static string GetWoWPath(List<string> WoWPaths)
        {
            foreach (string path in WoWPaths)
                return path;
            return string.Empty;
        }
    }
}