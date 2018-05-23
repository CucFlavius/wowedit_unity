using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class M2
{
    public static Queue<M2Data> AllM2Data = new Queue<M2Data>();
    public static string[] KeyBoneLookupList = {"ArmL","ArmR", "ShoulderL", "ShoulderR", "SpineLow", "Waist", "Head", "Jaw", "IndexFingerR",
            "MiddleFingerR", "PinkyFingerR", "RingFingerR", "ThumbR", "IndexFingerL", "MiddleFingerL", "PinkyFingerL", "RingFingerL",
            "ThumbL", "$BTH", "$CSR", "$CSL", "_Breath", "_Name", "_NameMount", "$CHD", "$CCH", "Root", "Wheel1", "Wheel2", "Wheel3",
            "Wheel4", "Wheel5", "Wheel6", "Wheel7", "Wheel8"};

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
        public List<SubmeshData> submeshData = new List<SubmeshData>();

        // Material //
        public List<M2BatchIndices> m2BatchIndices = new List<M2BatchIndices>();

        // Texture //
        public List<M2Texture> m2Tex = new List<M2Texture>();
        public List<int> textureLookupTable = new List<int>();

        // Bones //
        public List<M2CompBone> m2CompBone = new List<M2CompBone>();
        public List<int> key_bone_lookup = new List<int>();
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

    public class SubmeshData
    {
        public int ID;
        public Vector3[] vertList;
        public Vector3[] normsList;
        public Vector2[] uvsList;
        public Vector2[] uvs2List;
        public int[] triList;
    }

    /*
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
    */
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
    public int Interpolationtype;
    public int GlobalSequenceID;
    public M2Array Timestamps;
    public M2Array Values;
}

public class M2Material
{
    public List<UInt16> flags = new List<UInt16>();
    public List<UInt16> blending_mode = new List<UInt16>();
}

public class M2Texture
{
    public int type;
    public int flags;
    public string filename;
    public Texture2Ddata texture2Ddata;
}

public class M2BatchIndices
{
    public int M2Batch_flags;
    public int M2Batch_shader_id;
    public int M2Batch_submesh_index;
    public int M2Batch_submesh_index2;
    public int M2Batch_color_index;
    public int M2Batch_render_flags;
    public int M2Batch_layer;
    public int M2Batch_op_count;
    public int M2Batch_texture;
    public int M2Batch_tex_unit_number2;
    public int M2Batch_transparency;
    public int M2Batch_texture_anim;
}

public class M2CompBone
{
    public int key_bone_id;
    public int flags;
    public int parent_bone;
    public int submesh_id;
    public int uDistToFurthDesc;
    public int uZRatioOfChain;
    public Vector3 translation;
    public Quaternion rotation;
    public Vector3 scale;
    public Vector3 pivot;
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

public enum InterpolationType : ushort
{
    None = 0,
    Linear = 1,
    Hermite = 2,
    Bezier = 3
}