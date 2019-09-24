using System.Collections.Generic;
using UnityEngine;
using Assets.WoWEditSettings;
using Assets.Data.WoW_Format_Parsers.ADT;
using Assets.World.Models;
using Assets.World.Terrain;

// World Loader 2 is a faster implementation of the world loading algorhythm
// Work in progress
public class WorldLoader2 : MonoBehaviour
{
    #region Globals

    [Header("Position Tracker")]
    private Vector2Int currentTrackerPosition;      // the current position of the loadTracker in block units
    private Vector2Int previousTrackerPosition;     // the previous position of the loadTracker in block units

    [Header("World")]
    private bool worldUpdateRequired;               // triggered when the tracker moved in block space and blocks need to be updated
    public BlockState[,] blockStates;
    public GameObject[,] blocks;
    public List<string> activeBlocks;
    public static List<Task> tasks;
    public uint WdtFileDataId;
    public bool[,] existingADTs;
    public bool worldLoaded = false;

    [Header("Reference")]
    public Transform loadTracker;                   // the object being tracked for world loading
    public TerrainHandler terrainHandler;
    public Transform cam;
    public GameObject ADTBlockObject;

    #endregion

    #region Enums & Structs

    public class Task
    {
        public Vector2Int blockPosition;
        public int LoD;
        public TaskType type;
        public bool inProgress;
    }

    public enum TaskType
    {
        None,
        Load0,
        Load1,
        Load2,
        Unload
    }

    public class BlockState
    {
        public BlockStatus LoD0 = BlockStatus.NotLoaded;
        public BlockStatus LoD1 = BlockStatus.NotLoaded;
        public BlockStatus LoD2 = BlockStatus.NotLoaded;
    }

    public enum BlockStatus
    {
        NotLoaded,
        Loading,
        Loaded
    }

    #endregion

    // run at start
    private void Start()
    {
        // reset variables to default
        previousTrackerPosition = new Vector2Int(-1, -1);
        worldUpdateRequired = false;
        tasks = new List<Task>();
        activeBlocks = new List<string>();
        blocks = new GameObject[64, 64];
        existingADTs = new bool[64, 64];

        // allocate block states matrix
        blockStates = new BlockState[Settings.MAX_WORLD_SIZE, Settings.MAX_WORLD_SIZE];
        for (int x = 0; x < Settings.MAX_WORLD_SIZE; x++)
        {
            for (int y = 0; y < Settings.MAX_WORLD_SIZE; y++)
            {
                blockStates[x, y] = new BlockState();
            }
        }

    }

    private void Update()
    {
        SpatialTracker();
        if (worldUpdateRequired && worldLoaded)
        {
            Spiral(currentTrackerPosition.x, currentTrackerPosition.y);
            worldUpdateRequired = false;
        }
        ProcessTasks();
    }

