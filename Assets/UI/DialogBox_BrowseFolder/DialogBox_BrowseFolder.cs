using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox_BrowseFolder : MonoBehaviour {

    public UnityEngine.UI.Text DialogBox_Title;
    public UnityEngine.UI.Text DialogBox_Message;
    public UnityEngine.UI.Text DialogBox_InputBoxDefault;
    public UnityEngine.UI.InputField DialogBox_InputBoxPath;
    public string ChosenPath;
    public GameObject FolderBrowserPanel;
    public UnityEngine.UI.Text FolderBrowser_SelectedFolderText;
    private string okMethodName;
    private string cancelMethodName;
    private MonoBehaviour scriptUsed;

    void Start () {

    }

    public void LoadInfo (string Title, string Message, string InputDefault)
    {
        DialogBox_Title.text = Title;
        DialogBox_Message.text = Message;
        DialogBox_InputBoxDefault.text = InputDefault;
        ChosenPath = "";
    }

    public void FillTextBox ()
    {
        ChosenPath = FolderBrowser_SelectedFolderText.text;
        DialogBox_InputBoxPath.text = FolderBrowser_SelectedFolderText.text;
    }

    public void BrowseButton ()
    {
        FolderBrowserPanel.SetActive(true);
        FolderBrowserPanel.GetComponent<FolderBrowserLogic>().Link("FillTextBox", this);
    }

    public void OkButton ()
    {
        scriptUsed.Invoke(okMethodName, 0);
        gameObject.SetActive(false);
    }

    public void CancelButton ()
    {
        scriptUsed.Invoke(cancelMethodName, 0);
        gameObject.SetActive(false);
    }

    public void Link (string OkMethodName, string CancelMethodName, MonoBehaviour ScriptUsed)
    {
        okMethodName = OkMethodName;
        cancelMethodName = CancelMethodName;
        scriptUsed = ScriptUsed;
    }
}
