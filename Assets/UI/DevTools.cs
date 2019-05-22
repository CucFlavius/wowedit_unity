using Assets.UI.CASC;
using Assets.World;
using CASCLib;
using UnityEngine;

public class DevTools : MonoBehaviour {

    public WorldLoader World;
    public Jenkins96 Hasher = new Jenkins96();
    public GameObject CASC;
    public CASCHandler CASCHandler;
    public CascHandler CascHandler;

    public void ReadDB2(string filename)
    {
        // CASCHandler = CASC.GetComponent<CascHandler>().cascHandler;
        CascHandler = CASC.GetComponent<CascHandler>();
        // int fileDataId = Casc.GetComponent<CascHandler>().cascHandler.Root.GetFileDataIdByName(filename);

        if (CascHandler.listfileLoader.ListFileEntry.TryGetValue(filename, out uint fileDataId))
            M2.Load(fileDataId, -1, Vector3.zero, Quaternion.identity, Vector3.one);
    }

    public void Button_LoadTestMap (string id)
    {
        //World.ClearAllTerrain();
        string mapName = "";
        Vector2 currentSelectedPlayerSpawn = Vector2.zero;

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
                    currentSelectedPlayerSpawn = new Vector2(18, 24);
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
        MinimapThread.CompileMapList(mapName);
        World.LoadFullWorld(mapName, currentSelectedPlayerSpawn);
    }

    public void Button_Reset ()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

}
