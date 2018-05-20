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
            M2.Load(@"character\draenei\female\draeneifemale.m2", 1, Vector3.zero, Quaternion.identity, Vector3.one);
            Debug.Log("Model Loaded");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
