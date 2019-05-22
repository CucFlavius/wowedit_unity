using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

using Assets.Data.WoW_Format_Parsers.M2;
using static Assets.Data.WoW_Format_Parsers.M2.M2_Data;
using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.Data.WoW_Format_Parsers;
using Assets.Tools.CSV;
using CASCLib;
using Assets.UI.CASC;
using Assets.Data.DataLocal;

public static partial class M2
{
    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;
    public static List<string> LoadedBLPs = new List<string>();
    public static GameObject Casc;
    public static Jenkins96 Hasher = new Jenkins96();
    public static List<uint> SkinFiles = new List<uint>();

    public static void Load(string dataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Casc = GameObject.Find("[CASC]");
        M2Data m2Data = new M2Data();
        M2Texture m2Tex = new M2Texture();

        m2Data.dataPath = dataPath;
        m2Data.uniqueID = uniqueID;
        m2Data.position = position;
        m2Data.rotation = rotation;
        m2Data.scale = scale;

        try
        {
            ThreadWorking = true;

            ParseM2_Root(dataPath, m2Data, m2Tex);
            
            for (int i = 0; i < SkinFiles.Count; i++)
            {
                string noExtension      = Path.GetFileNameWithoutExtension(dataPath);
                string directoryPath    = Path.GetDirectoryName(dataPath);
                string path = $@"{directoryPath}\{noExtension}0{i}.skin";

                ParseM2_Skin(path, m2Data);
            }

            AllM2Data.Enqueue(m2Data);

            ThreadWorking = false;
        }
        catch (Exception ex)
        {
            Debug.Log("Error : Trying to parse M2 - " + dataPath);
            Debug.LogException(ex);
        }
    }

    public static void Load(ulong Hash, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Casc = GameObject.Find("[CASC]");
        M2Data m2Data = new M2Data();
        M2Texture m2Tex = new M2Texture();

        m2Data.dataHash = Hash;
        m2Data.uniqueID = uniqueID;
        m2Data.position = position;
        m2Data.rotation = rotation;
        m2Data.scale = scale;

        try
        {
            ThreadWorking = true;

            ParseM2_Root(Hash, m2Data, m2Tex);

            foreach (uint skinFile in SkinFiles)
            {
                ulong hash = Hasher.ComputeHash(skinFile.ToString());
                ParseM2_Skin(hash, m2Data);
            }

            AllM2Data.Enqueue(m2Data);

            ThreadWorking = false;
        }
        catch (Exception ex)
        {
            Debug.Log("Error : Trying to parse M2 - " + Hash);
            Debug.LogException(ex);
        }
    }

    private static void ParseM2_Root(string dataPath, M2Data m2Data, M2Texture m2Tex)
    {
        var stream = DataLocalHandler.GetFileStream(dataPath);
        ParseM2(stream, m2Data, m2Tex);
    }

    private static void ParseM2_Root(ulong hash, M2Data m2Data, M2Texture m2Tex)
    {
        if (Casc.GetComponent<CascHandler>().cascHandler.FileExists(hash))
        {
            var stream = Casc.GetComponent<CascHandler>().cascHandler.OpenFile(hash);
            ParseM2(stream, m2Data, m2Tex);
        }
    }

    private static void ParseM2(Stream stream, M2Data m2Data, M2Texture m2Tex)
    {
        using (BinaryReader reader = new BinaryReader(stream))
        {
            while (stream.Position < stream.Length)
            {
                M2ChunkId chunkID = (M2ChunkId)reader.ReadInt32();
                int chunkSize = reader.ReadInt32();

                switch (chunkID)
                {
                    case M2ChunkId.MD21:
                        ReadMD21(reader, m2Data, m2Tex);
                        break;
                    case M2ChunkId.TXID:
                        ReadTXID(reader, m2Data);
                        break;
                    case M2ChunkId.SKID:
                        break;
                    default:
                        SkipUnknownChunk(stream, chunkID, chunkSize);
                        break;
                }
            }
        };
    }

    private static void ParseM2_Skin(string dataPath, M2Data m2Data)
    {
        var stream = DataLocalHandler.GetFileStream(dataPath);
        ParseSkin(stream, m2Data);
    }

    private static void ParseM2_Skin(ulong hash, M2Data m2Data)
    {
        if (Casc.GetComponent<CascHandler>().cascHandler.FileExists(hash))
        {
            var stream = Casc.GetComponent<CascHandler>().cascHandler.OpenFile(hash);
            ParseSkin(stream, m2Data);
        }
    }

    private static void ParseSkin(Stream stream, M2Data m2Data)
    {
        using (BinaryReader reader = new BinaryReader(stream))
        {
            ParseSkin(reader, m2Data);
        }
    }
}
