using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MinimapHandler : MonoBehaviour {

    public GameObject MinimapBlock;
    public GameObject ScrollPanel;
    public GameObject World;
    private List<string> MinimapFileList = new List<string>();
    public string minimap_Path;
    public string minimapName;
    private int firstxCoord;
    private int firstyCoord;
    private int lastyCoord;
    private int lastxCoord;

    public void LoadMinimaps(string minimapPath)
    {
        // reset global variables //
        ClearData();

        // update global list of minimap files : MinimapFileList //
        string[] FileList = Casc.GetFileListFromFolder(minimapPath);
        foreach (string file in FileList)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            if (fileName.StartsWith("m"))
            {
                MinimapFileList.Add(fileName);
            }
        }

        // resize scroll area to encapsulate all minimap blocks //
        AdjustScrollableArea();

        // Create minimap block instances //
        minimapName = Path.GetFileName(minimapPath.TrimEnd(@"\"[0]));
        GenerateMinimaps(minimapName);
    }

    public void ClickedLoadFull ()
    {
        World.GetComponent<WorldLoader>().LoadFullWorld(MinimapFileList, minimapName);
    }

    private void AdjustScrollableArea ()
    {
        //find minimum x,y
        string firstFile = MinimapFileList[0];
        string firstFile1 = firstFile.Split("map"[2])[1];
        firstxCoord = int.Parse(firstFile1.Split("_"[0])[0]);
        firstyCoord = int.Parse(firstFile1.Split("_"[0])[1]);
        foreach (string fileName4 in MinimapFileList)
        {
            string lastFile2 = fileName4.Split("map"[2])[1];
            int previousyCoord1 = int.Parse(lastFile2.Split("_"[0])[1]);
            if (previousyCoord1 < firstyCoord) firstyCoord = previousyCoord1;
        }
        //find maximum x,y
        lastxCoord = 0;
        lastyCoord = 0;
        foreach (string fileName3 in MinimapFileList)
        {
            string lastFile1 = fileName3.Split("map"[2])[1];
            int previousxCoord = int.Parse(lastFile1.Split("_"[0])[0]);
            int previousyCoord = int.Parse(lastFile1.Split("_"[0])[1]);
            if (previousyCoord > lastyCoord) lastyCoord = previousyCoord;
            if (previousxCoord > lastxCoord) lastxCoord = previousxCoord;
        }
        //// scale scroll pannel to minimaps size ////
        ScrollPanel.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((lastxCoord - firstxCoord + 1) * 100, (lastyCoord - firstyCoord + 1) * 100);
    }

    private void GenerateMinimaps (string minimapName)
    {
        //instantiate minimap blocks and assign textures
        foreach (string fileName1 in MinimapFileList)
        {
            string fileName2 = fileName1.Split("map"[2])[1];
            int xCoord = int.Parse(fileName2.Split("_"[0])[0]);
            int yCoord = int.Parse(fileName2.Split("_"[0])[1]);
            GameObject instance = Instantiate(MinimapBlock, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(ScrollPanel.transform, false);
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector2((xCoord - firstxCoord) * 100, -(yCoord - firstyCoord) * 100);
            instance.name = fileName1;

            AssignMinimapTexture(instance.gameObject, minimapName);
        }
    }

    private void AssignMinimapTexture(GameObject MinimapObject, string minimapName)
    {
        string path = @"world\minimaps\" + minimapName + @"\" + MinimapObject.name + ".blp";
        string extractedPath = Casc.GetFile(path);
        Stream stream = File.Open(extractedPath, FileMode.Open);

        byte[] data = BLP.GetUncompressed(stream, false);
        BLPinfo info = BLP.Info();
        Texture2D tex = new Texture2D(info.width, info.height, BLP.TxFormat(), false);
        tex.LoadRawTextureData(data);
        MinimapObject.GetComponent<RawImage>().texture = tex;
        MinimapObject.GetComponent<RawImage>().uvRect = new Rect(0, 0, 1, -1);
        tex.Apply();
        stream.Close();
        BLP.Close();
    }

    public void LoadBlankMinimaps(string mapPath)
    {
        ClearData();
    }

    private void ClearData()
    {
        MinimapFileList.Clear();
    }

    public void ClearMinimaps()
    {
        foreach (Transform child  in ScrollPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
