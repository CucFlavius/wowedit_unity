using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Test : MonoBehaviour {

    public WMOhandler wmoHandler;

	// Use this for initialization
	void Start () {
        Invoke("Delayed", 0.1f);
	}
	
    void Delayed ()
    {
        //wmoHandler.AddToQueue(@"world\wmo\zuldazar\troll\8tr_zandalari_pyramid.wmo", Vector3.zero, Quaternion.identity, Vector3.one);
        //wmoHandler.AddToQueue(@"world\wmo\azeroth\buildings\duskwood_barn\duskwood_barn.wmo", Vector3.zero, Quaternion.identity, Vector3.one);
    }
}
