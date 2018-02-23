using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class Casc {

    public static string GetFile (string fileName)
    {
        string fileLocation = null;
        if (Settings.Data[2] == "0") // game
        {
            if (File.Exists(Settings.Data[0] + @"\WoWData\" + fileName))
            {
                fileLocation = Settings.Data[0] + @"\WoWData\" + fileName;
            }
            else
            {
                fileLocation = ExtractFileToCache(FileListDictionary[fileName], fileName);
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

    public static string[] GetFileListFromFolder (string path)
    {
        if (Settings.Data[2] == "0") // game
        {
            return null;
        }
        else if (Settings.Data[2] == "1") // online
        {
            return null;
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            string [] list = Directory.GetFiles(path);
            return list;
        }
        return null;
    }

    public static string[] GetFolderListFromFolder(string path)
    {
        if (Settings.Data[2] == "0") // game
        {
            return null;
        }
        else if (Settings.Data[2] == "1") // online
        {
            return null;
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            string[] list = Directory.GetDirectories(path);
            return list;
        }
        return null;
    }

    public static bool FolderExists (string path)
    {
        if (Settings.Data[2] == "0") // game
        {
            return false;
        }
        else if (Settings.Data[2] == "1") // online
        {
            return false;
        }
        else if (Settings.Data[2] == "2") // extracted
        {
            bool exists = Directory.Exists(path);
            return exists;
        }
        return false;
    }
}
