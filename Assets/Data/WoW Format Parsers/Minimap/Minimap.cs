using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private GameObject ScrollParent;
    public GameObject MinimapPrefab;
    public GameObject LoadingPanel;
    public Image LoadingBar;
    public GameObject PanelErrorMessage;
    public Text ErrorMessageText;
    private int RemainingMinimaps = 0;
    public bool pause = false;
    
    void Update()
    {
        if (MinimapThread.ResetParentSize)
        {
            ScrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2((MinimapData.Max.x - MinimapData.Min.x + 1) * 100, (MinimapData.Max.y - MinimapData.Min.y + 1) * 100);
            RemainingMinimaps = MinimapData.Total;
            MinimapThread.ResetParentSize = false;
        }
        if (MinimapData.MinimapDataQueue.Count > 0)
        {
            AssembleMinimap();
            RemainingMinimaps--;
            LoadingBar.fillAmount = 1 - ((float)RemainingMinimaps / (float)MinimapData.Total);
        }
        if (LoadingBar.fillAmount >= 0.99f)
        {
            if (LoadingPanel.activeSelf)
                LoadingPanel.SetActive(false);
        }
        if (MinimapThread.checkWMOonly)
        {
            if (MinimapThread.WMOOnlyZone)
            {
                if (LoadingPanel.activeSelf)
                    LoadingPanel.SetActive(false);
                if (!PanelErrorMessage.activeSelf)
                {
                    PanelErrorMessage.SetActive(true);
                    ErrorMessageText.text = "WMO Only";
                }
            }
            else
            {
                if (!LoadingPanel.activeSelf)
                    LoadingPanel.SetActive(true);
                if (PanelErrorMessage.activeSelf)
                {
                    PanelErrorMessage.SetActive(false);
                }
            }
            MinimapThread.checkWMOonly = false;
        }
    }

    // Create Minimap Blocks //
    public void Load(string mapName, GameObject scrollParent)
    {
        ScrollParent = scrollParent;
        RemainingMinimaps = 1; // resetting above 0
        MinimapData.Total = 0;
        LoadingBar.fillAmount = 0;
        LoadingPanel.SetActive(true);
        MinimapThread.currentMapName = mapName;
        System.Threading.Thread minimapThread = new System.Threading.Thread(MinimapThread.LoadThread);
        minimapThread.IsBackground = true;
        minimapThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        minimapThread.Start();
        //MinimapThread.LoadThread(); // Nonthreaded, for debug
    }

    // Assemble the Minimap GameObjects //
    private void AssembleMinimap ()
    {
        MinimapData.MinimapBlockData blockData = MinimapData.MinimapDataQueue.Dequeue();
        GameObject instance = Instantiate(MinimapPrefab, Vector3.zero, Quaternion.identity);
        instance.transform.SetParent(ScrollParent.transform, false);
        instance.GetComponent<RectTransform>().anchoredPosition = new Vector2((blockData.coords.x - MinimapData.Min.x) * 100, -(blockData.coords.y - MinimapData.Min.y) * 100);
        instance.name = "map" + blockData.coords.x + "_" + blockData.coords.y + ".blp";
        instance.tag = "MinimapBlock";
        instance.GetComponent<MinimapBlock>().minimapCoords = blockData.coords;
        if (MinimapData.mapAvailability[(int)blockData.coords.x, (int)blockData.coords.y].Minimap)
        {
            Texture2D tex = new Texture2D(blockData.textureInfo.width, blockData.textureInfo.height, blockData.textureInfo.textureFormat, false);
            tex.LoadRawTextureData(blockData.minimapByteData);
            instance.GetComponent<RawImage>().texture = tex;
            instance.GetComponent<RawImage>().uvRect = new Rect(0, 0, 1, -1);
            tex.Apply();
        }
    }

    public void ClearMinimaps(GameObject minimapScrollPanel)
    {
        MinimapData.MinimapDataQueue.Clear();
        foreach (Transform child in minimapScrollPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public struct MinimapRequest
    {
        public string mapName;
        public Vector2 coords;
    }

}
