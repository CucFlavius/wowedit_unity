using Assets.WoWEditSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserPreferences
{
    public static void Load()
    {
        try
        {
            Settings.terrainImport.LoadWMOs = (PlayerPrefs.GetInt("SettingsTerrainImport.LoadWMOs") == 1) ? true : false;
            Settings.terrainImport.LoadM2s = (PlayerPrefs.GetInt("SettingsTerrainImport.LoadM2s") == 1) ? true : false;
            Settings.terrainImport.ShowVertexColors = (PlayerPrefs.GetInt("Settings.showVertexColor") == 1) ? true : false;
            RenderSettings.fog = (PlayerPrefs.GetInt("RenderSettings.fog") == 1) ? true : false;
            Shader.SetGlobalFloat("_terrainWireframeOn", PlayerPrefs.GetInt("TerrainWireframe"));
            Shader.SetGlobalFloat("_terrainVertexColorOn", PlayerPrefs.GetInt("Settings.showVertexColor"));
        }
        catch
        {
            // prefs not saved
        }
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("SettingsTerrainImport.LoadWMOs", Settings.terrainImport.LoadWMOs ? 1 : 0);
        PlayerPrefs.SetInt("SettingsTerrainImport.LoadM2s", Settings.terrainImport.LoadM2s ? 1 : 0);
        PlayerPrefs.SetInt("Settings.showVertexColor", Settings.terrainImport.ShowVertexColors ? 1 : 0);
        PlayerPrefs.SetInt("RenderSettings.fog", RenderSettings.fog ? 1 : 0);
        PlayerPrefs.SetInt("TerrainWireframe", (int)Shader.GetGlobalFloat("_terrainWireframeOn"));
        PlayerPrefs.Save();
    }

}
