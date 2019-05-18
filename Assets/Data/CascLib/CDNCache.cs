using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace CASCLib
{
    public class CacheMetaData
    {
        public long Size { get; }
        public string MD5 { get; }

        public CacheMetaData(long size, string md5)
        {
            Size = size;
            MD5 = md5;
        }
    }

    public class CDNCache
    {
        public static bool Enabled { get; set; } = true;
        public static bool CacheData { get; set; } = false;
        public static bool Validate { get; set; } = true;
        public static bool ValidateFast { get; set; } = true;
        public static string CachePath { get; set; } = "cache";

        private readonly MD5 _md5 = MD5.Create();

        private readonly Dictionary<string, Stream> _dataStreams = new Dictionary<string, Stream>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, CacheMetaData> _metaData;

        private readonly CASCConfig _config;

        private static CDNCache _instance;
        public static CDNCache Instance => _instance;

        private CDNCache(CASCConfig config)
        {
            if (Enabled)
            {
                _config = config;

                string metaFile = Path.Combine(CachePath, "cache.meta");

                _metaData = new Dictionary<string, CacheMetaData>(StringComparer.OrdinalIgnoreCase);

                if (File.Exists(metaFile))
                {
                    var lines = File.ReadLines(metaFile);

                    foreach (var line in lines)
                    {
                        string[] tokens = line.Split(' ');
                        _metaData[tokens[0]] = new CacheMetaData(Convert.ToInt64(tokens[1]), tokens[2]);
                    }
                }
            }
        }

        public static void Init(CASCConfig config)
        {
            _instance = new CDNCache(config);
        }

        public Stream OpenFile(string cdnPath, bool isData)
        {
            if (!Enabled)
                return null;

            if (isData && !CacheData)
                return null;

            string file = Path.Combine(CachePath, cdnPath);

            Logger.WriteLine("CDNCache: {0} opening...", file);

            Stream stream = GetDataStream(file, cdnPath);

            if (stream != null)
            {
                Logger.WriteLine("CDNCache: {0} has been opened", file);
                numFilesOpened++;
            }

            return stream;
        }

        private Stream GetDataStream(string file, string cdnPath)
        {
            string fileName = Path.GetFileName(file);

            if (_dataStreams.TryGetValue(fileName, out Stream stream))
                return stream;

            FileInfo fi = new FileInfo(file);

            if (!fi.Exists)
            {
                if (!DownloadFile(cdnPath, file))
                    return null;
            }

            stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            if (Validate || ValidateFast)
            {
                if (!_metaData.TryGetValue(fileName, out CacheMetaData meta))
                    meta = GetMetaData(cdnPath, fileName);

                if (meta == null)
                    throw new Exception(string.Format("unable to validate file {0}", file));

                bool sizeOk, md5Ok;

                sizeOk = stream.Length == meta.Size;
                md5Ok = ValidateFast || _md5.ComputeHash(stream).ToHexString() == meta.MD5;

                if (sizeOk && md5Ok)
                {
                    _dataStreams.Add(fileName, stream);
                    return stream;
                }
                else
                {
                    Logger.WriteLine($"CDNCache: {file} not validated, sizeOk {sizeOk}, md5Ok {md5Ok}, size {stream.Length}, expected size {meta.Size}");

                    stream.Close();
                    _metaData.Remove(fileName);
                    fi.Delete();
                    return GetDataStream(file, cdnPath);
                }
            }

            _dataStreams.Add(fileName, stream);
            return stream;
        }

        private CacheMetaData CacheFile(HttpWebResponse resp, string fileName)
        {
            string md5 = resp.Headers[HttpResponseHeader.ETag].Split(':')[0].Substring(1);
            CacheMetaData meta = new CacheMetaData(resp.ContentLength, md5);
            _metaData[fileName] = meta;

            using (var sw = File.AppendText(Path.Combine(CachePath, "cache.meta")))
            {
                sw.WriteLine(string.Format("{0} {1} {2}", fileName, resp.ContentLength, md5.ToUpper()));
            }

            return meta;
        }

        public void InvalidateFile(string fileName)
        {
            fileName = fileName.ToLower();
            _metaData.Remove(fileName);

            if (_dataStreams.TryGetValue(fileName, out Stream stream))
                stream.Dispose();

            _dataStreams.Remove(fileName);

            string file = _config.CDNPath + "/data/" + fileName.Substring(0, 2) + "/" + fileName.Substring(2, 2) + "/" + fileName;

            File.Delete(Path.Combine(CachePath, file));

            using (var sw = File.AppendText(Path.Combine(CachePath, "cache.meta")))
            {
                foreach (var meta in _metaData)
                {
                    sw.WriteLine($"{meta.Key} {meta.Value.Size} {meta.Value.MD5}");
                }
            }
        }

        public static TimeSpan timeSpentDownloading = TimeSpan.Zero;
        public static int numFilesOpened = 0;
        public static int numFilesDownloaded = 0;

        private bool DownloadFile(string cdnPath, string path, int numRetries = 0)
        {
            if (numRetries >= 5)
            {
                Logger.WriteLine($"CDNCache: failed to download {cdnPath} after 5 tries");
                return false;
            }

            string url = "http://" + _config.CDNHost + "/" + cdnPath;

            Logger.WriteLine("CDNCache: downloading file {0} to {1}", url, path);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            //using (var client = new HttpClient())
            //{
            //    var msg = client.GetAsync(url).Result;

            //    using (Stream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            //    {
            //        //CacheMetaData.AddToCache(resp, path);
            //        //CopyToStream(stream, fs, resp.ContentLength);

            //        msg.Content.CopyToAsync(fs).Wait();
            //    }
            //}

            DateTime startTime = DateTime.Now;

            //long fileSize = GetFileSize(cdnPath);

            //if (fileSize == -1)
            //    return false;

            HttpWebRequest req = WebRequest.CreateHttp(url);
            //req.AddRange(0, fileSize - 1);

            HttpWebResponse resp;

            try
            {
                using (resp = (HttpWebResponse)req.GetResponse())
                using (Stream stream = resp.GetResponseStream())
                using (Stream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyToStream(fs, resp.ContentLength);
                    CacheFile(resp, Path.GetFileName(path));
                }
            }
            catch (WebException exc)
            {
                resp = (HttpWebResponse)exc.Response;

                if (exc.Status == WebExceptionStatus.ProtocolError && resp.StatusCode == (HttpStatusCode)429)
                {
                    return DownloadFile(cdnPath, path, numRetries + 1);
                }
                else
                {
                    Logger.WriteLine($"CDNCache: error while downloading {url}: Status {exc.Status}, StatusCode {resp?.StatusCode}");
                    return false;
                }
            }

            TimeSpan timeSpent = DateTime.Now - startTime;
            timeSpentDownloading += timeSpent;
            numFilesDownloaded++;

            Logger.WriteLine("CDNCache: {0} has been downloaded, spent {1}", url, timeSpent);

            return true;
        }

        private long GetFileSize(string cdnPath, int numRetries = 0)
        {
            if (numRetries >= 5)
            {
                Logger.WriteLine($"CDNCache: GetFileSize for {cdnPath} failed after 5 tries");
                return -1;
            }

            string url = "http://" + _config.CDNHost + "/" + cdnPath;

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = "HEAD";

            HttpWebResponse resp;

            try
            {
                using (resp = (HttpWebResponse)req.GetResponse())
                {
                    return resp.ContentLength;
                }
            }
            catch (WebException exc)
            {
                resp = (HttpWebResponse)exc.Response;

                if (exc.Status == WebExceptionStatus.ProtocolError && resp.StatusCode == (HttpStatusCode)429)
                {
                    return GetFileSize(cdnPath, numRetries + 1);
                }
                else
                {
                    Logger.WriteLine($"CDNCache: error at GetFileSize {url}: Status {exc.Status}, StatusCode {resp.StatusCode}");
                    return -1;
                }
            }
        }

        private CacheMetaData GetMetaData(string cdnPath, string fileName, int numRetries = 0)
        {
            if (numRetries >= 5)
            {
                Logger.WriteLine($"CDNCache: GetMetaData for {cdnPath} failed after 5 tries");
                return null;
            }

            string url = "http://" + _config.CDNHost + "/" + cdnPath;

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = "HEAD";

            HttpWebResponse resp;

            try
            {
                using (resp = (HttpWebResponse)req.GetResponse())
                {
                    return CacheFile(resp, fileName);
                }
            }
            catch (WebException exc)
            {
                resp = (HttpWebResponse)exc.Response;

                if (exc.Status == WebExceptionStatus.ProtocolError && resp.StatusCode == (HttpStatusCode)429)
                {
                    return GetMetaData(cdnPath, fileName, numRetries + 1);
                }
                else
                {
                    Logger.WriteLine($"CDNCache: error at GetMetaData {url}: Status {exc.Status}, StatusCode {resp.StatusCode}");
                    return null;
                }
            }
        }
    }
}
