using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.World.Terrain;
using Assets.WoWEditSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.World.Models
{
    public class WMOhandler : MonoBehaviour
    {
        public TerrainHandler terrainHandler;
        public bool busy;
        public bool working;
        public Queue<WMOQueueItem> WMOThreadQueue = new Queue<WMOQueueItem>();
        public GameObject WMObatchprefab;
        public static Thread WMOThread;
        public Material missingMaterial;
        public Material[] WMOmaterials; // 0 - diffuse, 1 - Specular, 2 - Metal, 3 - Environment Mapped, 4 - Opaque

        private string currentWMOdatapath;
        private ulong currentWMOdatahash;
        private int currentWMOuniqueID;
        private Vector3 currentWMOposition;
        private Quaternion currentWMOrotation;
        private Vector3 currentWMOscale;
        private Dictionary<string, Texture2D> LoadedWMOTextures = new Dictionary<string, Texture2D>();
        private List<WMOQueueItem> WMOClones = new List<WMOQueueItem>();

        public class WMOQueueItem
        {
            public string objectDataPath;
            public ulong Hash;
            public int uniqueID;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }

        void Start()
        {
            WMO.ThreadWorking = false;
            WMOThreadQueue = new Queue<WMOQueueItem>();
        }

        public void AddToQueue(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            WMOQueueItem item = new WMOQueueItem();
            item.objectDataPath = objectDataPath;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            WMOThreadQueue.Enqueue(item);
        }

        public void AddToQueue(ulong Hash, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            WMOQueueItem item = new WMOQueueItem();
            item.Hash = Hash;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            WMOThreadQueue.Enqueue(item);
        }

        // Parsing thread - Unless it's a copy //
        public void WMOThreadRun(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            currentWMOdatapath = objectDataPath;
            currentWMOuniqueID = uniqueID;
            currentWMOposition = position;
            currentWMOrotation = rotation;
            currentWMOscale = scale;

            if (!terrainHandler.LoadedWMOs.ContainsKey(objectDataPath))
            {
                //ParseWMOBlock(); // nonthreaded - for testing purposes
                terrainHandler.LoadedWMOs.Add(objectDataPath, null);
                WMOThread = new Thread(ParseWMOBlock);
                WMOThread.IsBackground = true;
                WMOThread.Priority = System.Threading.ThreadPriority.AboveNormal;
                WMOThread.Start();
            }
            else
            {
                CloneWMO(objectDataPath, uniqueID, position, rotation, scale);
            }
        }

        public void WMOThreadRun(ulong Hash, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            currentWMOdatahash = Hash;
            currentWMOuniqueID = uniqueID;
            currentWMOposition = position;
            currentWMOrotation = rotation;
            currentWMOscale = scale;

            if (!terrainHandler.LoadedWMOHashes.ContainsKey(Hash))
            {
                //ParseWMOBlock(); // nonthreaded - for testing purposes
                terrainHandler.LoadedWMOHashes.Add(Hash, null);
                WMOThread = new Thread(ParseWMOBlock);
                WMOThread.IsBackground = true;
                WMOThread.Priority = System.Threading.ThreadPriority.AboveNormal;
                WMOThread.Start();
            }
            else
            {
                CloneWMO(Hash, uniqueID, position, rotation, scale);
            }
        }

        // Add WMO copies to a list so they will be copied after loading is done //
        public void CloneWMO(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            WMOQueueItem item = new WMOQueueItem();
            item.objectDataPath = objectDataPath;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            WMOClones.Add(item);
        }
        public void CloneWMO(ulong Hash, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            WMOQueueItem item = new WMOQueueItem();
            item.Hash = Hash;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            WMOClones.Add(item);
        }

        void Update()
        {
            if (WMOThreadQueue.Count > 0)
            {
                if (!WMO.ThreadWorking)
                {
                    WMOQueueItem queueItem = WMOThreadQueue.Dequeue();

                    if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                        WMOThreadRun(queueItem.objectDataPath, queueItem.uniqueID, queueItem.Position, queueItem.Rotation, queueItem.Scale);
                    else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                        WMOThreadRun(queueItem.Hash, queueItem.uniqueID, queueItem.Position, queueItem.Rotation, queueItem.Scale);
                }
            }
            else if (WMOThreadQueue.Count == 0)
                busy = false;

            if (WMO.AllWMOData.Count > 0)
            {
                if (!WMOThread.IsAlive)
                {
                    if (!terrainHandler.frameBusy)
                    {
                        terrainHandler.frameBusy = true;
                        CreateWMOObject();
                    }
                }
            }

            if (WMOClones.Count > 0)
            {
                List<WMOQueueItem> RemoveElements = new List<WMOQueueItem>();
                // Check if Copies are Required //
                foreach (WMOQueueItem item in WMOClones)
                {
                    if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                    {
                        if (terrainHandler.LoadedWMOs.ContainsKey(item.objectDataPath))
                        {
                            if (terrainHandler.LoadedWMOs[item.objectDataPath] != null)
                            {
                                WMOQueueItem clone = item;
                                RemoveElements.Add(item);
                                GameObject instance = Instantiate(terrainHandler.LoadedWMOs[item.objectDataPath]);
                                instance.transform.position = clone.Position;
                                instance.transform.rotation = clone.Rotation;
                                instance.transform.localScale = Vector3.one;
                                instance.transform.SetParent(terrainHandler.ADTBlockWMOParents[item.uniqueID].transform);
                            }
                        }
                    }
                    else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                    {
                        if (terrainHandler.LoadedWMOHashes.ContainsKey(item.Hash))
                        {
                            if (terrainHandler.LoadedWMOHashes[item.Hash] != null)
                            {
                                WMOQueueItem clone = item;
                                RemoveElements.Add(item);
                                GameObject instance = Instantiate(terrainHandler.LoadedWMOHashes[item.Hash]);
                                instance.transform.position = clone.Position;
                                instance.transform.rotation = clone.Rotation;
                                instance.transform.localScale = Vector3.one;
                                instance.transform.SetParent(terrainHandler.ADTBlockWMOParents[item.uniqueID].transform);
                            }
                        }
                    }
                }
                // Remove 
                foreach (WMOQueueItem removeItem in RemoveElements)
                    WMOClones.Remove(removeItem);

                RemoveElements.Clear();
            }
        }

        public void ParseWMOBlock()
        {
            if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                WMO.Load(currentWMOdatapath, currentWMOuniqueID, currentWMOposition, currentWMOrotation, currentWMOscale);
            else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                WMO.Load(currentWMOdatahash, currentWMOuniqueID, currentWMOposition, currentWMOrotation, currentWMOscale);
        }

        public void CreateWMOObject()
        {
            if (terrainHandler.working)
            {
                WMO.WMOStruct data = WMO.AllWMOData.Dequeue();
                GameObject WMOinstance = new GameObject();

                if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                    terrainHandler.LoadedWMOs[data.dataPath] = WMOinstance;
                else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                    terrainHandler.LoadedWMOHashes[data.dataHash] = WMOinstance;


                int nGroups = data.Info.nGroups;
                for (int g = 0; g < nGroups; g++)
                {
                    // group object //
                    GameObject GroupInstance = new GameObject();
                    GroupInstance.isStatic = true;

                    if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                        GroupInstance.transform.SetParent(terrainHandler.LoadedWMOs[data.dataPath].transform);
                    else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                        GroupInstance.transform.SetParent(terrainHandler.LoadedWMOHashes[data.dataHash].transform);

                    GroupInstance.name = data.groupsData[g].groupName;

                    LODGroup Lodgroup = GroupInstance.AddComponent<LODGroup>();
                    LOD[] lods = new LOD[1];
                    Renderer[] renderers = new Renderer[data.groupsData[g].nBatches];

                    // Batches //
                    for (int bn = 0; bn < data.groupsData[g].nBatches; bn++)
                    {
                        ////////////////////////////////
                        #region object

                        GameObject BatchInstance = new GameObject();
                        BatchInstance.isStatic = true;
                        BatchInstance.transform.SetParent(GroupInstance.transform);
                        BatchInstance.name = bn.ToString();
                        BatchInstance.transform.transform.eulerAngles = new Vector3(BatchInstance.transform.transform.eulerAngles.x, BatchInstance.transform.transform.eulerAngles.y - 180, GroupInstance.transform.transform.eulerAngles.z);

                        #endregion
                        ////////////////////////////////

                        ////////////////////////////////
                        #region mesh

                        BatchInstance.AddComponent<MeshRenderer>();
                        BatchInstance.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                        renderers[bn] = BatchInstance.GetComponent<MeshRenderer>();
                        BatchInstance.AddComponent<MeshFilter>();
                        Mesh bmesh = new Mesh();

                        uint batchVertSize = data.groupsData[g].batch_MaxIndex[bn] - data.groupsData[g].batch_MinIndex[bn] + 1;

                        Vector3[] batchVertices         = new Vector3[batchVertSize];
                        Vector2[] batchUVs              = new Vector2[batchVertSize];
                        Vector3[] batchNormals          = new Vector3[batchVertSize];
                        Color32[] batchVertexColors     = new Color32[batchVertSize];
                        List<int> batchTrianglesList    = new List<int>();
                        int[] batchTriangles;

                        int arrayPosition = 0;
                        uint batch_startVertex = data.groupsData[g].batch_MinIndex[bn];
                        uint batch_endVertex = data.groupsData[g].batch_MaxIndex[bn];
                        for (uint v = batch_startVertex; v <= batch_endVertex; v++)
                        {
                            batchVertices[arrayPosition]    = data.groupsData[g].vertices[v];
                            batchUVs[arrayPosition]         = data.groupsData[g].UVs[v];
                            batchNormals[arrayPosition]     = data.groupsData[g].normals[v];
                            if (!data.groupsData[g].flags.Hasvertexolors)
                                batchVertexColors[arrayPosition] = new Color32(127, 127, 127, 127);
                            else
                                batchVertexColors[arrayPosition] = data.groupsData[g].vertexColors[(int)v];
                            arrayPosition++;
                        }

                        uint batch_startIndex   = data.groupsData[g].batch_StartIndex[bn];
                        uint batch_nIndices     = data.groupsData[g].batch_Count[bn];
                        for (uint idx = batch_startIndex; idx <= batch_startIndex + batch_nIndices - 2; idx = idx + 3)
                        {
                            uint in1 = data.groupsData[g].triangles[idx + 0];
                            uint in2 = data.groupsData[g].triangles[idx + 1];
                            uint in3 = data.groupsData[g].triangles[idx + 2];
                            int a = (int)(in1 - batch_startVertex);
                            int b = (int)(in2 - batch_startVertex);
                            int c = (int)(in3 - batch_startVertex);

                            batchTrianglesList.Add(a);
                            batchTrianglesList.Add(b);
                            batchTrianglesList.Add(c);
                        }
                        batchTrianglesList.Reverse();
                        batchTriangles = batchTrianglesList.ToArray();

                        bmesh.vertices = batchVertices;
                        bmesh.uv = batchUVs;
                        bmesh.normals = batchNormals;
                        bmesh.triangles = batchTriangles;
                        bmesh.colors32 = batchVertexColors;
                        BatchInstance.GetComponent<MeshFilter>().mesh = bmesh;
                        BatchInstance.GetComponent<MeshRenderer>().sharedMaterial = missingMaterial;

                        #endregion
                        ////////////////////////////////

                        ////////////////////////////////
                        #region material

                        string textureName = data.texturePaths[data.materials[data.groupsData[g].batchMaterialIDs[bn]].TextureId1];
                        BatchInstance.GetComponent<Renderer>().material = WMOmaterials[(int)data.materials[data.groupsData[g].batchMaterialIDs[bn]].ShaderType];

                        ////////////////////////////////
                        #region Set Fragment Shader

                        WMOFragmentShader shader = data.materials[data.groupsData[g].batchMaterialIDs[bn]].ShaderType;
                        switch (shader)
                        {
                            case WMOFragmentShader.Diffuse:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderDiffuse", 1.0f);
                                    break;
                                }
                            case WMOFragmentShader.Specular:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderSpecular", 1.0f);
                                    break;
                                }
                            case WMOFragmentShader.Metal:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderMetal", 1.0f);
                                    break;
                                }
                            case WMOFragmentShader.Env:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderEnv", 1.0f);
                                    break;
                                }
                            case WMOFragmentShader.Opaque:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderOpaque", 1.0f);
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_AlphaToMask", 1.0f);
                                    break;
                                }
                            default:
                                {
                                    BatchInstance.GetComponent<Renderer>().material.SetFloat("_ShaderDiffuse", 1.0f);
                                    break;
                                }
                        }


                        #endregion
                        ////////////////////////////////

                        ////////////////////////////////
                        #region Set Material Flags

                        // F_UNCULLED //
                        int Culling = 2; // on (only front)
                        if (data.materials[data.groupsData[g].batchMaterialIDs[bn]].flags.F_UNCULLED)
                            Culling = 0; // off (both sides_
                        BatchInstance.GetComponent<Renderer>().material.SetFloat("F_UNCULLED", Culling);
                        // F_UNLIT //
                        if (data.materials[data.groupsData[g].batchMaterialIDs[bn]].flags.F_UNLIT)
                            BatchInstance.GetComponent<Renderer>().material.EnableKeyword("F_UNLIT");
                        //BatchInstance.GetComponent<Renderer>().material.SetFloat("_F_UNLIT", data.materials[data.groupsData[g].batchMaterialIDs[bn]].flags.F_UNLIT ? 1 : 0);
                        // F_UNFOGGED //
                        BatchInstance.GetComponent<Renderer>().material.SetFloat("_F_UNFOGGED", data.materials[data.groupsData[g].batchMaterialIDs[bn]].flags.F_UNFOGGED ? 1 : 0);

                        #endregion
                        ////////////////////////////////

                        ////////////////////////////////
                        #region Set Blending Mode

                        // set default blend: One Zero, basicly off
                        UnityEngine.Rendering.BlendMode source = UnityEngine.Rendering.BlendMode.One;
                        UnityEngine.Rendering.BlendMode destination = UnityEngine.Rendering.BlendMode.Zero;

                        BlendingMode blending = data.materials[data.groupsData[g].batchMaterialIDs[bn]].BlendMode;

                        switch (blending)
                        {
                            case BlendingMode.Opaque:
                                {
                                    source = UnityEngine.Rendering.BlendMode.One;
                                    destination = UnityEngine.Rendering.BlendMode.Zero;
                                    break;
                                }
                            case BlendingMode.AlphaKey:
                                {
                                    source = UnityEngine.Rendering.BlendMode.One;
                                    destination = UnityEngine.Rendering.BlendMode.Zero;
                                    break;
                                }
                            case BlendingMode.Alpha:
                                {
                                    source = UnityEngine.Rendering.BlendMode.SrcAlpha;
                                    destination = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                                    break;
                                }
                            case BlendingMode.Additive:
                                {
                                    source = UnityEngine.Rendering.BlendMode.One;
                                    destination = UnityEngine.Rendering.BlendMode.One;
                                    break;
                                }
                            case BlendingMode.Modulate:
                                {
                                    source = UnityEngine.Rendering.BlendMode.DstColor;
                                    destination = UnityEngine.Rendering.BlendMode.Zero;
                                    break;
                                }
                            case BlendingMode.Modulate2x:
                                {
                                    source = UnityEngine.Rendering.BlendMode.DstColor;
                                    destination = UnityEngine.Rendering.BlendMode.SrcColor;
                                    break;
                                }
                            case BlendingMode.ModulateAdditive:
                                {
                                    source = UnityEngine.Rendering.BlendMode.DstColor;
                                    destination = UnityEngine.Rendering.BlendMode.One;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("BlendMode To Add: " + blending.ToString() + " Texture Used: " + textureName);
                                    source = UnityEngine.Rendering.BlendMode.One;
                                    destination = UnityEngine.Rendering.BlendMode.Zero;
                                    break;
                                }
                        }
                        BatchInstance.GetComponent<Renderer>().material.SetInt("MySrcMode", (int)source);
                        BatchInstance.GetComponent<Renderer>().material.SetInt("MyDstMode", (int)destination);

                        #endregion
                        ////////////////////////////////

                        ////////////////////////////////
                        #region Assign Textures

                        if (LoadedWMOTextures.ContainsKey(textureName))
                        {
                            BatchInstance.GetComponent<Renderer>().material.SetTexture("_MainTex", LoadedWMOTextures[textureName]);
                        }
                        else
                        {
                            try
                            {
                                Texture2Ddata tdata = data.textureData[textureName];
                                Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                                tex.LoadRawTextureData(tdata.TextureData);
                                tex.Apply();
                                LoadedWMOTextures[textureName] = tex;
                                BatchInstance.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
                            }
                            catch (Exception ex)
                            {
                                Debug.Log("Error: Loading RawTextureData @ WMOhandler");
                                Debug.LogException(ex);
                            }
                        }
                        #endregion
                        ////////////////////////////////

                        #endregion
                        ////////////////////////////////
                    }

                    lods[0] = new LOD(.1f, renderers);
                    Lodgroup.SetLODs(lods);
                    Lodgroup.animateCrossFading = true;
                    Lodgroup.fadeMode = LODFadeMode.SpeedTree;
                    Lodgroup.RecalculateBounds();
                }

                if (Settings.GetSection("misc").GetString("wowsource") == "extracted")
                {
                    terrainHandler.LoadedWMOs[data.dataPath].transform.position = data.position;
                    terrainHandler.LoadedWMOs[data.dataPath].transform.rotation = data.rotation;
                    terrainHandler.LoadedWMOs[data.dataPath].transform.localScale = data.scale;
                    if (data.uniqueID != -1)
                    {
                        if (terrainHandler.ADTBlockWMOParents[data.uniqueID] != null)
                            terrainHandler.LoadedWMOs[data.dataPath].transform.SetParent(terrainHandler.ADTBlockWMOParents[data.uniqueID].transform);
                        else
                            Destroy(terrainHandler.LoadedWMOs[data.dataPath]);
                    }
                    terrainHandler.LoadedWMOs[data.dataPath].name = data.Info.wmoID.ToString();
                }
                else if (Settings.GetSection("misc").GetString("wowsource") == "game")
                {
                    terrainHandler.LoadedWMOHashes[data.dataHash].transform.position = data.position;
                    terrainHandler.LoadedWMOHashes[data.dataHash].transform.rotation = data.rotation;
                    terrainHandler.LoadedWMOHashes[data.dataHash].transform.localScale = data.scale;
                    if (data.uniqueID != -1)
                    {
                        if (terrainHandler.ADTBlockWMOParents[data.uniqueID] != null)
                            terrainHandler.LoadedWMOHashes[data.dataHash].transform.SetParent(terrainHandler.ADTBlockWMOParents[data.uniqueID].transform);
                        else
                            Destroy(terrainHandler.LoadedWMOHashes[data.dataHash]);
                    }
                    terrainHandler.LoadedWMOHashes[data.dataHash].name = data.Info.wmoID.ToString();
                }

                terrainHandler.frameBusy = false;
            }
        }

        public void StopLoading()
        {
            WMO.AllWMOData.Clear();
            WMOThreadQueue.Clear();
        }
    }
}