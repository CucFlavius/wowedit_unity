using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CASCLib
{
    public sealed class CASCHandlerLite : CASCHandlerBase
    {
        private readonly Dictionary<ulong, MD5Hash> HashToKey = new Dictionary<ulong, MD5Hash>();
        private readonly Dictionary<int, ulong> FileDataIdToHash = new Dictionary<int, ulong>();
        private static readonly MD5HashComparer comparer = new MD5HashComparer();
        private readonly Dictionary<MD5Hash, IndexEntry> CDNIndexData;
        private readonly Dictionary<MD5Hash, IndexEntry> LocalIndexData;

        private CASCHandlerLite(CASCConfig config, LocaleFlags locale) : base(config)
        {
            if (config.GameType != CASCGameType.WoW)
                throw new Exception("Unsupported game " + config.BuildUID);

            Debug.Log("CASCHandlerLite: loading encoding data...");

            EncodingHandler EncodingHandler;

            using (var fs = OpenEncodingFile(this))
                EncodingHandler = new EncodingHandler(fs);

            Debug.Log($"CASCHandlerLite: loaded {EncodingHandler.Count} encoding data");

            Debug.Log("CASCHandlerLite: loading root data...");

            WowRootHandler RootHandler;

            using (var fs = OpenRootFile(EncodingHandler, this))
                RootHandler = new WowRootHandler(fs);

            Debug.Log($"CASCHandlerLite: loaded {RootHandler.Count} root data");

            RootHandler.SetFlags(locale, false, false);

            CDNIndexData = new Dictionary<MD5Hash, IndexEntry>(comparer);

            if (LocalIndex != null)
                LocalIndexData = new Dictionary<MD5Hash, IndexEntry>(comparer);

            RootEntry rootEntry;

            foreach (var entry in RootHandler.GetAllEntries())
            {
                rootEntry = entry.Value;

                if ((rootEntry.LocaleFlags == locale || (rootEntry.LocaleFlags & locale) != LocaleFlags.None) && (rootEntry.ContentFlags & ContentFlags.Alternate) == ContentFlags.None)
                {
                    if (EncodingHandler.GetEntry(rootEntry.MD5, out EncodingEntry enc))
                    {
                        if (!HashToKey.ContainsKey(entry.Key))
                        {
                            HashToKey.Add(entry.Key, enc.Key);
                            FileDataIdToHash.Add(RootHandler.GetFileDataIdByHash(entry.Key), entry.Key);

                            if (LocalIndex != null)
                            {
                                IndexEntry iLocal = LocalIndex.GetIndexInfo(enc.Key);

                                if (iLocal != null && !LocalIndexData.ContainsKey(enc.Key))
                                    LocalIndexData.Add(enc.Key, iLocal);
                            }

                            IndexEntry iCDN = CDNIndex.GetIndexInfo(enc.Key);

                            if (iCDN != null && !CDNIndexData.ContainsKey(enc.Key))
                                CDNIndexData.Add(enc.Key, iCDN);
                        }
                    }
                }
            }

            CDNIndex.Clear();
            //CDNIndex = null;
            LocalIndex?.Clear();
            LocalIndex = null;
            RootHandler.Clear();
            RootHandler = null;
            EncodingHandler.Clear();
            EncodingHandler = null;
            GC.Collect();

            Debug.Log($"CASCHandlerLite: loaded {HashToKey.Count} files");
        }

        protected override Stream OpenFileOnline(MD5Hash key)
        {
            IndexEntry idxInfo = CDNIndex.GetIndexInfo(key);

            if (idxInfo == null)
                CDNIndexData.TryGetValue(key, out idxInfo);

            return OpenFileOnlineInternal(idxInfo, key);
        }

        protected override Stream GetLocalDataStream(MD5Hash key)
        {
            IndexEntry idxInfo;

            if (LocalIndex != null)
                idxInfo = LocalIndex.GetIndexInfo(key);
            else
                LocalIndexData.TryGetValue(key, out idxInfo);

            return GetLocalDataStreamInternal(idxInfo, key);
        }

        protected override void ExtractFileOnline(MD5Hash key, string path, string name)
        {
            IndexEntry idxInfo = CDNIndex.GetIndexInfo(key);

            if (idxInfo == null)
                CDNIndexData.TryGetValue(key, out idxInfo);

            ExtractFileOnlineInternal(idxInfo, key, path, name);
        }

        public static CASCHandlerLite OpenStorage(LocaleFlags locale, CASCConfig config)
        {
            return Open(locale, config);
        }

        public static CASCHandlerLite OpenLocalStorage(string basePath, LocaleFlags locale, string product = null)
        {
            CASCConfig config = CASCConfig.LoadLocalStorageConfig(basePath);

            return Open(locale, config);
        }

        public static CASCHandlerLite OpenOnlineStorage(string product, LocaleFlags locale, string region = "us")
        {
            CASCConfig config = CASCConfig.LoadOnlineStorageConfig(product, region);

            return Open(locale, config);
        }

        private static CASCHandlerLite Open(LocaleFlags locale, CASCConfig config)
        {
            return new CASCHandlerLite(config, locale);
        }

        public override bool FileExists(int fileDataId) => FileDataIdToHash.ContainsKey(fileDataId);

        public override bool FileExists(string file) => FileExists(Hasher.ComputeHash(file));

        public override bool FileExists(ulong hash) => HashToKey.ContainsKey(hash);

        public override Stream OpenFile(int filedata)
        {
            if (FileDataIdToHash.TryGetValue(filedata, out ulong hash))
                return OpenFile(hash);

            return null;
        }

        public override Stream OpenFile(string name) => OpenFile(Hasher.ComputeHash(name));

        public override Stream OpenFile(ulong hash)
        {
            if (HashToKey.TryGetValue(hash, out MD5Hash key))
                return OpenFile(key);

            return null;
        }

        public override void SaveFileTo(ulong hash, string extractPath, string fullName)
        {
            if (HashToKey.TryGetValue(hash, out MD5Hash key))
            {
                SaveFileTo(key, extractPath, fullName);
                return;
            }

            if (CASCConfig.ThrowOnFileNotFound)
                throw new FileNotFoundException(fullName);
        }
    }
}
