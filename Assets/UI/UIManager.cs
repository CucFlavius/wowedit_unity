using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject Panel_DataSourceManager;
    public GameObject Panel_Settings;
    public GameObject Panel_DatabaseView;

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

    public void Button_Settings()
    {
        Panel_Settings.SetActive(true);
        Panel_Settings.GetComponent<SettingsWindow>().Initialize();
    }

    public void Button_DatabaseView ()
    {
        Panel_DatabaseView.SetActive(true);
        Panel_DatabaseView.GetComponent<DatabaseView>().Initialize();
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
