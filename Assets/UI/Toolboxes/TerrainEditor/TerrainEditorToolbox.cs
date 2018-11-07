using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditorToolbox : MonoBehaviour
{
    public GameObject brushProjectorA;
    public GameObject brushProjectorB;
    private int layerMask;
    public static bool Enabled;
    public static bool projectorEnabled;

    public void Awake()
    {
        Enabled = true;
        layerMask = LayerMask.GetMask("Terrain");
    }

    public void Update()
    {
        if (Enabled)
        {
            // position the brush gizmo //
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (!projectorEnabled)
                    EnableBrushProjector();
                brushProjectorA.transform.position = hit.point + new Vector3(0.0f, 10.0f, 0.0f);
            }
            else
            {
                DisableBrushProjector();
            }

            // interact with the terrain //
            if (Input.GetMouseButtonDown(0))
            {

            }
        }
    }

    public void Disable()
    {
        Enabled = false;
        DisableBrushProjector();
    }

    public void EnableBrushProjector()
    {
        brushProjectorA.SetActive(true);
        brushProjectorB.SetActive(true);
        projectorEnabled = true;
    }

    public void DisableBrushProjector()
    {
        brushProjectorA.SetActive(false);
        brushProjectorB.SetActive(false);
        projectorEnabled = false;
    }

}
