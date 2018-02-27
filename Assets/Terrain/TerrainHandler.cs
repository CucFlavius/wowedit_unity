using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainHandler : MonoBehaviour
{
    public string MapName;
    public int BlockX;
    public int BlockY;
    public GameObject currentBlock;
    public static System.Threading.Thread ADTThread;
    public GameObject ChunkPrefab;

    public Queue<QueueItem> ADTThreadQueue = new Queue<QueueItem>();
    public Queue<QueueItem> currentLoadingBlocks; // = new Queue<QueueItem>();
    public static bool FinishedCreatingObject;

    public Dictionary<string, Texture2D> LoadedTerrainTextures = new Dictionary<string, Texture2D>();

    public class QueueItem
    {
        public string mapName;
        public int x;
        public int y;
        public GameObject Block;
    }

    // Use this for initialization
    void Start () {
        ADT.BlockDataReady = false;
        ADT.ThreadWorking = false;
        ADTThreadQueue = new Queue<QueueItem>();
        currentLoadingBlocks = new Queue<QueueItem>();
    }

    public void AddToQueue (string mapName, int x, int y, GameObject Block)
    {
        QueueItem item = new QueueItem();
        item.mapName = mapName;
        item.x = x;
        item.y = y;
        item.Block = Block;
        ADTThreadQueue.Enqueue(item);
        currentLoadingBlocks.Enqueue(item);
    }

    public void ADTThreadRun (string mapName, int x, int y, GameObject Block)
    {
        MapName = mapName;
        BlockX = x;
        BlockY = y;
        currentBlock = Block;
        //ParseADTBlock(); // nonthreaded - for testing purposes
        ADTThread = new System.Threading.Thread(ParseADTBlock);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        ADTThread.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (ADTThreadQueue.Count > 0 && !ADT.ThreadWorking)
        {
            ADT.ThreadWorking = true;
            QueueItem queueItem = ADTThreadQueue.Dequeue();
            ADTThreadRun(queueItem.mapName, queueItem.x, queueItem.y, queueItem.Block);
        }
        if (ADT.AllBlockData.Count > 0 && !ADTThread.IsAlive)
        {
            CreateADTObject();
        }
    }

    public void ParseADTBlock ()
    {
        string ADTpath = @"\world\maps\" + MapName + @"\";
        ADT.Load(ADTpath, MapName, new Vector2(BlockX, BlockY));
    }

    public void CreateADTObject()
    {
        QueueItem Gobject = currentLoadingBlocks.Dequeue();
        Gobject.Block.name = Gobject.mapName + "_" + Gobject.x + "_" + Gobject.y;
        // Create LoD0 Parent GameObject //
        GameObject LoD0 = new GameObject();
        LoD0.name = "LoD0";
        LoD0.transform.SetParent(Gobject.Block.transform);
        // Get ADT Block Data //
        ADT.BlockDataType data = ADT.AllBlockData.Dequeue();
        for (int i = 0; i < 256; i++)
        {
            // Create GameObject //
            GameObject ChunkObj = Instantiate(ChunkPrefab, Vector3.zero, Quaternion.identity);
            ChunkObj.isStatic = true;
            ChunkObj.name = "Chunk_" + i.ToString();
            ChunkObj.transform.position = data.ChunksData[i].MeshPosition;
            ChunkObj.transform.SetParent(LoD0.transform);
            // Create Mesh //
            Mesh mesh = new Mesh();
            mesh.vertices = data.ChunksData[i].VertexArray;
            mesh.triangles = data.ChunksData[i].TriangleArray;
            mesh.uv = data.ChunksData[i].UVArray;
            mesh.normals = data.ChunksData[i].VertexNormals;
            mesh.colors32 = data.ChunksData[i].VertexColors;
            ChunkObj.GetComponent<MeshFilter>().mesh = mesh;
            // Assign textures //

	        for (int tl = 0; tl < data.ChunksData[i].NumberOfTextureLayers; tl++) {
                if (tl == 0)
                {
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_MainTex", LoadedTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));
                    }
                    else
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        Debug.Log(textureName);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));
                    }
                }
                // 1 Alpha layer //
                else if (tl == 1)
                {
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex1", LoadedTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex1", new Vector2(4, 4));
                    }
                    else
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex1", tex);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex1", new Vector2(4, 4));
                    }
                    if (data.ChunksData[i].alphaLayers.Count > 0) {
                        if (data.ChunksData[i].alphaLayers[0] != null)
                        {
                            Texture2D textureAlpha = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                            textureAlpha.LoadRawTextureData(data.ChunksData[i].alphaLayers[0]);
                            Color32[] pixels = textureAlpha.GetPixels32();
                            pixels = ADT.RotateMatrix(pixels, 64);
                            textureAlpha.SetPixels32(pixels);
                            textureAlpha.Apply();
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTexAmount1", textureAlpha);
                            ChunkObj.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, -1);
                        }
                    }
                }
                // 2 Alpha layers //
                else if (tl == 2)
                {
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex2", LoadedTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex2", new Vector2(4, 4));
                    }
                    else
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex2", tex);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex2", new Vector2(4, 4));
                    }
                    if (data.ChunksData[i].alphaLayers.Count > 0)
                    {
                        if (data.ChunksData[i].alphaLayers[1] != null)
                        {
                            Texture2D textureAlpha = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                            textureAlpha.LoadRawTextureData(data.ChunksData[i].alphaLayers[1]);
                            Color32[] pixels = textureAlpha.GetPixels32();
                            pixels = ADT.RotateMatrix(pixels, 64);
                            textureAlpha.SetPixels32(pixels);
                            textureAlpha.Apply();
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTexAmount2", textureAlpha);
                            ChunkObj.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, -1);
                        }
                    }
                }
                // 3 Alpha layers //
                else if (tl == 3)
                {
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex3", LoadedTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex3", new Vector2(4, 4));
                    }
                    else
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTex3", tex);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_BlendTex3", new Vector2(4, 4));
                    }
                    if (data.ChunksData[i].alphaLayers.Count > 0)
                    {
                        if (data.ChunksData[i].alphaLayers[2] != null)
                        {
                            Texture2D textureAlpha = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                            textureAlpha.LoadRawTextureData(data.ChunksData[i].alphaLayers[2]);
                            Color32[] pixels = textureAlpha.GetPixels32();
                            pixels = ADT.RotateMatrix(pixels, 64);
                            textureAlpha.SetPixels32(pixels);
                            textureAlpha.Apply();
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_BlendTexAmount3", textureAlpha);
                            ChunkObj.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, -1);
                        }
                    }
                }
	        }
        }
    }
}
