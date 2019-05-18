using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public static ulong ComputeHash(int fileDataId)
        {
            ulong baseOffset = 0xCBF29CE484222325UL;

            for (int i = 0; i < 4; i++)
            {
                baseOffset = 0x100000001B3L * ((((uint)fileDataId >> (8 * i)) & 0xFF) ^ baseOffset);
            }

            return baseOffset;
        }
    }

    public class WowRootHandler : RootHandlerBase
    {
        private MultiDictionary<int, RootEntry> RootData = new MultiDictionary<int, RootEntry>();
        private Dictionary<int, ulong> FileDataStore = new Dictionary<int, ulong>();
        private Dictionary<ulong, int> FileDataStoreReverse = new Dictionary<ulong, int>();
        private HashSet<ulong> UnknownFiles = new HashSet<ulong>();

        public override int Count => RootData.Count;
        public override int CountTotal => RootData.Sum(re => re.Value.Count);
        public override int CountUnknown => UnknownFiles.Count;

        public WowRootHandler(BinaryReader stream, BackgroundWorkerEx worker)
        {
            worker?.ReportProgress(0, "Loading \"root\"...");

            int magic = stream.ReadInt32();

            int numFilesTotal = 0, numFilesWithNameHash = 0, numFilesRead = 0;

            const int TSFMMagic = 0x4D465354;

            if (magic == TSFMMagic)
            {
                numFilesTotal = stream.ReadInt32();
                numFilesWithNameHash = stream.ReadInt32();
            }
            else
            {
                stream.BaseStream.Position -= 4;
            }

            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                int count = stream.ReadInt32();

                numFilesRead += count;

                ContentFlags contentFlags = (ContentFlags)stream.ReadUInt32();
                LocaleFlags localeFlags = (LocaleFlags)stream.ReadUInt32();

                if (localeFlags == LocaleFlags.None)
                    throw new Exception("block.LocaleFlags == LocaleFlags.None");

                if (contentFlags != ContentFlags.None && (contentFlags & (ContentFlags.F00000001 | ContentFlags.Windows | ContentFlags.MacOS | ContentFlags.Alternate | ContentFlags.NotCompressed | ContentFlags.NoNameHash | ContentFlags.F20000000)) == 0)
                    throw new Exception("block.ContentFlags != ContentFlags.None");

                RootEntry[] entries = new RootEntry[count];
                int[] filedataIds = new int[count];

                int fileDataIndex = 0;

                for (var i = 0; i < count; ++i)
                {
                    entries[i].LocaleFlags = localeFlags;
                    entries[i].ContentFlags = contentFlags;

                    filedataIds[i] = fileDataIndex + stream.ReadInt32();
                    fileDataIndex = filedataIds[i] + 1;
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
                    int fileDataId = filedataIds[i];

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
                            Logger.WriteLine("ERROR: got miltiple hashes for filedataid {0}", fileDataId);
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

                worker?.ReportProgress((int)(stream.BaseStream.Position / (float)stream.BaseStream.Length * 100));
            }
        }

        public IEnumerable<RootEntry> GetAllEntriesByFileDataId(int fileDataId) => GetAllEntries(GetHashByFileDataId(fileDataId));

        public override IEnumerable<KeyValuePair<ulong, RootEntry>> GetAllEntries()
        {
            foreach (var set in RootData)
                foreach (var entry in set.Value)
                    yield return new KeyValuePair<ulong, RootEntry>(FileDataStore[set.Key], entry);
        }

        public override IEnumerable<RootEntry> GetAllEntries(ulong hash)
        {
            if (!FileDataStoreReverse.TryGetValue(hash, out int fileDataId))
                yield break;

            if (!RootData.TryGetValue(fileDataId, out List<RootEntry> result))
                yield break;

            foreach (var entry in result)
                yield return entry;
        }

        public IEnumerable<RootEntry> GetEntriesByFileDataId(int fileDataId) => GetEntries(GetHashByFileDataId(fileDataId));

        // Returns only entries that match current locale and override setting
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

        public bool FileExist(int fileDataId) => RootData.ContainsKey(fileDataId);

        public ulong GetHashByFileDataId(int fileDataId)
        {
            FileDataStore.TryGetValue(fileDataId, out ulong hash);
            return hash;
        }

        public int GetFileDataIdByHash(ulong hash)
        {
            FileDataStoreReverse.TryGetValue(hash, out int fid);
            return fid;
        }

        public int GetFileDataIdByName(string name) => GetFileDataIdByHash(Hasher.ComputeHash(name));

        public override void LoadListFile(string path, BackgroundWorkerEx worker = null)
        {
            //CASCFile.Files.Clear();

            using (var _ = new PerfCounter("WowRootHandler::LoadListFile()"))
            {
                worker?.ReportProgress(0, "Loading \"listfile\"...");

                if (!File.Exists(path))
                {
                    Logger.WriteLine("WowRootHandler: list file missing!");
                    return;
                }

                bool isCsv = Path.GetExtension(path) == ".csv";

                Logger.WriteLine($"WowRootHandler: loading listfile {path}...");

                using (var fs2 = File.Open(path, FileMode.Open))
                using (var sr = new StreamReader(fs2))
                {
                    string line;

                    char[] splitChar = isCsv ? new char[] { ';' } : new char[] { ' ' };

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(splitChar, 2);

                        if (tokens.Length != 2)
                        {
                            Logger.WriteLine($"Invalid line in listfile: {line}");
                            continue;
                        }

                        if (!int.TryParse(tokens[0], out int fileDataId))
                        {
                            Logger.WriteLine($"Invalid line in listfile: {line}");
                            continue;
                        }

                        // skip invalid names
                        if (!RootData.ContainsKey(fileDataId))
                        {
                            Logger.WriteLine($"Invalid fileDataId in listfile: {line}");
                            continue;
                        }

                        string file = tokens[1];

                        ulong fileHash = FileDataStore[fileDataId];

                        if (!CASCFile.Files.ContainsKey(fileHash))
                            CASCFile.Files.Add(fileHash, new CASCFile(fileHash, file));
                        else
                            Logger.WriteLine($"Duplicate fileDataId {fileDataId} detected: {line}");

                        worker?.ReportProgress((int)(sr.BaseStream.Position / (float)sr.BaseStream.Length * 100));
                    }
                }

                Logger.WriteLine($"WowRootHandler: loaded {CASCFile.Files.Count} valid file names");
            }
        }

        protected override CASCFolder CreateStorageTree()
        {
            var root = new CASCFolder("root");

            // Reset counts
            CountSelect = 0;
            UnknownFiles.Clear();

            // Create new tree based on specified locale
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

            Logger.WriteLine("WowRootHandler: {0} file names missing for locale {1}", CountUnknown, Locale);

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

        public override void Dump()
        {
            Logger.WriteLine("WowRootHandler Dump:");

            foreach (var fd in RootData.OrderBy(r => r.Key))
            {
                string name;

                if (FileDataStore.TryGetValue(fd.Key, out ulong hash) && CASCFile.Files.TryGetValue(hash, out CASCFile file))
                    name = file.FullName;
                else
                    name = $"FILEDATA_{fd.Key}";

                Logger.WriteLine("{0:D7} {1:X16} {2} {3}", fd.Key, fd.Key, fd.Value.Aggregate(LocaleFlags.None, (a, b) => a | b.LocaleFlags), name);

                foreach (var entry in fd.Value)
                {
                    Logger.WriteLine("\t{0} - {1} - {2}", entry.MD5.ToHexString(), entry.LocaleFlags, entry.ContentFlags);
                }
            }
        }
    }
}
