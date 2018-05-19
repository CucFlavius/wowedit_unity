using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class M2
{
    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;

    public static void Load()
    {
        ParseM2_Root("character/draenei/female/draeneifemale.m2");
    }

    private static void ParseM2_Root(string dataPath)
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
                        ReadMD21(ms);
                        break;
                    default:
                        SkipUnknownChunk(ms, chunkID, chunkSize);
                        break;
                }
            }
        };
    }
}