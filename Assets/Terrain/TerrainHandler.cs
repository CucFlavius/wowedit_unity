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
    public Dictionary<string, Texture2D> LoadedHTerrainTextures = new Dictionary<string, Texture2D>();

    public WMOhandler WMOHandler;
    public Dictionary<string, GameObject> LoadedWMOs = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> ADTBlockWMOParents = new Dictionary<int, GameObject>();
    public List<int> LoadedUniqueWMOs = new List<int>();

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

        GameObject WMO0 = new GameObject();
        WMO0.transform.parent = Gobject.Block.transform;

        // Create WMO Objects - Send work to the WMO thread //
        foreach (ADT.WMOPlacementInfo wmoInfo in data.WMOInfo)
        {
            if (!LoadedUniqueWMOs.Contains(wmoInfo.uniqueID))
            {
                LoadedUniqueWMOs.Add(wmoInfo.uniqueID);
                ADTBlockWMOParents.Add(wmoInfo.uniqueID, WMO0);
                string wmoPath = data.WMOPaths[data.WMOOffsets[wmoInfo.nameID]];
                Vector3 addPosition = new Vector3(wmoInfo.position.x + LoD0.transform.position.x, wmoInfo.position.y + LoD0.transform.position.y, wmoInfo.position.z + LoD0.transform.position.z);
                WMOHandler.AddToQueue(wmoPath, wmoInfo.uniqueID, addPosition, wmoInfo.rotation, Vector3.one);
            }
        }

        // Create Terrain Chunks //
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
            float heightScale0 = 0f;
            float heightScale1 = 0f;
            float heightScale2 = 0f;
            float heightScale3 = 0f;
            float heightOffset0 = 0f;
            float heightOffset1 = 0f;
            float heightOffset2 = 0f;
            float heightOffset3 = 0f;

            for (int tl = 0; tl < data.ChunksData[i].NumberOfTextureLayers; tl++)
            {
                if (tl == 0)
                {
                    // Layer 0 Texture //
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (!LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                    }
                    ChunkObj.GetComponent<Renderer>().material.SetTexture("_layer0", LoadedTerrainTextures[textureName]);
                    ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_layer0", new Vector2(-1, -1));
                    // Layer 0 Height Texture //
                    if (data.terrainHTextures.ContainsKey(textureName))
                    {
                        if (!LoadedHTerrainTextures.ContainsKey(textureName))
                        {
                            ADT.Texture2Ddata tdata = data.terrainHTextures[textureName];
                            Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                            tex.LoadRawTextureData(tdata.TextureData);
                            tex.Apply();
                            LoadedHTerrainTextures[textureName] = tex;
                        }
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_height0", LoadedHTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_height0", new Vector2(-1, -1));
                    }
                    data.heightScales.TryGetValue(textureName, out heightScale0);
                    data.heightOffsets.TryGetValue(textureName, out heightOffset0);
                }
                else if (tl == 1)
                {
                    // Layer 1 Texture //
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (!LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                    }
                    ChunkObj.GetComponent<Renderer>().material.SetTexture("_layer1", LoadedTerrainTextures[textureName]);
                    ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_layer1", new Vector2(4, 4));
                    // Layer 1 Height Texture //
                    if (data.terrainHTextures.ContainsKey(textureName))
                    {
                        if (!LoadedHTerrainTextures.ContainsKey(textureName))
                        {
                            ADT.Texture2Ddata tdata = data.terrainHTextures[textureName];
                            Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                            tex.LoadRawTextureData(tdata.TextureData);
                            tex.Apply();
                            LoadedHTerrainTextures[textureName] = tex;
                        }
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_height1", LoadedHTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_height1", new Vector2(-1, -1));
                    }
                    data.heightScales.TryGetValue(textureName, out heightScale1);
                    data.heightOffsets.TryGetValue(textureName, out heightOffset1);
                    // Layer 1 Alpha //
                    if (data.ChunksData[i].alphaLayers.Count > 0) {
                        if (data.ChunksData[i].alphaLayers[0] != null)
                        {
                            Texture2D textureAlpha = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                            textureAlpha.LoadRawTextureData(data.ChunksData[i].alphaLayers[0]);
                            textureAlpha.wrapMode = TextureWrapMode.Clamp;
                            Color32[] pixels = textureAlpha.GetPixels32();
                            pixels = ADT.RotateMatrix(pixels, 64);
                            textureAlpha.SetPixels32(pixels);
                            textureAlpha.Apply();
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_blend1", textureAlpha);
                        }
                    }
                }
                else if (tl == 2)
                {
                    // Layer 2 Texture //
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (!LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                    }
                    ChunkObj.GetComponent<Renderer>().material.SetTexture("_layer2", LoadedTerrainTextures[textureName]);
                    ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_layer2", new Vector2(4, 4));
                    // Layer 2 Height Texture //
                    if (data.terrainHTextures.ContainsKey(textureName))
                    {
                        if (!LoadedHTerrainTextures.ContainsKey(textureName))
                        {
                            ADT.Texture2Ddata tdata = data.terrainHTextures[textureName];
                            Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                            tex.LoadRawTextureData(tdata.TextureData);
                            tex.Apply();
                            LoadedHTerrainTextures[textureName] = tex;
                        }
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_height2", LoadedHTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_height2", new Vector2(-1, -1));
                    }
                    data.heightScales.TryGetValue(textureName, out heightScale2);
                    data.heightOffsets.TryGetValue(textureName, out heightOffset2);
                    // Layer 2 Alpha //
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
                            textureAlpha.wrapMode = TextureWrapMode.Clamp;
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_blend2", textureAlpha);
                        }
                    }
                }
                else if (tl == 3)
                {
                    // Layer 3 Texture //
                    string textureName = data.terrainTexturePaths[data.ChunksData[i].textureIds[tl]];
                    if (!LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                    }
                    ChunkObj.GetComponent<Renderer>().material.SetTexture("_layer3", LoadedTerrainTextures[textureName]);
                    ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_layer3", new Vector2(4, 4));
                    // Layer 3 Height Texture //
                    if (data.terrainHTextures.ContainsKey(textureName))
                    {
                        if (!LoadedHTerrainTextures.ContainsKey(textureName))
                        {
                            ADT.Texture2Ddata tdata = data.terrainHTextures[textureName];
                            Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                            tex.LoadRawTextureData(tdata.TextureData);
                            tex.Apply();
                            LoadedHTerrainTextures[textureName] = tex;
                        }
                        ChunkObj.GetComponent<Renderer>().material.SetTexture("_height3", LoadedHTerrainTextures[textureName]);
                        ChunkObj.GetComponent<Renderer>().material.SetTextureScale("_height3", new Vector2(-1, -1));
                    }
                    data.heightScales.TryGetValue(textureName, out heightScale3);
                    data.heightOffsets.TryGetValue(textureName, out heightOffset3);
                    // Layer 3 Alpha //
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
                            textureAlpha.wrapMode = TextureWrapMode.Clamp;
                            ChunkObj.GetComponent<Renderer>().material.SetTexture("_blend3", textureAlpha);
                        }
                    }
                }
	        }
            if (data.MTXP)
            {
                ChunkObj.GetComponent<Renderer>().material.SetVector("heightScale", new Vector4(heightScale0, heightScale1, heightScale2, heightScale3));
                ChunkObj.GetComponent<Renderer>().material.SetVector("heightOffset", new Vector4(heightOffset0, heightOffset1, heightOffset2, heightOffset3));
            }

            // Shadow Map //
            if (ADTSettings.LoadShadowMaps)
            {
                if (data.ChunksData[i].shadowMapTexture.Length > 0)
                {
                    Texture2D textureAlpha = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                    textureAlpha.LoadRawTextureData(data.ChunksData[i].shadowMapTexture);
                    Color32[] pixels = textureAlpha.GetPixels32();
                    pixels = ADT.RotateMatrix(pixels, 64);
                    textureAlpha.SetPixels32(pixels);
                    textureAlpha.Apply();
                    textureAlpha.wrapMode = TextureWrapMode.Clamp;
                    ChunkObj.GetComponent<Renderer>().material.SetTexture("_shadowMap", textureAlpha);

                    // need to enable in shader too //
                }
            }
        }
    }
}
