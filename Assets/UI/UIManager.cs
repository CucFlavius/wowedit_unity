using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject Panel_DataSourceManager;
    public GameObject Panel_Settings;
    public static UIManager staticUIManager;        // static reference to this script

    // Use this for initialization
    public void Initialize ()
    {
        staticUIManager = this;
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

    ///////////////////////////////
    ///       Menu Buttons      ///
    ///////////////////////////////
    public void ExitApplication ()
    {
        // check if you need to save project first // soon
        Application.Quit();
    }
}
