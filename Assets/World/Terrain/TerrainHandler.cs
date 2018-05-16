
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TerrainHandler : MonoBehaviour
{
    public string MapName;
    public int BlockX;
    public int BlockY;
    public GameObject currentBlock;
    public static System.Threading.Thread ADTThread;
    public GameObject ChunkPrefab;
    public GameObject BlockChunksPrefab;
    public GameObject WMOParent;
    public Material HChunkMaterial;
    public Material LChunkMaterial;
    public List<Material> LChunkMaterials;
    public float finishedTimeAssembleHT;
    public float finishedTimeAssembleHTextures;
    public Shader shaderWoWTerrainLow;
    public Shader shaderWoWTerrainHigh;
    public bool frameBusy;

    public List<QueueItem> LoadedQueueItems = new List<QueueItem>();
    //public Queue<QueueItem> ADTRootQueue = new Queue<QueueItem>();
    public List<QueueItem> ADTRootQueue = new List<QueueItem>();
    //public Queue<QueueItem> ADTTexQueue = new Queue<QueueItem>();
    public List<QueueItem> ADTTexQueue = new List<QueueItem>();
    //public Queue<QueueItem> ADTObjQueue = new Queue<QueueItem>();
    public List<QueueItem> ADTObjQueue = new List<QueueItem>();

    public Queue<QueueItem> currentLoadingHTerrainBlock;
    public Queue<QueueItem> currentLoadingHTextureBlock;
    public Queue<QueueItem> currentLoadingObjBlock;
    public static Queue<QueueItem> mapTextureQueue = new Queue<QueueItem>();
    public static bool FinishedCreatingObject;

    public Dictionary<string, Texture2D> LoadedTerrainTextures = new Dictionary<string, Texture2D>();
    public Dictionary<string, Texture2D> LoadedHTerrainTextures = new Dictionary<string, Texture2D>();

    public WMOhandler WMOHandler;
    public Dictionary<string, GameObject> LoadedWMOs = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> ADTBlockWMOParents = new Dictionary<int, GameObject>();
    public List<int> LoadedUniqueWMOs = new List<int>();

    private QueueItem currentMapTexture;
    private QueueItem currentHTexture;
    private QueueItem currentHTerrain;
    private QueueItem currentObj;

    public class ADTBlockInfo
    {
        public string mapName;
        public int x;
        public int y;
    }

    public class QueueItem
    {
        public string mapName;
        public int x;
        public int y;
        public GameObject Block;
    }

    // Use this for initialization
    void Start () {
        frameBusy = false;
        ADT.ThreadWorkingMesh = false;
        ADT.ThreadWorkingTextures = false;
        ADT.ThreadWorkingModels = false;
        MapTexture.MapTextureThreadRunning = false;
        //ADTRootQueue = new Queue<QueueItem>();
        ADTRootQueue = new List<QueueItem>();
        //ADTTexQueue = new Queue<QueueItem>();
        ADTTexQueue = new List<QueueItem>();
        //ADTObjQueue = new Queue<QueueItem>();
        ADTObjQueue = new List<QueueItem>();
        currentLoadingHTerrainBlock = new Queue<QueueItem>();
        currentLoadingHTextureBlock = new Queue<QueueItem>();
        currentLoadingObjBlock = new Queue<QueueItem>();
        //currentLoadingObj = new Queue<QueueItem>();
        mapTextureQueue = new Queue<QueueItem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Parsers //
        if (ADTRootQueue.Count > 0 && !ADT.ThreadWorkingMesh)
        {
            ADT.ThreadWorkingMesh = true;
            //QueueItem q = ADTRootQueue.Dequeue();
            QueueItem q = ADTRootQueue[0];
            ADTRootQueue.RemoveAt(0);
            ADTRootThreadRun(q);
        }
        if (ADTObjQueue.Count > 0 && !ADT.ThreadWorkingModels)
        {
            ADT.ThreadWorkingModels = true;
            //QueueItem q = ADTObjQueue.Dequeue();
            QueueItem q = ADTObjQueue[0];
            ADTObjQueue.RemoveAt(0);
            ADTObjThreadRun(q);
        }
        if (ADTTexQueue.Count > 0 && !ADT.ThreadWorkingTextures)
        {
            ADT.ThreadWorkingTextures = true;
            //QueueItem q = ADTTexQueue.Dequeue();
            QueueItem q = ADTTexQueue[0];
            ADTTexQueue.RemoveAt(0);
            ADTTexThreadRun(q);
        }

        if (mapTextureQueue.Count > 0 && !MapTexture.MapTextureThreadRunning)
        {
            MapTexture.MapTextureThreadRunning = true;
            QueueItem q = mapTextureQueue.Dequeue();
            MapTextureThreadRun(q);
        }
        // Assemblers //
        if (frameBusy == false)
        {
            // if there's Hterrain data ready
            if (ADTRootData.MeshBlockDataQueue.Count > 0)
            {
                frameBusy = true;
                StartCoroutine(AssembleHTBlock());
            }
            // if there's LTexture data ready
            if (MapTexture.MapTextureDataQueue.count > 0)
            {
                frameBusy = true;
                LoadLTexture();
            }
            if (ADTTexData.TextureBlockDataQueue.Count > 0)
            {
                frameBusy = true;
                StartCoroutine(AssembleHTextures());
            }
            if (ADTObjData.ModelBlockDataQueue.Count > 0 && !WMOHandler.busy)
            {
                LoadWMOs();
            }
        }
    }

    public void AddToQueue(string mapName, int x, int y, GameObject Block)
    {
        QueueItem item = new QueueItem();
        item.mapName = mapName;
        item.x = x;
        item.y = y;
        item.Block = Block;
        //ADTRootQueue.Enqueue(item);
        ADTRootQueue.Add(item);
        currentLoadingHTerrainBlock.Enqueue(item);
    }

    ///////////////////////////////////
    #region Parsing/Processing Threads

    public void ADTRootThreadRun(QueueItem queueItem)
    {
        currentHTerrain = queueItem;
        //ParseHTerrainMesh(); // nonthreaded - for testing purposes
        ADTThread = new System.Threading.Thread(ADTRootThread);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        ADTThread.Start();
    }

    public void ADTTexThreadRun(QueueItem queueItem)
    {
        currentHTexture = queueItem;
        //ADTTexThread(); // nonthreaded
        ADTThread = new System.Threading.Thread(ADTTexThread);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        ADTThread.Start();
    }

    public void ADTObjThreadRun(QueueItem queueItem)
    {
        currentObj = queueItem;
        //ADTObjThread();
        ADTThread = new System.Threading.Thread(ADTObjThread);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        ADTThread.Start();
    }

    public void MapTextureThreadRun(QueueItem queueItem)
    {
        currentMapTexture = queueItem;
        //MapTextureThread(); // nonthreaded
        ADTThread = new System.Threading.Thread(MapTextureThread);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        ADTThread.Start();
    }

    public void ADTRootThread()
    {
        string ADTpath = @"\world\maps\" + currentHTerrain.mapName + @"\";
        ADT.LoadTerrainMesh(ADTpath, currentHTerrain.mapName, new Vector2(currentHTerrain.x, currentHTerrain.y));
    }

    public void ADTTexThread()
    {
        string ADTpath = @"\world\maps\" + currentHTexture.mapName + @"\";
        ADT.LoadTerrainTextures(ADTpath, currentHTexture.mapName, new Vector2(currentHTexture.x, currentHTexture.y));
    }

    public void ADTObjThread()
    {
        string ADTpath = @"\world\maps\" + currentObj.mapName + @"\";
        ADT.LoadTerrainModels(ADTpath, currentObj.mapName, new Vector2(currentObj.x, currentObj.y));
    }

    public void MapTextureThread ()
    {
        MapTexture.Load(currentMapTexture.mapName, new Vector2(currentMapTexture.x, currentMapTexture.y));
    }

    #endregion
    ///////////////////////////////////

    // Assemble high resolution terrain and block //
    IEnumerator AssembleHTBlock()
    {
        float startTimeAssembleHT = Time.time;

        ADTRootData.MeshBlockData HTData = ADTRootData.MeshBlockDataQueue.Dequeue();
        QueueItem HTGroupItem = currentLoadingHTerrainBlock.Dequeue();

        if (HTGroupItem.Block != null)
        {

            HTGroupItem.Block.name = HTGroupItem.mapName + "_" + HTGroupItem.x + "_" + HTGroupItem.y;

            // generate mesh objects //
            int frameSpread = 8; // spreading terrain chunks creation over multiple frames
            for (int i = 1; i <= frameSpread; i++)
            {
                CreateHTBlockQuarter(frameSpread, i, HTData, HTGroupItem.Block);
                yield return null;
            }

            // Batch terrain meshes //
            StaticBatchingUtility.Combine(HTGroupItem.Block);

            HTGroupItem.Block.GetComponent<ADTBlock>().enabled = true;
            LoadedQueueItems.Add(HTGroupItem);

            // request Objs //
            ADTObjQueue.Add(HTGroupItem);
            currentLoadingObjBlock.Enqueue(HTGroupItem);

            // request the LTexture //
            mapTextureQueue.Enqueue(HTGroupItem);

            // request the HTexture //
            ADTTexQueue.Add(HTGroupItem);
            currentLoadingHTextureBlock.Enqueue(HTGroupItem);

        }

        frameBusy = false;
        finishedTimeAssembleHT = Time.time - startTimeAssembleHT;
    }

    // Assemble high resolution textures //
    IEnumerator AssembleHTextures()
    {
        float startTimeAssembleHTextures = Time.time;

        ADTTexData.TextureBlockData HTexData = ADTTexData.TextureBlockDataQueue.Dequeue();
        QueueItem HTextureItem = currentLoadingHTextureBlock.Dequeue();
        if (HTextureItem.Block != null)
        {
            HTextureItem.Block.name = HTextureItem.mapName + "_" + HTextureItem.x + "_" + HTextureItem.y;

            // generate mesh objects //
            int frameSpread = 8; // spreading terrain chunks creation over multiple frames
            for (int i = 1; i <= frameSpread; i++)
            {
                CreateHTextureQuarter(frameSpread, i, HTexData, HTextureItem.Block);
                yield return null;
            }
        }
        frameBusy = false;
        finishedTimeAssembleHTextures = Time.time - startTimeAssembleHTextures;
    }

    // Create a part of the ADT block meshes //
    private void CreateHTBlockQuarter (int fS , int Q, ADTRootData.MeshBlockData data, GameObject Block)
    {
        for (int i = (256 / fS) * (Q - 1); i < (256 / fS) * Q; i++)
        {
            //////////////////////////////
            #region Create GameObject 

            GameObject Chunk = Instantiate(ChunkPrefab);
            Chunk.isStatic = true;
            Chunk.name = "chunk_" + i.ToString();
            Chunk.transform.position = data.meshChunksData[i].MeshPosition;
            Chunk.transform.SetParent(Block.transform);
            #endregion
            //////////////////////////////

            //////////////////////////////
            #region Create Mesh

            Mesh mesh0 = new Mesh();
            mesh0.vertices = data.meshChunksData[i].VertexArray;
            mesh0.triangles = data.meshChunksData[i].TriangleArray;
            mesh0.uv = ADT.Chunk_UVs;
            mesh0.uv2 = ADT.Chunk_UVs2[i];
            mesh0.normals = data.meshChunksData[i].VertexNormals;
            mesh0.colors32 = data.meshChunksData[i].VertexColors;
            //Chunk.GetComponent<MeshFilter>().mesh = mesh0;
            Chunk.GetComponent<ADTChunk>().mesh = mesh0;
            Chunk.GetComponent<MeshFilter>().sharedMesh = Chunk.GetComponent<ADTChunk>().mesh;
            //mesh0.Clear();
            //mesh0 = null;
            #endregion
            //////////////////////////////
        }
    }

    // Create a part of the ADT block textures //
    private void CreateHTextureQuarter(int fS, int Q, ADTTexData.TextureBlockData data, GameObject Block)
    {
        if (Block != null)
        {
            StreamTools s = new StreamTools();
            for (int i = (256 / fS) * (Q - 1); i < (256 / fS) * Q; i++)
            {
                //////////////////////////////
                #region Textures
                //////////////////////////////

                float[] HeightScales = new float[4];
                float[] heightOffsets = new float[4];
                string[] DiffuseLayers = new string[4];
                string[] HeightLayers = new string[4];
                Flags.TerrainTextureFlag[] TextureFlags = new Flags.TerrainTextureFlag[4];
                Texture2D[] AlphaLayers = new Texture2D[4];
                Texture2D ShadowMap = null;

                for (int layer = 0; layer < data.textureChunksData[i].NumberOfTextureLayers; layer++)
                {
                    // Diffuse Texture //
                    string textureName = data.terrainTexturePaths[data.textureChunksData[i].textureIds[layer]];
                    if (!LoadedTerrainTextures.ContainsKey(textureName))
                    {
                        ADTTexData.Texture2Ddata tdata = data.terrainTextures[textureName];
                        Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                        tex.LoadRawTextureData(tdata.TextureData);
                        tex.mipMapBias = Settings.highMipMapBias;
                        tex.Apply();
                        LoadedTerrainTextures[textureName] = tex;
                    }
                    DiffuseLayers[layer] = textureName;

                    // Height Texture //
                    if (data.terrainHTextures.ContainsKey(textureName))
                    {
                        if (!LoadedHTerrainTextures.ContainsKey(textureName))
                        {
                            ADTTexData.Texture2Ddata tdata = data.terrainHTextures[textureName];
                            Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                            tex.LoadRawTextureData(tdata.TextureData);
                            tex.Apply();
                            LoadedHTerrainTextures[textureName] = tex;
                        }
                        HeightLayers[layer] = textureName;
                    }
                    // Height Values //
                    data.heightScales.TryGetValue(textureName, out HeightScales[layer]);
                    data.heightOffsets.TryGetValue(textureName, out heightOffsets[layer]);
                    data.textureFlags.TryGetValue(textureName, out TextureFlags[layer]);

                    // Alpha Texture //
                    if (data.textureChunksData[i].alphaLayers.Count > 0 && layer > 0)
                    {
                        if (data.textureChunksData[i].alphaLayers[0] != null)
                        {
                            AlphaLayers[layer] = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                            AlphaLayers[layer].LoadRawTextureData(data.textureChunksData[i].alphaLayers[layer - 1]);
                            AlphaLayers[layer].wrapMode = TextureWrapMode.Clamp;
                            AlphaLayers[layer].Apply();
                        }
                    }
                }
                #endregion

                //////////////////////////////
                #region Shadow Maps
                //////////////////////////////

                if (SettingsTerrainImport.LoadShadowMaps)
                {
                    if (data.textureChunksData[i].shadowMapTexture.Length > 0)
                    {
                        ShadowMap = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                        ShadowMap.LoadRawTextureData(data.textureChunksData[i].shadowMapTexture);
                        //Color32[] pixels = ShadowMap.GetPixels32();
                        //pixels = s.RotateMatrix(pixels, 64);
                        //ShadowMap.SetPixels32(pixels);
                        ShadowMap.Apply();
                        ShadowMap.wrapMode = TextureWrapMode.Clamp;
                        // need to enable in shader too //
                    }
                }
                #endregion

                //////////////////////////////
                #region Material
                //////////////////////////////

                Material mat = new Material(shaderWoWTerrainHigh);

                for (int ln = 0; ln < 4; ln++)
                {
                    if (DiffuseLayers[ln] != null)
                        mat.SetTexture("_layer" + ln, LoadedTerrainTextures[DiffuseLayers[ln]]);
                    mat.SetTextureScale("_layer" + ln, new Vector2(1, 1));
                    if (HeightLayers[ln] != null)
                        mat.SetTexture("_height" + ln, LoadedHTerrainTextures[HeightLayers[ln]]);
                    mat.SetTextureScale("_height" + ln, new Vector2(1, 1));
                    if (ln > 0 && AlphaLayers[ln] != null)
                        mat.SetTexture("_blend" + ln, AlphaLayers[ln]);
                    if (data.MTXP)
                        mat.SetFloat("layer" + ln + "scale", TextureFlags[ln].texture_scale);
                }
                if (data.MTXP)
                {
                    mat.SetVector("heightScale", new Vector4(HeightScales[0], HeightScales[1], HeightScales[2], HeightScales[3]));
                    mat.SetVector("heightOffset", new Vector4(heightOffsets[0], heightOffsets[1], heightOffsets[2], heightOffsets[3]));
                }
                if (SettingsTerrainImport.LoadShadowMaps)
                {
                    mat.SetTexture("_shadowMap", ShadowMap);
                }
                Block.transform.GetChild(i).GetComponent<ADTChunk>().MaterialReady(0, mat);

                #endregion
            }
        }
    }

    // Assemble low resolution map textures //
    private void LoadLTexture ()
    {
        MapTexture.MapTextureBlock mapTextureBlock;
        MapTexture.MapTextureDataQueue.Dequeue(out mapTextureBlock);
        Texture2D Ltexture = new Texture2D(mapTextureBlock.data.width, mapTextureBlock.data.height, mapTextureBlock.data.textureFormat,false);
        Ltexture.wrapMode = TextureWrapMode.Clamp;
        try
        {
            Ltexture.LoadRawTextureData(mapTextureBlock.data.TextureData);
            Ltexture.mipMapBias = Settings.lowMipMapBias;
            Ltexture.Apply();
        }
        catch
        {
            Debug.Log("Not enough texture data: " + mapTextureBlock.data.TextureData.Length);
        }
        Material mat = new Material(shaderWoWTerrainLow);
        mat.SetTexture("_MainTex2", Ltexture);
        mat.SetTextureScale("_MainTex2", new Vector2(1, 1));
        mat.enableInstancing = true;
        for (int i = 0; i < LoadedQueueItems.Count; i++)
        {
            if (LoadedQueueItems[i].mapName == mapTextureBlock.mapName && LoadedQueueItems[i].x == mapTextureBlock.coords.x && LoadedQueueItems[i].y == mapTextureBlock.coords.y)
            {
                for (int j = 0; j < 256; j++)
                {
                    if (LoadedQueueItems[i].Block != null)
                    LoadedQueueItems[i].Block.transform.GetChild(j).GetComponent<ADTChunk>().MaterialReady(1, mat);
                }
            }
        }
        frameBusy = false;
    }

    private void LoadWMOs ()
    {
        // Get ADT Block Data //
        ADTObjData.ModelBlockData data = ADTObjData.ModelBlockDataQueue.Dequeue();
        QueueItem Gobject = currentLoadingObjBlock.Dequeue();

        if (Gobject.Block != null)
        {

            float blockSize = 533.33333f / Settings.worldScale;

            if (SettingsTerrainImport.LoadWMOs)
            {
                GameObject WMO0 = new GameObject();
                Vector2 terrainPos = new Vector2(data.terrainPos.x, data.terrainPos.y);
                WMO0.name = "WMO_" + terrainPos.x + "_" + terrainPos.y;
                WMO0.transform.position = new Vector3(((32 - terrainPos.y) * blockSize), 0, ((32 - terrainPos.x) * blockSize));
                WMO0.transform.parent = WMOParent.transform; // Gobject.Block.transform;

                // Create WMO Objects - Send work to the WMO thread //
                foreach (ADTObjData.WMOPlacementInfo wmoInfo in data.WMOInfo)
                {
                    if (!LoadedUniqueWMOs.Contains(wmoInfo.uniqueID))
                    {
                        LoadedUniqueWMOs.Add(wmoInfo.uniqueID);
                        ADTBlockWMOParents.Add(wmoInfo.uniqueID, WMO0);
                        string wmoPath = data.WMOPaths[data.WMOOffsets[wmoInfo.nameID]];
                        //Vector3 addPosition = new Vector3(wmoInfo.position.x + Gobject.Block.transform.position.x,
                        //                                  wmoInfo.position.y + Gobject.Block.transform.position.y,
                        //                                  wmoInfo.position.z + Gobject.Block.transform.position.z);
                        Vector3 addPosition = new Vector3(wmoInfo.position.x,// + WMO0.transform.position.x,
                                                          wmoInfo.position.y,
                                                          wmoInfo.position.z);// + WMO0.transform.position.z);
                        WMOHandler.AddToQueue(wmoPath, wmoInfo.uniqueID, addPosition, wmoInfo.rotation, Vector3.one);
                    }
                }
            }
            WMOHandler.busy = true;
        }
    }


    // Rotate a Color32 array of square size n by 90 degrees //
    static Color32[] RotateMatrix(Color32[] matrix, int n)
    {
        Color32[] ret = new Color32[n * n];

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                ret[i * n + j] = matrix[(n - j - 1) * n + i];
            }
        }
        return ret;
    }
}
