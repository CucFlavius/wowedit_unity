using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT  {

    public class WDTflagsdata
    {
        public bool adt_has_big_alpha;
        public bool adt_has_height_texturing;
        public bool[,] HasADT = new bool[64,64];
    }

    public static Dictionary<uint, WDTflagsdata> Flags = new Dictionary<uint, WDTflagsdata>(); // flags in a mapname dictionary (one wdt per map)
    public static Dictionary<(uint, uint), WDTEntry> WDTEntries = new Dictionary<(uint, uint), WDTEntry>();
    public class WDTEntry
    {
        public uint RootADT;
        public uint OBJ0ADT;
        public uint OBJ1ADT;
        public uint TEX0ADT;
        public uint LODADT;
        public uint MapTexture;
        public uint MapTextureN;
        public uint MiniMapTexture;
    }
}
