using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using Assets.World;
using CASCLib;
using Assets.UI.CASC;

public class MinimapHandler : MonoBehaviour {
    public Jenkins96 Hasher = new Jenkins96();
    public GameObject MinimapBlock;
    public GameObject ScrollPanel;
    public GameObject World;
    public GameObject CASC;
    public GameObject SelectPlayerBlockIcon;
    public GameObject SelectPlayerBlockIcon_prefab;
    public GameObject LoadingText;
    public Vector2 currentSelectedPlayerSpawn;
    public uint FileDataId;

    private List<uint> MinimapFileList = new List<uint>();
    private int firstxCoord;
    private int firstyCoord;
    private int lastyCoord;
    private int lastxCoord;

    void Start()
    {
        CASC = GameObject.Find("[CASC]");
        currentSelectedPlayerSpawn = new Vector2(0, 0); // default
    }

    public void LoadMinimaps(uint fileDataId)
    {
        // // reset global variables //
        // ClearData();
        // 
        // // resize scroll area to encapsulate all minimap blocks //
        // AdjustScrollableArea();
        // 
        // // Create minimap block instances //
        // map_name = mapName;
        // GenerateMinimaps(mapName);
    }

    public void ClickedLoadFull()
    {
        if (currentSelectedPlayerSpawn == new Vector2(0,0) || currentSelectedPlayerSpawn == null)
            currentSelectedPlayerSpawn = new Vector2(firstyCoord + ((lastyCoord - firstyCoord) / 2), firstxCoord + ((lastxCoord - firstxCoord) / 2));

        Debug.Log("Spawn : " + currentSelectedPlayerSpawn.x + " " + currentSelectedPlayerSpawn.y);
        // World.GetComponent<WorldLoader>().LoadFullWorld(map_name, currentSelectedPlayerSpawn);
        LoadingText.SetActive(true);
    }

    private void AdjustScrollableArea ()
    {
        //find minimum x,y
        // uint firstFile = MinimapFileList[0];
        // uint firstFile1 = firstFile.Split("map"[2])[1];
        // firstxCoord = int.Parse(firstFile1.Split("_"[0])[0]);
        // firstyCoord = int.Parse(firstFile1.Split("_"[0])[1]);
        // foreach (string fileName4 in MinimapFileList)
        // {
        //     string lastFile2 = fileName4.Split("map"[2])[1];
        //     int previousyCoord1 = int.Parse(lastFile2.Split("_"[0])[1]);
        //     if (previousyCoord1 < firstyCoord) firstyCoord = previousyCoord1;
        // }
        // //find maximum x,y
        // lastxCoord = 0;
        // lastyCoord = 0;
        // foreach (string fileName3 in MinimapFileList)
        // {
        //     string lastFile1 = fileName3.Split("map"[2])[1];
        //     int previousxCoord = int.Parse(lastFile1.Split("_"[0])[0]);
        //     int previousyCoord = int.Parse(lastFile1.Split("_"[0])[1]);
        //     if (previousyCoord > lastyCoord) lastyCoord = previousyCoord;
        //     if (previousxCoord > lastxCoord) lastxCoord = previousxCoord;
        // }
        // //// scale scroll pannel to minimaps size ////
        // ScrollPanel.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((lastxCoord - firstxCoord + 1) * 100, (lastyCoord - firstyCoord + 1) * 100);
    }

    private void AdjustScrollableAreaFromWDT()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[FileDataId].HasADT[x, y])
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
                if (WDT.Flags[FileDataId].HasADT[x, y])
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

    private void GenerateMinimaps(uint FileDataId)
    {
        //instantiate minimap blocks and assign textures
        // foreach (string fileName1 in MinimapFileList)
        // {
        //     string fileName2 = fileName1.Split("map"[2])[1];
        //     int xCoord = int.Parse(fileName2.Split("_"[0])[0]);
        //     int yCoord = int.Parse(fileName2.Split("_"[0])[1]);
        //     GameObject instance = Instantiate(MinimapBlock, Vector3.zero, Quaternion.identity);
        //     instance.transform.SetParent(ScrollPanel.transform, false);
        //     instance.GetComponent<RectTransform>().anchoredPosition = new Vector2((xCoord - firstxCoord) * 100, -(yCoord - firstyCoord) * 100);
        //     instance.name = fileName1;
        //     instance.tag = "MinimapBlock";
        //     instance.GetComponent<MinimapBlock>().minimapCoords = new Vector2(yCoord, xCoord);
        // 
        //     AssignMinimapTexture(instance.gameObject, FileDataId);
        // }
    }

    private void AssignMinimapTexture(GameObject MinimapObject, uint FileDataId)
    {
        // BLP blp         = new BLP();
        // if (CASC.GetComponent<CascHandler>().cascHandler.FileExists(hash))
        // {
        //     using (var stream = CASC.GetComponent<CascHandler>().cascHandler.OpenFile(hash))
        //     {
        //         byte[] data     = blp.GetUncompressed(stream, false);
        //         BLPinfo info    = blp.Info();
        //         Texture2D tex   = new Texture2D(info.width, info.height, blp.TxFormat(), false);
        //         tex.LoadRawTextureData(data);
        //         MinimapObject.GetComponent<RawImage>().texture = tex;
        //         MinimapObject.GetComponent<RawImage>().uvRect = new Rect(0, 0, 1, -1);
        //         tex.Apply();
        //         stream.Close();
        //         blp.Close();
        //     }
        // }
    }

    public void LoadBlankMinimaps(uint fileDataId)
    {
        FileDataId = fileDataId;
        ClearData();

        AdjustScrollableAreaFromWDT();

        //instantiate empty minimap blocks
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (WDT.Flags[FileDataId].HasADT[x, y])
                {
                    GameObject instance = Instantiate(MinimapBlock, Vector3.zero, Quaternion.identity);
                    instance.transform.SetParent(ScrollPanel.transform, false);
                    instance.GetComponent<RectTransform>().anchoredPosition = new Vector2((x - firstxCoord) * 100, -(y - firstyCoord) * 100);
                    instance.name = fileDataId + "_" + x + "_" + y;
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
            Destroy(child.gameObject);
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
