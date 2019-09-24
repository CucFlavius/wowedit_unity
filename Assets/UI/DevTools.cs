using Assets.Data.WoW_Format_Parsers;
using Assets.UI.CASC;
using Assets.World;
using CASCLib;
using System.IO;
using UnityEngine;
using static DB2;

public class DevTools : MonoBehaviour {

    public WorldLoader world;
    public WorldLoader2 world2;
    public Jenkins96 Hasher = new Jenkins96();
    public GameObject CASC;
    public CASCHandler CASCHandler;
    public CascHandler CascHandler;

    public void ReadDB2(string fileDataId)
    {
        // CASCHandler = CASC.GetComponent<CascHandler>().cascHandler;
        // 
        // M2.Load(FileDataID, -1, Vector3.zero, Quaternion.identity, Vector3.one);
    }

    public void Button_LoadTestMap (string id)
    {
        CASCHandler = CASC.GetComponent<CascHandler>().cascHandler;
        //World.ClearAllTerrain();
        uint WdtFileDataId = 0;
        Vector2 currentSelectedPlayerSpawn = Vector2.zero;

        switch (id)
        {
            case "BC":
                {
                    WdtFileDataId = 828395;
                    currentSelectedPlayerSpawn = new Vector2(32, 27);
                }
                break;
            case "Wrath":
                {
                    WdtFileDataId = 822688;
                    currentSelectedPlayerSpawn = new Vector2(18, 24);
                }
                break;
            case "Cata":
                {
                    WdtFileDataId = 782779;
                    currentSelectedPlayerSpawn = new Vector2(33, 35);
                }
                break;
            case "MoP":
                {
                    WdtFileDataId = 805681;
                    currentSelectedPlayerSpawn = new Vector2(32, 31);
                }
                break;
            default:
                Debug.Log("Unknown Map");
                break;
        }

        WDT.WDTEntries.Clear();
        if (WDT.ParseWDT(WdtFileDataId))
            //World.LoadSingleADT(WdtFileDataId, currentSelectedPlayerSpawn);
            //world2.LoadWorld(WdtFileDataId, currentSelectedPlayerSpawn);
            world.LoadWorld(WdtFileDataId, currentSelectedPlayerSpawn);
        else
            Debug.Log("Error");
    }

    public void Button_Reset ()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

}
