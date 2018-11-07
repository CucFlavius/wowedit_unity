using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditorToolbox : MonoBehaviour
{
    public GameObject brushProjectorA;
    public GameObject brushProjectorB;

    void Awake()
    {
        EnableTerrainTool();
    }

    public void EnableTerrainTool()
    {
        brushProjectorA.SetActive(true);
        brushProjectorB.SetActive(true);
    }

    public void DisableTerrainTool()
    {
        brushProjectorA.SetActive(false);
        brushProjectorB.SetActive(false);
    }

}
