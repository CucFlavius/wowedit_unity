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
    public UnityEngine.UI.Toggle wmoToggle;
    public UnityEngine.UI.Toggle m2Toggle;

    public Dictionary<string ,GameObject> MapTabs = new Dictionary<string, GameObject>();
    public List<string> ExtractedMapList;
    public List<string> FilteredList;
    public string FilterWord;
    public static bool Initialized = false;
    public Minimap minimap;
    public GameObject minimapScrollPanel;
    public Vector2 currentSelectedPlayerSpawn;
    public GameObject SelectPlayerBlockIcon;
    public GameObject SelectPlayerBlockIcon_prefab;
    public GameObject World;
    private string selectedMapName = "";
    public GameObject LoadingText;

    void Start()
    {
        currentSelectedPlayerSpawn = new Vector2(0, 0); // default
    }

    public void MapSelected(string mapName)
    {
        selectedMapName = mapName;
        minimap.ClearMinimaps(minimapScrollPanel);
        minimap.Load(mapName, minimapScrollPanel);
    }

    public void Initialize()
    {
        string mapPath = @"world\maps\";
        if (Settings.Data[2] == "2") // extracted //
            DataText.text = "Data: Extracted";
        else if (Settings.Data[2] == "0") // game //
            DataText.text = "Data: " + Casc.WoWVersion;
        GetMapList(mapPath);
        ClearMapList();
        PopulateMapList();
        Initialized = true;
    }

    public void OpenTerrainImporter ()
    {
        if (!Initialized)
        {
            Initialize();
            minimap.pause = false;
        }
        TerrainImporterPanel.SetActive(true);

        // reset toggles //
        wmoToggle.isOn = ADTSettings.LoadWMOs;
        m2Toggle.isOn = ADTSettings.LoadM2s;
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

    public void ClearMapList ()
    {
        MapTabs.Clear();
        foreach (Transform child in MapScrollList.transform)
        {
            Destroy(child);
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
        List<string> files = Casc.GetFileListFromFolder(path);
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

    public void SelectPlayerSpawn(GameObject minimapBlock)
    {
        if (SelectPlayerBlockIcon == null)
            SelectPlayerBlockIcon = Instantiate(SelectPlayerBlockIcon_prefab);

        SelectPlayerBlockIcon.SetActive(true);
        SelectPlayerBlockIcon.transform.SetParent(minimapBlock.transform);
        SelectPlayerBlockIcon.GetComponent<RectTransform>().localPosition = new Vector2(50,-50);
        SelectPlayerBlockIcon.GetComponent<RectTransform>().localScale = minimapBlock.transform.localScale;
        currentSelectedPlayerSpawn = minimapBlock.GetComponent<MinimapBlock>().minimapCoords;
        //Debug.Log(currentSelectedPlayerSpawn);
    }

    public void ClickedLoadFull()
    {
        minimap.pause = true;
        if (currentSelectedPlayerSpawn == new Vector2(0, 0) || currentSelectedPlayerSpawn == null)
        {
            currentSelectedPlayerSpawn = new Vector2(MinimapData.Min.y + ((MinimapData.Max.y - MinimapData.Min.y) / 2), MinimapData.Min.x + ((MinimapData.Max.x - MinimapData.Min.x) / 2));
        }
        Debug.Log("Spawn : " + currentSelectedPlayerSpawn.x + " " + currentSelectedPlayerSpawn.y);
        World.GetComponent<WorldLoader>().LoadFullWorld(selectedMapName, currentSelectedPlayerSpawn);
        LoadingText.SetActive(true);
    }

    public void Toggle_WMO(bool on)
    {
        ADTSettings.LoadWMOs = on;
    }

    public void Toggle_M2(bool on)
    {
        ADTSettings.LoadM2s = on;
    }

}