    public void LoadWorld(uint WdtFileDataId, Vector2 playerSpawn)
    {
        ADT.working = true;
        terrainHandler.frameBusy = false;
        this.WdtFileDataId = WdtFileDataId;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                existingADTs[x, y] = MinimapData.mapAvailability[x, y].ADT;
            }
        }

        // Initial spawn //
        playerSpawn = new Vector2(playerSpawn.y, playerSpawn.x);

        // position camera obj //
        cam.transform.position = new Vector3((32 - playerSpawn.x) * Settings.BLOCK_SIZE, 60f, (32 - playerSpawn.y) * Settings.BLOCK_SIZE);

        currentTrackerPosition = new Vector2Int((int)playerSpawn.y, (int)playerSpawn.x);
        previousTrackerPosition = currentTrackerPosition;

        worldUpdateRequired = true;
        worldLoaded = true;
    }

    private void ProcessTasks()
    {
        for (int t = 0; t < tasks.Count; t++)
        {
            int x = tasks[t].blockPosition.x;
            int y = tasks[t].blockPosition.y;
            float zPos = (32 - x) * Settings.BLOCK_SIZE;
            float xPos = (32 - y) * Settings.BLOCK_SIZE;

            if (blocks[x, y] != null)
            {

            }
            else
            {
                blocks[tasks[t].blockPosition.x, tasks[t].blockPosition.y] = Instantiate(ADTBlockObject, new Vector3(xPos, 0, zPos), Quaternion.identity);
                blocks[tasks[t].blockPosition.x, tasks[t].blockPosition.y].transform.SetParent(terrainHandler.transform);
                blocks[tasks[t].blockPosition.x, tasks[t].blockPosition.y].GetComponent<ADTBlock>().coords = new Vector2(x, y);
                blocks[tasks[t].blockPosition.x, tasks[t].blockPosition.y].GetComponent<ADTBlock>().FileDataId = WDT.WDTEntries[(x, y)].RootADT;

                terrainHandler.AddToQueue(WDT.WDTEntries[(x, y)].RootADT, x, y, blocks[tasks[t].blockPosition.x, tasks[t].blockPosition.y], WdtFileDataId);
            }
            tasks[t].inProgress = true;
            //tasks.RemoveAt(t);
        }
    }

    // track player/camera position in 3d space to change what needs to be loaded
    public void SpatialTracker()
    {
        currentTrackerPosition = new Vector2Int(
            (int)Mathf.Floor(32 + (-loadTracker.position.z / Settings.BLOCK_SIZE)),
            (int)Mathf.Floor(32 + (-loadTracker.position.x / Settings.BLOCK_SIZE))
            ); // the current position of the load tracker in block units

        // if the camera changed position to another block
        if (currentTrackerPosition.x != previousTrackerPosition.x || currentTrackerPosition.y != previousTrackerPosition.y)
        {
            previousTrackerPosition.x = currentTrackerPosition.x;
            previousTrackerPosition.y = currentTrackerPosition.y;

            worldUpdateRequired = true;
        }
    }

    /*
    public void UpdateWorld()
    {
        // get a new tasks list based on the new distances
        List<string> newBlocks = Spiral(currentTrackerPosition.x, currentTrackerPosition.y);

        // compare with current blocks list
        List<string> firstNotSecond = activeBlocks.Except(newBlocks).ToList();      // blocks that need unloading
        List<string> secondNotFirst = newBlocks.Except(activeBlocks).ToList();      // blocks that need loading fresh
        List<string> both = new List<string>();                                     // blocks that might need updating
        foreach (string block in newBlocks)
        {
            if (activeBlocks.Contains(block))
                both.Add(block);
        }

        // update tasks
        foreach (Task task in tasks)
        {
            // remove tasks
            foreach (string blockString in firstNotSecond)
            {
                Vector2Int blockPosition = BlockstringToVector2(blockString);
                if (task.blockPosition.x == blockPosition.x && task.blockPosition.y == blockPosition.y)
                {

                }
            }

            // add new tasks
            foreach (string blockString in secondNotFirst)
            {
                Vector2Int blockPosition = BlockstringToVector2(blockString);
                // check if already loading, else load
                if (task.blockPosition.x == blockPosition.x && task.blockPosition.y == blockPosition.y)
                {

                }
                else
                {

                }
            }


        }

    }
    */

    private Vector2Int BlockstringToVector2(string blockString)
    {
        string[] split = blockString.Split('_');
        int x = -1;
        int.TryParse(split[0], out x);
        int y = -1;
        int.TryParse(split[1], out y);
        return new Vector2Int(x, y);
    }

    // spiral progression algorhythm that updates block states and tasks
    public void Spiral(int X, int Y)
    {
        int taskPosition = 0;
        int x, y, dx, dy;
        x = y = dx = 0;
        dx = 0;
        dy = -1;
        int t = Settings.MAX_BLOCK_DISTANCE * 2 + 1;
        //int maxI = t * t;  // scan up to max draw distance
        int maxI = 64 * 64; // scan whole matrix
        for (int i = 0; i < maxI; i++)
        {
            if (((x + X) > 0) && ((x + X) < Settings.MAX_WORLD_SIZE) && ((y + Y) > 0) && ((y + Y) < Settings.MAX_WORLD_SIZE))
            {
                int newX = x + X;
                int newY = y + Y;
                // up to lod 0
                if (Mathf.Abs(x) <= Settings.TERRAIN_DISTANCE_LOD0 && Mathf.Abs(y) <= Settings.TERRAIN_DISTANCE_LOD0)
                {
                    //print(newX + " " + newY);
                    if (blockStates[newX,newY].LoD0 == BlockStatus.NotLoaded)
                    {
                        if (blockStates[newX, newY].LoD1 == BlockStatus.NotLoaded)
                        {
                            if (blockStates[newX, newY].LoD2 == BlockStatus.NotLoaded)
                            {
                                tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 0, type = TaskType.Load2, inProgress = false });
                                taskPosition++;
                                blockStates[newX, newY].LoD2 = BlockStatus.Loading;
                            }
                            tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 0, type = TaskType.Load1, inProgress = false });
                            taskPosition++;
                            blockStates[newX, newY].LoD1 = BlockStatus.Loading;
                        }
                        tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 0, type = TaskType.Load0, inProgress = false });
                        taskPosition++;
                        blockStates[newX, newY].LoD0 = BlockStatus.Loading;
                    }
                }
                // up to lod 1
                else if (Mathf.Abs(x) <= Settings.TERRAIN_DISTANCE_LOD1 && Mathf.Abs(y) <= Settings.TERRAIN_DISTANCE_LOD1)
                {
                    if (blockStates[newX, newY].LoD1 == BlockStatus.NotLoaded)
                    {
                        if (blockStates[newX, newY].LoD2 == BlockStatus.NotLoaded)
                        {
                            tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 1, type = TaskType.Load2, inProgress = false });
                            taskPosition++;
                            blockStates[newX, newY].LoD2 = BlockStatus.Loading;
                        }
                        tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 1, type = TaskType.Load1, inProgress = false });
                        taskPosition++;
                        blockStates[newX, newY].LoD1 = BlockStatus.Loading;
                    }
                    
                    // if LoD 0 is loading, cancel it
                    if (blockStates[newX, newY].LoD0 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 0);
                    }
                }
                // up to lod 2
                else if (Mathf.Abs(x) <= Settings.TERRAIN_DISTANCE_LOD2 && Mathf.Abs(y) <= Settings.TERRAIN_DISTANCE_LOD2)
                {
                    if (blockStates[newX, newY].LoD2 == BlockStatus.NotLoaded)
                    {
                        tasks.Insert(taskPosition, new Task { blockPosition = new Vector2Int(newX, newY), LoD = 2, type = TaskType.Load2, inProgress = false });
                        taskPosition++;
                        blockStates[newX, newY].LoD2 = BlockStatus.Loading;
                    }

                    // if LoD 0 is loading, cancel it
                    if (blockStates[newX, newY].LoD0 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 0);
                    }

                    // if LoD 1 is loading, cancel it
                    if (blockStates[newX, newY].LoD1 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 1);
                    }
                }
                // up to world bounds
                else
                {
                    // unload LoD0
                    if (blockStates[newX, newY].LoD0 == BlockStatus.Loaded)
                    {
                        QueueDestroyBlock(new Vector2Int(newX, newY));
                    }
                    // stop loading LoD0
                    else if (blockStates[newX, newY].LoD0 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 0);
                    }
                    blockStates[newX, newY].LoD0 = BlockStatus.NotLoaded;

                    // unload LoD1
                    if (blockStates[newX, newY].LoD1 == BlockStatus.Loaded)
                    {
                        QueueDestroyBlock(new Vector2Int(newX, newY));
                    }
                    // stop loading LoD1
                    else if (blockStates[newX, newY].LoD1 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 1);
                    }
                    blockStates[newX, newY].LoD1 = BlockStatus.NotLoaded;

                    // unload LoD2
                    if (blockStates[newX, newY].LoD2 == BlockStatus.Loaded)
                    {
                        QueueDestroyBlock(new Vector2Int(newX, newY));
                    }
                    // stop loading LoD2
                    else if (blockStates[newX, newY].LoD2 == BlockStatus.Loading)
                    {
                        StopTask(new Vector2Int(newX, newY), 2);
                    }
                    blockStates[newX, newY].LoD2 = BlockStatus.NotLoaded;
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

    private void SwitchBlockState(Vector2Int position, int destinationLoD)
    {

    }

    private void QueueDestroyBlock(Vector2Int position)
    {

    }

    private void StopTask(Vector2Int position, int LoD)
    {
        foreach (Task task in tasks)
        {
            if (task.blockPosition.x == position.x && task.blockPosition.y == position.y)
            {
                if (!task.inProgress)
                    tasks.Remove(task);
                else
                {
                    StopThreadProgress(new Vector2Int(position.x, position.y), 2);
                }
            }
        }
    }

    private void StopThreadProgress (Vector2Int position, int LoD)
    {

    }
}
