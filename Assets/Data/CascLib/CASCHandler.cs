using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CASCLib
{
    public sealed class CASCHandler : CASCHandlerBase
    {
        private EncodingHandler EncodingHandler;
        private DownloadHandler DownloadHandler;
        private RootHandlerBase RootHandler;
        private InstallHandler InstallHandler;

        public EncodingHandler Encoding => EncodingHandler;
        public DownloadHandler Download => DownloadHandler;
        public RootHandlerBase Root => RootHandler;
        public InstallHandler Install => InstallHandler;

        private CASCHandler(CASCConfig config) : base(config)
        {
            Debug.Log("CASCHandler: loading encoding data...");

            using (var fs = OpenEncodingFile(this))
                EncodingHandler = new EncodingHandler(fs);

            Debug.Log($"CASCHandler: loaded {EncodingHandler.Count} encoding data");

            if ((CASCConfig.LoadFlags & LoadFlags.Download) != 0)
            {
                Debug.Log("CASCHandler: loading download data...");

                using (var fs = OpenDownloadFile(EncodingHandler, this))
                    DownloadHandler = new DownloadHandler(fs);

                Debug.Log($"CASCHandler: loaded {EncodingHandler.Count} download data");
            }

            Debug.Log("CASCHandler: loading root data...");

            using (var fs = OpenRootFile(EncodingHandler, this))
            {
                if (config.GameType == CASCGameType.WoW)
                    RootHandler = new WowRootHandler(fs);
                else
                {
                    using (var ufs = new FileStream("unk_root", FileMode.Create))
                        fs.BaseStream.CopyTo(ufs);
                    throw new Exception("Unsupported game " + config.BuildUID);
                }
            }

            Debug.Log($"CASCHandler: loaded {RootHandler.Count} root data");

            if ((CASCConfig.LoadFlags & LoadFlags.Install) != 0)
            {
                Debug.Log("CASCHandler: loading install data...");

                using (var fs = OpenInstallFile(EncodingHandler, this))
                    InstallHandler = new InstallHandler(fs);

                Debug.Log($"CASCHandler: loaded {InstallHandler.Count} install data");
            }
        }

        public static CASCHandler OpenStorage(CASCConfig config, BackgroundWorkerEx worker = null) => Open(config);

        public static CASCHandler OpenLocalStorage(string basePath, string product = null)
        {
            CASCConfig config = CASCConfig.LoadLocalStorageConfig(basePath, product);

            return Open(config);
        }

        public static CASCHandler OpenOnlineStorage(string product, string region = "us")
        {
            CASCConfig config = CASCConfig.LoadOnlineStorageConfig(product, region);

            return Open(config);
        }

        private static CASCHandler Open(CASCConfig config)
        {
            return new CASCHandler(config);
        }

        public override bool FileExists(int fileDataId)
        {
            if (Root is WowRootHandler rh)
                return rh.FileExist(fileDataId);
            return false;
        }

        public override bool FileExists(string file) => FileExists(Hasher.ComputeHash(file));

        public override bool FileExists(ulong hash) => RootHandler.GetAllEntries(hash).Any();

        public bool GetEncodingEntry(ulong hash, out EncodingEntry enc)
        {
            var rootInfos = RootHandler.GetEntries(hash);
            if (rootInfos.Any())
                return EncodingHandler.GetEntry(rootInfos.First().MD5, out enc);

            if ((CASCConfig.LoadFlags & LoadFlags.Install) != 0)
            {
                var installInfos = Install.GetEntries().Where(e => Hasher.ComputeHash(e.Name) == hash && e.Tags.Any(t => t.Type == 1 && t.Name == RootHandler.Locale.ToString()));
                if (installInfos.Any())
                    return EncodingHandler.GetEntry(installInfos.First().MD5, out enc);

                installInfos = Install.GetEntries().Where(e => Hasher.ComputeHash(e.Name) == hash);
                if (installInfos.Any())
                    return EncodingHandler.GetEntry(installInfos.First().MD5, out enc);
            }

            enc = default;
            return false;
        }

        public override Stream OpenFile(int fileDataId)
        {
            if (Root is WowRootHandler rh)
                return OpenFile(rh.GetHashByFileDataId(fileDataId));

            if (CASCConfig.ThrowOnFileNotFound)
                throw new FileNotFoundException("FileData: " + fileDataId.ToString());
            return null;
        }

        public override Stream OpenFile(string name) => OpenFile(Hasher.ComputeHash(name));

        public override Stream OpenFile(ulong hash)
        {
            if (GetEncodingEntry(hash, out EncodingEntry encInfo))
                return OpenFile(encInfo.Key);

            if (CASCConfig.ThrowOnFileNotFound)
                throw new FileNotFoundException(string.Format("{0:X16}", hash));
            return null;
        }

        public override void SaveFileTo(ulong hash, string extractPath, string fullName)
        {
            if (GetEncodingEntry(hash, out EncodingEntry encInfo))
            {
                SaveFileTo(encInfo.Key, extractPath, fullName);
                return;
            }

            if (CASCConfig.ThrowOnFileNotFound)
                throw new FileNotFoundException(fullName);
        }

        protected override Stream OpenFileOnline(MD5Hash key)
        {
            IndexEntry idxInfo = CDNIndex.GetIndexInfo(key);
            return OpenFileOnlineInternal(idxInfo, key);
        }

        protected override Stream GetLocalDataStream(MD5Hash key)
        {
            IndexEntry idxInfo = LocalIndex.GetIndexInfo(key);
            if (idxInfo == null)
                Debug.Log($"Local index missing: {key.ToHexString()}");

            return GetLocalDataStreamInternal(idxInfo, key);
        }

        protected override void ExtractFileOnline(MD5Hash key, string path, string name)
        {
            IndexEntry idxInfo = CDNIndex.GetIndexInfo(key);
            ExtractFileOnlineInternal(idxInfo, key, path, name);
        }

        public void Clear()
        {
            CDNIndex?.Clear();
            CDNIndex = null;

            foreach (var stream in DataStreams)
                stream.Value.Dispose();

            DataStreams.Clear();

            EncodingHandler?.Clear();
            EncodingHandler = null;

            InstallHandler?.Clear();
            InstallHandler = null;

            LocalIndex?.Clear();
            LocalIndex = null;

            RootHandler?.Clear();
            RootHandler = null;

            DownloadHandler?.Clear();
            DownloadHandler = null;
        }
    }
}
