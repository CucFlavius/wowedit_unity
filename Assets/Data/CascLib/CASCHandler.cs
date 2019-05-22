using System;
using System.IO;
using System.Linq;

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
            using (var fs = OpenEncodingFile(this))
                EncodingHandler = new EncodingHandler(fs);

            if ((CASCConfig.LoadFlags & LoadFlags.Download) != 0)
            {
                using (var fs = OpenDownloadFile(EncodingHandler, this))
                    DownloadHandler = new DownloadHandler(fs);
            }

            using (var fs = OpenRootFile(EncodingHandler, this))
                RootHandler = new WowRootHandler(fs);

            if ((CASCConfig.LoadFlags & LoadFlags.Install) != 0)
            {
                using (var fs = OpenInstallFile(EncodingHandler, this))
                    InstallHandler = new InstallHandler(fs);
            }
        }

        public static CASCHandler OpenStorage(CASCConfig config)
        {
            return new CASCHandler(config);
        }

        public override bool FileExists(int fileDataId)
        {
            if (Root is WowRootHandler rh)
                return FileExists(rh.GetHashByFileDataId(fileDataId));
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

        protected override Stream OpenFileOnline(MD5Hash key)
        {
            IndexEntry idxInfo = CDNIndex.GetIndexInfo(key);
            return OpenFileOnlineInternal(idxInfo, key);
        }

        protected override Stream GetLocalDataStream(MD5Hash key)
        {
            IndexEntry idxInfo = LocalIndex.GetIndexInfo(key);
            if (idxInfo == null)
                Console.WriteLine("Local index missing: {0}", key.ToHexString());

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
