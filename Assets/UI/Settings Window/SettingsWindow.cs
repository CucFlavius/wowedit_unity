using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    public GameObject[] TabPanels;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize ()
    {

    }

    public void Button_OK()
    {
        gameObject.SetActive(false);
    }

    public void Button_Close()
    {
        gameObject.SetActive(false);
    }

    public void ClickedSettingsTab(int tab)
    {
        CloseAllTabs();
        TabPanels[tab].SetActive(true);
    }
    public void CloseAllTabs()
    {
        for (int i = 0; i < TabPanels.Length; i++)
        {
            TabPanels[i].SetActive(false);
        }
    }
}
