using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour {


    public GameObject Camera; // get vector3 location
    public GameObject TerrainParent; // with terrain handler script
    public GameObject WMOParent;
    public GameObject ADTBlockObject;
    public MinimapHandler minimapHandler;
    public int maxWorldSize = 64;
    public GameObject[,] ADTmatrix;// = new GameObject[maxWorldSize, maxWorldSize];
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

    // Use this for initialization
    void Start () {

        blockSize = 533.33333f / Settings.worldScale;

        // create matrices //
        ADTmatrix = new GameObject[maxWorldSize, maxWorldSize];
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

	}

    public void LoadFullWorld (List<string> MinimapFileList, string map_name, Vector2 playerSpawn)
    {
        MapName = map_name;

        // clear Matrix //
        ClearMatrix();

        if (MinimapFileList.Count > 0) // build a terrain list based on loaded minimaps 
        {
            // fill Matrix with available //
            foreach (string minimap in MinimapFileList)
            {
                string split0 = minimap.Split("map"[2])[1];
                int xCoord = int.Parse(split0.Split("_"[0])[0]);
                int yCoord = int.Parse(split0.Split("_"[0])[1]);
                existingADTs[xCoord, yCoord] = true;
            }

            // find a middle one //
            string split1 = MinimapFileList[MinimapFileList.Count / 2].Split("map"[2])[1];
            int MidBlockX = int.Parse(split1.Split("_"[0])[0]);
            int MidBlockY = int.Parse(split1.Split("_"[0])[1]);
        }
        else // check WDT for existing ADT's instead
        {
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    if (WDT.Flags[map_name].HasADT[x, y])
                    {
                        //Debug.Log(x + " " + y);
                        existingADTs[x, y] = true;
                    }
                }
            }
        }
        // Initial spawn //
        ClearLoDArray(previousTerrainLod);
        //ClearLoDArray(currentTerrainLod);

        // position camera obj //
        Camera.transform.position = new Vector3((32-playerSpawn.x)*blockSize, 60f, (32-playerSpawn.y)*blockSize);

        int CurrentCamX = (int)playerSpawn.y;
        int CurrentCamY = (int)playerSpawn.x;
        PreviousCamX = CurrentCamX;
        PreviousCamY = CurrentCamY;

        UpdateLodMatrices(CurrentCamX, CurrentCamY);
        Loader();
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
                }
                else if (Mathf.Abs(x) <= drawDistanceLOD1 && Mathf.Abs(y) <= drawDistanceLOD1)
                {
                    currentTerrainLod[x + X, y + Y] = 1;
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
                        if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 10)
                        {
                            if (ADTmatrix[x, y] == null)
                            {
                                ADTmatrix[x, y] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                ADTmatrix[x, y].transform.SetParent(TerrainParent.transform);
                            }
                            ADTmatrix[x, y].SetActive(true);
                            
                            if (!ADTmatrix[x, y].GetComponent<ADTBlock>().LoD0Loaded)
                            {
                                TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTmatrix[x, y]);
                                ADTmatrix[x, y].GetComponent<ADTBlock>().LoD0Loaded = true;
                            }
                        }

                        if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 10)
                        {
                            if (ADTmatrix[x, y] == null)
                            {
                                ADTmatrix[x, y] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                ADTmatrix[x, y].transform.SetParent(TerrainParent.transform);
                            }
                            ADTmatrix[x, y].SetActive(true);

                            if (!ADTmatrix[x, y].GetComponent<ADTBlock>().LoD1Loaded)
                            {
                                //TerrainParent.GetComponent<TerrainHandler>().ADTThreadRun(MapName, x, y, ADTmatrix[x, y]);
                                //ADTmatrix[x, y].GetComponent<ADTBlock>().LoD1Loaded = true;
                            }
                        }

                        if (currentTerrainLod[x, y] == 1 && previousTerrainLod[x, y] == 0)
                        {
                            if (ADTmatrix[x, y] != null)
                            {
                                ADTmatrix[x, y].SetActive(true);
                                //TerrainParent.GetComponent<TerrainHandler>().ADTThreadRun(MapName, x, y, ADTmatrix[x, y]);
                                //ADTmatrix[x, y].GetComponent<ADTBlock>().LoadLod1();
                            }
                        }

                        if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 1)
                        {
                            ADTmatrix[x, y].SetActive(true);

                            if (!ADTmatrix[x, y].GetComponent<ADTBlock>().LoD0Loaded)
                            {
                                TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTmatrix[x, y]);
                                ADTmatrix[x, y].GetComponent<ADTBlock>().LoD0Loaded = true;
                            }
                        }

                        if (currentTerrainLod[x, y] == 10 && previousTerrainLod[x, y] != 10)
                        {
                            if (ADTmatrix[x, y] != null)
                                ADTmatrix[x, y].SetActive(false);
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
        minimapHandler.currentSelectedPlayerSpawn = new Vector2(0, 0);
        ClearMatrix();

        foreach (Transform child in TerrainParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
