using Assets.Data.CASC;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MinimapThread
{
    public static bool ResetParentSize = false;
    public static bool ThreadAlive = false;
    public static string currentMapName = "";
    public static bool WMOOnlyZone = false;
    public static bool checkWMOonly = false;

    public static void LoadThread()
    {
        ThreadAlive = true;
        CompileMapList(currentMapName);
        GetMinMax();
        checkWMOonly = true;
        ResetParentSize = true;
        RequestAvailableBLPs(currentMapName);
        ThreadAlive = false;
    }

    // Build an array of available minimaps and maps //
    public static void CompileMapList(string mapName)
    {
        MinimapData.dataExists.WDT = false;
        if (!WDT.Flags.ContainsKey(mapName))
            MinimapData.dataExists.WDT = WDT.Load(@"world\maps\" + mapName + @"\", mapName);
        else
            MinimapData.dataExists.WDT = true;
        MinimapData.dataExists.ADT = Casc.FolderExists(@"world\maps\" + mapName);
        MinimapData.dataExists.Minimap = Casc.FolderExists(@"world\minimaps\" + mapName);
        // sort data //
        MinimapData.mapAvailability = new MinimapData.MapAvailability[64, 64];
        List<string> ADTs = new List<string>();
        List<string> Minimaps = new List<string>();

        if (MinimapData.dataExists.ADT)
            ADTs = Casc.GetFileListFromFolder(@"world\maps\" + mapName);
        if (MinimapData.dataExists.Minimap)
            Minimaps = Casc.GetFileListFromFolder(@"world\minimaps\" + mapName);
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                // WDT data //
                if (MinimapData.dataExists.WDT)
                {
                    MinimapData.mapAvailability[x, y].WDT = WDT.Flags[mapName].HasADT[x, y];
                }
                // ADT files //
                if (MinimapData.dataExists.ADT)
                {
                    if (ADTs.Contains(mapName + "_" + x + "_" + y + ".adt"))
                        MinimapData.mapAvailability[x, y].ADT = true;
                    else
                        MinimapData.mapAvailability[x, y].ADT = false;
                }
                // Minimap files //
                if (MinimapData.dataExists.Minimap)
                {
                    if (Minimaps.Contains("map" + x + "_" + y + ".blp"))
                        MinimapData.mapAvailability[x, y].Minimap = true;
                    else
                        MinimapData.mapAvailability[x, y].Minimap = false;
                }
            }
        }
    }

    private static void GetMinMax()
    {
        int firstXCoord = 64;
        int firstYCoord = 64;
        int lastXCoord = 0;
        int lastYCoord = 0;

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (MinimapData.mapAvailability[x, y].ADT || MinimapData.mapAvailability[x, y].Minimap)
                {
                    if (x < firstXCoord) firstXCoord = x;
                    if (y < firstYCoord) firstYCoord = y;
                    MinimapData.Total++;
                }
            }
        }

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (MinimapData.mapAvailability[x, y].ADT || MinimapData.mapAvailability[x, y].Minimap)
                {
                    if (x > lastXCoord) lastXCoord = x;
                    if (y > lastYCoord) lastYCoord = y;
                }
            }
        }
        MinimapData.Min = new Vector2(firstXCoord, firstYCoord);
        MinimapData.Max = new Vector2(lastXCoord, lastYCoord);
        if (MinimapData.Total == 0)
            WMOOnlyZone = true;
        else
            WMOOnlyZone = false;
    }

    // Request BLP blocks //
    private static void RequestAvailableBLPs(string mapName)
    {
        int X = (int)(MinimapData.Min.x + ((MinimapData.Max.x - MinimapData.Min.x) / 2));
        int Y = (int)(MinimapData.Min.x + ((MinimapData.Max.x - MinimapData.Min.x) / 2));
        Debug.Log(X + " " + Y);
        int x, y, dx, dy;
        x = y = dx = 0;
        dx = 0;
        dy = -1;
        int t = 64;
        int maxI = t * t;
        for (int i = 0; i < maxI; i++)
        {
            if (((x + X) > 0) && ((x + X) < maxI) && ((y + Y) > 0) && ((y + Y) < maxI))
            {
                if (MinimapData.mapAvailability[x + Y, y + Y].Minimap)
                {
                    Minimap.MinimapRequest minimapRequest = new Minimap.MinimapRequest();
                    minimapRequest.mapName = mapName;
                    minimapRequest.coords = new Vector2(x + X, y + Y);
                    RequestBlock(minimapRequest);
                }
                else
                {
                    if (MinimapData.mapAvailability[x + Y, y + Y].ADT)
                    {
                        Minimap.MinimapRequest minimapRequest = new Minimap.MinimapRequest();
                        minimapRequest.mapName = mapName;
                        minimapRequest.coords = new Vector2(x + Y, y + Y);
                        EnqueueEmptyBlock(minimapRequest);
                    }
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

    // Request a minimap image from the parser //
    private static void RequestBlock(Minimap.MinimapRequest minimapRequest)
    {
        string mapName = minimapRequest.mapName;
        string fileName = "map" + minimapRequest.coords.x + "_" + minimapRequest.coords.y + ".blp";
        string path = @"world\minimaps\" + mapName + @"\" + fileName;
        int fdid = Casc.GetFileDataIdByName(path);
        string extractedPath = Casc.GetFile(fdid);
        using (Stream stream = File.Open(extractedPath, FileMode.Open))
        {
            BLP blp = new BLP();
            byte[] data = blp.GetUncompressed(stream, true);
            BLPinfo info = blp.Info();
            MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
            blockData.mapName = mapName;
            blockData.coords = minimapRequest.coords;
            blockData.textureInfo = info;
            blockData.minimapByteData = data;

            MinimapData.MinimapDataQueue.Enqueue(blockData);
        }
    }

    private static void EnqueueEmptyBlock(Minimap.MinimapRequest minimapRequest)
    {
        string mapName = minimapRequest.mapName;
        MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
        blockData.mapName = mapName;
        blockData.coords = minimapRequest.coords;
        blockData.minimapByteData = null;
        MinimapData.MinimapDataQueue.Enqueue(blockData);
    }
}
