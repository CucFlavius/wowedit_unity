using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Assets.WoWEditSettings;
using static Assets.Data.CASC.Casc;

namespace Assets.Data.CASC
{
    public class BuildInfoDataEntry
    {
        public string Index;
        public string Type;
        public string Data;
    }

    public class BLTEChunk
    {
        public int CompSize;
        public int DecompSize;
        public byte[] Hash;
        public byte[] Data;
    }

    public struct EncodingEntry
    {
        public MD5Hash Key;
        public long Size;
    }

    public static partial class Casc
    {
        public static string WoWDataPath;
        public static List<List<BuildInfoDataEntry>> buildInfoData = new List<List<BuildInfoDataEntry>>();
        public static string WoWVersion = null;
        public static string WoWEncodingKey = null;
        public static string WoWRootKey = null;
        public static List<string> WoWExeVariants = new List<string> { "Wow.exe", "WowB.exe", "WowT.exe", "WowT-64.exe", "WowB-64.exe", "Wow-64.exe" };

        //data
        public static Dictionary<String, IndexEntry> LocalIndexData = new Dictionary<string, IndexEntry>();
        public static Dictionary<ulong, String> MyRootData = new Dictionary<ulong, string>();
        public static List<string> FileList = new List<string>();
        public static Dictionary<String, ulong> FileListDictionary = new Dictionary<string, ulong>();
        public static Dictionary<string, string[]> FileTree = new Dictionary<string, string[]>();
        public static Dictionary<string, string[]> FolderTree = new Dictionary<string, string[]>();

        public static Dictionary<MD5Hash, EncodingEntry> EncodingData = new Dictionary<MD5Hash, EncodingEntry>();
        public static RootFile rootFile = new RootFile();
        public static Dictionary<int, ulong> FileDataStore = new Dictionary<int, ulong>();
        public static Dictionary<ulong, int> FileDataStoreReverse = new Dictionary<ulong, int>();

