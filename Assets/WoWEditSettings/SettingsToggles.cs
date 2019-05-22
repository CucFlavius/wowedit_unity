using Assets.WoWEditSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggles : MonoBehaviour {

    public Material TerrainMat;
    public UnityEngine.UI.Toggle[] toggles;

    public void Start()
    {
        // update toggles
        toggles[0].isOn = (PlayerPrefs.GetInt("Settings.showVertexColor") == 1) ? true : false;
        toggles[1].isOn = (PlayerPrefs.GetInt("RenderSettings.fog") == 1) ? true : false;
        toggles[2].isOn = (PlayerPrefs.GetInt("TerrainWireframe") == 1) ? true : false;

    }

    public void Toggle_showVertexColor(bool Toggle)
    {
        Settings.ShowVertexColors = Toggle;
        Shader.SetGlobalFloat("_terrainVertexColorOn", (Toggle == true) ? 1 : 0);
        UserPreferences.Save();
    }

    public void Toggle_showDistanceFog(bool Toggle)
    {
        RenderSettings.fog = Toggle;
        UserPreferences.Save();
    }

    public void Toggle_showTerrainWireframe(bool Toggle)
    {
        Shader.SetGlobalFloat("_terrainWireframeOn", (Toggle==true)? 1 : 0);
        UserPreferences.Save();
    }
}
