using Assets.Data.Agent;
using Assets.Data.CASC;
using Assets.Data.WoW_Format_Parsers.ADT;
using Assets.Tools.CSV;
using System.IO;
using UnityEngine;
using Assets.WoWEditSettings;

public class RunAtLaunch : MonoBehaviour {

    // Object References
    public GameObject FolderBrowserDialog;
    public GameObject DataSourceManagerPanel;

    /// <summary>
    ///  Run this code at launch
    /// </summary>

    void Start()
    {
        Settings.LoadConfig();
        UserPreferences.Load();
        //CSVReader.LoadCSV();
        //Agent.FindWowInstalls();
        //Network.Disconnect();
        Settings.ApplicationPath = Application.streamingAssetsPath;
        SettingsInit();
        ADT.Initialize();
    }

    private void SettingsInit()
    {
        // Check if cache dir exists
        if (SettingsManager<Configuration>.Config.CachePath == string.Empty || 
            SettingsManager<Configuration>.Config.CachePath == "")
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
        if (!Directory.Exists(Settings.CachePath))
            Directory.CreateDirectory(Settings.CachePath + @"\Cache");
    }

    void DialogBoxCache_Ok()
    {
        Settings.CachePath = FolderBrowserDialog.GetComponent<DialogBox_BrowseFolder>().ChosenPath + @"\Cache";
        CreateCacheDir();
        Settings.SaveFile();
        CheckWoWInstalls();
    }

    void DialogBoxCache_Cancel()
    {
        Settings.CachePath = "Cache";
        CreateCacheDir();
        Settings.SaveFile();
        CheckWoWInstalls();
    }

    public void CheckWoWInstalls()
    {
        CreateCacheDir();
        Settings.SaveFile();
        CheckDataSource();
    }
        
    public void CheckDataSource()
    {
        if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Null)
        {
            // open Data Source Manager //
            DataSourceManagerPanel.GetComponent<DataSourceManager>().Initialize();
            DataSourceManagerPanel.SetActive(true);
        }

        if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game mode //
        {
            // DataSourceManagerPanel.GetComponent<DataSourceManager>().Initialize();
            CascInitialize.Start();
        }
    }
}

