using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT
{

    public static bool BlockDataReady;
    public static bool ThreadWorking;
    public static float finishedTime;

    // Precalculated data //
    public static Vector3[] Chunk_Vertices;
    public static int[] Chunk_Triangles;
    public static Vector2[] Chunk_UVs;

    public static void Initialize()
    {
        ////////////////////////////////////////
        // Verts

        Chunk_Vertices = new Vector3[145];
        int currentVertex = 0;
        for (int i = 0; i < 17; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < 9; j++)
                {
                    Chunk_Vertices[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / Settings.worldScale,
                                                                        0,
                                                                        (float)(-j * 0.208333125 * 20) / Settings.worldScale);
                    currentVertex++;
                }
            }
            if (i % 2 == 1)
            {
                for (int j1 = 0; j1 < 8; j1++)
                {
                    Chunk_Vertices[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / Settings.worldScale,
                                                                        0,
                                                                        (float)((-j1 - 0.5) * 0.208333125 * 20) / Settings.worldScale);
                    currentVertex++;
                }
            }
        }
        currentVertex = 0;

        ////////////////////////////////////////
        // Triangles

        Chunk_Triangles = new int[256 * 3];
        int triOffset = 0;
        //create 8 strips//
        for (int strip = 0; strip < 8; strip++)
        {
            //   case \/   //
            for (int t = 0; t < 8; t++)
            {
                Chunk_Triangles[triOffset + 0] = t + strip * 17;
                Chunk_Triangles[triOffset + 1] = t + 1 + strip * 17;
                Chunk_Triangles[triOffset + 2] = t + 9 + strip * 17;
                triOffset = triOffset + 3;
            }
            //   case /\   //
            for (int t1 = 0; t1 < 8; t1++)
            {
                Chunk_Triangles[triOffset + 0] = t1 + (strip + 1) * 17;
                Chunk_Triangles[triOffset + 1] = t1 - 8 + (strip + 1) * 17;
                Chunk_Triangles[triOffset + 2] = t1 + 1 + (strip + 1) * 17;
                triOffset = triOffset + 3;
            }
            //   case >   //
            for (int t2 = 0; t2 < 8; t2++)
            {
                Chunk_Triangles[triOffset + 0] = t2 + strip * 17;
                Chunk_Triangles[triOffset + 1] = t2 + 9 + strip * 17;
                Chunk_Triangles[triOffset + 2] = t2 + 17 + strip * 17;
                triOffset = triOffset + 3;
            }
            //   case <   //
            for (int t3 = 0; t3 < 8; t3++)
            {
                Chunk_Triangles[triOffset + 0] = t3 + 9 + strip * 17;
                Chunk_Triangles[triOffset + 1] = t3 + 1 + strip * 17;
                Chunk_Triangles[triOffset + 2] = t3 + 18 + strip * 17;
                triOffset = triOffset + 3;
            }
        }

        ////////////////////////////////////////
        // UV's

        Chunk_UVs = new Vector2[145];
        for (int u = 0; u < 145; u++)
        {
            Chunk_UVs[u] = new Vector2(Chunk_Vertices[u].x / (33.3333f / Settings.worldScale),
                                       Chunk_Vertices[u].z / (33.3333f / Settings.worldScale));
        }

    }

    public static void Load(string Path, string MapName, Vector2 coords)
    {
        
        ThreadWorking = true;
        //float startTime = Time.time;
        long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        blockData = new BlockDataType();
        blockData.ChunksData = new List<ChunkData>();
        blockData.terrainTexturePaths = new List<string>();
        blockData.terrainTextures = new Dictionary<string, Texture2Ddata>();
        blockData.textureFlags = new Dictionary<string, TerrainTextureFlag>();
        blockData.heightScales = new Dictionary<string, float>();
        blockData.heightOffsets = new Dictionary<string, float>();

        ParseADT_Main(Path, MapName, coords);
        ParseADT_Tex(Path, MapName, coords);
        //if (ADTSettings.LoadWMOs || ADTSettings.LoadM2s)
            //ParseADT_Obj(Path, MapName, coords);
        ADT_ProcessData.GenerateMeshArrays();
        if (ADTSettings.LoadShadowMaps)
            ADT_ProcessData.AdjustAlphaBasedOnShadowmap(MapName);
        ADT_ProcessData.Load_hTextures();
        AllBlockData.Enqueue(blockData);

        long millisecondsB = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        finishedTime = (millisecondsB - milliseconds)/1000f;

        ThreadWorking = false;

    }


    private static void ParseADT_Main(string Path, string MapName, Vector2 coords)  // MS version
    {
        string ADTmainPath = Path + MapName + "_" + coords.x + "_" + coords.y + ".adt";
        string path = Casc.GetFile(ADTmainPath);
        byte[] ADTmainData = File.ReadAllBytes(path);

        int MCNKchunkNumber = 0;
        long streamPosition = 0;

        using (MemoryStream ms = new MemoryStream(ADTmainData))
        {

            while (streamPosition < ms.Length)
            {
                ms.Position = streamPosition;
                int chunkID = ReadLong(ms);
                int chunkSize = ReadLong(ms);
                streamPosition = ms.Position + chunkSize;

                switch (chunkID)
                {
                    case (int)ADTchunkID.MVER:
                        ReadMVER(ms); // ADT file version
                        break;
                    case (int)ADTchunkID.MHDR:
                        ReadMHDR(ms); // Offsets for specific chunks 0000 if chunks don't exist.
                        break;
                    case (int)ADTchunkID.MH2O:
                        ReadMH2O(ms, chunkSize); // Water Data
                        break;
                    case (int)ADTchunkID.MCNK:
                        {
                            ReadMCNK(ms, MCNKchunkNumber, chunkSize); // Terrain Data - 256chunks
                            MCNKchunkNumber++;
                        }
                        break;
                    case (int)ADTchunkID.MFBO:
                        ReadMFBO(ms); // FlightBounds plane & Death plane
                        break;
                    default:
                        SkipUnknownChunk(ms, chunkID, chunkSize);
                        break;
                }
            }
        }

        ADTmainData = null;
    }



    private static void ParseADT_Tex(string Path, string MapName, Vector2 coords)
    {
        string ADTtexPath = Path + MapName + "_" + coords.x + "_" + coords.y + "_tex0" + ".adt";
        string path = Casc.GetFile(ADTtexPath);

        byte[] ADTtexData = File.ReadAllBytes(path);

        int MCNKchunkNumber = 0;
        long streamPosition = 0;

        using (MemoryStream ms = new MemoryStream(ADTtexData))
        {
            while (streamPosition < ms.Length)
            {
                ms.Position = streamPosition;
                int chunkID = ReadLong(ms);
                int chunkSize = ReadLong(ms);
                streamPosition = ms.Position + chunkSize;

                switch (chunkID)
                {
                    case (int)ADTchunkID.MVER:
                        ReadMVER(ms); // ADT file version
                        break;
                    case (int)ADTchunkID.MAMP:
                        ReadMAMP(ms); // Single value - texture size = 64
                        break;
                    case (int)ADTchunkID.MTEX:
                        ReadMTEX(ms, chunkSize); // Texture Paths
                        break;
                    case (int)ADTchunkID.MCNK:
                        {
                            ReadMCNKtex(ms, MapName, MCNKchunkNumber, chunkSize); // Texture Data - 256chunks
                            MCNKchunkNumber++;
                        }
                        break;
                    case (int)ADTchunkID.MTXF:
                        ReadMTXF(ms, chunkSize); // Texture Paths
                        break;
                    case (int)ADTchunkID.MTXP:
                        ReadMTXP(ms, chunkSize); // Texture Paths
                        break;
                    default:
                        SkipUnknownChunk(ms, chunkID, chunkSize);
                        break;
                }
            }
        }

        ADTtexData = null;
    }

    /*
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
    */
    public static void SkipUnknownChunk(MemoryStream ADTstream, int chunkID, int chunkSize)
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
