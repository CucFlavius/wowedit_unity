using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M2handler : MonoBehaviour
{
    public TerrainHandler terrainHandler;
    public bool busy;
    public Queue<M2QueueItem> M2ThreadQueue = new Queue<M2QueueItem>();
    public static Thread M2Thread;
    public Material defaultMaterial;

    private string currentM2datapath;
    private int currentM2uniqueID;
    private Vector3 currentM2position;
    private Quaternion currentM2rotation;
    private Vector3 currentM2scale;
    private Dictionary<string, Texture2D> LoadedM2Textures = new Dictionary<string, Texture2D>();
    private List<M2QueueItem> M2Clones = new List<M2QueueItem>();

    public class M2QueueItem
    {
        public string objectDataPath;
        public int uniqueID;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }

    public void AddToQueue(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        M2QueueItem item = new M2QueueItem();
        item.objectDataPath = objectDataPath;
        item.uniqueID = uniqueID;
        item.Position = position;
        item.Rotation = rotation;
        item.Scale = scale;
        M2ThreadQueue.Enqueue(item);
    }

    public void M2ThreadRun(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        currentM2datapath = objectDataPath;
        currentM2uniqueID = uniqueID;
        currentM2position = position;
        currentM2rotation = rotation;
        currentM2scale = scale;

        if (!terrainHandler.LoadedM2s.ContainsKey(objectDataPath))
        {
            // ParseM2Block); //nonthreaded - for testing purposes
            terrainHandler.LoadedM2s.Add(objectDataPath, null);
            M2Thread = new Thread(ParseM2Block);
            M2Thread.IsBackground = true;
            M2Thread.Priority = System.Threading.ThreadPriority.AboveNormal;
            M2Thread.Start();
        }
        else
        {
            CloneM2(objectDataPath, uniqueID, position, rotation, scale);
        }
    }

    void Start()
    {
        M2.ThreadWorking = false;
        M2ThreadQueue = new Queue<M2QueueItem>();
    }

    void Update ()
    {
		if (M2ThreadQueue.Count > 0)
        {
            M2QueueItem queueItem = M2ThreadQueue.Dequeue();
            M2ThreadRun(queueItem.objectDataPath, queueItem.uniqueID, queueItem.Position, queueItem.Rotation, queueItem.Scale);
        }
        else if (M2ThreadQueue.Count == 0)
        {
            busy = false;
        }

        if (M2.AllM2Data.Count > 0)
        {
            if (!M2Thread.IsAlive)
            {
                terrainHandler.frameBusy = true;
                CreateM2Object(M2.AllM2Data.Dequeue());
            }
        }

        if (M2Clones.Count > 0)
        {
            List<M2QueueItem> RemoveElements = new List<M2QueueItem>();
            // Check if copies are Required //
            foreach (M2QueueItem item in M2Clones)
            {
                if (terrainHandler.LoadedM2s.ContainsKey(item.objectDataPath))
                {
                    if (terrainHandler.LoadedM2s[item.objectDataPath] != null)
                    {
                        M2QueueItem clone = item;
                        RemoveElements.Add(item);
                        GameObject instance = Instantiate(terrainHandler.LoadedM2s[item.objectDataPath]);
                        instance.transform.position = clone.Position;
                        instance.transform.rotation = clone.Rotation;
                        instance.transform.localScale = Vector3.one;
                        instance.transform.SetParent(terrainHandler.ADTBlockM2Parents[item.uniqueID].transform);
                    }
                }
            }

            // Remove
            foreach(M2QueueItem removeItem in RemoveElements)
            {
                M2Clones.Remove(removeItem);
            }
            RemoveElements.Clear();
        }
	}

    public void CloneM2(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        M2QueueItem item = new M2QueueItem();
        item.objectDataPath = objectDataPath;
        item.uniqueID = uniqueID;
        item.Position = position;
        item.Rotation = rotation;
        item.Scale = scale;
        M2Clones.Add(item);
    }

    public void ParseM2Block()
    {
        M2.Load(currentM2datapath, currentM2uniqueID, currentM2position, currentM2rotation, currentM2scale);
    }

    public void CreateM2Object(M2.M2Data m2Data)
    {
        // m2 parent object //
        GameObject m2Object = new GameObject();
        m2Object.name = m2Data.dataPath;
        m2Object.transform.position = Vector3.zero;
        m2Object.transform.rotation = Quaternion.identity;
        m2Object.transform.SetParent(transform);

        Debug.Log(m2Data.submeshData.Count);

        for (int batch = 0; batch < m2Data.submeshData.Count; batch++)
        {
            // m2 batch object //
            GameObject batchObj = new GameObject();
            batchObj.name = "batch_" + m2Data.submeshData[batch].ID;
            batchObj.AddComponent<MeshRenderer>();
            batchObj.AddComponent<MeshFilter>();
            batchObj.transform.position = Vector3.zero;
            batchObj.transform.rotation = Quaternion.identity;
            batchObj.GetComponent<MeshRenderer>().material = defaultMaterial;
            batchObj.transform.SetParent(m2Object.transform);

            // mesh //
            Mesh m = new Mesh();
            m.vertices = m2Data.submeshData[batch].vertList;
            m.normals = m2Data.submeshData[batch].normsList;
            m.uv = m2Data.submeshData[batch].uvsList;
            m.uv2 = m2Data.submeshData[batch].uvs2List;
            m.triangles = m2Data.submeshData[batch].triList;
            m.name = "batch_" + m2Data.submeshData[batch].ID + "_mesh";

            batchObj.GetComponent<MeshFilter>().mesh = m;
        }
    }
}
