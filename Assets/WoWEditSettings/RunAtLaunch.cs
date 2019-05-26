using Assets.Data.Agent;
using Assets.Data.WoW_Format_Parsers.ADT;
using System.IO;
using UnityEngine;
using Assets.WoWEditSettings;
using CASCLib;
using Assets.UI.CASC;
using System.Threading;
using System.Collections;
using System;

public class RunAtLaunch : MonoBehaviour {

    // Object References
    public GameObject FolderBrowserDialog;
    public GameObject DataSourceManagerPanel;
    public GameObject CASC;

    private LocaleFlags firstInstalledLocale = LocaleFlags.enUS;

    /// <summary>
    ///  Run this code at launch
    /// </summary>
    void Start()
    {
        UserPreferences.Load();

        Settings.ApplicationPath = Application.streamingAssetsPath;
        Settings.Load();

        SettingsInit();
        ADT.Initialize(CASC.GetComponent<CascHandler>().cascHandler);

        if (Settings.GetSection("path").GetString("wowsource") == null ||
            Settings.GetSection("path").GetString("wowsource") == "") { }
        else
        {
            CASCConfig config = null;
            if (Settings.GetSection("misc").GetString("wowsource") == "game")
                config = CASCConfig.LoadLocalStorageConfig(Settings.GetSection("path").GetString("selectedpath"), Settings.GetSection("misc").GetString("localproduct"));
            else if (Settings.GetSection("misc").GetString("wowsource") == "online")
                config = CASCConfig.LoadOnlineStorageConfig(Settings.GetSection("misc").GetString("onlineproduct"), "us", true);

            CASC.GetComponent<CascHandler>().InitCasc(config, firstInstalledLocale);
        }
    }

    private void SettingsInit()
    {
        // Check if cache dir exists
        if (Settings.GetSection("path").GetString("cachepath") == null || Settings.GetSection("path").GetString("cachepath") == "")
        {
            // open dialog to pick cache dir
            //Debug.Log("pick cache dir");
            FolderBrowserDialog.SetActive(true);
            FolderBrowserDialog.GetComponent<DialogBox_BrowseFolder>().LoadInfo("Cache Folder",
                "Choose a location where the Cache folder will be created, or Cancel to save in the current folder. " +
                "\nRecomended 1 GB of free space, minimum." +
                "\nThe Cache folder can be changed later on from the Settings.",
                "Enter address or click Browse...");
            FolderBrowserDialog.GetComponent<DialogBox_BrowseFolder>().Link("DialogBoxCache_Ok", "DialogBoxCache_Cancel", this);
        }
        else
            CheckWoWInstalls();
    }

    private void CreateCacheDir()
    {
        if (!Directory.Exists(Settings.GetSection("path").GetString("cachepath")))
            Directory.CreateDirectory(Settings.GetSection("path").GetString("cachepath") + @"\Cache");
    }

    void DialogBoxCache_Ok()
    {
        Settings.GetSection("path").SetValueOfKey("cachepath", FolderBrowserDialog.GetComponent<DialogBox_BrowseFolder>().ChosenPath + @"\Cache");
        CreateCacheDir();
        Settings.Save();
        CheckWoWInstalls();
    }

    void DialogBoxCache_Cancel()
    {
        Settings.GetSection("path").SetValueOfKey("cachepath", "Cache");
        CreateCacheDir();
        Settings.Save();
        CheckWoWInstalls();
    }

    public void CheckWoWInstalls()
    {
        CreateCacheDir();
        Settings.Save();
        CheckDataSource();
    }
        
    public void CheckDataSource()
    {
        if (Settings.GetSection("misc").GetString("wowsource") == null || 
            Settings.GetSection("misc").GetString("wowsource") == "")
        {
            // open Data Source Manager //
            DataSourceManagerPanel.GetComponent<DataSourceManager>().Initialize();
            DataSourceManagerPanel.SetActive(true);
        }
    }
}

