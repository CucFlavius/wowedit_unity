using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceManager : MonoBehaviour {

    public Toggle ToggleGame;
    public Toggle ToggleOnline;
    public Toggle ToggleExtracted;

    public Dropdown DropdownGame;
    public Dropdown DropdownOnline;
    public InputField Extracted;

    public GameObject FolderBrowser;
    public UnityEngine.UI.Text FolderBrowser_SelectedFolderText;

    private List<string> DropdownGameList = new List<string>();

    public void Initialize ()
    {
        // Update Game List //
        DropdownGameList.Clear();
        if (Settings.Data[4] != null)
            DropdownGameList.Add(Settings.Data[4]);
        if (Settings.Data[5] != null )
            DropdownGameList.Add(Settings.Data[5]);
        if (Settings.Data[6] != null )
            DropdownGameList.Add(Settings.Data[6]);
        // Add custom game path to the list //
        if (Settings.Data[9] != null && Settings.Data[9].Length > 1)
            DropdownGameList.Add(Settings.Data[9]);
        DropdownGame.ClearOptions();
        DropdownGame.AddOptions(DropdownGameList);

        // Update Online List //
        DropdownOnline.ClearOptions();

        // Update Toggles //
        if (Settings.Data[2] == "0")
            ToggleGame.isOn = true;
        else if (Settings.Data[2] == "1")
            ToggleOnline.isOn = true;
        else if (Settings.Data[2] == "2")
            ToggleExtracted.isOn = true;

        // Update Extracted Path //
        if (Settings.Data[8] != null)
        {
            Extracted.text = Settings.Data[8];
        }
    }

    public void Ok ()
    {
        if (ToggleGame.isOn)
        {
            Settings.Data[2] = "0";
            // start Initialize casc thread //
            Settings.Save();
            CascInitialize.Start();
            gameObject.SetActive(false);
        }
        if (ToggleOnline.isOn)
        {
            Settings.Data[2] = "1";
            Settings.Save();
            gameObject.SetActive(false);
        }
        if (ToggleExtracted.isOn)
        {
            if (Extracted.text != "" && Extracted.text != null)
            {
                Settings.Data[2] = "2";
                Settings.Data[8] = Extracted.text;
                Settings.Save();
                gameObject.SetActive(false);
            }
        }
    }

    public void AddButon ()
    {
        FolderBrowser.SetActive(true);
        FolderBrowser.GetComponent<FolderBrowserLogic>().Link("AddGamePath", this);
    }

    public void AddGamePath ()
    {
        string tempPath = FolderBrowser_SelectedFolderText.text + @"\";
        if (!DropdownGameList.Contains(tempPath))
        {
            //print(tempPath);
            if (CheckValidWoWPath(tempPath))
            {
                // correct path //
                Settings.Data[9] = tempPath;
                DropdownGameList.Add(tempPath);
                DropdownGame.ClearOptions();
                DropdownGame.AddOptions(DropdownGameList);
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
        if (File.Exists(path + "Wow-64.exe") || File.Exists(path + "WowB-64.exe") || File.Exists(path + "WowT-64.exe"))
        {
            print("yep");
            return true;
        }
        else
        {
            return false;
        }
    }
}
