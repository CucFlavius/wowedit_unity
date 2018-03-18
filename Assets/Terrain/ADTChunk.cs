using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTChunk : MonoBehaviour {

    // Material LoDs // store materials of the chunk here upon data loaded
    public Material high;
    public Material low;
	
	// Update is called once per frame
	void Update () {

        // check distance from camera

        // if distance long
            // swap to low material
        // if distance short
            // if material high is loaded != null
                // swap to high material

	}

    public void MaterialReady(int lod, Material mat)
    {
        if (lod == 0)
        {
            high = mat;
            //GetComponent<Renderer>().material = high;
        }
        if (lod == 1)
        {
            low = mat;
            GetComponent<Renderer>().material = low;
        }
    }

}
