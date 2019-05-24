﻿using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace CASCLib
{
    public class CacheMetaData
    {
        public long Size { get; }
        public byte[] MD5 { get; }

        public CacheMetaData(long size, byte[] md5)
        {
            Size = size;
            MD5 = md5;
        }

        public void Save(string file)
        {
            File.WriteAllText(file + ".dat", $"{Size} {MD5.ToHexString()}");
        }

        public static CacheMetaData Load(string file)
        {
            if (File.Exists(file + ".dat"))
            {
                string[] tokens = File.ReadAllText(file + ".dat").Split(' ');
                return new CacheMetaData(Convert.ToInt64(tokens[0]), tokens[1].ToByteArray());
            }

            return null;
        }

        public static CacheMetaData AddToCache(HttpWebResponse resp, string file)
        {
            string md5 = resp.Headers[HttpResponseHeader.ETag].Split(':')[0].Substring(1);
            CacheMetaData meta = new CacheMetaData(resp.ContentLength, md5.ToByteArray());
            meta.Save(file);
            return meta;
        }
    }

    public class CDNCache
    {
        public static bool Enabled { get; set; } = true;
        public static bool CacheData { get; set; } = false;
        public static bool Validate { get; set; } = true;

        private readonly string _cachePath;
        private readonly SyncDownloader _downloader = new SyncDownloader();

        private readonly MD5 _md5 = MD5.Create();

        public CDNCache(string path)
        {
            _cachePath = path;
        }

        public MemoryStream OpenFile(string name, string url, bool isData)
        {
            if (!Enabled)
                return null;

            if (isData && !CacheData)
                return null;

            string file = Path.Combine(_cachePath, name);

            FileInfo fi = new FileInfo(file);

            if (!fi.Exists)
                _downloader.DownloadFile(url, file);

            if (Validate)
            {
                // CacheMetaData meta = CacheMetaData.Load(file) ?? _downloader.GetMetaData(url, file);
                // 
                // if (meta == null)
                //     throw new Exception(string.Format("unable to validate file {0}", file));
                // 
                // bool sizeOk, md5Ok;
                // 
                // using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                // {
                //     sizeOk = fs.Length == meta.Size;
                //     md5Ok = _md5.ComputeHash(fs).EqualsTo(meta.MD5);
                // }
                // 
                // if (!sizeOk || !md5Ok)
                //     _downloader.DownloadFile(url, file);
            }

            MemoryStream ms = new MemoryStream();
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                fileStream.CopyTo(ms);

            return ms;
        }

        public bool HasFile(string name)
        {
            return File.Exists(Path.Combine(_cachePath, name));
        }
    }
}
