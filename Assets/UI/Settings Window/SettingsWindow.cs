using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : MonoBehaviour {

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
}
