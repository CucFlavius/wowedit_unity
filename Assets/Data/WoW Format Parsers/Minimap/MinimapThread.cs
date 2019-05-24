using Assets.UI.CASC;
using CASCLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MinimapThread
{
    public static bool ResetParentSize = false;
    public static bool ThreadAlive = false;
    public static uint currentMapFileDataId;
    public static bool WMOOnlyZone = false;
    public static bool checkWMOonly = false;
    public static CASCHandler CascHandler;

    public static void LoadThread()
    {
        CascHandler = GameObject.Find("[CASC]").GetComponent<CascHandler>().cascHandler;

        ThreadAlive = true;
        CompileMapList(currentMapFileDataId, CascHandler);
        GetMinMax();
        checkWMOonly = true;
        ResetParentSize = true;
        RequestAvailableBLPs(currentMapFileDataId);
        ThreadAlive = false;
    }

    // Build an array of available minimaps and maps //
    public static void CompileMapList(uint FileDataId, CASCHandler cascHandler)
    {
        MinimapData.dataExists.WDT = false;

        // if (Settings.)
        // if (!WDT.Flags.ContainsKey(FileDataId))
        //     MinimapData.dataExists.WDT = WDT.ParseWDT(FileDataId);
        // else
        //     MinimapData.dataExists.WDT = true;

        // sort data //
        MinimapData.mapAvailability = new MinimapData.MapAvailability[64, 64];

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                // WDT data //
                if (MinimapData.dataExists.WDT)
                    MinimapData.mapAvailability[x, y].WDT = WDT.Flags[FileDataId].HasADT[x, y];
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
    private static void RequestAvailableBLPs(uint FileDataId)
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
                    minimapRequest.fileDataId = FileDataId;
                    minimapRequest.coords = new Vector2(x + X, y + Y);
                    RequestBlock(minimapRequest);
                }
                else
                {
                    if (MinimapData.mapAvailability[x + Y, y + Y].ADT)
                    {
                        Minimap.MinimapRequest minimapRequest = new Minimap.MinimapRequest();
                        minimapRequest.fileDataId = FileDataId;
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
        // string mapName = minimapRequest.mapName;
        // string fileName = "map" + minimapRequest.coords.x + "_" + minimapRequest.coords.y + ".blp";
        // string path = @"world\minimaps\" + mapName + @"\" + fileName;
        // int fdid = Casc.GetFileDataIdByName(path);
        // string extractedPath = Casc.GetFile(fdid);
        // using (Stream stream = File.Open(extractedPath, FileMode.Open))
        // {
        //     BLP blp = new BLP();
        //     byte[] data = blp.GetUncompressed(stream, true);
        //     BLPinfo info = blp.Info();
        //     MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
        //     blockData.mapName = mapName;
        //     blockData.coords = minimapRequest.coords;
        //     blockData.textureInfo = info;
        //     blockData.minimapByteData = data;

        //     MinimapData.MinimapDataQueue.Enqueue(blockData);
        // }
    }

    private static void EnqueueEmptyBlock(Minimap.MinimapRequest minimapRequest)
    {
        uint fileDataId = minimapRequest.fileDataId;
        MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
        blockData.fileDataId = fileDataId;
        blockData.coords = minimapRequest.coords;
        blockData.minimapByteData = null;
        MinimapData.MinimapDataQueue.Enqueue(blockData);
    }
}
