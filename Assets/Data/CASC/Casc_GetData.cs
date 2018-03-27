using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class Casc {

    public static string GetFile (string fileNameRaw)
    {
        string fileLocation = null;
        string fileName = "";
        if (fileNameRaw[0] == (@"/"[0]) || fileNameRaw[0] == (@"\"[0]))
        {
            fileName = fileNameRaw.Remove(0, 1);
        }
        else
        {
            fileName = fileNameRaw;
        }
        if (Settings.Data[2] == "0") // game
        {
            if (File.Exists(Settings.Data[0] + @"\WoWData\" + fileName))
            {
                fileLocation = Settings.Data[0] + @"\WoWData\" + fileName;
            }
            else
            {
                fileLocation = ExtractFileToCache(FileListDictionary[fileName.Replace(@"\"[0], @"/"[0]).ToLower()], fileName);
            }
        }
        else if (Settings.Data[2] == "1") // online
        {
            /* ??? */
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            if (File.Exists(Settings.Data[8] + @"\" + fileName))
            {
                fileLocation = Settings.Data[8] + @"\" + fileName;
            }
            else
            {
                Debug.Log("Missing Extracted File : " + (Settings.Data[8] + @"\" + fileName));
            }
        }
        return fileLocation;
    }

    public static Stream GetFileStream (string filename)
    {
        Stream fileStream = File.OpenRead(GetFile(filename));
        return fileStream;
    }

    public static List<string> GetFileListFromFolder (string path)
    {
        if (Settings.Data[2] == "0") // game
        {
            string[] list = FileTree[path.Replace(@"\"[0], @"/"[0]).TrimEnd(@"/"[0])];
            return new List<string>(list);
        }
        else if (Settings.Data[2] == "1") // online
        {
            /* ??? */
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            string[] list = Directory.GetFiles(Settings.Data[8] + @"\" + path);
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
        if (Settings.Data[2] == "0") // game
        {
            return FolderTree[path.Replace(@"\"[0], @"/"[0]).TrimEnd(@"/"[0])];
        }
        else if (Settings.Data[2] == "1") // online
        {
            /* ??? */
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            string[] list = Directory.GetDirectories(Settings.Data[8] + @"\" + path);
            return list;
        }
        return null;
    }

    public static bool FolderExists (string path)
    {
        try
        {
            if (Settings.Data[2] == "0") // game
            {
                return FileTree.ContainsKey(path.Replace(@"\"[0], @"/"[0]));
            }
            else if (Settings.Data[2] == "1") // online
            {
                /* ??? */
            }
            else if (Settings.Data[2] == "2") // extracted
            {
                bool exists = Directory.Exists(Settings.Data[8] + @"\" + path);
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

    public static bool FileExists (string path)
    {
        try
        {
            if (Settings.Data[2] == "0") // game
            {
                return FileList.Contains(path.Replace(@"\"[0], @"/"[0]));
            }
            else if (Settings.Data[2] == "1") // online
            {
                /* ??? */
            }
            else if (Settings.Data[2] == "2") // extracted
            {
                bool exists = File.Exists(Settings.Data[8] + @"\" + path);
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
