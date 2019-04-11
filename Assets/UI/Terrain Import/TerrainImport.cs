using Assets.Data.CASC;
using Assets.World;
using Assets.WoWEditSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TerrainImport : MonoBehaviour
{
    ////////////////////
    #region References
    
    public GameObject TerrainImporterPanel;
    public GameObject MapScrollList;
    public GameObject MapTabPrefab;
    public GameObject PanelErrorMessage;
    public GameObject UIManager;
    public GameObject minimapScrollPanel;
    public GameObject SelectPlayerBlockIcon;
    public GameObject SelectPlayerBlockIcon_prefab;
    public GameObject World;
    public GameObject LoadingText;
    public Minimap minimap;
    public UnityEngine.UI.Text DataText;
    public UnityEngine.UI.Text ErrorMessageText;
    public UnityEngine.UI.Toggle wmoToggle;
    public UnityEngine.UI.Toggle m2Toggle;

    #endregion
    ////////////////////

    ////////////////////
    #region Globals

    public Dictionary<string ,GameObject> MapTabs = new Dictionary<string, GameObject>();
    public List<string> ExtractedMapList;
    public List<string> FilteredList;
    public string FilterWord;
    public static bool Initialized = false;
    public Vector2 currentSelectedPlayerSpawn = new Vector2(0, 0); // default
    private string selectedMapName = "";

    #endregion
    ////////////////////

    // Initialize Terrain Importer //
    public void Initialize()
    {
        string mapPath = @"world\maps\";
        if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Extracted) // extracted //
            DataText.text = "Data: Extracted";
        else if (SettingsManager<Configuration>.Config.WoWSource == WoWSource.Game) // game //
            DataText.text = "Data: " + Casc.WoWVersion;
        GetMapList(mapPath);
        ClearMapList();
        PopulateMapList();
        Initialized = true;
    }

    // Open the Terrain Importer Panel //
    public void OpenTerrainImporter ()
    {
        if (!Initialized)
        {
            Initialize();
            minimap.pause = false;
        }
        TerrainImporterPanel.SetActive(true);

        // reset spawn //
        currentSelectedPlayerSpawn = new Vector2(0, 0); // default

        // reset toggles //
        wmoToggle.isOn  = SettingsManager<Configuration>.Config.TerrainImport.LoadWMOs;
        m2Toggle.isOn   = SettingsManager<Configuration>.Config.TerrainImport.LoadM2s;
    }

    ////////////////////
    #region Map List Methods
    
    // Get a List of Maps from the ADT Maps Directory //
    public void GetMapList (string mapPath)
    {
        string[] list = Casc.GetFolderListFromFolder(mapPath);

        ExtractedMapList = new List<string>();
        ExtractedMapList.AddRange(list);
    }

    // Create UI Buttons in the Map List Panel //
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

    // Destroy all UI Buttons in the Map List Panel //
    public void ClearMapList ()
    {
        MapTabs.Clear();
        foreach (Transform child in MapScrollList.transform)
        {
            Destroy(child);
        }
    }

    // Filter Buttons in the Map List Panel based on keyword //
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
                if (entry.Key.Contains(filter.ToLower()))
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

    #endregion
    ////////////////////

    ////////////////////
    #region UI Interaction

    // Map Selected in the Map List Panel //
    public void MapSelected(string mapName)
    {
        selectedMapName = mapName;
        minimap.ClearMinimaps(minimapScrollPanel);
        minimap.Load(mapName, minimapScrollPanel);
    }

    // Select a Player Spawn when Right Clicking on a Minimap Block //
    public void SelectPlayerSpawn(GameObject minimapBlock)
    {
        if (SelectPlayerBlockIcon == null)
            SelectPlayerBlockIcon = Instantiate(SelectPlayerBlockIcon_prefab);

        SelectPlayerBlockIcon.SetActive(true);
        SelectPlayerBlockIcon.transform.SetParent(minimapBlock.transform);
        SelectPlayerBlockIcon.GetComponent<RectTransform>().localPosition = new Vector2(50,-50);
        SelectPlayerBlockIcon.GetComponent<RectTransform>().localScale = minimapBlock.transform.localScale;
        currentSelectedPlayerSpawn = minimapBlock.GetComponent<MinimapBlock>().minimapCoords;
    }

    // Clicked the Load Full Map Button //
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

    // Load WMO's Toggle Interaction //
    public void Toggle_WMO(bool on)
    {
        Settings.LoadWMOs = on;
        Settings.SaveFile();
    }

    // Load M2's Toggle Interaction //
    public void Toggle_M2(bool on)
    {
        Settings.LoadM2s = on;
        Settings.SaveFile();
    }

    #endregion
    ////////////////////
}