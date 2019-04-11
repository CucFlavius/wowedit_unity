using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Assets.Data;
using Assets.WoWEditSettings;

namespace Assets.Data.CASC
{
    public static partial class Casc
    {
        public static string GetFile(int filedataId)
        {
            string fileLocation = null;
            string fileName     = string.Empty;
        
            if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game)
            {
                var target = "";
                foreach (var entry in rootFile.RootData)
                {
                    if (entry.Value[0].fileDataId == filedataId)
                    {
                        RootEntry? prioritizedEntry = entry.Value.First(subentry =>
                            subentry.ContentFlags.HasFlag(ContentFlags.LowViolence) == false && (subentry.LocaleFlags.HasFlag(LocaleFlags.All_WoW) || subentry.LocaleFlags.HasFlag(LocaleFlags.enUS))
                        );

                        var selectedEntry = (prioritizedEntry != null) ? prioritizedEntry.Value : entry.Value.First();
                        target = selectedEntry.MD5.ToHexString();
                    }
                }

                if (string.IsNullOrEmpty(target))
                    Debug.Log($"No file found in root for FileDataId: {filedataId}");
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online)
            {
                /* ??? */
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted)
            {
                if (File.Exists($@"{SettingsManager<Configuration>.Config.ExtractedPath}\{fileName}") && FileListDictionary.ContainsKey(fileName))
                    fileLocation = $@"{SettingsManager<Configuration>.Config.ExtractedPath}\{fileName}";
                else
                    Debug.Log($@"Missing Extracted File : {SettingsManager<Configuration>.Config.ExtractedPath}\{fileName}");
            }
        
            return fileLocation;
        }

        public static Stream GetFileStream(string filename)
        {
            int DataId          = GetFileDataIdByName(filename);
            Stream fileStream   = File.OpenRead(GetFile(DataId));
            return fileStream;
        }

        public static List<string> GetFileListFromFolder(string path)
        {
            if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game
            {
                string[] list = FileTree[path.Replace(@"\"[0], @"/"[0]).TrimEnd(@"/"[0])];
                return new List<string>(list);
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online) // online
            {
                /* ??? */
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted) // extracted
            {
                string[] list = Directory.GetFiles($@"{SettingsManager<Configuration>.Config.ExtractedPath}\{path}");
                List<string> listl = new List<string>();
                for (int i = 0; i < list.Length; i++)
                {
                    listl.Add(Path.GetFileName(list[i]));
                }
                return listl;
            }
            return null;
        }

        public static string[] GetFolderListFromFolder(string path)
        {
            if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game
            {
                return FolderTree[path.Replace(@"\"[0], @"/"[0]).TrimEnd(@"/"[0])];
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online) // online
            {
                /* ??? */
            }
            else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted) // extracted
            {
                string[] list = Directory.GetDirectories($@"{SettingsManager<Configuration>.Config.ExtractedPath}\{path}");
                return list;
            }
            return null;
        }

        public static bool FolderExists(string path)
        {
            try
            {
                if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game
                {
                    return FileTree.ContainsKey(path.Replace(@"\"[0], @"/"[0]));
                }
                else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online) // online
                {
                    /* ??? */
                }
                else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted) // extracted
                {
                    bool exists = Directory.Exists($@"{SettingsManager<Configuration>.Config.ExtractedPath}\{path}");
                    return exists;
                }
                return false;
            }
            catch
            {
                Debug.Log("CASC Error: Can't check if the folder exists.");
                return false;
            }
        }

        public static bool FileExists(string path)
        {
            try
            {
                if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game
                {
                    return FileList.Contains(path.Replace(@"\"[0], @"/"[0]));
                }
                else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online) // online
                {
                    /* ??? */
                }
                else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted) // extracted
                {
                    bool exists = File.Exists($@"{SettingsManager<Configuration>.Config.ExtractedPath}\{path}");
                    return exists;
                }
                return false;
            }
            catch
            {
                Debug.Log("CASC Error: Can't check if the file exists.");
                return false;
            }
        }
    }
}