using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string path = @"F:\ExportedWOW\root\tileset\generic\black.blp";
        Stream str = File.Open(path, FileMode.Open);
        byte[] data = BLP.GetUncompressed(str, false);
        str.Close();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
