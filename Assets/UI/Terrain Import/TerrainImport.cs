using CASCLib;
using Assets.World;
using Assets.WoWEditSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DB2;

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
    public Minimap MiniMap;
    public Text DataText;
    public Text ErrorMessageText;
    public Text WDTFileDataIDText;
    public Toggle wmoToggle;
    public Toggle m2Toggle;

    #endregion
    ////////////////////

    ////////////////////
    #region Globals

    public Dictionary<string, GameObject> MapTabs = new Dictionary<string, GameObject>();
    public static bool Initialized = false;
    public Vector2 currentSelectedPlayerSpawn = new Vector2(0, 0); // default
    public uint selectedWDTId;
    public Storage<MapRecord> MapRecords;
    public Dictionary<string, MapRecord> miniMap = new Dictionary<string, MapRecord>();

    #endregion
    ////////////////////

    // Initialize Terrain Importer //
    public void Initialize()
    {
        var reader = new DB2Reader(1349477);
        MapRecords = reader.GetRecords<MapRecord>();

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
            MiniMap.Pause = false;
        }
        TerrainImporterPanel.SetActive(true);

        // reset spawn //
        currentSelectedPlayerSpawn = new Vector2(0, 0); // default

        // reset toggles //
        wmoToggle.isOn  = SettingsTerrainImport.LoadWMOs;
        m2Toggle.isOn   = SettingsTerrainImport.LoadM2s;
    }

    ////////////////////
    #region Map List Methods

    // Create UI Buttons in the Map List Panel //
    public void PopulateMapList ()
    {
        foreach (var record in MapRecords)
        {
            GameObject MapItem  = Instantiate(MapTabPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            string MapName      = $"{record.Value.Directory} ({record.Value.MapName})";

            MapTabs.Add(MapName, MapItem);

            MapItem.transform.SetParent(MapScrollList.transform);
            MapItem.transform.GetChild(0).GetComponent<Text>().text = MapName;

            if (!miniMap.ContainsKey(MapName))
                miniMap.Add(MapName, record.Value);
        }
    }

    // Destroy all UI Buttons in the Map List Panel //
    public void ClearMapList ()
    {
        MapTabs.Clear();
        foreach (Transform child in MapScrollList.transform)
            Destroy(child);
    }

    // Filter Buttons in the Map List Panel based on keyword //
    public void FilterMapList (string filter)
    {
        if (filter == null)
        {
            foreach (KeyValuePair<string, GameObject> entry in MapTabs)
                entry.Value.SetActive(true);
        }
        else
        {
            foreach (KeyValuePair<string, GameObject> entry in MapTabs)
            {
                if (entry.Key.Contains(filter))
                    entry.Value.SetActive(true);
                else
                    entry.Value.SetActive(false);
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
        MiniMap.ClearMinimaps(minimapScrollPanel);

        if (miniMap.TryGetValue(mapName, out MapRecord record))
        {
            selectedWDTId = record.WdtFileDataID;

            WDTFileDataIDText.text = $"{selectedWDTId}";
            WDT.WDTEntries.Clear();
            if (WDT.ParseWDT(record.WdtFileDataID))
                MiniMap.Load(record.WdtFileDataID, minimapScrollPanel);
        }
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
        MiniMap.Pause = true;
        if (currentSelectedPlayerSpawn == new Vector2(0, 0) || currentSelectedPlayerSpawn == null)
            currentSelectedPlayerSpawn = new Vector2(MinimapData.Min.y + ((MinimapData.Max.y - MinimapData.Min.y) / 2), MinimapData.Min.x + ((MinimapData.Max.x - MinimapData.Min.x) / 2));
        
        Debug.Log($"Spawn : {currentSelectedPlayerSpawn.x} {currentSelectedPlayerSpawn.y}");

        World.GetComponent<WorldLoader>().LoadWorld(selectedWDTId, currentSelectedPlayerSpawn);
        LoadingText.SetActive(true);
    }

    // Clicked the Load Selected Area //
    public void ClickedLoadSelectedArea()
    {
        MiniMap.Pause = true;

        if (currentSelectedPlayerSpawn == new Vector2(0, 0) || currentSelectedPlayerSpawn == null)
            currentSelectedPlayerSpawn = new Vector2(MinimapData.Min.y + ((MinimapData.Max.y - MinimapData.Min.y) / 2), MinimapData.Min.x + ((MinimapData.Max.x - MinimapData.Min.x) / 2));

        Debug.Log($"Spawn: {currentSelectedPlayerSpawn.x} {currentSelectedPlayerSpawn.y}");

        // uint ADTFileDataId = WDT.WDTEntries[((int)currentSelectedPlayerSpawn.x, (int)currentSelectedPlayerSpawn.y)].RootADT;
        World.GetComponent<WorldLoader>().LoadSingleADT(selectedWDTId, currentSelectedPlayerSpawn);
        LoadingText.SetActive(true);
    }

    // Load WMO's Toggle Interaction //
    public void Toggle_WMO(bool on)
    {
        SettingsTerrainImport.LoadWMOs = on;
        PlayerPrefs.SetInt("LoadWMOs", on ? 1 : 0);
        PlayerPrefs.Save();
        //Settings.Save();
    }

    // Load M2's Toggle Interaction //
    public void Toggle_M2(bool on)
    {
        SettingsTerrainImport.LoadM2s = on;
        PlayerPrefs.SetInt("LoadM2s", on ? 1 : 0);
        PlayerPrefs.Save();
        //Settings.Save();
    }

    #endregion
    ////////////////////
}