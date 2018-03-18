
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
    public Material HChunkMaterial;
    public Material LChunkMaterial;
    public List<Material> LChunkMaterials;
    public float finishedTimeAssembleHT;
    public float finishedTimeAssembleHTextures;
    public Shader shaderWoWTerrainLow;
    public Shader shaderWoWTerrainHigh;
    public bool frameBusy;

    public List<QueueItem> LoadedQueueItems = new List<QueueItem>();
    public Queue<QueueItem> ADTRootQueue = new Queue<QueueItem>();
    public Queue<QueueItem> ADTTexQueue = new Queue<QueueItem>();
    public Queue<QueueItem> currentLoadingHTerrainBlock;
    public Queue<QueueItem> currentLoadingHTextureBlock;
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
        ADTRootQueue = new Queue<QueueItem>();
        ADTTexQueue = new Queue<QueueItem>();
        currentLoadingHTerrainBlock = new Queue<QueueItem>();
        currentLoadingHTextureBlock = new Queue<QueueItem>();
        mapTextureQueue = new Queue<QueueItem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Parsers //
        if (ADTRootQueue.Count > 0 && !ADT.ThreadWorkingMesh)
        {
            ADT.ThreadWorkingMesh = true;
            ADTRootThreadRun(ADTRootQueue.Dequeue());
        }
        if (ADTTexQueue.Count > 0 && !ADT.ThreadWorkingTextures && !ADT.ThreadWorkingMesh)
        {
            //Debug.Log("Yep");
            ADT.ThreadWorkingTextures = true;
            ADTTexThreadRun(ADTTexQueue.Dequeue());
        }
        if (mapTextureQueue.Count > 0 && !MapTexture.MapTextureThreadRunning)
        {
            MapTexture.MapTextureThreadRunning = true;
            MapTextureThreadRun(mapTextureQueue.Dequeue());
        }
        // Assemblers //
        if (frameBusy == false)
        {
            // if there's Hterrain data ready
            if (ADT.MeshBlockDataQueue.Count > 0)
            {
                //StartCoroutine(CreateADTObject());
                frameBusy = true;
                StartCoroutine(AssembleHTBlock());
            }
            // if there's LTexture data ready
            if (MapTexture.MapTextureDataQueue.Count > 0)
            {
                frameBusy = true;
                LoadLTexture();
            }
            if (ADT.TextureBlockDataQueue.Count > 0)
            {
                frameBusy = true;
                StartCoroutine(AssembleHTextures());
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
        ADTRootQueue.Enqueue(item);
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
        ADTTexThread(); // nonthreaded
        ADTThread = new System.Threading.Thread(ADTTexThread);
        ADTThread.IsBackground = true;
        ADTThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        //ADTThread.Start();
    }

    public void MapTextureThreadRun(QueueItem queueItem)
    {
        currentMapTexture = queueItem;
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

        ADT.MeshBlockData HTData = ADT.MeshBlockDataQueue.Dequeue();
        QueueItem HTGroupItem = currentLoadingHTerrainBlock.Dequeue();
        HTGroupItem.Block.name = HTGroupItem.mapName + "_" + HTGroupItem.x + "_" + HTGroupItem.y;

        // generate mesh objects //
        int frameSpread = 8; // spreading terrain chunks creation over multiple frames
        for (int i = 1; i <= frameSpread; i++)
        {
            CreateHTBlockQuarter(frameSpread ,i, HTData, HTGroupItem.Block);
            yield return null;
        }
        StaticBatchingUtility.Combine(HTGroupItem.Block);

        LoadedQueueItems.Add(HTGroupItem);
        // request the LTexture //
        mapTextureQueue.Enqueue(HTGroupItem);
        // request the HTexture //
        ADTTexQueue.Enqueue(HTGroupItem);
        //ADT.LoadTerrainTextures(HTGroupItem.Block.name, HTGroupItem.mapName, new Vector2(HTGroupItem.x, HTGroupItem.y));
        currentLoadingHTextureBlock.Enqueue(HTGroupItem);
        frameBusy = false;
        finishedTimeAssembleHT = Time.time - startTimeAssembleHT;
    }

    // Assemble high resolution textures //
    IEnumerator AssembleHTextures()
    {
        float startTimeAssembleHTextures = Time.time;

        ADT.TextureBlockData HTexData = ADT.TextureBlockDataQueue.Dequeue();
        QueueItem HTextureItem = currentLoadingHTextureBlock.Dequeue();
        HTextureItem.Block.name = HTextureItem.mapName + "_" + HTextureItem.x + "_" + HTextureItem.y;

        // generate mesh objects //
        int frameSpread = 8; // spreading terrain chunks creation over multiple frames
        for (int i = 1; i <= frameSpread; i++)
        {
            CreateHTextureQuarter(frameSpread, i, HTexData, HTextureItem.Block);
            yield return null;
        }

        frameBusy = false;
        finishedTimeAssembleHTextures = Time.time - startTimeAssembleHTextures;
    }

    // Create a part of the ADT block meshes //
    private void CreateHTBlockQuarter (int fS , int Q, ADT.MeshBlockData data, GameObject Block)
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
            mesh0.triangles = ADT.Chunk_Triangles;
            mesh0.uv = ADT.Chunk_UVs;
            mesh0.uv2 = ADT.Chunk_UVs2[i];
            mesh0.normals = data.meshChunksData[i].VertexNormals;
            mesh0.colors32 = data.meshChunksData[i].VertexColors;
            Chunk.GetComponent<MeshFilter>().mesh = mesh0;
            #endregion
            //////////////////////////////
        }
    }

    // Create a part of the ADT block textures //
    private void CreateHTextureQuarter(int fS, int Q, ADT.TextureBlockData data, GameObject Block)
    {
        for (int i = (256 / fS) * (Q - 1); i < (256 / fS) * Q; i++)
        {
            //////////////////////////////
            #region Textures
            //////////////////////////////

            float[] HeightScales = new float[4];
            float[] heightOffsets = new float[4];
            string[] DiffuseLayers = new string[4];
            string[] HeightLayers = new string[4];
            Texture2D[] AlphaLayers = new Texture2D[4];
            Texture2D ShadowMap = null;

            //Debug.Log(data.textureChunksData[i].NumberOfTextureLayers);
            for (int layer = 0; layer < data.textureChunksData[i].NumberOfTextureLayers; layer++)
            {
                // Diffuse Texture //
                string textureName = data.terrainTexturePaths[data.textureChunksData[i].textureIds[layer]];
                if (!LoadedTerrainTextures.ContainsKey(textureName))
                {
                    ADT.Texture2Ddata tdata = data.terrainTextures[textureName];
                    Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                    tex.LoadRawTextureData(tdata.TextureData);
                    tex.mipMapBias = .5f;
                    tex.Apply();
                    LoadedTerrainTextures[textureName] = tex;
                }
                DiffuseLayers[layer] = textureName;

                // Height Texture //
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
                    HeightLayers[layer] = textureName;
                }
                // Height Values //
                data.heightScales.TryGetValue(textureName, out HeightScales[layer]);
                data.heightOffsets.TryGetValue(textureName, out heightOffsets[layer]);

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

            if (ADTSettings.LoadShadowMaps)
            {
                if (data.textureChunksData[i].shadowMapTexture.Length > 0)
                {
                    ShadowMap = new Texture2D(64, 64, TextureFormat.Alpha8, false);
                    ShadowMap.LoadRawTextureData(data.textureChunksData[i].shadowMapTexture);
                    Color32[] pixels = ShadowMap.GetPixels32();
                    pixels = ADT.RotateMatrix(pixels, 64);
                    ShadowMap.SetPixels32(pixels);
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
                string diffuseName = "_layer" + ln;
                if (DiffuseLayers[ln] != null)
                    mat.SetTexture(diffuseName, LoadedTerrainTextures[DiffuseLayers[ln]]);
                mat.SetTextureScale(diffuseName, new Vector2(-1, -1));
                string heightName = "_height" + ln;
                if (HeightLayers[ln] != null)
                    mat.SetTexture(heightName, LoadedHTerrainTextures[HeightLayers[ln]]);
                mat.SetTextureScale(heightName, new Vector2(-1, -1));
                string alphaName = "_blend" + ln;
                if (ln > 0 && AlphaLayers[ln] != null)
                    mat.SetTexture(alphaName, AlphaLayers[ln]);
            }
            if (data.MTXP)
            {
                mat.SetVector("heightScale", new Vector4(HeightScales[0], HeightScales[1], HeightScales[2], HeightScales[3]));
                mat.SetVector("heightOffset", new Vector4(heightOffsets[0], heightOffsets[1], heightOffsets[2], heightOffsets[3]));
            }
            if (ADTSettings.LoadShadowMaps)
            {
                mat.SetTexture("_shadowMap", ShadowMap);
            }
            Block.transform.GetChild(i).GetComponent<ADTChunk>().MaterialReady(0, mat);

            #endregion
        }
    }

    // Assemble low resolution map textures //
    private void LoadLTexture ()
    {
        MapTexture.MapTextureBlock mapTextureBlock = MapTexture.MapTextureDataQueue.Dequeue();
        Texture2D Ltexture = new Texture2D(mapTextureBlock.data.width, mapTextureBlock.data.height, mapTextureBlock.data.textureFormat, mapTextureBlock.data.hasMipmaps);
        Ltexture.wrapMode = TextureWrapMode.Clamp;
        Ltexture.LoadRawTextureData(mapTextureBlock.data.TextureData);
        Ltexture.Apply();

        Material mat = new Material(shaderWoWTerrainLow);
        mat.SetTexture("_MainTex2", Ltexture);
        mat.SetTextureScale("_MainTex2", new Vector2(1, 1));

        for (int i = 0; i < LoadedQueueItems.Count; i++)
        {
            if (LoadedQueueItems[i].mapName == mapTextureBlock.mapName && LoadedQueueItems[i].x == mapTextureBlock.coords.x && LoadedQueueItems[i].y == mapTextureBlock.coords.y)
            {
                for (int j = 0; j < 256; j++)
                {
                    LoadedQueueItems[i].Block.transform.GetChild(j).GetComponent<ADTChunk>().MaterialReady(1, mat);
                }
            }
        }
        frameBusy = false;
    }

    /*
    IEnumerator CreateADTObject()
    {
        float startTime = Time.time;

        // Get ADT Block Data //
        ADT.BlockData data = ADT.AllBlockData.Dequeue();
        QueueItem Gobject = currentLoadingBlocks.Dequeue();

        //////////////////////////////////////////////
        ///////////          WMO          ////////////
        //////////////////////////////////////////////
        if (ADTSettings.LoadWMOs)
        {
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
                    Vector3 addPosition = new Vector3(wmoInfo.position.x + Gobject.Block.transform.position.x,
                                                      wmoInfo.position.y + Gobject.Block.transform.position.y,
                                                      wmoInfo.position.z + Gobject.Block.transform.position.z);
                    WMOHandler.AddToQueue(wmoPath, wmoInfo.uniqueID, addPosition, wmoInfo.rotation, Vector3.one);
                }
            }
        }
        //////////////////////////////////////////////
        ///////////       Terrain        /////////////
        //////////////////////////////////////////////

        Gobject.Block.name = Gobject.mapName + "_" + Gobject.x + "_" + Gobject.y;

        for (int i = 1; i <= 4; i++)
        {
            CreateChunkQuarter(i, data, Gobject.Block);
            yield return null;
        }
        finishedTime = Time.time - startTime;
    }
    */

    /*
    //////////////////////////////
    #region Create GameObject LoD1 (unused)
    //////////////////////////////
    
    GameObject Chunk1 = Instantiate(ChunkPrefab);

    // Object //
    Chunk1.isStatic = true;
    Chunk1.name = "LoD1_" + i.ToString();
    Chunk1.transform.position = data.ChunksData[i].MeshPosition;
    Chunk1.transform.SetParent(LoD1.transform);

    // Mesh LoD1//
    Mesh mesh1 = new Mesh();
    Vector3[] lod1Verts = new Vector3[81];
    Vector3[] lod1Normals = new Vector3[81];
    Color32[] lod1VertexColors = new Color32[81];
    int currentVert = 0;
    int currentWriteVert = 0;
    for (int v = 0; v < 17; v++)
    {
        if (v % 2 == 0)
        {
            for (int v1 = 0; v1 < 9; v1++)
            {
                lod1Verts[currentWriteVert] = data.ChunksData[i].VertexArray[currentVert];
                lod1Normals[currentWriteVert] = data.ChunksData[i].VertexNormals[currentVert];
                lod1VertexColors[currentWriteVert] = data.ChunksData[i].VertexColors[currentVert];
                currentVert++;
                currentWriteVert++;
            }
        }
        else
        {
            currentVert = currentVert + 8;
        }
    }
    mesh1.vertices = lod1Verts;
    mesh1.triangles = ADT.Chunk_TrianglesLoD1;
    mesh1.uv = ADT.Chunk_UVsLod1;
    mesh1.normals = lod1Normals;
    mesh1.colors32 = lod1VertexColors;
    Chunk1.GetComponent<MeshFilter>().mesh = mesh1;

    // LoD Array //
    renderers1[i] = Chunk1.GetComponent<Renderer>();

    // Material //
    Chunk1.GetComponent<Renderer>().material = ChunkMaterial;
    for (int ln = 0; ln < 4; ln++)
    {
        string diffuseName = "_layer" + ln;
        if (DiffuseLayers[ln] != null)
            Chunk1.GetComponent<Renderer>().material.SetTexture(diffuseName, LoadedTerrainTextures[DiffuseLayers[ln]]);
        Chunk1.GetComponent<Renderer>().material.SetTextureScale(diffuseName, new Vector2(-1, -1));
        string heightName = "_height" + ln;
        if (HeightLayers[ln] != null)
            Chunk1.GetComponent<Renderer>().material.SetTexture(heightName, LoadedHTerrainTextures[HeightLayers[ln]]);
        Chunk1.GetComponent<Renderer>().material.SetTextureScale(heightName, new Vector2(-1, -1));
        string alphaName = "_blend" + ln;
        if (ln > 0 && AlphaLayers[ln] != null)
            Chunk1.GetComponent<Renderer>().material.SetTexture(alphaName, AlphaLayers[ln]);
    }

    if (data.MTXP)
    {
        Chunk1.GetComponent<Renderer>().material.SetVector("heightScale", new Vector4(HeightScales[0], HeightScales[1], HeightScales[2], HeightScales[3]));
        Chunk1.GetComponent<Renderer>().material.SetVector("heightOffset", new Vector4(heightOffsets[0], heightOffsets[1], heightOffsets[2], heightOffsets[3]));
    }
    if (ADTSettings.LoadShadowMaps)
    {
        Chunk1.GetComponent<Renderer>().material.SetTexture("_shadowMap", ShadowMap);
    }
    #endregion
    */

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
