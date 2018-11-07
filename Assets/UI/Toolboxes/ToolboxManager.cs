using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxManager : MonoBehaviour
{
    public List<GameObject> toolboxes;
    public List<GameObject> toolboxButtons;


    public void OpenTerrainEditor(bool toggle)
    {
        if (toggle)
            toolboxes[0].SetActive(true);
        else
            CloseTerrainEditor();
    }

    public void CloseTerrainEditor()
    {
        toolboxes[0].SetActive(false);
        toolboxButtons[0].GetComponent<UnityEngine.UI.Toggle>().isOn = false;
    }

    public void CloseToolboxes()
    {
        foreach (GameObject toolbox in toolboxes)
        {
            toolbox.SetActive(false);
        }
    }

}
