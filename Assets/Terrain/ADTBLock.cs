using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTBlock : MonoBehaviour {

    public int LoD;
    public bool LoD0Loaded = false;
    public bool LoD1Loaded = false;
    public Texture[] textures;
    public Renderer rend;

    public void LoadLod0()
    {
        //ADT.Load( Path, "MapName", "coords")
        //rend = GetComponent<Renderer>();
        //rend.material.mainTexture = textures[0];
    }

    public void LoadLod1()
    {
        rend = GetComponent<Renderer>();
        rend.material.mainTexture = textures[1];
    }

}
