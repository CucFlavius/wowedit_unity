using Assets.UI.CASC;
using Assets.WoWEditSettings;
using CASCLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceManager : MonoBehaviour
{
    public Toggle ToggleGame;
    public Toggle ToggleOnline;
    public Toggle ToggleExtracted;
    public GameObject World;
    public GameObject CASC;

    public bool IsExtracted;

    public InputField WoWPath;
    public InputField Extracted;
    public Dropdown DropdownOnline;
    public GameObject terrainImport;
    public GameObject FolderBrowser;
    public Text FolderBrowser_SelectedFolderText;

    private CASCGameType _gameType;
    public CASCHandler cascHandler;

    public void Initialize ()
    {
        // Update Online List //
        DropdownOnline.ClearOptions();

        // Update Toggles //
        if (Settings.GetSection("misc").GetString("wowsource") == "game")
            ToggleGame.isOn = true;
        // else if (Settings.GetSection("misc").GetString("wowsource") == "online")
        //     ToggleOnline.isOn = true;
        else if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
            ToggleExtracted.isOn = true;
        else
            ToggleGame.isOn = true;

        // Update WoW Path //
        if (Settings.GetSection("path").GetString("selectedpath") != null)
            WoWPath.text = Settings.GetSection("path").GetString("selectedpath");

        // Update Extracted Path //
        if (Settings.GetSection("path").GetString("extracted") != null)
            Extracted.text = Settings.GetSection("path").GetString("extracted");
    }

    public void Ok ()
    {
        if (ToggleGame.isOn)
        {
            Settings.GetSection("misc").SetValueOfKey("wowsource", "game");
            Settings.GetSection("path").SetValueOfKey("selectedpath", WoWPath.text);

            // start Initialize casc thread //
            _gameType = CASCGame.DetectLocalGame(WoWPath.text);

            CASCConfig config = CASCConfig.LoadLocalStorageConfig(Settings.GetSection("path").GetString("selectedpath"), "wowt");
            new Thread(() => {
                cascHandler = CASCHandler.OpenStorage(config);
                cascHandler.Root.SetFlags(LocaleFlags.None, false);
                Debug.Log($"Locale: {cascHandler.Root.Locale} Count Unk: {cascHandler.Root.CountUnknown} Total: {cascHandler.Root.CountTotal}");
            }).Start();

            Settings.GetSection("misc").SetValueOfKey("wowproduct", _gameType.ToString());
            Settings.Save();

            gameObject.SetActive(false);
        }
        // if (ToggleOnline.isOn)
        // {
        //     Settings.GetSection("misc").SetValueOfKey("wowsource", "online");
        //     Settings.Save();
        //     gameObject.SetActive(false);
        // }
        if (ToggleExtracted.isOn)
        {
            if (Extracted.text != "" && Extracted.text != null)
            {
                Settings.GetSection("misc").SetValueOfKey("wowsource", "extracted");
                Settings.GetSection("misc").SetValueOfKey("extractedpath", Extracted.text);
                Settings.Save();
                gameObject.SetActive(false);
            }
            IsExtracted = true;
        }
        if (Settings.GetSection("misc").GetString("wowsource") == "online")
            terrainImport.GetComponent<TerrainImport>().Initialize();
    }

    public void AddButon ()
    {
        FolderBrowser.SetActive(true);
        FolderBrowser.GetComponent<FolderBrowserLogic>().Link("AddGamePath", this);
    }

    public void AddGamePath ()
    {
        string tempPath = FolderBrowser_SelectedFolderText.text + @"\";
        WoWPath.text = tempPath;
        Settings.GetSection("path").SetValueOfKey("selectedpath", tempPath);
        Settings.Save();
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
        if (File.Exists($@"{path}\\_retail_\\Wow.exe") || File.Exists($@"{path}\\_beta_\\WowB.exe") || File.Exists($@"{path}\\_ptr_\\WowT.exe") || File.Exists($@"{path}\\_retail_\\Wow.exe"))
            return true;
        else
            return false;
    }
}
