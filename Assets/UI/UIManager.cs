using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject Panel_DataSourceManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    ///////////////////////////////
    ///     Toolbar Buttons     ///
    ///////////////////////////////

    public void Button_DataSourceManager ()
    {
        Panel_DataSourceManager.SetActive(true);
        Panel_DataSourceManager.GetComponent<DataSourceManager>().Initialize();
    }

    ///////////////////////////////
    ///       Menu Buttons      ///
    ///////////////////////////////
    public void ExitApplication ()
    {
        // check if you need to save project first // soon
        Application.Quit();
    }
}
