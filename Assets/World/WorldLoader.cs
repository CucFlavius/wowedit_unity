using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour {


    public GameObject Camera; // get vector3 location
    public GameObject TerrainParent; // with terrain handler script
    public GameObject WMOParent;
    public GameObject M2Parent;
    public GameObject ADTBlockObject;
    public GameObject ADTLowBlockObject;
    public TerrainImport terrainImport;
    public TerrainHandler terainHandler;
    public int maxWorldSize = 64;
    public int[,] ADTMatrix;// = new GameObject[maxWorldSize, maxWorldSize];
    //public Queue<GameObject> LoadedADTBlocks;
    public List<GameObject> LoadedADTBlocks;
    public GameObject[,] ADTLowMatrix;
    public bool[,] existingADTs;// = new bool[maxWorldSize, maxWorldSize];
    public int[,] previousTerrainLod;// = new int[maxWorldSize, maxWorldSize];
    public int[,] currentTerrainLod;// = new int[maxWorldSize, maxWorldSize];
    private float blockSize;
    public int drawDistanceLOD0;
    public int drawDistanceLOD1;
    public Vector2 CameraStartBlock;
    private int PreviousCamX;
    private int PreviousCamY;
    public string MapName;
    private int pullFrom = 0;

    // Use this for initialization
    void Start () {

        //InvokeRepeating("FreeMemory", 2.0f, 10f);

        blockSize = 533.33333f / Settings.worldScale;

        // create matrices //
        ADTMatrix = new int[maxWorldSize, maxWorldSize];
        //LoadedADTBlocks = new Queue<GameObject>();
        LoadedADTBlocks = new List<GameObject>();
        ADTLowMatrix = new GameObject[maxWorldSize, maxWorldSize];
        existingADTs = new bool[maxWorldSize, maxWorldSize];
        previousTerrainLod = new int[maxWorldSize, maxWorldSize];
        currentTerrainLod = new int[maxWorldSize, maxWorldSize];
        // clear Matrix //
        ClearMatrix();
    }
	
	// Update is called once per frame
	void Update () {

        // check spatial position //
        int CurrentCamX = (int)Mathf.Floor(32+(-Camera.transform.position.z / blockSize));
        int CurrentCamY = (int)Mathf.Floor(32+(-Camera.transform.position.x / blockSize));
        if (CurrentCamX != PreviousCamX || CurrentCamY != PreviousCamY)
        {
            PreviousCamX = CurrentCamX;
            PreviousCamY = CurrentCamY;
            UpdateLodMatrices(CurrentCamX, CurrentCamY);
            Loader();
        }

        if (LoadedADTBlocks.Count > 60)
        {
            if (!LoadedADTBlocks[pullFrom].activeSelf)
            {
                GameObject PulledObj = LoadedADTBlocks[pullFrom];
                Vector2 coords = PulledObj.GetComponent<ADTBlock>().coords;
                string mapName = PulledObj.GetComponent<ADTBlock>().mapName;
                ADTMatrix[(int)coords.x, (int)coords.y] = 0;
                TerrainHandler.QueueItem queueItem = new TerrainHandler.QueueItem();
                queueItem.x = (int)coords.x;
                queueItem.y = (int)coords.y;
                queueItem.mapName = mapName;
                queueItem.Block = PulledObj;
                if (terainHandler.ADTTexQueue.Contains(queueItem))
                {
                    terainHandler.ADTTexQueue.Remove(queueItem);
                }
                PulledObj.GetComponent<ADTBlock>().UnloadAsset();
                LoadedADTBlocks.RemoveAt(pullFrom);
                terainHandler.LoadedQueueItems.Remove(queueItem);
                pullFrom = 0;
            }
            else
            {
                pullFrom++;
            }
        }
        
	}

    public void FreeMemory()
    {
        Resources.UnloadUnusedAssets();
    }

    public void LoadFullWorld (string map_name, Vector2 playerSpawn)
    {
        ADT.working = true;
        TerrainParent.GetComponent<TerrainHandler>().frameBusy = false;
        pullFrom = 0;
        MapName = map_name;

        // clear Matrix //
        ClearMatrix();

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                existingADTs[x, y] = MinimapData.mapAvailability[x, y].ADT;
            }
        }

        // Initial spawn //
        ClearLoDArray(previousTerrainLod);
        //ClearLoDArray(currentTerrainLod);

        playerSpawn = new Vector2(playerSpawn.y, playerSpawn.x);

        // position camera obj //
        Camera.transform.position = new Vector3((32-playerSpawn.x)*blockSize, 60f, (32-playerSpawn.y)*blockSize);

        int CurrentCamX = (int)playerSpawn.y;
        int CurrentCamY = (int)playerSpawn.x;
        PreviousCamX = CurrentCamX;
        PreviousCamY = CurrentCamY;

        UpdateLodMatrices(CurrentCamX, CurrentCamY);
        //Loader();
    }

    public void UpdateLodMatrices (int currentPosX, int currentPosY)
    {
        ClearLoDArray(currentTerrainLod);
        Spiral(currentPosX, currentPosY);
    }

    
    public void Spiral(int X, int Y)
    {
        int x, y, dx, dy;
        x = y = dx = 0;
        dx = 0;
        dy = -1;
        int t = drawDistanceLOD1*2+1;
        int maxI = t * t;
        for (int i = 0; i < maxI; i++)
        {
            if (((x + X) > 0) && ((x + X) < maxWorldSize) && ((y + Y) > 0) && ((y + Y) < maxWorldSize))
            {
                if (Mathf.Abs(x) <= drawDistanceLOD0 && Mathf.Abs(y) <= drawDistanceLOD0)
                {
                    currentTerrainLod[x + X, y + Y] = 0;
                    SpiralLoader(x + X, y + Y);
                }
                else if (Mathf.Abs(x) <= drawDistanceLOD1 && Mathf.Abs(y) <= drawDistanceLOD1)
                {
                    currentTerrainLod[x + X, y + Y] = 1;
                    SpiralLoader(x + X, y + Y);
                }
            }
            if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)) )
            {
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }

    public void SpiralLoader(int x, int y)
    {
        if (existingADTs[x, y])
        {
            if (currentTerrainLod[x, y] != previousTerrainLod[x, y])
            {
                float zPos = (32 - x) * blockSize;
                float xPos = (32 - y) * blockSize;
                // no terrain exists - load high quality
                if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 10)
                {
                    if (ADTMatrix[x, y] == 0)
                    {
                        bool Exists = false;
                        foreach (Transform child in TerrainParent.transform)
                        {
                            Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                            if (coords == new Vector2(x, y))
                            {
                                Exists = true;
                                ADTMatrix[x, y] = 1;
                                child.gameObject.SetActive(true);
                            }
                        }
                        if (!Exists)
                        {
                            ADTMatrix[x, y] = 1;
                            GameObject ADTblock = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                            ADTblock.transform.SetParent(TerrainParent.transform);
                            ADTblock.GetComponent<ADTBlock>().coords = new Vector2(x, y);
                            ADTblock.GetComponent<ADTBlock>().mapName = MapName;
                            TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTblock);
                            LoadedADTBlocks.Add(ADTblock);
                        }
                    }
                    else
                    {
                        //ADTMatrix[x, y].SetActive(true);
                    }
                    //ADTMatrix[x, y].SetActive(true);
                }
                // no terrain exists - load low quality
                if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 10)
                {
                    if (ADTLowMatrix[x, y] == null)
                    {
                        //ADTLowMatrix[x, y] = Instantiate(ADTLowBlockObject, new Vector3(xPos-20, 0, zPos-20), Quaternion.identity);
                        //ADTLowMatrix[x, y].transform.SetParent(TerrainParent.transform);
                    }
                    //ADTLowMatrix[x, y].SetActive(true);
                }
                // high quality exists - load low quality
                if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 0)
                {
                    if (ADTLowMatrix[x, y] == null)
                    {
                        //ADTLowMatrix[x, y] = Instantiate(ADTLowBlockObject, new Vector3(xPos-20, 0, zPos-20), Quaternion.identity);
                        //ADTLowMatrix[x, y].transform.SetParent(TerrainParent.transform);
                    }
                    else
                    {
                        //ADTLowMatrix[x, y].SetActive(true);
                    }
                    /*
                    if (ADTMatrix[x, y] != null)
                    {
                        ADTMatrix[x, y].SetActive(false);
                    }
                    */
                }
                // low quality exists - load high quality
                if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 1)
                {
                    if (ADTMatrix[x, y] == 0)
                    {
                        bool Exists = false;
                        foreach (Transform child in TerrainParent.transform)
                        {
                            Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                            if (coords == new Vector2(x, y))
                            {
                                Exists = true;
                                ADTMatrix[x, y] = 1;
                                child.gameObject.SetActive(true);
                            }
                        }
                        if (!Exists)
                        {
                            ADTMatrix[x, y] = 1;
                            GameObject ADTblock = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                            ADTblock.transform.SetParent(TerrainParent.transform);
                            ADTblock.GetComponent<ADTBlock>().coords = new Vector2(x, y);
                            ADTblock.GetComponent<ADTBlock>().mapName = MapName;
                            TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTblock);
                            LoadedADTBlocks.Add(ADTblock);
                        }
                    }
                    else
                    {
                        //ADTMatrix[x, y].SetActive(true);
                    }
                    if (ADTLowMatrix[x, y] != null)
                    {
                        //ADTLowMatrix[x, y].SetActive(false);
                    }
                }
                // destroy both low and high quality
                if (currentTerrainLod[x, y] == 10 && previousTerrainLod[x, y] != 10)
                {
                    if (ADTMatrix[x, y] != 0)
                    {
                        //ADTMatrix[x, y].SetActive(false);
                        foreach (Transform child in TerrainParent.transform)
                        {
                            Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                            if (coords == new Vector2(x, y))
                            {
                                child.gameObject.SetActive(false);
                                ADTMatrix[x, y] = 0;
                            }
                        }
                    }
                    if (ADTLowMatrix[x, y] != null)
                    {
                        //Destroy(ADTLowMatrix[x, y].gameObject);
                        //ADTLowMatrix[x, y] = null;
                    }
                }

                previousTerrainLod[x, y] = currentTerrainLod[x, y];
            }
        }
    }

    public void Loader ()
    {
        for (int x = 0; x < maxWorldSize - 1; x++)
        {
            for (int y = 0; y < maxWorldSize - 1; y++)
            {
                if (existingADTs[x, y])
                {
                    if (currentTerrainLod[x, y] != previousTerrainLod[x, y])
                    {
                        float zPos = (32 - x) * blockSize;
                        float xPos = (32 - y) * blockSize;
                        // no terrain exists - load high quality
                        if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 10)
                        {
                            if (ADTMatrix[x, y] == 0)
                            {
                                bool Exists = false;
                                foreach (Transform child in TerrainParent.transform)
                                {
                                    Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                                    if (coords == new Vector2(x, y))
                                    {
                                        Exists = true;
                                        ADTMatrix[x, y] = 1;
                                        child.gameObject.SetActive(true);
                                    }
                                }
                                if (!Exists)
                                {
                                    ADTMatrix[x, y] = 1;
                                    GameObject ADTblock = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                    ADTblock.transform.SetParent(TerrainParent.transform);
                                    ADTblock.GetComponent<ADTBlock>().coords = new Vector2(x, y);
                                    ADTblock.GetComponent<ADTBlock>().mapName = MapName;
                                    TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTblock);
                                    LoadedADTBlocks.Add(ADTblock);
                                }
                            }
                            else
                            {
                                //ADTMatrix[x, y].SetActive(true);
                            }
                            //ADTMatrix[x, y].SetActive(true);
                        }
                        // no terrain exists - load low quality
                        if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 10)
                        {
                            if (ADTLowMatrix[x, y] == null)
                            {
                                //ADTLowMatrix[x, y] = Instantiate(ADTLowBlockObject, new Vector3(xPos-20, 0, zPos-20), Quaternion.identity);
                                //ADTLowMatrix[x, y].transform.SetParent(TerrainParent.transform);
                            }
                            //ADTLowMatrix[x, y].SetActive(true);
                        }
                        // high quality exists - load low quality
                        if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 0)
                        {
                            if (ADTLowMatrix[x, y] == null)
                            {
                                //ADTLowMatrix[x, y] = Instantiate(ADTLowBlockObject, new Vector3(xPos-20, 0, zPos-20), Quaternion.identity);
                                //ADTLowMatrix[x, y].transform.SetParent(TerrainParent.transform);
                            }
                            else
                            {
                                //ADTLowMatrix[x, y].SetActive(true);
                            }
                            /*
                            if (ADTMatrix[x, y] != null)
                            {
                                ADTMatrix[x, y].SetActive(false);
                            }
                            */
                        }
                        // low quality exists - load high quality
                        if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 1)
                        {
                            if (ADTMatrix[x, y] == 0)
                            {
                                bool Exists = false;
                                foreach (Transform child in TerrainParent.transform)
                                {
                                    Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                                    if (coords == new Vector2(x, y))
                                    {
                                        Exists = true;
                                        ADTMatrix[x, y] = 1;
                                        child.gameObject.SetActive(true);
                                    }
                                }
                                if (!Exists)
                                {
                                    ADTMatrix[x, y] = 1;
                                    GameObject ADTblock = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                    ADTblock.transform.SetParent(TerrainParent.transform);
                                    ADTblock.GetComponent<ADTBlock>().coords = new Vector2(x, y);
                                    ADTblock.GetComponent<ADTBlock>().mapName = MapName;
                                    TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTblock);
                                    LoadedADTBlocks.Add(ADTblock);
                                }
                            }
                            else
                            {
                                //ADTMatrix[x, y].SetActive(true);
                            }
                            if (ADTLowMatrix[x, y] != null)
                            {
                                //ADTLowMatrix[x, y].SetActive(false);
                            }
                        }
                        // destroy both low and high quality
                        if (currentTerrainLod[x, y] == 10 && previousTerrainLod[x, y] != 10)
                        {
                            if (ADTMatrix[x, y] != 0)
                            {
                                //ADTMatrix[x, y].SetActive(false);
                                foreach (Transform child in TerrainParent.transform)
                                {
                                    Vector2 coords = child.gameObject.GetComponent<ADTBlock>().coords;
                                    if (coords == new Vector2(x,y))
                                    {
                                        child.gameObject.SetActive(false);
                                        ADTMatrix[x, y] = 0;
                                    }
                                }
                            }
                            if (ADTLowMatrix[x, y] != null)
                            {
                                //Destroy(ADTLowMatrix[x, y].gameObject);
                                //ADTLowMatrix[x, y] = null;
                            }
                        }

                        previousTerrainLod[x, y] = currentTerrainLod[x, y];
                    }
                }
            }
        }
    }

    public void ClearMatrix ()
    {
        for(int x = 0; x < maxWorldSize-1; x++)
        {
            for (int y = 0; y < maxWorldSize - 1; y++)
            {
                existingADTs[x, y] = false;
                ADTMatrix[x, y] = 0;
            }
        }
    }

    public void ClearLoDArray (int [,] array)
    {
        for (int x = 0; x < maxWorldSize - 1; x++)
        {
            for (int y = 0; y < maxWorldSize - 1; y++)
            {
                array[x, y] = 10;
            }
        }
    }

    public void ClearAllTerrain()
    {
        if (TerrainParent.transform.childCount > 0)
        {
            terrainImport.currentSelectedPlayerSpawn = new Vector2(0, 0);
            LoadedADTBlocks.Clear();

            // Stop working world threads //
            TerrainParent.GetComponent<TerrainHandler>().StopLoading();
            WMOParent.GetComponent<WMOhandler>().StopLoading();
            M2Parent.GetComponent<M2handler>().StopLoading();

            ADTMatrix = new int[maxWorldSize, maxWorldSize];
            LoadedADTBlocks = new List<GameObject>();
            ADTLowMatrix = new GameObject[maxWorldSize, maxWorldSize];
            existingADTs = new bool[maxWorldSize, maxWorldSize];
            previousTerrainLod = new int[maxWorldSize, maxWorldSize];
            currentTerrainLod = new int[maxWorldSize, maxWorldSize];
            pullFrom = 0;
            // clear Matrix //
            ClearMatrix();


            foreach (Transform child in WMOParent.transform)
            {
                //child.gameObject.GetComponent<WMOObject>().UnloadAsset();
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in TerrainParent.transform)
            {
                child.gameObject.GetComponent<ADTBlock>().UnloadAsset();
                DiscordController.mapName = "";
            }
            foreach (Transform child in M2Parent.transform)
            {
                //child.gameObject.GetComponent<M2Object>().UnloadAsset();
                GameObject.Destroy(child.gameObject);
            }
            Resources.UnloadUnusedAssets();
        }
    }
}
