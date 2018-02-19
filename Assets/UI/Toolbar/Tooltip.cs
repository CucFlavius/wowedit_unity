using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Display a tooltip on mouse over UI elements.
/// Based on the name of the UI gameObject.
/// </summary>

public class Tooltip : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print(eventData.pointerEnter.gameObject.name);
    }
}