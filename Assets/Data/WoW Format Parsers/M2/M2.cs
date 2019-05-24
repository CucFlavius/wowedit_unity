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
using CASCLib;
using Assets.UI.CASC;
using Assets.Data.DataLocal;

public static partial class M2
{
    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;
    public static List<string> LoadedBLPs = new List<string>();
    public static List<uint> LoadedBLPFileDataIds = new List<uint>();
    public static GameObject Casc;
    public static CASCHandler CascHandler;
    public static Jenkins96 Hasher = new Jenkins96();
    public static List<uint> SkinFiles = new List<uint>();
    public static DataLocalHandler Local = new DataLocalHandler();

    public static void Load(string dataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
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

    public static void Load(uint FileDataId, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Casc = GameObject.Find("[CASC]");
        CascHandler = Casc.GetComponent<CascHandler>().cascHandler;

        M2Data m2Data = new M2Data();
        M2Texture m2Tex = new M2Texture();

        m2Data.FileDataId = FileDataId;
        m2Data.uniqueID = uniqueID;
        m2Data.position = position;
        m2Data.rotation = rotation;
        m2Data.scale = scale;

        try
        {
            ThreadWorking = true;

            ParseM2_Root(FileDataId, m2Data, m2Tex);

            foreach (uint skinFile in SkinFiles)
                ParseM2_Skin(skinFile, m2Data);

            AllM2Data.Enqueue(m2Data);

            ThreadWorking = false;
        }
        catch (Exception ex)
        {
            Debug.Log("Error : Trying to parse M2 - " + FileDataId);
            Debug.LogException(ex);
        }
    }

    private static void ParseM2_Root(string dataPath, M2Data m2Data, M2Texture m2Tex)
    {
        var stream = Local.GetFileStream(dataPath);
        ParseM2(stream, m2Data, m2Tex);
    }
    
    private static void ParseM2_Root(uint fileDataID, M2Data m2Data, M2Texture m2Tex)
    {
        var stream = CascHandler.OpenFile(fileDataID);
        ParseM2(stream, m2Data, m2Tex);
    }

    private static void ParseM2(Stream stream, M2Data m2Data, M2Texture m2Tex)
    {
        long streamPos = 0;

        using (BinaryReader reader = new BinaryReader(stream))
        {
            while (streamPos < stream.Length)
            {
                stream.Position = streamPos;
                M2ChunkId chunkID = (M2ChunkId)reader.ReadUInt32();
                uint chunkSize = reader.ReadUInt32();

                streamPos = stream.Position + chunkSize;

                // Debug.Log($"M2: ChunkId -> {chunkID} ChunkSize -> {chunkSize} Position -> {reader.BaseStream.Position}:{stream.Position}");

                switch (chunkID)
                {
                    case M2ChunkId.MD21:
                        ReadMD21(reader, m2Data, m2Tex);
                        break;
                    case M2ChunkId.SFID:
                        ReadSFID(reader);
                        break;
                    case M2ChunkId.TXID:
                        ReadTXID(reader, m2Data);
                        break;
                    default:
                        SkipUnknownChunk(stream, chunkSize);
                        break;
                }
            }
        };
    }

    private static void ParseM2_Skin(string dataPath, M2Data m2Data)
    {
        var stream = Local.GetFileStream(dataPath);
        ParseSkin(stream, m2Data);
    }

    private static void ParseM2_Skin(uint skinFileId, M2Data m2Data)
    {
        var stream = CascHandler.OpenFile(skinFileId);
        ParseSkin(stream, m2Data);
    }

    private static void ParseSkin(Stream stream, M2Data m2Data)
    {
        using (BinaryReader reader = new BinaryReader(stream))
        {
            ParseSkin(reader, m2Data);
        }
    }
}
