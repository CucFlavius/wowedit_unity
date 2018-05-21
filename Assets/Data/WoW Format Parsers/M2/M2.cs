using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public static partial class M2
{
    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;

    public static void Load(string dataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        M2Data m2Data = new M2Data();

        m2Data.dataPath = dataPath;
        m2Data.uniqueID = uniqueID;
        m2Data.position = position;
        m2Data.rotation = rotation;
        m2Data.scale = scale;

        try
        {
            ThreadWorking = true;

            ParseM2_Root(dataPath, m2Data);
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

    private static void ParseM2_Root(string dataPath, M2Data m2Data)
    {
        StreamTools s = new StreamTools();
        string path = Casc.GetFile(dataPath);
        byte[] M2MainData = File.ReadAllBytes(path);
        long streamPosition = 0;

        using (MemoryStream ms = new MemoryStream(M2MainData))
        {
            while (streamPosition < ms.Length)
            {
                ms.Position = streamPosition;
                int chunkID = s.ReadLong(ms);
                int chunkSize = s.ReadLong(ms);

                streamPosition = ms.Position + chunkSize;

                switch (chunkID)
                {
                    case (int)ChunkID.M2ChunkID.MD21:
                        ReadMD21(ms, m2Data);
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
            {
                skinCount++;
            }
            else
            {
                break;
            }
        }

        if (skinCount > 0)
        {
            // Load only skin00 for now //
            string skin0dataPath = directoryPath + @"\" + noExtension + "00" + ".skin";
            string skin0path = Casc.GetFile(skin0dataPath);
            byte[] M2SkinData = File.ReadAllBytes(skin0path);

            using (MemoryStream ms = new MemoryStream(M2SkinData))
            {
                ParseSkin(ms, m2Data);
            }
        }
    }
}