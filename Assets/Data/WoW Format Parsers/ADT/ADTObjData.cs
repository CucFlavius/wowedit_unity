using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ADTObjData {

    public static Queue<ModelBlockData> ModelBlockDataQueue = new Queue<ModelBlockData>();
    public static ModelBlockData modelBlockData = new ModelBlockData();

    public class ModelBlockData
    {
        // Terrain Positioning //
        public Vector2 terrainPos = new Vector2();

        // WMO data //
        public Dictionary<int, string> WMOPaths = new Dictionary<int, string>();
        public List<int> WMOOffsets = new List<int>();
        public List<WMOPlacementInfo> WMOInfo = new List<WMOPlacementInfo>();

        // M2 data //
        public Dictionary<int, string> M2Paths = new Dictionary<int, string>();
        public List<int> M2Offsets = new List<int>();
        public List<M2PlacementInfo> M2Info = new List<M2PlacementInfo>();
    }

    public class M2PlacementInfo
    {
        public int nameID;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public float scale;
        public Flags.MDDFFlags flags;
    }

    public class WMOPlacementInfo
    {
        public int nameID;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public BoundingBox extents;
        public Flags.MODFFlags flags;
        public int doodadSet;
        public int nameSet;
    }
}
