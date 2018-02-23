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

    

    public Queue<QueueItem> ADTThreadQueue; // = new Queue<QueueItem>();
    public Queue<QueueItem> currentLoadingBlocks; // = new Queue<QueueItem>();
    public static bool FinishedCreatingObject;

    public class QueueItem
    {
        public string mapName;
        public int x;
        public int y;
        public GameObject Block;
    }


    // Use this for initialization
    void Start () {
        ADT.ADTdataReady = false;
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

        ADTThread = new System.Threading.Thread(ParseADTBlock);
        ADTThread.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (ADTThreadQueue.Count > 0 && ADT.ThreadWorking == false)
        {
            ADT.ThreadWorking = true;
            QueueItem queueItem = ADTThreadQueue.Dequeue();
            print ("parse " + System.DateTime.Now);
            ADTThreadRun(queueItem.mapName, queueItem.x, queueItem.y, queueItem.Block);
        }

        if (ADT.AllADTdata.Count > 0 && !ADTThread.IsAlive)
        {
            print("create " + System.DateTime.Now);
            CreateADTObject();
            //ADT.AllADTdata.RemoveAt(0);
        }
    }

    public void ParseADTBlock ()
    {
        string ADTpath = @"\world\maps\" + MapName + @"\" + MapName + "_" + BlockX + "_" + BlockY + ".adt";
        //print(ADTpath);
        ADT.Load(ADTpath, MapName, new Vector2(BlockX, BlockY));
    }

    public void CreateADTObject()
    {
        FinishedCreatingObject = false;

        QueueItem Gobject = currentLoadingBlocks.Dequeue();
        Gobject.Block.name = Gobject.mapName + "_" + Gobject.x + "_" + Gobject.y;
        GameObject LoD0 = new GameObject();
        LoD0.transform.SetParent(Gobject.Block.transform);

        List<ADT.ChunkData> data = ADT.AllADTdata.Dequeue();

        for (int i = 0; i < 256; i++)
        {
            

            GameObject ChunkObj = Instantiate(ChunkPrefab, Vector3.zero, Quaternion.identity);
            ChunkObj.isStatic = true;
            Mesh mesh = new Mesh();

            mesh.vertices = data[i].VertexArray;
            mesh.triangles = data[i].TriangleArray;
            mesh.uv = data[i].UVArray;
            mesh.RecalculateNormals();
            ChunkObj.GetComponent<MeshFilter>().mesh = mesh;
            ChunkObj.name = "Chunk_" + i.ToString();
            ChunkObj.transform.position = data[i].MeshPosition; //new Vector3 (data[i].MeshPosition.x, data[i].MeshPosition.z, data[i].MeshPosition.y);
            ChunkObj.transform.SetParent(LoD0.transform);

        }
        FinishedCreatingObject = true;
    }
}
