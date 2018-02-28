using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTChunk : MonoBehaviour {

    // block settings //
    private bool current_showVertexColor;

    // Use this for initialization
    void Start () {
        current_showVertexColor = Settings.showVertexColor;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        // Show Vertex Color Toggle //
		if (Settings.showVertexColor != current_showVertexColor)
        {
            //float value = 2;
            current_showVertexColor = Settings.showVertexColor;
            Renderer renderer = GetComponent<Renderer>();
            Material mat = renderer.material;
            if (Settings.showVertexColor == true)
            {
                //value = 1;
                Debug.Log("Yay");
                mat.EnableKeyword("VERTEX_COLOR_ON");
                mat.DisableKeyword("VERTEX_COLOR_OFF");
            }
            else if (Settings.showVertexColor == false)
            {
                //value = 0;
                Debug.Log("Nay");

                mat.EnableKeyword("VERTEX_COLOR_OFF");
                mat.DisableKeyword("VERTEX_COLOR_ON");

            }
            current_showVertexColor = Settings.showVertexColor;
            
            
        }
    */
	}
}
