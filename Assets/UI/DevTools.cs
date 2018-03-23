using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DevTools : MonoBehaviour {

    public WorldLoader World;
    private int firstxCoord;
    private int firstyCoord;
    private int lastxCoord;
    private int lastyCoord;
    private List<string> MinimapFileList = new List<string>();
    private Vector2 currentSelectedPlayerSpawn;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Button_LoadTestMap (string id)
    {
        World.ClearAllTerrain();
        string mapName = "";

        switch (id)
        {
            case "BC":
                {
                    mapName = "expansion01";
                    currentSelectedPlayerSpawn = new Vector2(32, 27);
                }
                break;
            case "Wrath":
                {
                    mapName = "northrend";
                    currentSelectedPlayerSpawn = new Vector2(24, 36);
                }
                break;
            case "Cata":
                {
                    mapName = "kalimdor";
                    currentSelectedPlayerSpawn = new Vector2(33, 35);
                }
                break;
            case "MoP":
                {
                    mapName = "hawaiimainland";
                    currentSelectedPlayerSpawn = new Vector2(32, 31);
                }
                break;
            case "WoD":
                {
                    mapName = "draenor";
                    currentSelectedPlayerSpawn = new Vector2(31, 29);
                }
                break;
            case "Legion":
                {
                    mapName = "troll raid";
                    currentSelectedPlayerSpawn = new Vector2(26, 24);
                }
                break;
            case "BfA":
                {
                    mapName = "kultiras";
                    currentSelectedPlayerSpawn = new Vector2(27, 31);
                }
                break;
            default:
                Debug.Log("Unknown Map");
                break;
        }

        if (!WDT.Flags.ContainsKey(mapName))
        {
            string wdtPath = @"world\maps\" + mapName + @"\";
            WDT.Load(wdtPath, mapName);
        }
        //GetWDTInfo(mapName);
        //SetMinimapListFromWDT(mapName);
        string mapPath = "";
        string minimapPath = "";
        if (Settings.Data[2] == "2") // extracted //
        {
            mapPath = Settings.Data[8] + @"\" + @"world\maps\" + mapName + @"\";
            minimapPath = Settings.Data[8] + @"\" + @"world\minimaps\" + mapName + @"\";
        }
        else if (Settings.Data[2] == "0") // game //
        {
            mapPath = @"world\maps\" + mapName;
            minimapPath = @"world\minimaps\" + mapName;
        }

        LoadMinimaps(minimapPath, mapName);

        //Vector2 currentSelectedPlayerSpawn = new Vector2(firstyCoord + ((lastyCoord - firstyCoord) / 2), firstxCoord + ((lastxCoord - firstxCoord) / 2));
        //Debug.Log(MinimapFileList.Count);
        World.LoadFullWorld(mapName, currentSelectedPlayerSpawn);
    }

    private void GetWDTInfo(string map_name)
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    firstxCoord = y;
                    firstyCoord = x;
                    break;
                }
            }
        }

        int previousxCoord = 0;
        int previousyCoord = 0;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    previousxCoord = y;
                    previousyCoord = x;
                    if (previousyCoord > lastyCoord) lastyCoord = previousyCoord;
                    if (previousxCoord > lastxCoord) lastxCoord = previousxCoord;
                }
            }
        }
    }

    private void SetMinimapListFromWDT(string map_name)
    {
        MinimapFileList = new List<string>();
        string mapPath = @"world\maps\" + map_name;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    MinimapFileList.Add("map" + x + "_" + y);
                }
            }
        }
    }

    public void LoadMinimaps(string minimapPath, string mapName)
    {
        // reset global variables //
        MinimapFileList.Clear();

        // update global list of minimap files : MinimapFileList //
        List<string> FileList = Casc.GetFileListFromFolder(minimapPath);
        foreach (string file in FileList)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            if (fileName.StartsWith("m"))
            {
                MinimapFileList.Add(fileName);
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
}
