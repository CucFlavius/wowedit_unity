using Assets.WoWEditSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data.CASC
{
    public static partial class Casc
    {
        public static void LoadWoWRootFile()
        {
            var rootFilePath = $@"{SettingsManager<Configuration>.Config.CachePath}\Root_{WoWVersion}.bin";
            // not cached //
            if (!File.Exists(rootFilePath))
            {
                if (WoWRootKey == null)
                {
                    Debug.Log("Error - WoWRootKey null");
                    return;
                }
                // convert root key string to byte array
                byte[] WoWRootKeyByte = ToByteArray(WoWRootKey);
                if (WoWRootKeyByte == null)
                {
                    Debug.Log("Error - WoWRootKey null");
                    return;
                }
                //// Extract Root File from BLTE and Read its Data ////
                var fs = GetEncodingData(ByteString(WoWRootKeyByte));
                StreamToFile(fs, rootFilePath);
                ReadRootFile(fs);
            }
            // cached //
            else if (File.Exists(rootFilePath))
            {
                FileStream fs1 = File.OpenRead(rootFilePath);
                ReadRootFile(fs1);
            }
        }

        public static void ReadRootFile(Stream fs)
        {
            if (fs != null)
            {
                rootFile.RootData       = new MultiDictionary<ulong, RootEntry>();
                using (BinaryReader br  = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int count = br.ReadInt32();

                        ContentFlags contentFlags = (ContentFlags)br.ReadUInt32();
                        LocaleFlags localeFlags = (LocaleFlags)br.ReadUInt32();

                        RootEntry[] entries = new RootEntry[count];
                        int[] fileDataIds = new int[count];

                        var fileDataIndex = 0;

                        for (var i = 0; i < count; ++i)
                        {
                            entries[i].LocaleFlags = localeFlags;
                            entries[i].ContentFlags = contentFlags;

                            fileDataIds[i] = fileDataIndex + br.ReadInt32();
                            entries[i].fileDataId = (uint)fileDataIds[i];

                            fileDataIndex = fileDataIds[i] + 1;
                        }

                        for (var i = 0; i < count; ++i)
                        {
                            entries[i].MD5      = br.Read<MD5Hash>();
                            entries[i].Lookup   = br.ReadUInt64();
                            rootFile.RootData.Add(entries[i].Lookup, entries[i]);
                        }
                    }
                }
            }
            fs.Close();
        }
    }
}
