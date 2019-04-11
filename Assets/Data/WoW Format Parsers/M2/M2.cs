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
using Assets.Data.CASC;

public static partial class M2
{
    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;
    public static List<string> LoadedBLPs = new List<string>();

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
            ParseM2_Skin(dataPath, m2Data);
            AllM2Data.Enqueue(m2Data);

            ThreadWorking = false;
        }
        catch (Exception ex)
        {
            Debug.Log("Error : Trying to parse M2 - " + dataPath);
            Debug.LogException(ex);
        }
    }

    private static void ParseM2_Root(string dataPath, M2Data m2Data, M2Texture m2Tex)
    {
        int fdid = Casc.GetFileDataIdByName(dataPath);
        string path = Casc.GetFile(fdid);
        byte[] M2MainData = File.ReadAllBytes(path);
        long streamPosition = 0;

        using (MemoryStream ms = new MemoryStream(M2MainData))
        using (BinaryReader reader = new BinaryReader(ms))
        {
            while (streamPosition < ms.Length)
            {
                ms.Position     = streamPosition;
                M2ChunkId chunkID = (M2ChunkId)reader.ReadInt32();
                int chunkSize   = reader.ReadInt32();

                streamPosition = ms.Position + chunkSize;

                switch (chunkID)
                {
                    case M2ChunkId.MD21:
                        ReadMD21(reader, m2Data, m2Tex);
                        break;
                    case M2ChunkId.TXID:
                        ReadTXID(reader, m2Data);
                        break;
                    default:
                        SkipUnknownChunk(ms, chunkID, chunkSize);
                        break;
                }
            }
        };
    }

    private static void ParseM2_Skin (string dataPath, M2Data m2Data)
    {
        // check how many skins files there are //
        string noExtension = Path.GetFileNameWithoutExtension(dataPath);
        string directoryPath = Path.GetDirectoryName(dataPath);

        int skinCount = 0;
        for (int i = 0; i <= 20; i++)
        {
            string fileNumber = i.ToString("00");
            if (Casc.FileExists(directoryPath + @"\" + noExtension + fileNumber + ".skin"))
                skinCount++;
            else
                break;
        }

        if (skinCount > 0)
        {
            // Load only skin00 for now //
            string skinDataPath = directoryPath + @"\" + noExtension + "00" + ".skin";
            int fdid        = Casc.GetFileDataIdByName(dataPath);
            string skinPath = Casc.GetFile(fdid);
            byte[] M2SkinData = File.ReadAllBytes(skinPath);

            using (MemoryStream ms = new MemoryStream(M2SkinData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                ParseSkin(reader, m2Data);
            }
        }
    }
}
