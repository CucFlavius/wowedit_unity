using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.World.Terrain;
using Assets.WoWEditSettings;
using CASCLib;
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

        private uint currentWMOFileDataId;
        private int currentWMOuniqueID;
        private Vector3 currentWMOposition;
        private Quaternion currentWMOrotation;
        private Vector3 currentWMOscale;
        private Dictionary<uint, Texture2D> LoadedWMOTextures = new Dictionary<uint, Texture2D>();
        private List<WMOQueueItem> WMOClones = new List<WMOQueueItem>();
        private CASCHandler CascHandler;

        public class WMOQueueItem
        {
            public uint FileDataId;
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

        public void AddToQueue(uint FileDataId, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale, CASCHandler Handler)
        {
            CascHandler = Handler;
            WMOQueueItem item = new WMOQueueItem();
            item.FileDataId = FileDataId;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            WMOThreadQueue.Enqueue(item);
        }

        public void WMOThreadRun(uint FileDataId, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            currentWMOFileDataId = FileDataId;
            currentWMOuniqueID = uniqueID;
            currentWMOposition = position;
            currentWMOrotation = rotation;
            currentWMOscale = scale;

            if (!terrainHandler.LoadedWMOIds.ContainsKey(FileDataId))
            {
                //ParseWMOBlock(); // nonthreaded - for testing purposes
                terrainHandler.LoadedWMOIds.Add(FileDataId, null);
                WMOThread = new Thread(ParseWMOBlock);
                WMOThread.IsBackground = true;
                WMOThread.Priority = System.Threading.ThreadPriority.AboveNormal;
                WMOThread.Start();
            }
            else
            {
                CloneWMO(FileDataId, uniqueID, position, rotation, scale);
            }
        }

        // Add WMO copies to a list so they will be copied after loading is done //
        public void CloneWMO(uint FileDataId, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            WMOQueueItem item = new WMOQueueItem();
            item.FileDataId = FileDataId;
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

                    WMOThreadRun(queueItem.FileDataId, queueItem.uniqueID, queueItem.Position, queueItem.Rotation, queueItem.Scale);
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
                    if (terrainHandler.LoadedWMOIds.ContainsKey(item.FileDataId))
                    {
                        if (terrainHandler.LoadedWMOIds[item.FileDataId] != null)
                        {
                            WMOQueueItem clone = item;
                            RemoveElements.Add(item);
                            GameObject instance = Instantiate(terrainHandler.LoadedWMOIds[item.FileDataId]);
                            instance.transform.position = clone.Position;
                            instance.transform.rotation = clone.Rotation;
                            instance.transform.localScale = Vector3.one;
                            instance.transform.SetParent(terrainHandler.ADTBlockWMOParents[item.uniqueID].transform);
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
            WMO.Load(currentWMOFileDataId, currentWMOuniqueID, currentWMOposition, currentWMOrotation, currentWMOscale, CascHandler);
        }

        public void CreateWMOObject()
        {
            if (terrainHandler.working)
            {
                WMO.WMOStruct data = WMO.AllWMOData.Dequeue();
                GameObject WMOinstance = new GameObject();

                terrainHandler.LoadedWMOIds[data.fileDataId] = WMOinstance;


                int nGroups = data.Info.nGroups;
                for (int g = 0; g < nGroups; g++)
                {
                    // group object //
                    GameObject GroupInstance = new GameObject();
                    GroupInstance.isStatic = true;
                    GroupInstance.transform.SetParent(terrainHandler.LoadedWMOIds[data.fileDataId].transform);
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

                        uint TextureFileDataId = data.texturePaths[data.materials[data.groupsData[g].batchMaterialIDs[bn]].TextureId1];
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
                                    Debug.Log("BlendMode To Add: " + blending.ToString() + " Texture Used: " + TextureFileDataId);
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

                        if (LoadedWMOTextures.ContainsKey(TextureFileDataId))
                        {
                            BatchInstance.GetComponent<Renderer>().material.SetTexture("_MainTex", LoadedWMOTextures[TextureFileDataId]);
                        }
                        else
                        {
                            try
                            {
                                Texture2Ddata tdata = data.textureData[TextureFileDataId];
                                Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                                tex.LoadRawTextureData(tdata.TextureData);
                                tex.Apply();
                                LoadedWMOTextures[TextureFileDataId] = tex;
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

                terrainHandler.LoadedWMOIds[data.fileDataId].transform.position = data.position;
                terrainHandler.LoadedWMOIds[data.fileDataId].transform.rotation = data.rotation;
                terrainHandler.LoadedWMOIds[data.fileDataId].transform.localScale = data.scale;
                if (data.uniqueID != -1)
                {
                    if (terrainHandler.ADTBlockWMOParents[data.uniqueID] != null)
                        terrainHandler.LoadedWMOIds[data.fileDataId].transform.SetParent(terrainHandler.ADTBlockWMOParents[data.uniqueID].transform);
                    else
                        Destroy(terrainHandler.LoadedWMOIds[data.fileDataId]);
                }
                terrainHandler.LoadedWMOIds[data.fileDataId].name = data.Info.wmoID.ToString();

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