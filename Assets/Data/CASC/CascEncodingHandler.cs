using Assets.WoWEditSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.CASC
{
    public static partial class Casc
    {
        public static void LoadEncodingFile()
        {
            string encodingFilePath = $@"{SettingsManager<Configuration>.Config.CachePath}\Encoding_{WoWVersion}.bin";
            // if not cached //
            if (!File.Exists(encodingFilePath))
            {
                // convert encoding key string to byte array
                Debug.Log("Encoding key : " + WoWEncodingKey);
                byte[] WoWEncodingKeyBytes = ToByteArray(WoWEncodingKey);
                //// Extract Encoding File from BLTE and Read its Data ////
                var fs = OpenWoWFile(WoWEncodingKeyBytes);
                // cache //
                StreamToFile(fs, encodingFilePath);
                // read //
                ReadEncodingFile(fs);
            }
            // if cached //
            else if (File.Exists(encodingFilePath))
            {
                FileStream fs1 = File.OpenRead(encodingFilePath);
                // read //
                ReadEncodingFile(fs1);
            }
        }

        public static void ReadEncodingFile(Stream fs)
        {
            if (fs != null)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.Skip(2);
                    byte Version = br.ReadByte();
                    byte CKeyLength = br.ReadByte();
                    byte EKeyLength = br.ReadByte();
                    int CKeyPageSize = br.ReadInt16BE() * 1024; // KB to bytes
                    int EKeyPageSize = br.ReadInt16BE() * 1024; // KB to bytes
                    int CKeyPageCount = br.ReadInt32BE();
                    int EKeyPageCount = br.ReadInt32BE();
                    byte unk1 = br.ReadByte(); // must be 0
                    int ESpecBlockSize = br.ReadInt32BE();

                    br.Skip(ESpecBlockSize);
                    br.Skip(CKeyPageCount * 32);

                    long chunkStart = br.BaseStream.Position;

                    for (int i = 0; i < CKeyPageCount; ++i)
                    {
                        byte keysCount;

                        while ((keysCount = br.ReadByte()) != 0)
                        {
                            long fileSize = br.ReadInt40BE();
                            MD5Hash cKey = br.Read<MD5Hash>();

                            EncodingEntry entry = new EncodingEntry()
                            {
                                Size = fileSize
                            };

                            for (int ki = 0; ki < keysCount; ++ki)
                            {
                                MD5Hash eKey = br.Read<MD5Hash>();

                                if (ki == 0)
                                    entry.Key = eKey;
                            }

                            EncodingData.Add(cKey, entry);
                        }

                        long remaining = CHUNK_SIZE - ((br.BaseStream.Position - chunkStart) % CHUNK_SIZE);

                        if (remaining > 1)
                            br.BaseStream.Position += remaining;
                    }

                    br.Skip(EKeyPageCount * 32);
                    long chunkStart2 = br.BaseStream.Position;

                    for (int i = 0; i < EKeyPageCount; ++i)
                    {
                        byte[] eKey = br.ReadBytes(16);
                        int eSpecIndex = br.ReadInt32BE();
                        long fileSize = br.ReadInt40BE();

                        long remaining = CHUNK_SIZE - ((br.BaseStream.Position - chunkStart2) % CHUNK_SIZE);

                        if (remaining > 1)
                            br.BaseStream.Position += remaining;
                    }
                }
            }
            else
            {
                Debug.Log("ReadEncodingFile null");
            }
        }
    }
}
