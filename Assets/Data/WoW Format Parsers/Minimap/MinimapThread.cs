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
    public static uint currentWdtFileDataId;
    public static bool WMOOnlyZone = false;
    public static bool checkWMOonly = false;
    public static CASCHandler CascHandler;

    public static void LoadThread(CASCHandler Handler)
    {
        CascHandler     = Handler;
        ThreadAlive     = true;
        CompileMapList(currentWdtFileDataId);
        GetMinMax();
        checkWMOonly    = true;
        ResetParentSize = true;
        ThreadAlive     = false;
    }

    // Build an array of available minimaps and maps //
    public static void CompileMapList(uint WdtFileDataId, CASCHandler Handler = null)
    {
        if (Handler != null)
            CascHandler = Handler;

        for (var x = 0; x < 64; x++)
        {
            for (var y = 0; y < 64; y++)
            {
                MinimapData.mapAvailability[x, y].WDT = WDT.Flags[WdtFileDataId].HasADT[x, y];

                var MiniMapTexture = WDT.WDTEntries[(x, y)].MiniMapTexture;
                var RootAdt        = WDT.WDTEntries[(x, y)].RootADT;
                
                if (MiniMapTexture != 0)
                {
                    MinimapData.mapAvailability[x, y].Minimap = true;
                    Minimap.MinimapRequest request = new Minimap.MinimapRequest();
                    request.coords = new Vector2(x, y);
                    request.fileDataId = MiniMapTexture;

                    RequestBlock(request);
                }
                else
                    MinimapData.mapAvailability[x, y].Minimap = false;

                if (RootAdt != 0)
                    MinimapData.mapAvailability[x, y].ADT = true;
                else
                    MinimapData.mapAvailability[x, y].ADT = false;
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

    // Request a minimap image from the parser //
    private static void RequestBlock(Minimap.MinimapRequest minimapRequest)
    {
        uint fileDataId = minimapRequest.fileDataId;
        using (Stream stream = CascHandler.OpenFile(fileDataId))
        {
            BLP blp                    = new BLP();
            byte[] data                = blp.GetUncompressed(stream, true);
            BLPinfo info               = blp.Info();
            MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
            blockData.fileDataId       = fileDataId;
            blockData.coords           = minimapRequest.coords;
            blockData.textureInfo      = info;
            blockData.minimapByteData  = data;

            MinimapData.MinimapDataQueue.Enqueue(blockData);
        }
    }

    private static void EnqueueEmptyBlock(Minimap.MinimapRequest minimapRequest)
    {
        uint fileDataId             = minimapRequest.fileDataId;
        MinimapData.MinimapBlockData blockData = new MinimapData.MinimapBlockData();
        blockData.fileDataId        = fileDataId;
        blockData.coords            = minimapRequest.coords;
        blockData.minimapByteData   = null;
        MinimapData.MinimapDataQueue.Enqueue(blockData);
    }
}
