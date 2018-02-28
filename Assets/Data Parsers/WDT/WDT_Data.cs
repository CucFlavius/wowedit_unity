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

    public static Dictionary<string, WDTflagsdata> Flags = new Dictionary<string, WDTflagsdata>(); // flags in a mapname dictionary (one wdt per map)

}
