using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CASCLib
{
    public abstract class CASCHandlerBase
    {
        protected LocalIndexHandler LocalIndex;
        protected CDNIndexHandler CDNIndex;

        protected static readonly Jenkins96 Hasher = new Jenkins96();

        protected readonly Dictionary<int, Stream> DataStreams = new Dictionary<int, Stream>();

        public CASCConfig Config { get; protected set; }

        public CASCHandlerBase(CASCConfig config)
        {
            Config = config;

            Debug.Log("CASCHandlerBase: Initializing Index Handler...");
            CDNIndex = CDNIndexHandler.Initialize(config);

            if (!config.OnlineMode)
            {
                CDNCache.Enabled = false;
                Debug.Log("CASCHandlerBase: Initializing Local Index Handler...");
                LocalIndex = LocalIndexHandler.Initialize(config);
            }
        }

        public abstract bool FileExists(uint fileDataId);
        public abstract bool FileExists(string file);
        public abstract bool FileExists(ulong hash);

        public abstract Stream OpenFile(uint filedata);
        public abstract Stream OpenFile(string name);
        public abstract Stream OpenFile(ulong hash);

        public Stream OpenFile(MD5Hash key)
        {
            try
            {
                if (Config.OnlineMode)
                    return OpenFileOnline(key);
                else
                    return OpenFileLocal(key);
            }
            catch (BLTEDecoderException exc) when (exc.ErrorCode == 3)
            {
                if (CASCConfig.ThrowOnMissingDecryptionKey)
                    throw exc;
                return null;
            }
            catch// (Exception exc) when (!(exc is BLTEDecoderException))
            {
                return OpenFileOnline(key);
            }
        }

        protected abstract Stream OpenFileOnline(MD5Hash key);

        protected Stream OpenFileOnlineInternal(IndexEntry idxInfo, MD5Hash key)
        {
            if (idxInfo != null)
            {
                Stream s = CDNIndex.OpenDataFile(idxInfo);
                return new BLTEStream(s, key);
            }
            else
            {
                Stream s = CDNIndex.OpenDataFileDirect(key);
                return new BLTEStream(s, key);
}
        }

        private Stream OpenFileLocal(MD5Hash key)
        {
            Stream stream = GetLocalDataStream(key);

            return new BLTEStream(stream, key);
        }

        protected abstract Stream GetLocalDataStream(MD5Hash key);

        protected Stream GetLocalDataStreamInternal(IndexEntry idxInfo, MD5Hash key)
        {
            if (idxInfo == null)
                throw new Exception("local index missing");

            Stream dataStream = GetDataStream(idxInfo.Index);
            dataStream.Position = idxInfo.Offset;

            using (BinaryReader reader = new BinaryReader(dataStream, Encoding.ASCII, true))
            {
                byte[] md5 = reader.ReadBytes(16);
                Array.Reverse(md5);

                if (!key.EqualsTo9(md5))
                    throw new Exception("local data corrupted");

                int size = reader.ReadInt32();

                if (size != idxInfo.Size)
                    throw new Exception("local data corrupted");

                //byte[] unkData1 = reader.ReadBytes(2);
                //byte[] unkData2 = reader.ReadBytes(8);
                dataStream.Position += 10;

                byte[] data = reader.ReadBytes(idxInfo.Size - 30);

                return new MemoryStream(data);
            }
        }

        public void SaveFileTo(MD5Hash key, string path, string name)
        {
            try
            {
                if (Config.OnlineMode)
                    ExtractFileOnline(key, path, name);
                else
                    ExtractFileLocal(key, path, name);
            }
            catch
            {
                ExtractFileOnline(key, path, name);
            }
        }

        protected abstract void ExtractFileOnline(MD5Hash key, string path, string name);

        protected void ExtractFileOnlineInternal(IndexEntry idxInfo, MD5Hash key, string path, string name)
        {
            if (idxInfo != null)
            {
                using (Stream s = CDNIndex.OpenDataFile(idxInfo))
                using (BLTEStream blte = new BLTEStream(s, key))
                {
                    blte.ExtractToFile(path, name);
                }
            }
            else
            {
                using (Stream s = CDNIndex.OpenDataFileDirect(key))
                using (BLTEStream blte = new BLTEStream(s, key))
                {
                    blte.ExtractToFile(path, name);
                }
            }
        }

        private void ExtractFileLocal(MD5Hash key, string path, string name)
        {
            Stream stream = GetLocalDataStream(key);

            using (BLTEStream blte = new BLTEStream(stream, key))
            {
                blte.ExtractToFile(path, name);
            }
        }

        protected static BinaryReader OpenInstallFile(EncodingHandler enc, CASCHandlerBase casc)
        {
            if (!enc.GetEntry(casc.Config.InstallMD5, out EncodingEntry encInfo))
                throw new FileNotFoundException("encoding info for install file missing!");

            //ExtractFile(encInfo.Key, ".", "install");

            return new BinaryReader(casc.OpenFile(encInfo.Key));
        }

        protected BinaryReader OpenDownloadFile(EncodingHandler enc, CASCHandlerBase casc)
        {
            if (!enc.GetEntry(casc.Config.DownloadMD5, out EncodingEntry encInfo))
                throw new FileNotFoundException("encoding info for download file missing!");

            //ExtractFile(encInfo.Key, ".", "download");

            return new BinaryReader(casc.OpenFile(encInfo.Key));
        }

        protected BinaryReader OpenRootFile(EncodingHandler enc, CASCHandlerBase casc)
        {
            if (!enc.GetEntry(casc.Config.RootMD5, out EncodingEntry encInfo))
                throw new FileNotFoundException("encoding info for root file missing!");

            //ExtractFile(encInfo.Key, ".", "root");

            return new BinaryReader(casc.OpenFile(encInfo.Key));
        }

        protected BinaryReader OpenEncodingFile(CASCHandlerBase casc)
        {
            //ExtractFile(Config.EncodingKey, ".", "encoding");

            return new BinaryReader(casc.OpenFile(casc.Config.EncodingKey));
        }

        private Stream GetDataStream(int index)
        {
            if (DataStreams.TryGetValue(index, out Stream stream))
                return stream;

            string dataFolder = CASCGame.GetDataFolder(Config.GameType);

            string dataFile = Path.Combine(Config.BasePath, dataFolder, "data", string.Format("data.{0:D3}", index));

            stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            DataStreams[index] = stream;

            return stream;
        }
    }
}
