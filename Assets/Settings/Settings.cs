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
    public static string[] Data = new string[10];
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
     */


    // World Settings //
    public static float worldScale = 10.0f;

    // Terrain Settings //
    public static bool showVertexColor = true;


    public static void Save ()
    {
        File.WriteAllLines("Settings.ini", Settings.Data);
    }

    public static void GetInstalledGames()
    {
        string[] stringSeparators = new string[] { "||" };
        // World of Warcraft //
        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft");
        if (key != null)
        {
            var obj = key.GetValue("InstallPath");
            if (obj != null)
            {
                if (Data[3] == null || Data[3] == "")
                {
                    Data[3] = obj.ToString();
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
                Data[5] = objPTR.ToString();
            }
        }
        RegistryKey keyBeta = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft\\Beta");
        if (keyBeta != null)
        {
            var objPTR = keyBeta.GetValue("InstallPath");
            if (objPTR != null)
            {
                Data[6] = objPTR.ToString();
            }
        }
    }
}
