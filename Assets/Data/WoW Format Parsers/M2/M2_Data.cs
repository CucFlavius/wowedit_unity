using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class M2
{
    public static Queue<M2Data> AllM2Data = new Queue<M2Data>();

    public class M2Data
    {
        // Object //
        public string dataPath;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        // Mesh //
        public MeshData meshData = new MeshData();
    }

    public class MeshData
    {
        public List<Vector3> pos = new List<Vector3>();
        public List<float[]> bone_weights = new List<float[]>();
        public List<int[]> bone_indices = new List<int[]>();
        public List<Vector3> normal = new List<Vector3>();
        public List<Vector2> tex_coords = new List<Vector2>();
        public List<Vector2> tex_coords2 = new List<Vector2>();
    }

    public class M2SkinSection
    {
        public List<UInt16> skinSectionId = new List<UInt16>();
        public List<UInt16> Level = new List<UInt16>();
        public List<UInt16> vertexStart = new List<UInt16>();
        public List<UInt16> vertexCount = new List<UInt16>();
        public List<UInt16> indexStart = new List<UInt16>();
        public List<UInt16> indexCount = new List<UInt16>();
        public List<UInt16> boneCount = new List<UInt16>();
        public List<UInt16> boneComboIndex = new List<UInt16>();
        public List<UInt16> boneInfluences = new List<UInt16>();

        public List<UInt16> centerBoneIndex = new List<UInt16>();
        public List<Vector3> centerPosition = new List<Vector3>();

        // if ≥ BC
        public List<Vector3> sortCenterPosition = new List<Vector3>();
        public List<float> sortRadius = new List<float>();
    }
}

public struct M2Array
{
    public int size;
    public int offset;
}

public struct M2Bounds
{
    public BoundingBox extent;
    public float radius;
}

public struct M2TrackBase
{
    public UInt16 trackType;
    public UInt16 loopIndex;
    M2Array sequenceTimes;
}

public struct M2Material
{
    public UInt16 flags;
    public UInt16 blending_mode;
}

public struct M2Texture
{
    public int type;
    public int flags;
    public M2Array filename;
}

public enum M2RenderFlags
{
    unlit           = 0x001,
    unfogged        = 0x002,
    twoSided        = 0x004,
    depthTest       = 0x008,
    depthWrite      = 0x010,
    shadowBatch     = 0x040,
    shadowBatch_2   = 0x080,
    unk             = 0x400,
    alpha           = 0x800
}

public enum M2BlendingMode
{
    Disabled    = 0,
    Mod         = 1,
    Decal       = 2,
    Add         = 3,
    Mod2x       = 4,
    Fade        = 5,
    DeepRunTram = 6,
    WoDPlus     = 7
}