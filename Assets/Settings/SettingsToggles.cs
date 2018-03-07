using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggles : MonoBehaviour {

    public Material TerrainMat;
    public UnityEngine.UI.Toggle distanceFogToggle;

    public void Start()
    {
        // reset toggles //
        //distanceFogToggle.isOn = Settings.Data["need fog bool here"];
    }

    public void Toggle_showVertexColor(bool Toggle)
    {
        Settings.showVertexColor = Toggle;

        if (Toggle)
        {
            //value = 1;
            Debug.Log("Yay");
            TerrainMat.EnableKeyword("VERTEX_COLOR_ON");
            TerrainMat.DisableKeyword("VERTEX_COLOR_OFF");
        }
        else if (!Toggle)
        {
            //value = 0;
            Debug.Log("Nay");

            TerrainMat.EnableKeyword("VERTEX_COLOR_OFF");
            TerrainMat.DisableKeyword("VERTEX_COLOR_ON");

        }
    }

    public void Toggle_showDistanceFog(bool Toggle)
    {
        RenderSettings.fog = Toggle;
    }

}
