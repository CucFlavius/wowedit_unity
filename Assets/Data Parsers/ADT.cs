using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT
{

    public static bool ADTdataReady;
    public static bool ThreadWorking;

    public static void Load(string Path, string MapName, Vector2 coords)
    {
        ADTdata = new List<ChunkData>();
        ThreadWorking = true;
        ParseADT_Main(Path, MapName, coords);
        GenerateMeshArrays();
        AllADTdata.Enqueue(ADTdata);
        ThreadWorking = false;
    }

    private static void ParseADT_Main (string Path, string MapName, Vector2 coords)
    {
        
        Stream ADTstream = Casc.GetFileStream(Path);

        ReadMVER(ADTstream); // ADT file version
        ReadMHDR(ADTstream); // Offsets for specific chunks 0000 if chunks don't exist.
        ReadMH2O(ADTstream); // Water Data
        ReadMCNK(ADTstream); // Terrain Data
        ReadMFBO(ADTstream); // FlightBounds plane & Death plane

        // will be extra chunks to parse //

        ADTstream.Close();
        ADTstream = null;
    }

    private static void ParseADT_Tex (string Path, string MapName, Vector2 coords)
    {

    }

}
