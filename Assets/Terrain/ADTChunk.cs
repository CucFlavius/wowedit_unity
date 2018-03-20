using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTChunk : MonoBehaviour {

    // Material LoDs // store materials of the chunk here upon data loaded
    public Material high;
    public Material low;
    //private int currentMatLoD; // 0-high 1-low
    //Camera cameraa;
    //Vector3 blockPosition;

    private void Start()
    {
        //cameraa = Camera.main;
        //blockPosition = transform.parent.position;
    }

    public void UpdateDistance (int LoD)
    {
        /*
        Camera camera = Camera.main;
        Vector3 heading = transform.position - camera.transform.position;
        float distance = Vector3.Dot(heading, camera.transform.forward);

        if (distance > Settings.terrainMaterialDistance / Settings.worldScale)
        {
            if (low != null)
            {
                if (currentMatLoD != 1)
                {
                    currentMatLoD = 1;
                    GetComponent<Renderer>().material = low;
                }
            }
        }
        else
        {
            if (high != null)
            {
                if (currentMatLoD != 0)
                {
                    currentMatLoD = 0;
                    GetComponent<Renderer>().material = high;
                }
            }
            else
            {
                GetComponent<Renderer>().material = low;
            }
        }
        */


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
                GetComponent<Renderer>().material = low;
            }
        }
        transform.parent.GetComponent<ADTBlock>().reCheck = true;
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
            //currentMatLoD = 1;
        }
    }

}
