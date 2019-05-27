using Assets.Data.DataLocal;
using Assets.Data.WoW_Format_Parsers;
using Assets.UI.CASC;
using CASCLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT
{
    // WDT files specify exactly which map tiles are present in a world, if any, and can also reference a "global" WMO. 
    // They have a chunked file structure.
    public static Jenkins96 Hasher = new Jenkins96();
    public static CASCHandler CASC;
    public static DataLocalHandler Local = new DataLocalHandler();

    public static bool ParseWDT(uint FileDataId)
    {
        long streamPos = 0;
        CASC = GameObject.Find("[CASC]").GetComponent<CascHandler>().cascHandler;

        if (CASC.FileExists(FileDataId))
        {
            using (var stream = CASC.OpenFile(FileDataId))
            using (var reader = new BinaryReader(stream))
            {
                WDTflagsdata WDTFlags = new WDTflagsdata();
                while (streamPos < stream.Length)
                {
                    stream.Position = streamPos;
                    WDTChunkId ChunkId = (WDTChunkId)reader.ReadUInt32();
                    uint ChunkSize = reader.ReadUInt32();

                    streamPos = stream.Position + ChunkSize;

                    switch (ChunkId)
                    {
                        case WDTChunkId.MVER:
                            ReadMVER(reader);
                            break;
                        case WDTChunkId.MPHD:
                            ReadMPHD(reader, WDTFlags);
                            break;
                        case WDTChunkId.MAIN:
                            ReadMAIN(reader, WDTFlags);
                            break;
                        case WDTChunkId.MAID:
                            ReadMAID(reader, FileDataId);
                            break;
                        default:
                            SkipUnknownChunk(stream, ChunkId, ChunkSize);
                            break;
                    }
                }
                
                if (!Flags.ContainsKey(FileDataId))
                    Flags.Add(FileDataId, WDTFlags);
            }
            return true;
        }
        else
            return false;
    }

    // Move the stream forward upon finding unknown chunks //
    public static void SkipUnknownChunk(Stream stream, WDTChunkId chunkID, uint chunkSize)
    {
        if (Enum.IsDefined(typeof(WDTChunkId), chunkID))
            Debug.Log($"Missing chunk ID : {chunkID}");

        stream.Seek(chunkSize, SeekOrigin.Current);
    }
}
