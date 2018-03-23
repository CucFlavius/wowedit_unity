using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickHandler : MonoBehaviour {

    public TerrainImport terrainImport;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach (var go in raycastResults)
                {
                    if (go.gameObject.CompareTag("MinimapBlock"))
                    {
                        terrainImport.SelectPlayerSpawn(go.gameObject);
                    }
                }
            }
        }
    }
}
