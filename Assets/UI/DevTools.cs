using Assets.World;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour {

    public WorldLoader World;

    public void ReadDB2(string filename)
    {
        //DB2 db2 = new DB2();
        //db2.Read(filename);
        M2.Load(@filename, -1, new Vector3(.5f, 0, 0), Quaternion.identity, new Vector3(.1f, .1f, .1f));
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
        MinimapThread.CompileMapList(mapName);
        World.LoadFullWorld(mapName, currentSelectedPlayerSpawn);
    }

    public void Button_Reset ()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

}
