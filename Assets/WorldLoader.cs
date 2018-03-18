using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour {


    public GameObject Camera; // get vector3 location
    public GameObject TerrainParent; // with terrain handler script
    public GameObject WMOParent;
    public GameObject ADTBlockObject;
    public GameObject ADTLowBlockObject;
    public MinimapHandler minimapHandler;
    public int maxWorldSize = 64;
    public GameObject[,] ADTMatrix;// = new GameObject[maxWorldSize, maxWorldSize];
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

    // Use this for initialization
    void Start () {

        blockSize = 533.33333f / Settings.worldScale;

        // create matrices //
        ADTMatrix = new GameObject[maxWorldSize, maxWorldSize];
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
    
    /*
    public void Spiral(int X, int Y)
    {
        int x, y, dx, dy;
        x = y = dx = 0;
        dx = 0;
        dy = -1;
        int t = drawDistanceLOD1 * 2 + 1;
        int maxI = t * t;
        for (int i = 0; i < maxI; i++)
        {
            if (((x + X) > 0) && ((x + X) < maxWorldSize) && ((y + Y) > 0) && ((y + Y) < maxWorldSize))
            {
                if (Mathf.Abs(x) <= drawDistanceLOD0 && Mathf.Abs(y) <= drawDistanceLOD0)
                {
                    int xC = x + X;
                    int yC = y + Y;
                    currentTerrainLod[xC, yC] = 0;
                    if (existingADTs[xC, yC])
                    {
                        if (previousTerrainLod[xC, yC] != 0)
                        {
                            float zPos = (32 - xC) * blockSize;
                            float xPos = (32 - yC) * blockSize;
                            if (previousTerrainLod[xC, yC] == 10)
                            {
                                if (ADTMatrix[xC, yC] == null)
                                {
                                    ADTMatrix[xC, yC] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                    ADTMatrix[xC, yC].transform.SetParent(TerrainParent.transform);
                                    TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, xC, yC, ADTMatrix[xC, yC]);
                                }
                                else
                                {
                                    ADTMatrix[xC, yC].SetActive(true);
                                }
                                //ADTMatrix[x, y].SetActive(true);
                            }
                            if (previousTerrainLod[xC, yC] == 1)
                            {
                                if (ADTMatrix[xC, yC] == null)
                                {
                                    ADTMatrix[xC, yC] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                    ADTMatrix[xC, yC].transform.SetParent(TerrainParent.transform);
                                }
                                else
                                {
                                    ADTMatrix[xC, yC].SetActive(true);
                                }
                                if (ADTLowMatrix[xC, yC] != null)
                                {
                                    //ADTLowMatrix[x, y].SetActive(false);
                                }
                            }
                        }
                    }
                    previousTerrainLod[xC, yC] = currentTerrainLod[xC, yC];
                }
                else if (Mathf.Abs(x) <= drawDistanceLOD1 && Mathf.Abs(y) <= drawDistanceLOD1)
                {
                    currentTerrainLod[x + X, y + Y] = 1;
                }
            }
            if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
            {
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }
    */

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
                            if (ADTMatrix[x, y] == null)
                            {
                                ADTMatrix[x, y] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                ADTMatrix[x, y].transform.SetParent(TerrainParent.transform);
                                TerrainParent.GetComponent<TerrainHandler>().AddToQueue(MapName, x, y, ADTMatrix[x, y]);
                            }
                            else
                            {
                                ADTMatrix[x, y].SetActive(true);
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
                            if (ADTMatrix[x, y] != null)
                            {
                                ADTMatrix[x, y].SetActive(false);
                            }

                        }
                        // low quality exists - load high quality
                        if (currentTerrainLod[x, y] == 0 && previousTerrainLod[x, y] == 1)
                        {
                            if (ADTMatrix[x, y] == null)
                            {
                                ADTMatrix[x, y] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                                ADTMatrix[x, y].transform.SetParent(TerrainParent.transform);
                            }
                            else
                            {
                                ADTMatrix[x, y].SetActive(true);
                            }
                            if (ADTLowMatrix[x, y] != null)
                            {
                                //ADTLowMatrix[x, y].SetActive(false);
                            }
                        }
                        // destroy both low and high quality
                        if (currentTerrainLod[x, y] == 10 && previousTerrainLod[x, y] != 10)
                        {
                            if (ADTMatrix[x, y] != null)
                            {
                                ADTMatrix[x, y].SetActive(false);
                                //Destroy(ADTMatrix[x, y].gameObject);
                                //ADTMatrix[x, y] = null;
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
