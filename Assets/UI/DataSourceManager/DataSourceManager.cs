using Assets.UI.CASC;
using Assets.WoWEditSettings;
using CASCLib;
using System;
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
    public GameObject World;
    public GameObject CASC;

    public bool IsExtracted;

    public InputField WoWPath;
    public Dropdown DropdownOnline;
    public Dropdown DropdownProduct;
    public GameObject terrainImport;
    public GameObject FolderBrowser;
    public Text FolderBrowser_SelectedFolderText;

    public GameObject SelectBuild;
    public Dropdown Builds;
    public CASCConfig config;

    public string _gameType;
    public CASCHandler cascHandler;

    public void Initialize ()
    {
        // Update Toggles //
        if (Settings.GetSection("misc").GetString("wowsource") == "game")
            ToggleGame.isOn = true;
        else if (Settings.GetSection("misc").GetString("wowsource") == "online")
            ToggleOnline.isOn = true;
        else
            ToggleGame.isOn = true;

        WoWPath.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        // Update WoW Path //
        if (Settings.GetSection("path").GetString("selectedpath") != null)
            WoWPath.text = Settings.GetSection("path").GetString("selectedpath");

        string onlineProduct = Settings.GetSection("misc").GetString("onlineproduct");
        // Update Online Product //
        if (onlineProduct != null || onlineProduct != "" || onlineProduct != string.Empty)
        {
            switch (onlineProduct)
            {
                case "wowt":
                    DropdownOnline.value = 1;
                    break;
                case "wow":
                    DropdownOnline.value = 2;
                    break;
                case "wow_classic_beta":
                    DropdownOnline.value = 3;
                    break;
                case "wow_classic":
                    DropdownOnline.value = 4;
                    break;
                case "wow_beta":
                    DropdownOnline.value = 5;
                    break;
                default:
                    DropdownOnline.value = 0;
                    break;
            }
        }
    }

    public void ValueChangeCheck()
    {
        DropdownProduct.interactable = true;
        DropdownProduct.AddOptions(CASC.GetComponent<CascHandler>().ReadBuildInfo(WoWPath.text));

        string localProduct = Settings.GetSection("misc").GetString("localproduct");
        // Update Game Product //
        if (localProduct == null || localProduct == "" || localProduct == string.Empty) { }
        else
        {
            switch (localProduct)
            {
                case "wowt":
                    DropdownProduct.value = DropdownProduct.options.FindIndex((i) => { return i.text.Equals("wowt"); });
                    break;
                case "wow":
                    DropdownProduct.value = DropdownProduct.options.FindIndex((i) => { return i.text.Equals("wow"); });
                    break;
                case "wow_classic_beta":
                    DropdownProduct.value = DropdownProduct.options.FindIndex((i) => { return i.text.Equals("wow_classic_beta"); });
                    break;
                case "wow_classic":
                    DropdownProduct.value = DropdownProduct.options.FindIndex((i) => { return i.text.Equals("wow_classic"); });
                    break;
                case "wow_beta":
                    DropdownProduct.value = DropdownProduct.options.FindIndex((i) => { return i.text.Equals("wow_beta"); });
                    break;
                default:
                    DropdownProduct.value = 0;
                    break;
            }
        }
    }

    public void Ok ()
    {
        if (ToggleGame.isOn)
        {
            Settings.GetSection("misc").SetValueOfKey("wowsource", "game");
            Settings.GetSection("path").SetValueOfKey("selectedpath", WoWPath.text);

            if (CheckValidWoWPath(WoWPath.text))
            {
                // start Initialize casc thread //
                _gameType = DropdownProduct.options[DropdownProduct.value].text;
                config = CASCConfig.LoadLocalStorageConfig(Settings.GetSection("path").GetString("selectedpath"), _gameType);

                new Thread(() => {
                    CASC.GetComponent<CascHandler>().InitCasc(config);
                }).Start();

                // Save Settings //
                Settings.GetSection("misc").SetValueOfKey("localproduct", _gameType);
                Settings.Save();

                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("ERROR: Incorrect WoW Path...");
            }
        }
        if (ToggleOnline.isOn)
        {
            Settings.GetSection("misc").SetValueOfKey("wowsource", "online");

            // Initializes CASC Thread //
            _gameType = DropdownOnline.options[DropdownOnline.value].text;

            config = CASCConfig.LoadOnlineStorageConfig(_gameType, "us");

            SelectBuild.SetActive(true);
            List<string> builds = new List<string>();

            foreach (var cfg in config.Builds)
                builds.Add(cfg["build-name"][0]);

            Builds.AddOptions(builds);

            new Thread(() => {
                CASC.GetComponent<CascHandler>().InitCasc(config);
            }).Start();

            // Save Settings //
            Settings.GetSection("misc").SetValueOfKey("onlineproduct", _gameType);
            Settings.Save();

            gameObject.SetActive(false);

            terrainImport.GetComponent<TerrainImport>().Initialize();
        }
    }

    public void OkBuildSelect()
    {
        config.ActiveBuild = Builds.value;
        SelectBuild.SetActive(false);
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

    public bool CheckValidWoWPath (string path)
    {
        if (File.Exists($@"{path}\\_retail_\\Wow.exe") || File.Exists($@"{path}\\_beta_\\WowB.exe") || File.Exists($@"{path}\\_ptr_\\WowT.exe") || File.Exists($@"{path}\\_retail_\\Wow.exe"))
            return true;
        else
            return false;
    }
}
