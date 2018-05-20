using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2handler : MonoBehaviour
{
    public TerrainHandler terrainHandler;
    public bool busy;
    public Queue<M2QueueItem> M2ThreadQueue = new Queue<M2QueueItem>();

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
        GameObject m2Obj = new GameObject();
        m2Obj.name = "";
        m2Obj.AddComponent<MeshRenderer>();
        m2Obj.AddComponent<MeshFilter>();
        m2Obj.transform.position = Vector3.zero;

        Mesh m = new Mesh();
        m.vertices = m2Data.meshData.pos.ToArray();
        m.normals = m2Data.meshData.normal.ToArray();
        m.uv = m2Data.meshData.tex_coords.ToArray();

        m2Obj.GetComponent<MeshFilter>().mesh = m;
    }
}
