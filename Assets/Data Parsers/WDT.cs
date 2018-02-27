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

        ReadMVER(WDTstream);
        ReadMPHD(WDTstream, MapName);
        ReadMAIN(WDTstream);

        // wmo only worlds specific chunk parsing here :

        WDTstream.Close();
        WDTstream = null;
    }

}
