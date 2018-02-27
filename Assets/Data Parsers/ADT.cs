using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT
{

    public static bool BlockDataReady;
    public static bool ThreadWorking;

    public static void Load(string Path, string MapName, Vector2 coords)
    {
        blockData = new BlockDataType();
        blockData.ChunksData = new List<ChunkData>();
        blockData.terrainTexturePaths = new List<string>();
        blockData.terrainTextures = new Dictionary<string, Texture2Ddata>();

        ThreadWorking = true;
        ParseADT_Main(Path, MapName, coords);
        ParseADT_Tex(Path, MapName, coords);
        ADT_ProcessData.GenerateMeshArrays();
        AllBlockData.Enqueue(blockData);
        ThreadWorking = false;
    }

    private static void ParseADT_Main (string Path, string MapName, Vector2 coords)
    {
        string ADTmainPath = Path + MapName + "_" + coords.x + "_" + coords.y + ".adt";
        Stream ADTstream = Casc.GetFileStream(ADTmainPath);

        ReadMVER(ADTstream); // ADT file version
        ReadMHDR(ADTstream); // Offsets for specific chunks 0000 if chunks don't exist.
        ReadMH2O(ADTstream); // Water Data
        ReadMCNK(ADTstream); // Terrain Data - 256chunks
        ReadMFBO(ADTstream); // FlightBounds plane & Death plane

        // will be extra chunks to parse //

        ADTstream.Close();
        ADTstream = null;
    }

    private static void ParseADT_Tex (string Path, string MapName, Vector2 coords)
    {
        string ADTtexPath = Path + MapName + "_" + coords.x + "_" + coords.y + "_tex0" + ".adt";
        Stream ADTtexstream = Casc.GetFileStream(ADTtexPath);

        ReadMVER(ADTtexstream); // ADT file version
        ReadMAMP(ADTtexstream); // Single value - texture size = 64
        ReadMTEX(ADTtexstream); // Texture Paths
        ReadMCNKtex(ADTtexstream, MapName); // Texture Data - 256chunks

        ADTtexstream.Close();
        ADTtexstream = null;
    }

}
