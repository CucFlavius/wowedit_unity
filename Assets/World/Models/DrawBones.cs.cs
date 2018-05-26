using System.Collections.Generic;
using UnityEngine;

public class DrawBones : MonoBehaviour
{
    List<Transform> bonePivots;

    private void Awake()
    {
        bonePivots = new List<Transform>();
    }

    void drawbone(Transform t)
    {
        foreach (Transform child in t)
        {
            Debug.DrawLine(t.position * 0.01f + child.position * 0.99f, t.position * 0.99f + child.position * 0.01f, new Color(.3f, .3f, .3f));
            bonePivots.Add(child);
            drawbone(child);
        }
    }

    void Update()
    {
        bonePivots.Clear();
        drawbone(transform);
    }

    void OnDrawGizmos()
    {
        foreach (Transform child in bonePivots)
        {
            Gizmos.color = new Color(.1f,.1f,.1f);
            Gizmos.DrawWireSphere(child.position, .001f);
        }
    }
}