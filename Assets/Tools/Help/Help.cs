using Assets.WoWEditSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LaunchHelpFile()
    {
        string path = SettingsManager<Configuration>.Config.ApplicationPath + @"\Help\WoWEditHelp.chm";
        Application.OpenURL(path);
    }
}
