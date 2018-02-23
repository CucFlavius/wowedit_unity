using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casc_LoaderUI : MonoBehaviour {

    public GameObject DataSourceManagerPanel;
    public GameObject LoadingPanel;
    public UnityEngine.UI.Text LoadText;
    public UnityEngine.UI.Image LoadingBar;

    // Use this for initialization
    void Start()
    {
        CascInitialize.Reset();
    }

    public void StopInitializing ()
    {
        CascInitialize.Reset();
        LoadingPanel.SetActive(false);
        CascInitialize.Stop();
        DataSourceManagerPanel.SetActive(true);
        DataSourceManagerPanel.GetComponent<DataSourceManager>().Initialize();
    }

    void Update()
    {
        if (CascInitialize.Working)
        {
            if (!LoadingPanel.activeSelf)
            {
                LoadingPanel.SetActive(true);
            }
            if (CascInitialize.CurrentWorkerText != CascInitialize.PreviousWorkerText)
            {
                CascInitialize.PreviousWorkerText = CascInitialize.CurrentWorkerText;
                LoadText.text = CascInitialize.PreviousWorkerText;
            }
            if (LoadingPanel.activeSelf)
            {
                if (CascInitialize.CurrentWorkerPercent != CascInitialize.PreviousWorkerPercent)
                {
                    CascInitialize.PreviousWorkerPercent = CascInitialize.CurrentWorkerPercent;
                    LoadingBar.fillAmount = CascInitialize.PreviousWorkerPercent;
                }
            }
        }
        else
        {
            if (LoadingPanel.activeSelf)
            {
                LoadingPanel.SetActive(false);
                CascInitialize.PreviousWorkerPercent = 0;
                CascInitialize.CurrentWorkerPercent = 0;
            }
        }
        if (CascInitialize.Working_InitializationFinished)
        {
            CascInitialize.Working_InitializationFinished = false;
            CascInitialize.Working = false;
        }
    }
}
