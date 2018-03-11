using System;
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
        ThreadWorking = true;

        blockData = new BlockDataType();
        blockData.ChunksData = new List<ChunkData>();
        blockData.terrainTexturePaths = new List<string>();
        blockData.terrainTextures = new Dictionary<string, Texture2Ddata>();
        blockData.textureFlags = new Dictionary<string, TerrainTextureFlag>();
        blockData.heightScales = new Dictionary<string, float>();
        blockData.heightOffsets = new Dictionary<string, float>();

        ParseADT_Main(Path, MapName, coords);
        ParseADT_Tex(Path, MapName, coords);
        ParseADT_Obj(Path, MapName, coords);
        ADT_ProcessData.GenerateMeshArrays();
        if (ADTSettings.LoadShadowMaps)
            ADT_ProcessData.AdjustAlphaBasedOnShadowmap(MapName);
        ADT_ProcessData.Load_hTextures();
        AllBlockData.Enqueue(blockData);

        ThreadWorking = false;
    }

    private static void ParseADT_Main (string Path, string MapName, Vector2 coords)
    {
        string ADTmainPath = Path + MapName + "_" + coords.x + "_" + coords.y + ".adt";
        Stream ADTstream = Casc.GetFileStream(ADTmainPath);

        int MCNKchunkNumber = 0;
        long streamPosition = 0;
        while (streamPosition < ADTstream.Length)
        {
            ADTstream.Position = streamPosition;
            int chunkID = ReadLong(ADTstream);
            int chunkSize = ReadLong(ADTstream);
            streamPosition = ADTstream.Position + chunkSize;

            switch (chunkID)
            {
                case (int)ADTchunkID.MVER:
                    ReadMVER(ADTstream); // ADT file version
                    break;
                case (int)ADTchunkID.MHDR:
                    ReadMHDR(ADTstream); // Offsets for specific chunks 0000 if chunks don't exist.
                    break;
                case (int)ADTchunkID.MH2O:
                    ReadMH2O(ADTstream, chunkSize); // Water Data
                    break;
                case (int)ADTchunkID.MCNK:
                    {
                        ReadMCNK(ADTstream, MCNKchunkNumber, chunkSize); // Terrain Data - 256chunks
                        MCNKchunkNumber++;
                    }
                    break;
                case (int)ADTchunkID.MFBO:
                    ReadMFBO(ADTstream); // FlightBounds plane & Death plane
                    break;
                default:
                    SkipUnknownChunk(ADTstream, chunkID, chunkSize);
                    break;
            }
        }

        ADTstream.Close();
        ADTstream = null;
    }

    private static void ParseADT_Tex (string Path, string MapName, Vector2 coords)
    {
        string ADTtexPath = Path + MapName + "_" + coords.x + "_" + coords.y + "_tex0" + ".adt";
        Stream ADTtexstream = Casc.GetFileStream(ADTtexPath);

        int MCNKchunkNumber = 0;
        long streamPosition = 0;
        while (streamPosition < ADTtexstream.Length)
        {
            ADTtexstream.Position = streamPosition;
            int chunkID = ReadLong(ADTtexstream);
            int chunkSize = ReadLong(ADTtexstream);
            streamPosition = ADTtexstream.Position + chunkSize;

            switch (chunkID)
            {
                case (int)ADTchunkID.MVER:
                    ReadMVER(ADTtexstream); // ADT file version
                    break;
                case (int)ADTchunkID.MAMP:
                    ReadMAMP(ADTtexstream); // Single value - texture size = 64
                    break;
                case (int)ADTchunkID.MTEX:
                    ReadMTEX(ADTtexstream, chunkSize); // Texture Paths
                    break;
                case (int)ADTchunkID.MCNK:
                    {
                        ReadMCNKtex(ADTtexstream, MapName, MCNKchunkNumber, chunkSize); // Texture Data - 256chunks
                        MCNKchunkNumber++;
                    }
                    break;
                case (int)ADTchunkID.MTXF:
                    ReadMTXF(ADTtexstream, chunkSize); // Texture Paths
                    break;
                case (int)ADTchunkID.MTXP:
                    ReadMTXP(ADTtexstream, chunkSize); // Texture Paths
                    break;
                default:
                    SkipUnknownChunk(ADTtexstream, chunkID, chunkSize);
                    break;
            }
        }
        ADTtexstream.Close();
        ADTtexstream = null;
    }

    public static void ParseADT_Obj(string Path, string MapName, Vector2 coords)
    {
        string ADTobjPath = Path + MapName + "_" + coords.x + "_" + coords.y + "_obj0" + ".adt";
        Stream ADTobjstream = Casc.GetFileStream(ADTobjPath);

        int MCNKchunkNumber = 0;
        long streamPosition = 0;
        while (streamPosition < ADTobjstream.Length)
        {
            ADTobjstream.Position = streamPosition;
            int chunkID = ReadLong(ADTobjstream);
            int chunkSize = ReadLong(ADTobjstream);
            streamPosition = ADTobjstream.Position + chunkSize;

            switch (chunkID)
            {
                case (int)ADTchunkID.MVER:
                    ReadMVER(ADTobjstream); // ADT file version
                    break;
                case (int)ADTchunkID.MMDX:
                    ReadMMDX(ADTobjstream, chunkSize); // List of filenames for M2 models
                    break;
                case (int)ADTchunkID.MMID:
                    ReadMMID(ADTobjstream, chunkSize); // List of offsets of model filenames in the MMDX chunk.
                    break;
                case (int)ADTchunkID.MWMO:
                    ReadMWMO(ADTobjstream, chunkSize); // List of filenames for WMOs (world map objects) that appear in this map tile.
                    break;
                case (int)ADTchunkID.MWID:
                    ReadMWID(ADTobjstream, chunkSize); // List of offsets of WMO filenames in the MWMO chunk.
                    break;
                case (int)ADTchunkID.MDDF:
                    ReadMDDF(ADTobjstream, chunkSize); // Placement information for doodads (M2 models).
                    break;
                case (int)ADTchunkID.MODF:
                    ReadMODF(ADTobjstream, chunkSize); // Placement information for WMOs.
                    break;
                case (int)ADTchunkID.MCNK:
                    {
                        ReadMCNKObj(ADTobjstream, MapName, MCNKchunkNumber, chunkSize); // 256chunks
                        MCNKchunkNumber++;
                    }
                    break;
                default:
                    SkipUnknownChunk(ADTobjstream, chunkID, chunkSize);
                    break;
            }
        }
        ADTobjstream.Close();
        ADTobjstream = null;
    }

    public static void SkipUnknownChunk(Stream ADTstream, int chunkID, int chunkSize)
    {
        try
        {
            //Debug.Log("Unknown chunk : " + (Enum.GetName(typeof(ADTchunkID), chunkID)).ToString() + " | Skipped");
        }
        catch
        {
            Debug.Log("Missing chunk ID : " + chunkID);
        }
        ADTstream.Seek(chunkSize, SeekOrigin.Current);
    }

}