        static readonly Jenkins96 Hasher = new Jenkins96();
        private const int CHUNK_SIZE = 4096;
        public static void ReadWoWFolder()
        {
            // Check if we're in Data folder //
            WoWDataPath = SettingsManager<Configuration>.Config.WoWPath;
            string[] fileInfo = Directory.GetFiles(WoWDataPath);
            foreach (string file in fileInfo)
            {
                if (WoWExeVariants.Contains(System.IO.Path.GetFileName(file)))
                {
                    if (WoWDataPath[WoWDataPath.Length - 1] == @"/"[0] || WoWDataPath[WoWDataPath.Length - 1] == @"\"[0])
                        WoWDataPath = WoWDataPath + @"Data";
                    else
                        WoWDataPath = WoWDataPath + @"\Data";

                    break;
                }
            }
        }

        public static void FindWoWBuildConfig()
        {
            string buildInfoFilePath = SettingsManager<Configuration>.Config.WoWPath + @"\.build.info";
            string filePathBuffer = "";
            string wowVersionBuffer = "";
            StreamReader buildInfoFileReader = new StreamReader(buildInfoFilePath);
            List<string> buildInfoFileLines = buildInfoFileReader.ReadToEnd().Split("\n"[0]).ToList();
            for (int line = 0; line < buildInfoFileLines.Count; line++)
            {
                if (buildInfoFileLines[line] == "" || buildInfoFileLines[line] == null)
                {
                    buildInfoFileLines.RemoveAt(line);
                }
            }
            string buildFilePath = null;
            string[] buildInfoFileData_Index = buildInfoFileLines[0].Split("|"[0]);
            List<string[]> buildInfoFileData_Entry_Locale = new List<string[]>();
            buildInfoData = new List<List<BuildInfoDataEntry>>(buildInfoFileLines.Count - 1);
            List<BuildInfoDataEntry> sublist;
            if (buildInfoFileLines.Count > 1)
            {
                for (int locale = 1; locale < buildInfoFileLines.Count; locale++)
                {
                    buildInfoFileData_Entry_Locale.Add(buildInfoFileLines[locale].Split("|"[0]));
                }
            }
            for (int L = 0; L < buildInfoFileData_Entry_Locale.Count; L++)
            {
                sublist = new List<BuildInfoDataEntry>();
                for (int entry = 0; entry < buildInfoFileData_Index.Length; entry++)
                {
                    BuildInfoDataEntry bide = new BuildInfoDataEntry();
                    bide.Index = buildInfoFileData_Index[entry].Split("!"[0])[0];
                    bide.Type = buildInfoFileData_Index[entry].Split("!"[0])[1];
                    bide.Data = buildInfoFileData_Entry_Locale[L][entry];
                    sublist.Add(bide);
                }
                buildInfoData.Add(sublist);
            }
            foreach (List<BuildInfoDataEntry> data in buildInfoData)
            {
                foreach (BuildInfoDataEntry entry in data)
                {
                    if (entry.Index == "Build Key")
                    {
                        filePathBuffer = WoWDataPath + @"\config\" + entry.Data.Substring(0, 2) + @"\" + entry.Data.Substring(2, 2) + @"\" + entry.Data;
                    }
                    if (entry.Index == "Version")
                    {
                        wowVersionBuffer = entry.Data;
                    }
                }
                if (File.Exists(filePathBuffer))
                {
                    buildFilePath = filePathBuffer;
                    Debug.Log("buildFilePath : " + buildFilePath);
                    WoWVersion = wowVersionBuffer;
                    break;
                }
            }
            StreamReader BuildReader = new StreamReader(buildFilePath);
            List<String> BuildfileLines = new List<String>();
            BuildfileLines = BuildReader.ReadToEnd().Split("\n"[0]).ToList();
            for (int line = 0; line < BuildfileLines.Count; line++)
            {
                if (BuildfileLines[line].Split('=')[0] == "encoding ")
                {
                    string key = BuildfileLines[line].Split('=')[1];
                    string[] strArr = key.Split(' ');
                    WoWEncodingKey = strArr[strArr.Length - 1];
                }
                if (BuildfileLines[line].Split('=')[0] == "root ")
                {
                    string key1 = BuildfileLines[line].Split('=')[1];
                    string[] strArr1 = key1.Split(' ');
                    WoWRootKey = strArr1[strArr1.Length - 1];
                }
            }
            BuildReader.Close();
            BuildReader = null;
        }

        public static void ReadWoWIDXfiles()
        {
            string[] idxFiles = Directory.GetFiles($@"{WoWDataPath}\data\", "*.idx");
            foreach (string idxfile in idxFiles)
            {
                IndexBlockParser.ParseIndex(idxfile);
            }
        }

        public static Stream OpenWoWFile(byte[] key)
        {
            if (key == null)
            {
                return null;
            }
            // trim the byte array encoding key to 9 bytes //
            var ninebyte = Strip9Bytes(key);
            var newString = ByteString(ninebyte);
            if (IndexBlockParser.LocalIndexData.ContainsKey(newString))
            {
                var FileIndex = IndexBlockParser.LocalIndexData[newString].Index;
                var FileOffset = IndexBlockParser.LocalIndexData[newString].Offset;
                var FileSize = IndexBlockParser.LocalIndexData[newString].Size;
                Debug.Log(FileIndex + " " + FileOffset + " " + FileSize);
                var stream = GetDatafileStream(FileIndex);
                if (stream == null) { return null; }
                stream.Position = FileOffset;
                stream.Position += 30;
                var blte = BLTE.OpenFile(stream, FileSize - 30);
                return blte;
            }
            else
            {
                Debug.Log("Error : local index missing");
                return null;
            }
        }

        public static void LoadFilelist()
        {
            if (!Directory.Exists(SettingsManager<Configuration>.Config.ApplicationPath + @"\ListFiles\"))
                Directory.CreateDirectory(SettingsManager<Configuration>.Config.ApplicationPath + @"\ListFiles\");
            if (File.Exists(SettingsManager<Configuration>.Config.ApplicationPath + @"\ListFiles\Listfile.txt"))
            {
                StreamReader sr = File.OpenText(SettingsManager<Configuration>.Config.ApplicationPath + @"\ListFiles\Listfile.txt");
                string[] ListFile = sr.ReadToEnd().Split("\n"[0]);
                Debug.Log("FileList Lines : " + ListFile.Length);
                foreach (string item in ListFile)
                {
                    if (item != null && item != "")
                    {
                        ulong hashUlong = Hasher.ComputeHash(item);
                        if (rootFile.RootData.ContainsKey(hashUlong))
                        {
                            FileList.Add(item);
                            FileListDictionary.Add(item, hashUlong);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Missing Listfile");
            }
        }

        public static void LoadTreeData()
        {
            // check if cached //
            string pathToLoadFileTree   = $@"{SettingsManager<Configuration>.Config.CachePath}\FileTree_{WoWVersion}_{FileList.Count}.bin";
            string pathToLoadFolderTree = $@"{SettingsManager<Configuration>.Config.CachePath}\FolderTree_{WoWVersion}_{FileList.Count}.bin";
            if (File.Exists(pathToLoadFileTree) && File.Exists(pathToLoadFolderTree))
            {
                // deserialize //
                FileStream theReader = new FileStream(pathToLoadFileTree, FileMode.Open);
                FileTree = DeserializeTree(theReader);
                theReader.Close();
                theReader = null;
                FileStream theReader1 = new FileStream(pathToLoadFolderTree, FileMode.Open);
                FolderTree = DeserializeTree(theReader1);
                theReader1.Close();
                theReader1 = null;
            }
            // else rebuild and cache //
            else
            {
                BuildTreeData();
            }
        }

        private static void BuildTreeData()
        {
            string CurrentString = "";
            List<string> CheckedFolders = new List<string>();
            List<string> fileGroup = new List<string>();
            //Restructure data into the trees//
            int debugcounter = 0;
            for (int i = 0; i < FileList.Count; i++)
            {
                //initial string parse//
                string stripFileName = Path.GetDirectoryName(FileList[i]).ToLower();
                string stripDirectory = Path.GetFileName(FileList[i]);
                string pathkey = stripFileName;
                // build similar folder list //
                if (CurrentString == pathkey || CurrentString == "") //this file still in the same folder
                {
                    fileGroup.Add(stripDirectory);
                    debugcounter++;
                }
                if (CurrentString != pathkey)
                {
                    if (CurrentString != "")
                    {
                        string[] fileGroupArray = fileGroup.ToArray();
                        if (FileTree.ContainsKey(CurrentString))
                        {
                            int oldListLength = FileTree[CurrentString].Length;
                            string[] mergedFolders = new string[oldListLength + fileGroupArray.Length];
                            FileTree[CurrentString].CopyTo(mergedFolders, 0);
                            fileGroupArray.CopyTo(mergedFolders, oldListLength);
                            FileTree[CurrentString] = mergedFolders;
                        }
                        else
                        {
                            FileTree.Add(CurrentString, fileGroupArray);
                        }
                        fileGroup.Clear();
                        debugcounter++;
                    }

                    fileGroup.Add(stripDirectory);

                    CurrentString = pathkey;
                    // Structure FOLDER data //
                    if (!CheckedFolders.Contains(pathkey) && !CheckedFolders.Contains(pathkey.ToLower()))
                    {
                        //iterate through whole folder path//
                        string[] names = pathkey.Split(new char[] { '/' });
                        if (names.Length > 0)
                        {
                            string pathtocheck = "";
                            for (int j = 0; j < names.Length - 1; j++)
                            {
                                pathtocheck = pathtocheck + names[j] + @"/";
                                if (CheckedFolders.Contains(pathtocheck))
                                {
                                    string[] oldFolders = FolderTree[pathtocheck.TrimEnd(@"/"[0])];
                                    string[] newFolders = new string[1];
                                    // checking if folder already exists //
                                    bool exists = false;
                                    foreach (string str in oldFolders)
                                    {
                                        if (str == names[j + 1])
                                        {
                                            exists = true;
                                        }
                                    }
                                    if (!exists)
                                    {
                                        if (names[j + 1] != "")
                                        {
                                            newFolders[0] = names[j + 1];
                                            string[] mergedFolders = new string[oldFolders.Length + newFolders.Length];
                                            oldFolders.CopyTo(mergedFolders, 0);
                                            newFolders.CopyTo(mergedFolders, oldFolders.Length);
                                            FolderTree[pathtocheck.TrimEnd(@"/"[0])] = mergedFolders;
                                        }
                                    }
                                }
                                else
                                {
                                    if (names[j + 1] != "")
                                    {
                                        string[] newFolderArray = new string[1];
                                        newFolderArray[0] = names[j + 1];
                                        CheckedFolders.Add(pathtocheck);
                                        FolderTree[pathtocheck.TrimEnd(@"/"[0])] = newFolderArray;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string pathToSaveFileTree   = $@"{SettingsManager<Configuration>.Config.CachePath}\FileTree_{WoWVersion}_{FileList.Count}.bin";
            string pathToSaveFolderTree = $@"{SettingsManager<Configuration>.Config.CachePath}\FolderTree_{WoWVersion}_{FileList.Count}.bin";
            FileStream theWriter = new FileStream(pathToSaveFileTree, FileMode.Create);
            Debug.Log("File Tree: " + FileTree.Count);
            SerializeTree(FileTree, theWriter);
            theWriter.Close();
            theWriter = null;
            FileStream theWriter1 = new FileStream(pathToSaveFolderTree, FileMode.Create);
            Debug.Log("Folder Tree: " + FolderTree.Count);
            SerializeTree(FolderTree, theWriter1);
            theWriter1.Close();
            theWriter1 = null;
        }


        public static string ExtractFileToCache(ulong md5, string path)
        {
            var fs = ExtractFileToMemory(md5);
            if (fs != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName($@"{SettingsManager<Configuration>.Config.CachePath}\WoWData\{path}"));
                StreamToFile(fs, $@"{SettingsManager<Configuration>.Config.CachePath}\WoWData\{path}");
                fs.Close();
                fs = null;
                return $@"{SettingsManager<Configuration>.Config.CachePath}\WoWData\{path}";
            }
            else
            {
                return null;
            }
        }

        public static Stream ExtractFileToMemory(ulong key)
        {
            var WoWEncodingKey = SearchRootFile(key);
            var fs = GetEncodingData(WoWEncodingKey);
            return fs;
        }

        public static string SearchRootFile(ulong key)
        {
            string value;
            if (MyRootData.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                Debug.Log("Error : Root Key Missing");
                return null;
            }
        }

        public static void ClearData()
        {
            WoWDataPath         = null;
            buildInfoData       = new List<List<BuildInfoDataEntry>>();
            WoWVersion          = null;
            WoWEncodingKey      = null;
            WoWRootKey          = null;

            //data
            LocalIndexData      = new Dictionary<string, IndexEntry>();
            EncodingData        = new Dictionary<MD5Hash, EncodingEntry>();
            rootFile.RootData    = new MultiDictionary<ulong, RootEntry>();
            FileList            = new List<string>();
            FileListDictionary  = new Dictionary<string, ulong>();
            FileTree            = new Dictionary<string, string[]>();
            FolderTree          = new Dictionary<string, string[]>();
        }

    }
}