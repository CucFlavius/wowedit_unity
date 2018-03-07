using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMOhandler : MonoBehaviour {

    public Queue<WMOQueueItem> WMOThreadQueue = new Queue<WMOQueueItem>();
    public GameObject WMObatchprefab;
    public static System.Threading.Thread WMOThread;
    public Material missingMaterial;

    private string currentWMOdatapath;
    private Vector3 currentWMOposition;
    private Quaternion currentWMOrotation;
    private Vector3 currentWMOscale;
    private Dictionary<string, Texture2D> LoadedWMOTextures = new Dictionary<string, Texture2D>();

    public class WMOQueueItem
    {
        public string objectDataPath;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }

    void Start ()
    {
        WMO.ThreadWorking = false;
        WMOThreadQueue = new Queue<WMOQueueItem>();
    }

    public void AddToQueue(string objectDataPath, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        WMOQueueItem item = new WMOQueueItem();
        item.objectDataPath = objectDataPath;
        item.Position = position;
        item.Rotation = rotation;
        item.Scale = scale;
        WMOThreadQueue.Enqueue(item);
    }

    public void WMOThreadRun(string objectDataPath, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        currentWMOdatapath = objectDataPath;
        currentWMOposition = position;
        currentWMOrotation = rotation;
        currentWMOscale = scale;
        ParseWMOBlock(); // nonthreaded - for testing purposes
        WMOThread = new System.Threading.Thread(ParseWMOBlock);
        WMOThread.IsBackground = true;
        WMOThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        //WMOThread.Start();
    }

    void Update()
    {
        if (WMOThreadQueue.Count > 0)
        {
            if (!WMO.ThreadWorking)
            {
                WMO.ThreadWorking = true;
                WMOQueueItem queueItem = WMOThreadQueue.Dequeue();
                WMOThreadRun(queueItem.objectDataPath, queueItem.Position, queueItem.Rotation, queueItem.Scale);
            }
        }
        if (WMO.AllWMOData.Count > 0)
        {
            if (!WMOThread.IsAlive)
            {
                CreateWMOObject();
            }
        }
    }

    public void ParseWMOBlock()
    {
        WMO.Load(currentWMOdatapath, currentWMOposition, currentWMOrotation, currentWMOscale);
    }

    public void CreateWMOObject()
    {
        WMO.WMOData data = WMO.AllWMOData.Dequeue();

        GameObject WMOinstance = new GameObject();
        WMOinstance.transform.SetParent(transform);
        WMOinstance.transform.position = data.position;
        WMOinstance.transform.rotation = data.rotation;
        WMOinstance.transform.localScale = data.scale;
        WMOinstance.name = data.Info.wmoID.ToString();

        int nGroups = data.Info.nGroups;
        for (int g = 0; g < nGroups; g++)
        {
            // object //
            GameObject GroupInstance = new GameObject();
            GroupInstance.transform.SetParent(WMOinstance.transform);
            GroupInstance.name = data.groupsData[g].groupName;

            for (int bn = 0; bn < data.groupsData[g].nBatches; bn++)
            {
                // object //
                GameObject BatchInstance = new GameObject();
                BatchInstance.transform.SetParent(GroupInstance.transform);
                BatchInstance.name = bn.ToString();
                BatchInstance.transform.transform.eulerAngles = new Vector3(BatchInstance.transform.transform.eulerAngles.x, BatchInstance.transform.transform.eulerAngles.y - 180, GroupInstance.transform.transform.eulerAngles.z);

                // mesh //
                BatchInstance.AddComponent<MeshRenderer>();
                BatchInstance.AddComponent<MeshFilter>();
                Mesh bmesh = new Mesh();

                int batchVertSize = (int)((data.groupsData[g].batch_EndVertex[bn] - data.groupsData[g].batch_StartVertex[bn]) + 1);

                Vector3[] batchVertices = new Vector3[batchVertSize];
                Vector2[] batchUVs = new Vector2[batchVertSize];
                Vector3[] batchNormals = new Vector3[batchVertSize];
                Color32[] batchVertexColors = new Color32[batchVertSize];
                List<int> batchTrianglesList = new List<int>();
                int[] batchTriangles;

                int arrayPosition = 0;
                uint batch_startVertex = data.groupsData[g].batch_StartVertex[bn];
                uint batch_endVertex = data.groupsData[g].batch_EndVertex[bn];
                for (uint v = batch_startVertex; v <= batch_endVertex; v++)
                {
                    batchVertices[arrayPosition] = data.groupsData[g].vertices[v];
                    batchUVs[arrayPosition] = data.groupsData[g].UVs[v];
                    batchNormals[arrayPosition] = data.groupsData[g].normals[v];
                    if (!data.groupsData[g].flags.Hasvertexolors)
                        batchVertexColors[arrayPosition] = new Color32(127, 127, 127, 127);
                    else
                        batchVertexColors[arrayPosition] = data.groupsData[g].vertexColors[(int)v];
                    arrayPosition++;
                }

                uint batch_startIndex = data.groupsData[g].batch_StartIndex[bn];
                uint batch_nIndices = data.groupsData[g].batch_nIndices[bn];
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

                // material //
                string textureName = data.texturePaths[data.materials[data.groupsData[g].batchMaterialIDs[bn]].texture1_offset];

                if (LoadedWMOTextures.ContainsKey(textureName))
                {
                    BatchInstance.GetComponent<Renderer>().material.SetTexture("_MainTex", LoadedWMOTextures[textureName]);
                }
                else
                {
                    Texture2Ddata tdata = data.textureData[textureName];
                    Texture2D tex = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                    tex.LoadRawTextureData(tdata.TextureData);
                    tex.Apply();
                    LoadedWMOTextures[textureName] = tex;
                    BatchInstance.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
                }
            }
        }
    }
}
