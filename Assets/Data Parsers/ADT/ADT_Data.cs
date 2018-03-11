using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ADT {

    // block queue //
    public static Queue<BlockDataType> AllBlockData = new Queue<BlockDataType>();

    // block buffer //
    public static BlockDataType blockData = new BlockDataType();

    // data structures //
    public class BlockDataType
    {
        // Per chunk data //
        public List<ChunkData> ChunksData = new List<ChunkData>();

        // Terrain textures //
        public List<string> terrainTexturePaths = new List<string>();
        public Dictionary<string, Texture2Ddata> terrainTextures = new Dictionary<string, Texture2Ddata>();

        // MTXP data //
        public bool MTXP = false;
        public Dictionary<string, TerrainTextureFlag> textureFlags = new Dictionary<string, TerrainTextureFlag>();
        public Dictionary<string, float> heightScales = new Dictionary<string, float>();
        public Dictionary<string, float> heightOffsets = new Dictionary<string, float>();
        public Dictionary<string, Texture2Ddata> terrainHTextures = new Dictionary<string, Texture2Ddata>();

        // WMO data //
        public Dictionary<int, string> WMOPaths = new Dictionary<int, string>();
        public List<int> WMOOffsets = new List<int>();
        public List<WMOPlacementInfo> WMOInfo = new List<WMOPlacementInfo>();

        // M2 data //
        public Dictionary<int, string> M2Paths = new Dictionary<int, string>();
        public List<int> M2Offsets = new List<int>();
        public List<M2PlacementInfo> M2Info = new List<M2PlacementInfo>();
    }

    public class ChunkData
    {
        // object properties //
        public MCNKflags flags;
        public int IndexX;
        public int IndexY;
        public int nLayers; // number of alpha layers
        public Vector3 MeshPosition;

        // mesh data //
        public List<float> VertexHeights = new List<float>();
        public Vector3[] VertexArray;
        public List<byte[]> VertexLighting = new List<byte[]>();
        public Color32[] VertexColors;
        public Vector3[] VertexNormals;
        public int[] TriangleArray;
        public Vector2[] UVArray;

        // texture data //
        public int NumberOfTextureLayers; // number of texture layers in this chunk 1=no alpha
        public int[] textureIds = new int[6]; // texture ID from terrainTextures per layer
        public bool[] alpha_map_compressed = new bool[4]; // flag that decides alpha compression per layer
        public int[] LayerOffsetsInMCAL = new int[4]; // offsets for each alpha layer data
        public List<byte[]> alphaLayers = new List<byte[]>(); // up to 3 x bytearrays per list

        // shadow map //
        public bool[] shadowMap = new bool[64 * 64];
        public byte[] shadowMapTexture = new byte[64 * 64];
    }

    public class Texture2Ddata
    {
        public byte[] TextureData;
        public int width;
        public int height;
        public bool hasMipmaps;
        public TextureFormat textureFormat;
    }

    public class M2PlacementInfo
    {
        public int nameID;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public float scale;
        public MDDFFlags flags;
    }

    public class WMOPlacementInfo
    {
        public int nameID;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public BoundingBox extents;
        public MODFFlags flags;
        public int doodadSet;
        public int nameSet;
    }

    public static void ClearBlockData ()
    {
        Debug.Log("Cleared buffer.");
        blockData.terrainTexturePaths.Clear();
        blockData.terrainTextures.Clear();
        blockData.ChunksData.Clear();
    }

    public enum ADTchunkID // blame Fabi
    {
        MVER = 0x4d564552,
        MHDR = 0x4d484452,
        MMID = 0x4d4d4944,
        MWMO = 0x4d574d4f,
        MWID = 0x4d574944,
        MDDF = 0x4d444446,
        MODF = 0x4d4f4446,
        MH2O = 0x4d48324f,
        MCNK = 0x4d434e4b,
        MCVT = 0x4d435654,
        MCLV = 0x4d434c56,
        MCCV = 0x4d434356,
        MCRF = 0x4d435246,
        MCRD = 0x4d435244,
        MCRW = 0x4d435257,
        MCLQ = 0x4d434c51,
        MCNR = 0x4d434e52,
        MCSE = 0x4d435345,
        MCBB = 0x4d434242,
        MCDD = 0x4d434444,
        MFBO = 0x4d46424f,
        MBMH = 0x4d424d48,
        MBBB = 0x4d424242,
        MBNV = 0x4d424e56,
        MBMI = 0x4d424d49,
        MTEX = 0x4d544558,
        MCLY = 0x4d434c59,
        MCSH = 0x4d435348,
        MCAL = 0x4d43414c,
        MCMT = 0x4d434d54,
        MTXF = 0x4d545846,
        MTXP = 0x4d545850,
        MAMP = 0x4d414d50,
        MLHD = 0x4d4c4844,
        MLVH = 0x4d4c5648,
        MLVI = 0x4d4c5649,
        MLLL = 0x4d4c4c4c,
        MLND = 0x4d4c4e44,
        MLSI = 0x4d4c5349,
        MLLD = 0x4d4c4c44,
        MLLN = 0x4d4c4c4e,
        MLLV = 0x4d4c4c56,
        MLLI = 0x4d4c4c49,
        MLMD = 0x4d4c4d44,
        MLMX = 0x4d4c4d58,
        MLDD = 0x4d4c4444,
        MLDX = 0x4d4c4458,
        MLDL = 0x4d4c444c,
        MLFD = 0x4d4c4644,
        MBMB = 0x4d424d42,
        MMDX = 0x4d4d4458
    }

    public enum TerrainTextureFlag : uint
    {
        do_not_load_specular_or_height_texture_but_use_cubemap = 1,
        Unknown = 3,
        texture_scale = 4,
        Unknown2 = 24
    }

    public struct MCNKflags
    {
        public bool has_mcsh; // if ADTtex has MCSH chunk
        public bool impass;
        public bool lq_river;
        public bool lq_ocean;
        public bool lq_magma;
        public bool lq_slime;
        public bool has_mccv;
        public bool unknown_0x80;
        public bool do_not_fix_alpha_map;  // "fix" alpha maps in MCAL (4 bit alpha maps are 63*63 instead of 64*64).
                                           // Note that this also means that it *has* to be 4 bit alpha maps, otherwise UnpackAlphaShadowBits will assert.
        public bool high_res_holes;  // Since ~5.3 WoW uses full 64-bit to store holes for each tile if this flag is set.
    }

    public struct MDDFFlags
    {
        public bool mddf_biodome;                   // this sets internal flags to | 0x800 (WDOODADDEF.var0xC).
        public bool mddf_shrubbery;                 // the actual meaning of these is unknown to me. maybe biodome is for really big M2s. 6.0.1.18179 seems not to check 
                                                    // for this flag
        public bool mddf_unk_4;                     // Legion+ᵘ
        public bool mddf_unk_8;                     // Legion+ᵘ
        public bool Flag_liquidKnown;               // Legion+ᵘ // SMDoodadDef::Flag_liquidKnown
        public bool mddf_entry_is_filedata_id;      // Legion+ᵘ nameId is a file data id to directly load
        public bool mddf_unk_100;                   // Legion+ᵘ
    }

    public struct MODFFlags
    {
        public bool modf_destroyable;         // set for destroyable buildings like the tower in DeathknightStart. This makes it a server-controllable game object.
        public bool modf_use_lod;             // WoD(?)+: also load _LOD1.WMO for use dependent on distance
        public bool modf_unk_4;               // Legion(?)+: unknown
        public bool modf_entry_is_filedata_id;// Legion+: nameId is a file data id to directly load //SMMapObjDef::FLAG_FILEDATAID
    }

}
