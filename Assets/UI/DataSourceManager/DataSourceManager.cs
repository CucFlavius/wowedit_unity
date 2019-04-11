using Assets.Data.CASC;
using Assets.WoWEditSettings;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceManager : MonoBehaviour {

    public Toggle ToggleGame;
    public Toggle ToggleOnline;
    public Toggle ToggleExtracted;

    public bool IsExtracted;

    public Dropdown DropdownGame;
    public Dropdown DropdownOnline;
    public Dropdown DropdownDefinitions;
    public InputField Extracted;
    public GameObject terrainImport;
    public GameObject FolderBrowser;
    public UnityEngine.UI.Text FolderBrowser_SelectedFolderText;

    public void Initialize ()
    {
        Debug.Log("Init");
        // Update Game List //
        Settings.DropdownGameList.Clear();
        if (SettingsManager<Configuration>.Config.WoWPath != null)
            Settings.DropdownGameList.Add(SettingsManager<Configuration>.Config.WoWPath);

        DropdownGame.ClearOptions();
        DropdownGame.AddOptions(Settings.DropdownGameList);

        // select previously used //
        for (int v = 0; v < Settings.DropdownGameList.Count; v++)
        {
            if (DropdownGame.options[v].text == SettingsManager<Configuration>.Config.WoWPath)
            {
                DropdownGame.value = v;
            }
        }

        // Update Online List //
        DropdownOnline.ClearOptions();

        // Update Toggles //
        if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game)
            ToggleGame.isOn = true;
        else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Online)
            ToggleOnline.isOn = true;
        else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted)
            ToggleExtracted.isOn = true;

        // Update Extracted Path //
        if (SettingsManager<Configuration>.Config.ExtractedPath != null)
        {
            Extracted.text = SettingsManager<Configuration>.Config.ExtractedPath;
        }

        // // Update Definitions List //
        // Settings.DropdownDefinitionsList.Clear();
        // List<string> elements = new List<string>(Settings.DB2XMLDefinitions.Keys);
        // foreach (string element in elements)
        // {
        //     string[] splits1 = element.Replace(" ", ".").Split(new char[] { '.' });
        //     string version = splits1[0] + "_" + splits1[1] + splits1[2] + splits1[3] + "_" + splits1[4].Trim('(').Trim(')');
        //     Settings.DropdownDefinitionsList.Add(version);
        // }
        // DropdownDefinitions.ClearOptions();
        // DropdownDefinitions.AddOptions(Settings.DropdownDefinitionsList);
        // 
        // // select stored definition //
        // for (int v = 0; v < Settings.DB2XMLDefinitions.Count; v++)
        // {
        //     if (DropdownDefinitions.options[v].text == Settings.Data[10])
        //     {
        //         DropdownDefinitions.value = v;
        //     }
        // }
        // 
        // //Settings.SetDefaultDefinitions(Settings.Data[4]);
    }

    public void Ok ()
    {
        if (ToggleGame.isOn)
        {
            Settings.WoWSource  = WoWSource.Game;
            Settings.WoWPath    = DropdownGame.options[DropdownGame.value].text;
            if (Settings.WoWPath != CascInitialize.CurrentDataVersion)
            {
                CascInitialize.Initialized = false; // changed data source so reinitialize
            }
            // start Initialize casc thread //
            Settings.SaveFile();
            CascInitialize.Start();
            gameObject.SetActive(false);
        }
        if (ToggleOnline.isOn)
        {
            Settings.WoWSource = WoWSource.Online;
            Settings.SaveFile();
            gameObject.SetActive(false);
        }
        if (ToggleExtracted.isOn)
        {
            if (Extracted.text != "" && Extracted.text != null)
            {
                Settings.WoWSource = WoWSource.Extracted;
                Settings.ExtractedPath = Extracted.text;
                Settings.SaveFile();
                gameObject.SetActive(false);
            }
            IsExtracted = true;
        }
        if (Settings.WoWSource == WoWSource.Extracted)
            terrainImport.GetComponent<TerrainImport>().Initialize();
        // Settings.SelectedDefinitions = DropdownDefinitions.options[DropdownDefinitions.value].text;
        // Settings.Data[10] = Settings.SelectedDefinitions;
        // DB2.InitializeDefinitions();
    }

    public void AddButon ()
    {
        FolderBrowser.SetActive(true);
        FolderBrowser.GetComponent<FolderBrowserLogic>().Link("AddGamePath", this);
    }

    public void AddGamePath ()
    {
        string tempPath = FolderBrowser_SelectedFolderText.text + @"\";
        if (!Settings.DropdownGameList.Contains(tempPath))
        {
            if (CheckValidWoWPath(tempPath))
            {
                // correct path //
                // Settings.Data[9] = tempPath;
                Settings.DropdownGameList.Add(tempPath);
                DropdownGame.ClearOptions();
                DropdownGame.AddOptions(Settings.DropdownGameList);
            }
            else
            {
                print("error: incorrect wow path");
            }
        }
        else
        {
            print("error: path already exists");
        }
        
    }

    public void BrowseButton ()
    {
        FolderBrowser.SetActive(true);
        FolderBrowser.GetComponent<FolderBrowserLogic>().Link("FillInExtractedPath", this);
    }

    public void FillInExtractedPath ()
    {
        Extracted.text = FolderBrowser_SelectedFolderText.text;
    }

    public bool CheckValidWoWPath (string path)
    {
        if (File.Exists(path + "Wow-64.exe") || File.Exists(path + "WowB-64.exe") || File.Exists(path + "WowT-64.exe") || File.Exists(path + "Wow.exe"))
            return true;
        else
            return false;
    }

    public void SelectedDifferentGame (int id)
    {
        //Settings.SetDefaultDefinitions(DropdownGame.options[id].text);
    }


}
