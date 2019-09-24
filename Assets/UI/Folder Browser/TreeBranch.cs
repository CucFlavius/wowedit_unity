using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TreeBranch : MonoBehaviour {

    public string Path;
    public List<string> Subfolders;
    bool Open;
    public List<GameObject> Children;
    public List<GameObject> Parents;
    public GameObject Content;
    public GameObject PanelFolderBrowser;


    void Start () {
        Open = false;
        PanelFolderBrowser = GameObject.Find("Panel_FolderBrowser");
        Content = GameObject.Find("Panel_FolderBrowser/Scroll View/Viewport/Content");//transform.parent.gameObject;

        FindSubfolders();

        if (Subfolders.Count == 0)
        {
            transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false); //expand button
            transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false); //contract button
        }
        if (Subfolders.Count != 0)
        {
            transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); //expand button
            transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);//contract button
        }
    }
	
	public void Clicked ()
    {
        if (Subfolders.Count != 0)
        {
            if (!Open) // expand
            {
                Children = new List<GameObject>();
                for (int i = 0; i < Subfolders.Count; i++)
			{
                    GameObject item = Instantiate(gameObject);
                    item.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 100);
                    item.transform.SetParent(Content.transform);
                    item.gameObject.GetComponent<TreeBranch>().Path = Subfolders[i];
                    item.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text > ().text = System.IO.Path.GetFileName(Subfolders[i]);
                    int index = transform.GetSiblingIndex();
                    item.transform.SetSiblingIndex(index + 1);
                    item.transform.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition.x + 20, item.transform.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition.y);
                    item.gameObject.GetComponent<TreeBranch>().Parents.Add(gameObject);
                    Children.Add(item);
                }
                Open = true;
                SendChildrenList();
                transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false); //expand button
                transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true); //contract button
            }
            else if (Open) // contract
            {
                foreach (GameObject go in Children)
                {
                    Destroy(go);
                }
                Open = false;
                transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); //expand button
                transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false); //contract button
            }
        }
        //Content.GetComponent<FolderBrowser>().selectedGameObject = this.gameObject;
        PanelFolderBrowser.gameObject.GetComponent<FolderBrowserLogic>().Selected(gameObject, Path);
    }

    void FindSubfolders ()
    {
        DirectoryInfo info = new DirectoryInfo(Path);
        if ((info.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (info.Attributes & FileAttributes.System) != FileAttributes.System)
        {
            try
            {
                if (System.IO.Directory.GetDirectories(Path + "\\").Length < 400)
                {
                    string[] SubfoldersArray = Directory.GetDirectories(Path + "\\");
                    Subfolders = new List<string>(SubfoldersArray);
                }
            }

            catch (System.Exception e)
            {
                //print("Can't access folder.");
            }

        }
        
        

        if (Path == "C:\\")
        {
            try
            {
                string[] SubfoldersArray = Directory.GetDirectories("C:\\");
                Subfolders = new List<string>(SubfoldersArray);
            }
            catch { }
        }
        else if (Path == "D:\\")
        {
            try
            {
                string[] SubfoldersArray = Directory.GetDirectories("D:\\");
                Subfolders = new List<string>(SubfoldersArray);
            }
            catch { }
        }
        else if (Path == "E:\\")
        {
            try
            {
                string[] SubfoldersArray = Directory.GetDirectories("E:\\");
                Subfolders = new List<string>(SubfoldersArray);
            }
            catch { }
        }
        else if (Path == "F:\\")
        {
            try
            {
                string[] SubfoldersArray = Directory.GetDirectories("F:\\");
                Subfolders = new List<string>(SubfoldersArray);
            }
            catch { }
        }
        else if (Path == "G:\\")
        {
            try
            {
                string[] SubfoldersArray = Directory.GetDirectories("G:\\");
                Subfolders = new List<string>(SubfoldersArray);
            }
            catch { }
        }
        
    }

    void SendChildrenList()
    {

        foreach (GameObject dad in Parents)
        {
            List<GameObject> MergedLists = Children.Union<GameObject>(dad.GetComponent<TreeBranch>().Children).ToList<GameObject>();
            dad.GetComponent<TreeBranch>().Children = MergedLists;
        }
    }

}
