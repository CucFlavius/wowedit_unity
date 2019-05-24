using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CASCLib
{
    [Flags]
    public enum LocaleFlags : uint
    {
        All = 0xFFFFFFFF,
        None = 0,
        //Unk_1 = 0x1,
        enUS = 0x2,
        koKR = 0x4,
        //Unk_8 = 0x8,
        frFR = 0x10,
        deDE = 0x20,
        zhCN = 0x40,
        esES = 0x80,
        zhTW = 0x100,
        enGB = 0x200,
        enCN = 0x400,
        enTW = 0x800,
        esMX = 0x1000,
        ruRU = 0x2000,
        ptBR = 0x4000,
        itIT = 0x8000,
        ptPT = 0x10000,
        enSG = 0x20000000, // custom
        plPL = 0x40000000, // custom
        All_WoW = enUS | koKR | frFR | deDE | zhCN | esES | zhTW | enGB | esMX | ruRU | ptBR | itIT | ptPT
    }

    public enum Locale
    {
        enUS = 0,
        koKR = 1,
        frFR = 2,
        deDE = 3,
        zhCN = 4,
        zhTW = 5,
        esES = 6,
        esMX = 7,
        ruRU = 8,
        None = 9,
        ptBR = 10,
        itIT = 11,

        Total
    }

    [Flags]
    public enum ContentFlags : uint
    {
        None = 0,
        F00000001 = 0x1, // seen on *.wlm files
        F00000002 = 0x2,
        F00000004 = 0x4,
        Windows = 0x8, // added in 7.2.0.23436
        MacOS = 0x10, // added in 7.2.0.23436
        Alternate = 0x80, // many chinese models have this flag
        F00000100 = 0x100, // apparently client doesn't load files with this flag
        F00000800 = 0x800, // only seen on UpdatePlugin files
        Encrypted = 0x8000000, // encrypted may be?
        NoNameHash = 0x10000000, // doesn't have name hash?
        F20000000 = 0x20000000, // added in 21737, used for many cinematics
        Bundle = 0x40000000, // not related to wow, used as some old overwatch hack
        NotCompressed = 0x80000000 // sounds have this flag
    }

    public unsafe struct MD5Hash
    {
        public fixed byte Value[16];
    }

    public struct RootEntry
    {
        public MD5Hash MD5;
        public ContentFlags ContentFlags;
        public LocaleFlags LocaleFlags;
    }

    class FileDataHash
    {
        public static ulong ComputeHash(uint fileDataId)
        {
            ulong baseOffset = 0xCBF29CE484222325UL;

            for (int i = 0; i < 4; i++)
            {
                baseOffset = 0x100000001B3L * (((fileDataId >> (8 * i)) & 0xFF) ^ baseOffset);
            }

            return baseOffset;
        }
    }

    public class WowRootHandler : RootHandlerBase
    {
        private MultiDictionary<uint, RootEntry> RootData = new MultiDictionary<uint, RootEntry>();
        private Dictionary<uint, ulong> FileDataStore = new Dictionary<uint, ulong>();
        private Dictionary<ulong, uint> FileDataStoreReverse = new Dictionary<ulong, uint>();
        private HashSet<ulong> UnknownFiles = new HashSet<ulong>();
        private Dictionary<string, uint> ListFileEntry = new Dictionary<string, uint>();
        private string ListFile = "listfile.csv";
        private string listUrl = "https://wow.tools/casc/listfile/download/csv/build?buildConfig=54b3dc4ced90d45071f72a05fecfd063";
        private WebClient client = new WebClient();

        public override Dictionary<string, uint> ListFileEntries => ListFileEntry;
        public override uint Count => (uint)RootData.Count;
        public override uint CountTotal => (uint)RootData.Sum(re => re.Value.Count);
        public override uint CountUnknown => (uint)UnknownFiles.Count;

        public WowRootHandler(BinaryReader stream)
        {
            uint magic = stream.ReadUInt32();

            uint numFilesTotal = 0, numFilesWithNameHash = 0, numFilesRead = 0;

            const uint TSFMMagic = 0x4D465354;

            if (magic == TSFMMagic)
            {
                numFilesTotal           = stream.ReadUInt32();
                numFilesWithNameHash    = stream.ReadUInt32();
            }
            else
            {
                stream.BaseStream.Position -= 4;
            }

            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                uint count = stream.ReadUInt32();

                numFilesRead += count;

                ContentFlags contentFlags = (ContentFlags)stream.ReadUInt32();
                LocaleFlags localeFlags = (LocaleFlags)stream.ReadUInt32();

                if (localeFlags == LocaleFlags.None)
                    throw new Exception("block.LocaleFlags == LocaleFlags.None");

                if (contentFlags != ContentFlags.None && (contentFlags & (ContentFlags.F00000001 | ContentFlags.Windows | ContentFlags.MacOS | ContentFlags.Alternate | ContentFlags.NotCompressed | ContentFlags.NoNameHash | ContentFlags.F20000000)) == 0)
                    throw new Exception("block.ContentFlags != ContentFlags.None");

                RootEntry[] entries = new RootEntry[count];
                uint[] filedataIds = new uint[count];

                uint fileDataIndex = 0;

                for (var i = 0; i < count; ++i)
                {
                    entries[i].LocaleFlags = localeFlags;
                    entries[i].ContentFlags = contentFlags;

                    filedataIds[i] = fileDataIndex + stream.ReadUInt32();
                    fileDataIndex  = filedataIds[i] + 1;
                }

                //Console.WriteLine("Block: {0} {1} (size {2})", block.ContentFlags, block.LocaleFlags, count);

                ulong[] nameHashes = null;

                if (magic == TSFMMagic)
                {
                    for (var i = 0; i < count; ++i)
                        entries[i].MD5 = stream.Read<MD5Hash>();

                    if (numFilesRead > numFilesTotal - numFilesWithNameHash)
                    {
                        nameHashes = new ulong[count];

                        for (var i = 0; i < count; ++i)
                            nameHashes[i] = stream.ReadUInt64();
                    }
                }
                else
                {
                    nameHashes = new ulong[count];

                    for (var i = 0; i < count; ++i)
                    {
                        entries[i].MD5 = stream.Read<MD5Hash>();
                        nameHashes[i] = stream.ReadUInt64();
                    }
                }

                for (var i = 0; i < count; ++i)
                {
                    uint fileDataId = filedataIds[i];

                    //Logger.WriteLine("filedataid {0}", fileDataId);

                    ulong hash;

                    if (nameHashes == null)
                    {
                        hash = FileDataHash.ComputeHash(fileDataId);
                    }
                    else
                    {
                        hash = nameHashes[i];
                    }

                    RootData.Add(fileDataId, entries[i]);

                    //Console.WriteLine("File: {0:X8} {1:X16} {2}", entries[i].FileDataId, hash, entries[i].MD5.ToHexString());

                    if (FileDataStore.TryGetValue(fileDataId, out ulong hash2))
                    {
                        if (hash2 == hash)
                        {
                            // duplicate, skipping
                        }
                        else
                        {
                            Console.WriteLine("ERROR: got multiple hashes for filedataid {0}", fileDataId);
                        }
                        continue;
                    }

                    FileDataStore.Add(fileDataId, hash);
                    FileDataStoreReverse.Add(hash, fileDataId);

                    if (nameHashes != null)
                    {
                        // generate our custom hash as well so we can still find file without calling GetHashByFileDataId in some weird cases
                        ulong fileDataHash = FileDataHash.ComputeHash(fileDataId);
                        FileDataStoreReverse.Add(fileDataHash, fileDataId);
                    }
                }
            }
        }

        public override IEnumerable<KeyValuePair<ulong, RootEntry>> GetAllEntries()
        {
            foreach (var set in RootData)
                foreach (var entry in set.Value)
                    yield return new KeyValuePair<ulong, RootEntry>(FileDataStore[set.Key], entry);
        }


        public override IEnumerable<RootEntry> GetAllEntries(ulong hash)
        {
            if (!FileDataStoreReverse.TryGetValue(hash, out uint fileDataId))
                yield break;

            if (!RootData.TryGetValue(fileDataId, out List<RootEntry> result))
                yield break;

            foreach (var entry in result)
                yield return entry;
        }

        public IEnumerable<RootEntry> GetEntriesByFileDataId(uint fileDataId) => GetEntries(GetHashByFileDataId(fileDataId));

        // Returns only entries that match current locale and content flags
        public override IEnumerable<RootEntry> GetEntries(ulong hash)
        {
            var rootInfos = GetAllEntries(hash);

            if (!rootInfos.Any())
                yield break;

            var rootInfosLocale = rootInfos.Where(re => (re.LocaleFlags & Locale) != LocaleFlags.None);

            if (rootInfosLocale.Count() > 1)
            {
                IEnumerable<RootEntry> rootInfosLocaleOverride;

                if (OverrideArchive)
                    rootInfosLocaleOverride = rootInfosLocale.Where(re => (re.ContentFlags & ContentFlags.Alternate) != ContentFlags.None);
                else
                    rootInfosLocaleOverride = rootInfosLocale.Where(re => (re.ContentFlags & ContentFlags.Alternate) == ContentFlags.None);

                if (rootInfosLocaleOverride.Any())
                    rootInfosLocale = rootInfosLocaleOverride;
            }

            foreach (var entry in rootInfosLocale)
                yield return entry;
        }

        public bool FileExist(uint fileDataId) => RootData.ContainsKey(fileDataId);

        public override ulong GetHashByFileDataId(uint FileDataId)
        {
            FileDataStore.TryGetValue(FileDataId, out ulong hash);
            return hash;
        }

        public override uint GetFileDataIdByHash(ulong hash)
        {
            FileDataStoreReverse.TryGetValue(hash, out uint fid);
            return fid;
        }

        public override uint GetFileDataIdByName(string name) => GetFileDataIdByHash(Hasher.ComputeHash(name));

        public override void LoadListFile()
        {
            if (!File.Exists(ListFile))
                client.DownloadFile(listUrl, ListFile);

            bool isCsv = Path.GetExtension(ListFile) == ".csv";

            using (var fs2 = File.Open(ListFile, FileMode.Open))
            using (var sr = new StreamReader(fs2))
            {
                string line;

                char[] splitChar = isCsv ? new char[] { ';' } : new char[] { ' ' };

                while ((line = sr.ReadLine()) != null)
                {
                    string[] tokens = line.Split(splitChar, 2);

                    if (tokens.Length != 2)
                    {
                        Console.WriteLine($"Invalid line in listfile: {line}");
                        continue;
                    }

                    if (!uint.TryParse(tokens[0], out uint fileDataId))
                    {
                        Console.WriteLine($"Invalid line in listfile: {line}");
                        continue;
                    }

                    // skip invalid names
                    if (!RootData.ContainsKey(fileDataId))
                    {
                        Console.WriteLine($"Invalid fileDataId in listfile: {line}");
                        continue;
                    }

                    string file = tokens[1];

                    ListFileEntry.Add(tokens[1], uint.Parse(tokens[0]));

                    ulong fileHash = FileDataStore[fileDataId];

                    if (!CASCFile.Files.ContainsKey(fileHash))
                        CASCFile.Files.Add(fileHash, new CASCFile(fileHash, file));
                    else
                        Debug.Log($"Duplicate fileDataId {fileDataId} detected: {line}");
                }

                Debug.Log($"WowRootHandler: Loaded {ListFileEntry.Count} file names");
                Debug.Log($"WowRootHandler: Loaded {CASCFile.Files.Count} valid file names");
            }
        }

        protected override CASCFolder CreateStorageTree()
        {
            var root = new CASCFolder("root");

            // Reset counts
            CountSelect = 0;
            UnknownFiles.Clear();

            foreach (var rootEntry in RootData)
            {
                var rootInfosLocale = rootEntry.Value.Where(re => (re.LocaleFlags & Locale) != LocaleFlags.None);

                if (rootInfosLocale.Count() > 1)
                {
                    IEnumerable<RootEntry> rootInfosLocaleOverride;

                    if (OverrideArchive)
                        rootInfosLocaleOverride = rootInfosLocale.Where(re => (re.ContentFlags & ContentFlags.Alternate) != ContentFlags.None);
                    else
                        rootInfosLocaleOverride = rootInfosLocale.Where(re => (re.ContentFlags & ContentFlags.Alternate) == ContentFlags.None);

                    if (rootInfosLocaleOverride.Any())
                        rootInfosLocale = rootInfosLocaleOverride;
                }

                if (!rootInfosLocale.Any())
                    continue;

                string filename;

                ulong hash = FileDataStore[rootEntry.Key];

                if (!CASCFile.Files.TryGetValue(hash, out CASCFile file))
                {
                    filename = "unknown\\" + "FILEDATA_" + rootEntry.Key;

                    UnknownFiles.Add(hash);
                }
                else
                {
                    filename = file.FullName;
                }

                CreateSubTree(root, hash, filename);
                CountSelect++;
            }

            Console.WriteLine($"WowRootHandler: {CountUnknown} filenames missing for locale {Locale}.");
            
            return root;
        }

        public bool IsUnknownFile(ulong hash) => UnknownFiles.Contains(hash);

        public override void Clear()
        {
            RootData.Clear();
            RootData = null;
            FileDataStore.Clear();
            FileDataStore = null;
            FileDataStoreReverse.Clear();
            FileDataStoreReverse = null;
            UnknownFiles.Clear();
            UnknownFiles = null;
            Root?.Entries.Clear();
            Root = null;
            CASCFile.Files.Clear();
        }
    }
}
