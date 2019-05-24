using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTab : MonoBehaviour {

    private GameObject terrain;

    public void MapTabClicked ()
    {
        string selectedMap = gameObject.transform.GetChild(0).GetComponent<Text>().text;

        terrain = GameObject.Find("[UI Manager]");
        TerrainImport terrainImport = terrain.GetComponent<TerrainImport>();
        terrainImport.MapSelected(selectedMap);
    }
}
