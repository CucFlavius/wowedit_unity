using Assets.Data.WoW_Format_Parsers.WMO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public static class ADTObjData
    {

        public static Queue<ModelBlockData> ModelBlockDataQueue = new Queue<ModelBlockData>();
        public static ModelBlockData modelBlockData = new ModelBlockData();

        public class ModelBlockData
        {
            // Terrain Positioning //
            public Vector2 terrainPos = new Vector2();

            // WMO data //
            public Dictionary<uint, uint> WMOPathFDIDs  = new Dictionary<uint, uint>();
            public Dictionary<uint, string> WMOPaths    = new Dictionary<uint, string>();
            public List<uint> WMOOffsets                = new List<uint>();
            public List<WMOPlacementInfo> WMOInfo       = new List<WMOPlacementInfo>();

            // M2 data //
            public Dictionary<uint, uint> M2PathFDIDs   = new Dictionary<uint, uint>();
            public Dictionary<uint, string> M2Paths     = new Dictionary<uint, string>();
            public List<uint> M2Offsets                 = new List<uint>();
            public List<M2PlacementInfo> M2Info         = new List<M2PlacementInfo>();
        }

        public class M2PlacementInfo
        {
            public uint nameId;
            public int uniqueID;
            public Vector3 position;
            public Quaternion rotation;
            public float scale;
            public Flags.MDDFFlags flags;
        }

        public class WMOPlacementInfo
        {
            public uint nameId;
            public int uniqueID;
            public Vector3 position;
            public Quaternion rotation;
            public BoundingBox extents;
            public Flags.MODFFlags flags;
            public int doodadSet;
            public int nameSet;
            public float Scale;
        }
    }
}