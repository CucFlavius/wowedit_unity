using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2handler : MonoBehaviour
{
    public TerrainHandler terrainHandler;
    public bool busy;
    public Queue<M2QueueItem> M2ThreadQueue = new Queue<M2QueueItem>();
    public Material defaultMaterial;

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

    void Start()
    {
        M2.ThreadWorking = false;
        M2ThreadQueue = new Queue<M2QueueItem>();
    }

    void Update ()
    {
		if (M2.AllM2Data.Count > 0)
        {
            CreateM2Object(M2.AllM2Data.Dequeue());
        }
	}

    public void CreateM2Object(M2.M2Data m2Data)
    {
        // m2 parent object //
        GameObject m2Object = new GameObject();
        m2Object.name = m2Data.dataPath;
        m2Object.transform.position = Vector3.zero;
        m2Object.transform.rotation = Quaternion.identity;
        m2Object.transform.SetParent(transform);

        for (int batch = 0; batch < m2Data.submeshData.Count; batch++)
        {
            // m2 batch object //
            GameObject batchObj = new GameObject();
            batchObj.name = "batch_" + batch;
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
            batchObj.GetComponent<MeshFilter>().mesh = m;
        }
    }
}
