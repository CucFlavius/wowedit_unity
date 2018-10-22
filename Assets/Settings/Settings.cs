using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// General Settings
/// Static Variables
/// 

public static class Settings
{
    // Data Settings //
    public static string ApplicationPath;
    public static string[] Data = new string[11];
    public static Dictionary<string, string> availableInstalls;
    /*
     * [0] = Cache Folder Location
     * [1] = First Run? 0/1
     * [2] = WoWSource: 0-game/1-online/2-extracted
     * [3] = WoWGameSelected
     * [4] = WoWLivePath
     * [5] = WoWPTRPath
     * [6] = WoWAlphaPath
     * [7] = WoWOnlineSelected
     * [8] = WoWExtractedPath
     * [9] = WoWCustomGameSource
     * [10]= SelectedDB2Definitions
     * 
     */

    public static string ListFileDownload = @"https://github.com/wowdev/wow-listfile/blob/master/listfile.txt";

    // World Settings //
    public static float worldScale = 10.0f;

    // LoD Settings //
    public static float terrainMaterialDistance = 300;
    public static float highMipMapBias = 0.5f; // negative = sharper, 0 = default, positive = blurryer
    public static float lowMipMapBias = 0f; // negative = sharper, 0 = default, positive = blurryer

    // Terrain Settings //
    public static bool showVertexColor = true;

    // Terrain Importer //
    //public static bool LoadWMOs = false;
    //public static bool LoadM2s = false;

    // DB2 Definitions //
    public static Dictionary<string, string> DB2XMLDefinitions = new Dictionary<string, string>()
    {
        { @"Legion 7.3.5 (25632).xml", @"https://raw.githubusercontent.com/WowDevTools/WDBXEditor/master/WDBXEditor/Definitions/Legion%207.3.5%20(25632).xml"},
        { @"BfA 8.0.1 (26231).xml", @"https://raw.githubusercontent.com/WowDevTools/WDBXEditor/master/WDBXEditor/Definitions/BfA%208.0.1%20(26231).xml"},
        { @"BfA 8.0.1 (26367).xml", @"https://raw.githubusercontent.com/WowDevTools/WDBXEditor/master/WDBXEditor/Definitions/BfA%208.0.1%20(26367).xml"}
    };
    public static string SelectedDefinitions = "";
    public static List<string> DropdownDefinitionsList = new List<string>();

    public static List<string> DropdownGameList = new List<string>();

    public static void Save ()
    {
        File.WriteAllLines("Settings.ini", Settings.Data);
    }

    public static void GetInstalledGames()
    {
        availableInstalls = new Dictionary<string, string>();
        string[] stringSeparators = new string[] { "||" };
        // World of Warcraft //
        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft");
        if (key != null)
        {
            var obj = key.GetValue("InstallPath");
            if (obj != null)
            {
                if (Data[3] == null || Data[3] == "" && File.Exists(obj + "Wow-64.exe"))
                {
                    Data[3] = obj.ToString();
                    availableInstalls.Add(obj.ToString(), GetWoWVersion(obj + "Wow-64.exe"));
                }
                Data[4] = obj.ToString();
            }
        }
        RegistryKey keyPTR = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft\\PTR");
        if (keyPTR != null)
        {
            var objPTR = keyPTR.GetValue("InstallPath");
            if (objPTR != null)
            {
                if (File.Exists(objPTR + "WowT-64.exe"))
                {
                    Data[5] = objPTR.ToString();
                    availableInstalls.Add(objPTR.ToString(), GetWoWVersion(objPTR + "WowT-64.exe"));
                }
            }
        }
        RegistryKey keyBeta = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft\\Beta");
        if (keyBeta != null)
        {
            var objPTR = keyBeta.GetValue("InstallPath");
            if (objPTR != null)
            {
                if (File.Exists(objPTR + "WowB.exe"))
                {
                    Data[6] = objPTR.ToString();
                    availableInstalls.Add(objPTR.ToString(), GetWoWVersion(objPTR + "WowB.exe"));
                }
            }
        }
        Debug.Log("Available Installs : " + availableInstalls.Count);
    }

    public static string GetWoWVersion (string exePath)
    {
        string versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(exePath).FileVersion;
        //Debug.Log(versionInfo);
        return versionInfo;
    }

    public enum WoWSource : uint
    {
        Game = 0,
        Online = 1,
        Extracted = 2
    }

    /*

    public static void SetDefaultDefinitions(string currentSelection)
    {
        
        if (Data[2] == WoWSource.Game.ToString())
        {
            Debug.Log("yeah");
            if (availableInstalls.ContainsKey(currentSelection))
            {
                string versionRaw = availableInstalls[currentSelection];
                string[] splits = versionRaw.Split(new char[]{'.'});
                int versionAvalue1 = int.Parse(splits[0] + splits[1] + splits[2]);
                int versionAvalue2 = int.Parse(splits[3]);
                Debug.Log(versionAvalue1 + " " + versionAvalue2);

                foreach (string version in DropdownDefinitionsList)
                {
                    //int versionBvalue1 = version
                    Debug.Log(version);
                }
            }
        }
        else
        {
            List<string> elements = new List<string>(DB2XMLDefinitions.Keys);
            string[] splits1 = elements[0].Replace(" ", ".").Split(new char[] { '.' });
            string version = splits1[0] + "_" + splits1[1] + splits1[2] + splits1[3] + "_" + splits1[4].Trim('(').Trim(')');
            SelectedDefinitions = version;
        }
    }

    */
}
