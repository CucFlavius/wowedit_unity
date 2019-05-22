using Assets.Data.WoW_Format_Parsers;
using Assets.UI.CASC;
using CASCLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT
{
    // WDT files specify exactly which map tiles are present in a world, if any, and can also reference a "global" WMO. 
    // They have a chunked file structure.
    public static Jenkins96 Hasher = new Jenkins96();
    public static GameObject CASC;

    public static void ParseWDT (string Path, string MapName)
    {
        CASC = GameObject.Find("[CASC]");

        string WDTpath = Path + MapName + ".wdt";
        ulong hash = Hasher.ComputeHash(WDTpath);

        if (CASC.GetComponent<CascHandler>().cascHandler.FileExists(hash))
        {
            var stream = CASC.GetComponent<CascHandler>().cascHandler.OpenFile(hash);
            if (stream != null)
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    WDTflagsdata WDTFlags = new WDTflagsdata();
                    while (stream.Position < stream.Length)
                    {
                        WDTChunkId ChunkId  = (WDTChunkId)reader.ReadUInt32();
                        uint ChunkSize       = reader.ReadUInt32();

                        switch (ChunkId)
                        {
                            case WDTChunkId.MVER:
                                ReadMVER(reader);
                                break;
                            case WDTChunkId.MPHD:
                                ReadMPHD(reader, MapName, WDTFlags);
                                break;
                            case WDTChunkId.MAIN:
                                ReadMAIN(reader, WDTFlags);
                                break;
                            case WDTChunkId.MAID:
                                ReadMAID(reader);
                                break;
                            default:
                                SkipUnknownChunk(reader, ChunkId, ChunkSize);
                                break;
                        }
                    }
                    Flags.Add(MapName, WDTFlags);
                }
            }
        }
    }

    // Move the stream forward upon finding unknown chunks //
    public static void SkipUnknownChunk(BinaryReader reader, WDTChunkId chunkID, uint chunkSize)
    {
        Debug.Log("Missing chunk ID : " + chunkID);
        reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
    }
}
