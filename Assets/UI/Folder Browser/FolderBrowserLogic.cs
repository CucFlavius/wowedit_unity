using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class FolderBrowserLogic : MonoBehaviour
{

    public GameObject FolderItem;
    public GameObject Content;
    public UnityEngine.UI.Text PanelInfo;

    public static UnityEngine.UI.Text SelectedFolderText;
    //public GameObject selectedGameObject = null;
    public GameObject currentlySelected;
    private GameObject returnObject;
    private string methodName;
    private MonoBehaviour className;

    // Use this for initialization
    void Start()
    {
        GetAvailableDrives();
        SelectedFolderText = transform.GetChild(4).GetComponent<UnityEngine.UI.Text> ();
        SelectedFolderText.text = " ";
    }

    void GetAvailableDrives()
    {
        string[] drives = Directory.GetLogicalDrives();
        foreach (string drive in drives)
        {
            GameObject item = Instantiate(FolderItem);
            item.gameObject.GetComponent<TreeBranch>().Path = drive;
            item.transform.SetParent(Content.transform);
            item.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = drive;
        }
    }

    public void Selected(GameObject Gobject, string text)
    {
        if (currentlySelected != null)
        {
            currentlySelected.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 100);
        }
        currentlySelected = Gobject;
        Gobject.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 0, 0, 100);
        SelectedFolderText.text = text;
    }


    public void Open(string info)
    {
        returnObject = EventSystem.current.currentSelectedGameObject;
        PanelInfo.text = info;
    }

    void Okay()
    {
        if (SelectedFolderText.text != " " && SelectedFolderText.text != "Must select a folder.")
        {
            className.Invoke(methodName, 0);
            gameObject.SetActive(false);
        }
        else
        {
            SelectedFolderText.text = "Must select a folder.";
        }
    }

    public void Link(string MethodName, MonoBehaviour ClassName)
    {
        methodName = MethodName;
        className = ClassName;
    }
}
