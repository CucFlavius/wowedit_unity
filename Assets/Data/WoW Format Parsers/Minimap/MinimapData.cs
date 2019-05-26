using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinimapData
{
    public static Queue<MinimapBlockData> MinimapDataQueue = new Queue<MinimapBlockData>();
    public static MapAvailability[,] mapAvailability = new MapAvailability[64,64];
    public static Vector2 Min;
    public static Vector2 Max;
    public static int Total = 0;

    public struct MapAvailability
    {
        public bool WDT;
        public bool Minimap;
        public bool ADT;
    }

    public struct MinimapBlockData
    {
        public uint fileDataId;
        public Vector2 coords;
        public byte[] minimapByteData;
        public BLPinfo textureInfo;
    } 
}




