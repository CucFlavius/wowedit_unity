using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.WoWEditSettings
{
    public class WorldSettings
    {
        public float WorldScale { get; set; }
        public float terrainMaterialDistance { get; set; }
        public float highMipMapBias { get; set; }
        public float lowMipMapBias { get; set; }
    }

    public class TerrainImport
    {
        public bool LoadWMOs { get; set; }
        public bool LoadM2s { get; set; }
        public bool LoadLights { get; set; }
        public bool LoadSoundEmitters { get; set; }
        public bool LoadShadowMaps { get; set; }
        public bool ShowVertexColors { get; set; }
    }

    public class Configuration
    {
        public string CachePath { get; set; }
        public List<string> WoWPath { get; set; }
        public string SelectedPath { get; set; }
        public string ApplicationPath { get; set; }
        public string ExtractedPath { get; set; }
        public WoWSource WoWSource { get; set; }
        public TerrainImport TerrainImport { get; set; }
        public WorldSettings WorldSettings { get; set; }
    }

    public enum WoWSource : int
    {
        Null        = -1,
        Game        = 0,
        Online      = 1,
        Extracted   = 2
    }
}