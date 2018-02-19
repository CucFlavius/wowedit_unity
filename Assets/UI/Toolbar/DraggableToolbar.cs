using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableToolbar : MonoBehaviour, IDragHandler
{
    public bool Horizontal = true;
    RectTransform m_transform = null;

    // Use this for initialization
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Horizontal)
        {
            m_transform.position += new Vector3(eventData.delta.x, 0);
            if (m_transform.position.x <= 28.5f)
            {
                //m_transform.position = new Vector3(28.5f, -20, 0);
            }
        }
        else
        {
            m_transform.position += new Vector3(0, eventData.delta.y);
        }

        // magic : add zone clamping if's here.
    }
}