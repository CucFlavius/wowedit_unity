using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using Assets.World;

public class MinimapHandler : MonoBehaviour {

    public GameObject MinimapBlock;
    public GameObject ScrollPanel;
    public GameObject World;
    public GameObject SelectPlayerBlockIcon;
    public GameObject SelectPlayerBlockIcon_prefab;
    public GameObject LoadingText;
    private List<string> MinimapFileList = new List<string>();
    public string minimap_Path;
    public string map_name;
    private int firstxCoord;
    private int firstyCoord;
    private int lastyCoord;
    private int lastxCoord;
    public Vector2 currentSelectedPlayerSpawn;

    void Start()
    {
        currentSelectedPlayerSpawn = new Vector2(0, 0); // default
    }

    public void LoadMinimaps(string minimapPath, string mapName)
    {
        // reset global variables //
        ClearData();

        // update global list of minimap files : MinimapFileList //
        List<string> FileList = Casc.GetFileListFromFolder(minimapPath);
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
        map_name = mapName;
        GenerateMinimaps(mapName);
    }

    public void ClickedLoadFull ()
    {
        if (currentSelectedPlayerSpawn == new Vector2(0,0) || currentSelectedPlayerSpawn == null)
        {
            currentSelectedPlayerSpawn = new Vector2(firstyCoord + ((lastyCoord - firstyCoord) / 2), firstxCoord + ((lastxCoord - firstxCoord) / 2));
            //currentSelectedPlayerSpawn = new Vector2(firstyCoord , firstxCoord);
        }
        Debug.Log("Spawn : " + currentSelectedPlayerSpawn.x + " " + currentSelectedPlayerSpawn.y);
        World.GetComponent<WorldLoader>().LoadFullWorld(map_name, currentSelectedPlayerSpawn);
        LoadingText.SetActive(true);
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

    private void AdjustScrollableAreaFromWDT()
    {

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    firstxCoord = y;
                    firstyCoord = x;
                    break;
                }
            }
        }

        int previousxCoord = 0;
        int previousyCoord = 0;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    previousxCoord = y;
                    previousyCoord = x;
                    if (previousyCoord > lastyCoord) lastyCoord = previousyCoord;
                    if (previousxCoord > lastxCoord) lastxCoord = previousxCoord;
                }
            }
        }
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
            instance.tag = "MinimapBlock";
            instance.GetComponent<MinimapBlock>().minimapCoords = new Vector2(yCoord, xCoord);

            AssignMinimapTexture(instance.gameObject, minimapName);
        }
    }

    private void AssignMinimapTexture(GameObject MinimapObject, string minimapName)
    {
        string path = @"world\minimaps\" + minimapName + @"\" + MinimapObject.name + ".blp";
        string extractedPath = Casc.GetFile(path);
        Stream stream = File.Open(extractedPath, FileMode.Open);
        BLP blp = new BLP();
        byte[] data = blp.GetUncompressed(stream, false);
        BLPinfo info = blp.Info();
        Texture2D tex = new Texture2D(info.width, info.height, blp.TxFormat(), false);
        tex.LoadRawTextureData(data);
        MinimapObject.GetComponent<RawImage>().texture = tex;
        MinimapObject.GetComponent<RawImage>().uvRect = new Rect(0, 0, 1, -1);
        tex.Apply();
        stream.Close();
        blp.Close();
    }

    public void LoadBlankMinimaps(string mapPath, string mapName)
    {
        map_name = mapName;
        ClearData();

        AdjustScrollableAreaFromWDT();

        //instantiate empty minimap blocks
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[map_name].HasADT[x, y])
                {
                    GameObject instance = Instantiate(MinimapBlock, Vector3.zero, Quaternion.identity);
                    instance.transform.SetParent(ScrollPanel.transform, false);
                    instance.GetComponent<RectTransform>().anchoredPosition = new Vector2((x - firstxCoord) * 100, -(y - firstyCoord) * 100);
                    instance.name = mapName + "_" + x + "_" + y;
                }
            }
        }
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

    public void SelectPlayerSpawn(GameObject minimapBlock)
    {
        if (SelectPlayerBlockIcon == null)
            SelectPlayerBlockIcon = Instantiate(SelectPlayerBlockIcon_prefab);

        SelectPlayerBlockIcon.SetActive(true);
        SelectPlayerBlockIcon.transform.SetParent(minimapBlock.transform);
        SelectPlayerBlockIcon.GetComponent<RectTransform>().localPosition = Vector2.zero;
        SelectPlayerBlockIcon.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        currentSelectedPlayerSpawn = minimapBlock.GetComponent<MinimapBlock>().minimapCoords;
    }
}
