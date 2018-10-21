using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class DB2
{
    public static string[] fileNames = new string[] { "animationdata.db2" };
    public static Dictionary<string, bool> availableFiles = new Dictionary<string, bool>();
    private static string dbfilesclient = @"dbfilesclient\";
    public static List<string> LoadedDB2 = new List<string>();
    public static string magic = "";

    public static void Initialize()
    {
        LoadedDB2 = new List<string>();

        // Preload DB2 //
        foreach (string fileName in fileNames)
        {
            if (Casc.FileExists (dbfilesclient + fileName))
            {
                availableFiles.Add(fileName, true);
                Read(fileName);
            }
            else
            {
                availableFiles.Add(fileName, false);
                Debug.LogWarning("Warning: " + "Missing " + fileName);
            }
        }
    }

    public static void Read(string fileName)
    {
        if (!LoadedDB2.Contains(fileName))
        {
            string dataPath = dbfilesclient + fileName;
            string path = Casc.GetFile(dataPath);
            byte[] fileData = File.ReadAllBytes(path);

            // Check DB2 Version //

            // WDC //
            if (fileData[0] == Convert.ToByte('W') && fileData[1] == Convert.ToByte('D') && fileData[2] == Convert.ToByte('C'))
            {
                // WDC 1 //
                if (fileData[3] == Convert.ToByte('1'))
                {
                    magic = "WDC1";
                    WDC1.Read(fileName, fileData);
                }
                // WDC 2 //
                if (fileData[3] == Convert.ToByte('2'))
                {
                    magic = "WDC2";
                    WDC2.Read(fileName, fileData);
                }
            }

            // WDB //
            else
            {
                Debug.LogWarning("Warning: " + "DB2 Format not supported: " + fileData[0] + fileData[1] + fileData[2] + fileData[3] + fileName);
            }
        }
        LoadedDB2.Add(fileName);
    }

}
