using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTChunk : MonoBehaviour {

    // Material LoDs // store materials of the chunk here upon data loaded
    public Material high;
    public Material low;
    public Mesh mesh;

    public void UpdateDistance (int LoD)
    {
        if (LoD == 1)
        {
            if (low != null)
            {
                GetComponent<Renderer>().material = low;
            }
        }
        else if (LoD == 0)
        {
            if (high != null)
            {
                GetComponent<Renderer>().material = high;
            }
            else
            {
                if (low != null)
                    GetComponent<Renderer>().material = low;
            }
        }
    }

    public void MaterialReady(int lod, Material mat)
    {
        if (lod == 0)
        {
            high = mat;
            UpdateDistance(0);
        }
        if (lod == 1)
        {
            low = mat;
            UpdateDistance(1);
        }
        transform.parent.GetComponent<ADTBlock>().reCheck = true;
    }
}
