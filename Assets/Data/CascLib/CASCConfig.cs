using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CASCLib
{
    [Flags]
    public enum LoadFlags
    {
        All = -1,
        None = 0,
        Download = 1,
        Install = 2,
    }

    class VerBarConfig
    {
        private readonly List<Dictionary<string, string>> Data = new List<Dictionary<string, string>>();

        public int Count => Data.Count;

        public Dictionary<string, string> this[int index] => Data[index];

        public static VerBarConfig ReadVerBarConfig(Stream stream)
        {
            using (var sr = new StreamReader(stream))
                return ReadVerBarConfig(sr);
        }

        public static VerBarConfig ReadVerBarConfig(TextReader reader)
        {
            var result = new VerBarConfig();

            int lineNum = 0;

            string[] fields = null;

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) // skip empty lines and comments
                    continue;

                string[] tokens = line.Split(new char[] { '|' });

                if (lineNum == 0) // keys
                {
                    fields = new string[tokens.Length];

                    for (int i = 0; i < tokens.Length; ++i)
                    {
                        fields[i] = tokens[i].Split(new char[] { '!' })[0].Replace(" ", "");
                    }
                }
                else // values
                {
                    result.Data.Add(new Dictionary<string, string>());

                    for (int i = 0; i < tokens.Length; ++i)
                    {
                        result.Data[lineNum - 1].Add(fields[i], tokens[i]);
                    }
                }

                lineNum++;
            }

            return result;
        }
    }

    public class KeyValueConfig
    {
        private readonly Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();

        public List<string> this[string key]
        {
            get
            {
                Data.TryGetValue(key, out List<string> ret);
                return ret;
            }
        }

        public static KeyValueConfig ReadKeyValueConfig(Stream stream)
        {
            var sr = new StreamReader(stream);
            return ReadKeyValueConfig(sr);
        }

        public static KeyValueConfig ReadKeyValueConfig(TextReader reader)
        {
            var result = new KeyValueConfig();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) // skip empty lines and comments
                    continue;

                string[] tokens = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != 2)
                    throw new Exception("KeyValueConfig: tokens.Length != 2");

                var values = tokens[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var valuesList = values.ToList();
                result.Data.Add(tokens[0].Trim(), valuesList);
            }
            return result;
        }
    }

    public class CASCConfig
    {
        KeyValueConfig _CDNConfig;

        List<KeyValueConfig> _Builds;

        VerBarConfig _BuildInfo;
        VerBarConfig _CDNData;
        VerBarConfig _VersionsData;

        public string Region { get; private set; }
        public CASCGameType GameType { get; private set; }
        public static bool ValidateData { get; set; } = true;
        public static bool ThrowOnFileNotFound { get; set; } = true;
        public static bool ThrowOnMissingDecryptionKey { get; set; } = true;
        public static LoadFlags LoadFlags { get; set; } = LoadFlags.None;

        private int _versionsIndex;

        public static CASCConfig LoadOnlineStorageConfig(string product, string region, bool useCurrentBuild = false)
        {
            var config = new CASCConfig { OnlineMode = true, Region = region, Product = product };

            using (var ribbit = new RibbitClient("us"))
            using (var cdnsStream = ribbit.GetAsStream($"v1/products/{product}/cdns"))
            //using (var cdnsStream = CDNIndexHandler.OpenFileDirect(string.Format("http://us.patch.battle.net:1119/{0}/cdns", product)))
            {
                config._CDNData = VerBarConfig.ReadVerBarConfig(cdnsStream);
            }

            using (var ribbit = new RibbitClient("us"))
            using (var versionsStream = ribbit.GetAsStream($"v1/products/{product}/versions"))
            //using (var versionsStream = CDNIndexHandler.OpenFileDirect(string.Format("http://us.patch.battle.net:1119/{0}/versions", product)))
            {
                config._VersionsData = VerBarConfig.ReadVerBarConfig(versionsStream);
            }

            for (int i = 0; i < config._VersionsData.Count; ++i)
            {
                if (config._VersionsData[i]["Region"] == region)
                {
                    config._versionsIndex = i;
                    break;
                }
            }

            CDNCache.Init(config);

            config.GameType = CASCGame.DetectOnlineGame(product);

            string cdnKey = config._VersionsData[config._versionsIndex]["CDNConfig"].ToLower();
            //string cdnKey = "da4896ce91922122bc0a2371ee114423";
            using (Stream stream = CDNIndexHandler.OpenConfigFileDirect(config, cdnKey))
            {
                config._CDNConfig = KeyValueConfig.ReadKeyValueConfig(stream);
            }

            config.ActiveBuild = 0;

            config._Builds = new List<KeyValueConfig>();

            if (config._CDNConfig["builds"] != null)
            {
                for (int i = 0; i < config._CDNConfig["builds"].Count; i++)
                {
                    try
                    {
                        using (Stream stream = CDNIndexHandler.OpenConfigFileDirect(config, config._CDNConfig["builds"][i]))
                        {
                            var cfg = KeyValueConfig.ReadKeyValueConfig(stream);
                            config._Builds.Add(cfg);
                        }
                    }
                    catch
                    {

                    }
                }

                if (useCurrentBuild)
                {
                    string curBuildKey = config._VersionsData[config._versionsIndex]["BuildConfig"];

                    int buildIndex = config._CDNConfig["builds"].IndexOf(curBuildKey);

                    if (buildIndex != -1)
                        config.ActiveBuild = buildIndex;
                }
            }

            string buildKey = config._VersionsData[config._versionsIndex]["BuildConfig"].ToLower();
            //string buildKey = "3b0517b51edbe0b96f6ac5ea7eaaed38";
            using (Stream stream = CDNIndexHandler.OpenConfigFileDirect(config, buildKey))
            {
                var cfg = KeyValueConfig.ReadKeyValueConfig(stream);
                config._Builds.Add(cfg);
            }

            return config;
        }

        public static CASCConfig LoadLocalStorageConfig(string basePath, string product = null)
        {
            var config = new CASCConfig { OnlineMode = false, BasePath = basePath, Product = product };

            config.GameType = CASCGame.DetectLocalGame(basePath);

            if (config.GameType == CASCGameType.Agent || config.GameType == CASCGameType.Hearthstone)
                throw new Exception("Local mode not supported for this game!");

            string buildInfoPath = Path.Combine(basePath, ".build.info");

            using (Stream buildInfoStream = new FileStream(buildInfoPath, FileMode.Open))
            {
                config._BuildInfo = VerBarConfig.ReadVerBarConfig(buildInfoStream);
            }

            Dictionary<string, string> bi = config.GetActiveBuild(product);

            if (bi == null)
                throw new Exception("Can't find active BuildInfoEntry");

            string dataFolder = CASCGame.GetDataFolder(config.GameType);

            config.ActiveBuild = 0;

            config._Builds = new List<KeyValueConfig>();

            string buildKey = bi["BuildKey"];
            //string buildKey = "5a05c58e28d0b2c3245954b6f4e2ae66";
            string buildCfgPath = Path.Combine(basePath, dataFolder, "config", buildKey.Substring(0, 2), buildKey.Substring(2, 2), buildKey);
            using (Stream stream = new FileStream(buildCfgPath, FileMode.Open))
            {
                config._Builds.Add(KeyValueConfig.ReadKeyValueConfig(stream));
            }

            string cdnKey = bi["CDNKey"];
            //string cdnKey = "23d301e8633baaa063189ca9442b3088";
            string cdnCfgPath = Path.Combine(basePath, dataFolder, "config", cdnKey.Substring(0, 2), cdnKey.Substring(2, 2), cdnKey);
            using (Stream stream = new FileStream(cdnCfgPath, FileMode.Open))
            {
                config._CDNConfig = KeyValueConfig.ReadKeyValueConfig(stream);
            }

            CDNCache.Init(config);

            return config;
        }

        private Dictionary<string, string> GetActiveBuild(string product = null)
        {
            if (_BuildInfo == null)
                return null;

            for (int i = 0; i < _BuildInfo.Count; ++i)
            {
                var bi = _BuildInfo[i];
                if (bi["Active"] == "1" && (product == null || bi["Product"] == product))
                {
                    return bi;
                }
            }

            return null;
        }

        public string BasePath { get; private set; }

        public bool OnlineMode { get; private set; }

        public int ActiveBuild { get; set; }

        public string BuildName { get { return GetActiveBuild(Product)?["Version"] ?? _VersionsData[_versionsIndex]["VersionsName"]; } }

        public string Product { get; private set; }

        public MD5Hash RootMD5 => _Builds[ActiveBuild]["root"][0].ToByteArray().ToMD5();

        public MD5Hash InstallMD5 => _Builds[ActiveBuild]["install"][0].ToByteArray().ToMD5();

        public string InstallSize => _Builds[ActiveBuild]["install-size"][0];

        public MD5Hash DownloadMD5 => _Builds[ActiveBuild]["download"][0].ToByteArray().ToMD5();

        public string DownloadSize => _Builds[ActiveBuild]["download-size"][0];

        //public MD5Hash PartialPriorityMD5 => _Builds[ActiveBuild]["partial-priority"][0].ToByteArray().ToMD5();

        //public string PartialPrioritySize => _Builds[ActiveBuild]["partial-priority-size"][0];

        public MD5Hash EncodingMD5 => _Builds[ActiveBuild]["encoding"][0].ToByteArray().ToMD5();

        public MD5Hash EncodingKey => _Builds[ActiveBuild]["encoding"][1].ToByteArray().ToMD5();

        public string EncodingSize => _Builds[ActiveBuild]["encoding-size"][0];

        public MD5Hash PatchKey => _Builds[ActiveBuild]["patch"][0].ToByteArray().ToMD5();

        public string PatchSize => _Builds[ActiveBuild]["patch-size"][0];

        public string BuildUID => _Builds[ActiveBuild]["build-uid"][0];

        private int cdnHostIndex;

        public string CDNHost
        {
            get
            {
                if (OnlineMode)
                {
                    for (int i = 0; i < _CDNData.Count; i++)
                    {
                        var cdn = _CDNData[i];

                        if (cdn["Name"] == Region)
                        {
                            var hosts = cdn["Hosts"].Split(' ');

                            if (cdnHostIndex >= hosts.Length)
                                cdnHostIndex = 0;

                            return hosts[cdnHostIndex++];
                            //for (int j = 0; j < hosts.Length; j++)
                            //{
                            //    if (hosts[j].Contains("edgecast") || hosts[j] == "cdn.blizzard.com")
                            //        continue;
                            //    return hosts[j];
                            //}
                        }
                    }
                    return _CDNData[0]["Hosts"].Split(' ')[0]; // use first
                }
                else
                {
                    return _BuildInfo[0]["CDNHosts"].Split(' ')[0];
                }
            }
        }

        public string CDNPath => OnlineMode ? _CDNData[0]["Path"] : _BuildInfo[0]["CDNPath"];

        public string CDNUrl
        {
            get
            {
                if (OnlineMode)
                {
                    int index = 0;

                    for (int i = 0; i < _CDNData.Count; ++i)
                    {
                        if (_CDNData[i]["Name"] == Region)
                        {
                            index = i;
                            break;
                        }
                    }
                    return string.Format("http://{0}/{1}", _CDNData[index]["Hosts"].Split(' ')[0], _CDNData[index]["Path"]);
                }
                else
                    return string.Format("http://{0}{1}", _BuildInfo[0]["CDNHosts"].Split(' ')[0], _BuildInfo[0]["CDNPath"]);
            }
        }

        public List<string> Archives => _CDNConfig["archives"];

        public string ArchiveGroup => _CDNConfig["archive-group"][0];

        public List<string> PatchArchives => _CDNConfig["patch-archives"];

        public string PatchArchiveGroup => _CDNConfig["patch-archive-group"][0];

        public List<KeyValueConfig> Builds => _Builds;
    }
}
