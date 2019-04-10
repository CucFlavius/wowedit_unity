using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.World;
using Assets.World.Models;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour {

    public WorldLoader World;

    public void ReadDB2(string filename)
    {
        //DB2 db2 = new DB2();
        //db2.Read(filename);
        WMO.Load(@"world\wmo\KulTiras\Human\8ara_warfronts_mine01.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
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
