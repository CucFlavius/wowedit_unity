using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TerrainImport : MonoBehaviour {

    public GameObject TerrainImporterPanel;
    public GameObject MapScrollList;
    public GameObject MapTabPrefab;
    public UnityEngine.UI.Text DataText;
    public GameObject PanelErrorMessage;
    public UnityEngine.UI.Text ErrorMessageText;
    public GameObject UIManager;
    public Dictionary<string ,GameObject> MapTabs = new Dictionary<string, GameObject>();
    public List<string> ExtractedMapList;
    public List<string> FilteredList;
    public string FilterWord;
    private static bool Initialized = false;

    public void MapSelected(string mapName)
    {
        UIManager.GetComponent<MinimapHandler>().ClearMinimaps();
        string mapPath = null;
        string minimapPath = null;

        // Parse WDT //
        if (!WDT.Flags.ContainsKey(mapName))
        {
            string wdtPath = @"world\maps\" + mapName + @"\";
            WDT.Load(wdtPath, mapName);
        }

        if (Settings.Data[2] == "2") // extracted //
        {
            mapPath = Settings.Data[8] + @"\" + @"world\maps\" + mapName + @"\";
            minimapPath = Settings.Data[8] + @"\" + @"world\minimaps\" + mapName + @"\";
        }
        else if (Settings.Data[2] == "0") // game //
        {
            mapPath =  @"world\maps\" + mapName;
            minimapPath = @"world\minimaps\" + mapName;
        }
        bool MinimapsExist = CheckForMinimaps(minimapPath);
        if (CheckForADTs(mapPath))
        {
            if (CheckForMinimaps(minimapPath))
            {
                PanelErrorMessage.SetActive(false);
                // load minimaps //
                UIManager.GetComponent<MinimapHandler>().LoadMinimaps(minimapPath, mapName);
            }
            else
            {
                PanelErrorMessage.SetActive(true);
                ErrorMessageText.text = "No minimaps available.";
                // load blank minimaps //
                UIManager.GetComponent<MinimapHandler>().LoadBlankMinimaps(mapPath, mapName);
            }
        }
        else
        {
            if (CheckForMinimaps(minimapPath))
            {
                PanelErrorMessage.SetActive(true);
                ErrorMessageText.text = "WMO Only Zone.";
                // load minimaps //
                UIManager.GetComponent<MinimapHandler>().LoadMinimaps(minimapPath, mapName);
            }
            else
            {
                PanelErrorMessage.SetActive(true);
                ErrorMessageText.text = "WMO Only Zone."+"\n"+"No minimaps available.";
            }
        }
    }

    public void Initialize()
    {
        string mapPath = null;
        if (Settings.Data[2] == "2") // extracted //
        {
            DataText.text = "Data: Extracted";
            mapPath = Settings.Data[8] + @"\" + @"world\maps\";
        }

        else if (Settings.Data[2] == "0") // game //
        {
            DataText.text = "Data: " + Casc.WoWVersion;
            mapPath = @"world\maps\";
        }
        GetMapList(mapPath);
        PopulateMapList();
        Initialized = true;
    }

    public void OpenTerrainImporter ()
    {
        if (!Initialized)
        {
            Initialize();
        }

        TerrainImporterPanel.SetActive(true);
    }

    public void GetMapList (string mapPath)
    {
        string[] list = Casc.GetFolderListFromFolder(mapPath);
        ExtractedMapList = new List<string>();
        ExtractedMapList.AddRange(list);
    }

    public void PopulateMapList ()
    {
        for (int i =0; i < ExtractedMapList.Count; i++)
        {
            GameObject MapItem = Instantiate(MapTabPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            string fileName = Path.GetFileName(ExtractedMapList[i]);
            MapTabs.Add(fileName, MapItem);
            MapItem.transform.SetParent(MapScrollList.transform);
            MapItem.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = fileName;
        }
    }

    public void FilterMapList (string filter)
    {
        if (filter == null)
        {
            foreach (KeyValuePair<string, GameObject> entry in MapTabs)
            {
                entry.Value.SetActive(true);
            }
        }
        else
        {
            foreach (KeyValuePair<string, GameObject> entry in MapTabs)
            {
                if (entry.Key.Contains(filter))
                {
                    entry.Value.SetActive(true);
                }
                else
                {
                    entry.Value.SetActive(false);
                }
            }
        }
    }

    public bool CheckForADTs(string path)
    {
        string[] files = Casc.GetFileListFromFolder(path);
        foreach (string file in files)
        {
            if (Path.GetExtension(file).ToLower() == ".adt")
                return true;
        }
        return false;
    }

    public bool CheckForMinimaps(string path)
    {
        if (Casc.FolderExists(path))
            return true;
        else
            return false;
    }

}
