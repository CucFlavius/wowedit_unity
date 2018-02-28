using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT  {

    // WDT files specify exactly which map tiles are present in a world, if any, and can also reference a "global" WMO. 
    // They have a chunked file structure.

    public static void Load (string Path, string MapName)
    {
        string WDTpath = Path + MapName + ".wdt";
        Stream WDTstream = Casc.GetFileStream(WDTpath);
        WDTflagsdata WDTflags = new WDTflagsdata();

        ReadMVER(WDTstream);
        ReadMPHD(WDTstream, MapName, WDTflags);
        ReadMAIN(WDTstream, WDTflags);

        // wmo only worlds specific chunk parsing here :

        Flags.Add(MapName, WDTflags);
        WDTstream.Close();
        WDTstream = null;
    }

}
