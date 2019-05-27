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
    public static List<uint> SkinFiles = new List<uint>();
    public static DataLocalHandler Local = new DataLocalHandler();

    public static void Load(uint FileDataId, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale, CASCHandler Handler)
    {
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

            ParseM2_Root(FileDataId, m2Data, m2Tex, Handler);

            foreach (uint skinFile in SkinFiles)
                ParseM2_Skin(skinFile, m2Data, Handler);

            AllM2Data.Enqueue(m2Data);

            ThreadWorking = false;
        }
        catch (Exception ex)
        {
            Debug.Log("Error : Trying to parse M2 - " + FileDataId);
            Debug.LogException(ex);
        }
    }
    private static void ParseM2_Root(uint fileDataID, M2Data m2Data, M2Texture m2Tex, CASCHandler CascHandler)
    {
        long streamPos = 0;

        using (var stream = CascHandler.OpenFile(fileDataID))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            while (streamPos < stream.Length)
            {
                stream.Position = streamPos;
                M2ChunkId chunkID = (M2ChunkId)reader.ReadUInt32();
                uint chunkSize = reader.ReadUInt32();

                streamPos = stream.Position + chunkSize;

                switch (chunkID)
                {
                    case M2ChunkId.MD21:
                        ReadMD21(reader, m2Data, m2Tex);
                        break;
                    case M2ChunkId.SFID:
                        ReadSFID(reader, chunkSize);
                        break;
                    case M2ChunkId.TXID:
                        ReadTXID(reader, m2Data, CascHandler, chunkSize);
                        break;
                    default:
                        SkipUnknownChunk(stream, chunkSize);
                        break;
                }
            }
        };
    }

    private static void ParseM2_Skin(uint skinFileId, M2Data m2Data, CASCHandler CascHandler)
    {
        var stream = CascHandler.OpenFile(skinFileId);
        using (BinaryReader reader = new BinaryReader(stream))
        {
            ParseSkin(reader, m2Data);
        }
    }
}
