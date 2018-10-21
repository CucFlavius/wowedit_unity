using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    Button applyButton;
    // Use this for initialization
    void Start () {
        applyButton = GameObject.FindGameObjectWithTag("ApplyButton").GetComponent<Button>();
        applyButton.onClick.AddListener(() => Apply_Settings());
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

    public void Default_Button()
    {

    }

    public void Apply_Settings()
    {
        applyButton = GameObject.FindGameObjectWithTag("ApplyButton").GetComponent<Button>();
        applyButton.interactable = false;
    }
}
