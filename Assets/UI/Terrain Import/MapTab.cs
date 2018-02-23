using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapTab : MonoBehaviour {

    public GameObject terrain;

    public void MapTabClicked ()
    {
        string selectedMap = gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;

        terrain = GameObject.Find("[UI Manager]");
        TerrainImport terrainImport = terrain.GetComponent<TerrainImport>();
        terrainImport.MapSelected(selectedMap);
    }
}
