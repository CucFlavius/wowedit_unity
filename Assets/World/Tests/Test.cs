using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Test : MonoBehaviour {

    public WMOhandler wmoHandler;
    public M2handler m2Handler;

	// Use this for initialization
	void Start () {
        Invoke("Delayed", 0.1f);
	}
	
    void Delayed ()
    {
        //wmoHandler.AddToQueue(@"world\wmo\zuldazar\troll\8tr_zandalari_pyramid.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
        //wmoHandler.AddToQueue(@"world\wmo\azeroth\buildings\duskwood_barn\duskwood_barn.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
        //wmoHandler.AddToQueue(@"world\wmo\azeroth\buildings\stormwind\stormwind2_016.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
        try
        {
            M2.Load(@"character\draenei\female\draeneifemale.m2", -1, Vector3.zero, Quaternion.identity, Vector3.one);
            M2.Load(@"world\expansion05\doodads\ashran\6as_rock_c01.m2", -1, new Vector3 (.5f,0,0), Quaternion.identity, new Vector3(.1f,.1f,.1f));
            Debug.Log("Model Loaded");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
