using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class WMO
{
    public static Queue<WMOData> AllWMOData = new Queue<WMOData>();
    public static WMOData wmoData = new WMOData();

    public struct WMOData
    {
        // object //
        public string dataPath;
        public int uniqueID;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        // root //
        public HeaderData Info;
        public Dictionary<int, string> texturePaths; // offset -> dataPath
        public Dictionary<string, Texture2Ddata> textureData; // dataPath -> Texture2Ddata
        public Dictionary<int, string> MOGNgroupnames; // offset -> name
        public List<WMOMaterial> materials;

        // groups //
        public List<GroupData> groupsData;
    }
}

public struct WMOMaterial
{
    public MaterialFlags flags;
    public int shader;
    public BlendingMode blendMode;
    public int texture1_offset;
    public RGBA color1;
    public MaterialFlags texture1_flags;
    public int texture2_offset;
    public RGBA color2;
    public int ground_type;
    public int texture3_offset;
    public RGBA color3;
    public MaterialFlags texture3_flags;
}

public struct GroupData
{
    // header data //
    public string groupName;
    public string descriptiveGroupName;
    public GroupFlags flags;
    public BoundingBox boundingBox;
    public int portalStart;
    public int portalCount;
    public int transBatchCount;
    public int intBatchCount;
    public int extBatchCount;
    public List<int> fogIds;
    public int groupLiquid;
    public int uniqueID;

    // materials //
    public List<TriangleMaterialFlags> MOPYtriangleMaterialFlags;
    public List<int> MOPYtriangleMaterialIndex;
    public List<int> batchMaterialIDs;

    // mesh //
    public uint[] triangles;
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] UVs;
    public int nBatches;
    public List<uint> batch_StartIndex;
    public List<uint> batch_nIndices;
    public List<uint> batch_StartVertex;
    public List<uint> batch_EndVertex;
    public List<Color32> vertexColors;
}


public struct BoundingBox
{
    public Vector3 min;
    public Vector3 max;
}

public struct ARGB
{
    public byte A;
    public byte R;
    public byte G;
    public byte B;
}

public struct BGRA
{
    public byte B;
    public byte G;
    public byte R;
    public byte A;
}

public struct RGBA
{
    public int R;
    public int G;
    public int B;
    public int A;
}

public struct HeaderData
{
    public int nTextures;
    public int nGroups;
    public int nPortals;
    public int nLights;
    public int nMaterials;
    public int wmoID;
}

public struct Texture2Ddata
{
    public byte[] TextureData;
    public int width;
    public int height;
    public bool hasMipmaps;
    public TextureFormat textureFormat;
}

public struct MaterialFlags
{
    public bool F_UNLIT;      // disable lighting logic in shader (but can still use vertex colors)
    public bool F_UNFOGGED;    // disable fog shading (rarely used)
    public bool F_UNCULLED;     // two-sided
    public bool F_EXTLIGHT;     // darkened, the intern face of windows are flagged 0x08
    public bool F_SIDN;         // (bright at night, unshaded) (used on windows and lamps in Stormwind, for example) (see emissive color)
    public bool F_WINDOW;       // lighting related (flag checked in CMapObj::UpdateSceneMaterials)
    public bool F_CLAMP_S;      // tex clamp S (force this material's textures to use clamp s addressing) //TextureWrappingClamp
    public bool F_CLAMP_T;      // tex clamp T (force this material's textures to use clamp t addressing) //TextureWrappingRepeat
    public bool flag_0x100;
    // unused 23 remaining flags;   // unused as of 7.0.1.20994
}

public enum WMOFragmentShader
{
    Diffuse = 0,                    // Simple diffuse shading
    Specular = 1,                   // Specularity shading
    Metal = 2,                      // Metallic shading
    Env = 3,                        // Environment mapped shading
    Opaque = 4,                     // Opaque shading
    EnvMetal = 5,                   // Environment mapped metallic shading
    TwoLayerDiffuse = 6,            // Two-layer diffuse shading
    TwolayerEnvMetal = 7,           // Two-layer environment mapped metallic shading
    TwoLayerTerrain = 8,            // Two-layer terrain shading
    DiffuseEmissive = 9,            // Emissive diffuse shading
    WaterWindow = 10,               // Water window shading
    MaskedEnvMetal = 11,            // Masked environment mapped metallic shading
    EnvMetalEmissive = 12,          // Environment mapped emissive metallic shading
    TwoLayerDiffuseOpaque = 13,     // Two-layer diffuse opaque shading
    TwoLayerDiffuseEmissive = 14,   // Two-layer diffuse emissive shading
    DiffuseTerrain = 16,            // Diffuse terrain shading
    AdditiveMaskedEnvMetal = 17     // Additive masked environment mapped metallic shading
}

public enum BlendingMode : ushort
{
    Opaque = 0, //
    AlphaKey = 1,
    Alpha = 2,
    Additive = 3,
    Modulate = 4, //
    Modulate2x = 5, //
    ModulateAdditive = 6, //
    InvertedSourceAlphaAdditive = 7,
    InvertedSourceAlphaOpaque = 8,
    SourceAlphaOpaque = 9,
    NoAlphaAdditive = 10,
    ConstantAlpha = 11,
    Screen = 12,
    BlendAdditive = 13
}

struct C4Plane
{
    public Vector3 normal;
    public float distance;
}

struct SMOPortal
{
    public int startVertex;
    public int count;
    public C4Plane plane;
}

struct VisibleBlock
{
    public int firstVertex;
    public int count;
}

enum LightType
{
    OmniLight = 0,
    SpotLight = 1,
    DirectLight = 2,
    AmbientLight = 3,
}

struct StaticLight
{
    public LightType type;
    public int useAttenuation;
    public int useUnknown1;
    public int useUnknown2;
    public BGRA color;
    public Vector3 position;
    public float intensity;
    public float attenuationStart;
    public float attenuationEnd;
    public float unknown1StartRadius;
    public float unknown1EndRadius;
    public float unknown2StartRadius;
    public float unknown2EndRadius;
}

struct DoodadSet
{
    public string name;
    public int firstDoodadInstanceIndex;
    public int doodadInstanceCount;
}

struct DoodadInstanceFlags
{
    public bool AcceptProjectedTexture;
    public bool Unknown1;
    public bool Unknown2;
    public bool Unknown3;
}

struct DoodadInstanceInfo
{
    public uint nameOffset;
    public DoodadInstanceFlags flags;
    public Vector3 position;
    public Quaternion orientation;
    public float scale;
    public BGRA staticLightingColor;
}

struct FogInstance
{
    public FogFlags flags;
    public Vector3 position;
    public float smaller_radius;
    public float larger_radius;
    public FogDefinition landFog;
    public FogDefinition underwaterFog;
}

struct FogFlags
{
    public bool flag_infinite_radius;
    public bool flag_0x10;
}

struct FogDefinition
{
    public float end; // endRadius
    public float start_scalar; // StartMultiplier  (0..1) -- minimum distance is end * 
    public BGRA color; // The back buffer is also cleared to this colour
}

public struct GroupFlags
{
    public bool HasBSPtree; // (MOBN and MOBR chunk).
    public bool Haslightmap; // (MOLM, MOLD). (UNUSED: 20740) possibly: subtract mohd.color in mocv fixing 
    public bool Hasvertexolors; // (MOCV chunk).
    public bool SMOGroupEXTERIOR; // -- Outdoor - also influences how doodads are culled
    public bool SMOGroupEXTERIOR_LIT; // -- "Do not use local diffuse lightning". Applicable for both doodads from this wmo group(color from MODD) and water(CWorldView::GatherMapObjDefGroupLiquids). 
    public bool SMOGroupUNREACHABLE;
    public bool Haslights; // (MOLR chunk)
    public bool SMOGroupLOD; // Legion+? Also load for LoD != 0 (_lod* groups)
    public bool Hasdoodads; // (MODR chunk)
    public bool SMOGroupLIQUIDSURFACE; // -- Has water(MLIQ chunk)
    public bool SMOGroupINTERIOR; // -- Indoor
    public bool SMOGroupALWAYSDRAW; // -- clear 0x8 after CMapObjGroup::Create() in MOGP and MOGI
    public bool HasMORIandMORBchunks;
    public bool Showskybox; // -- automatically unset if MOSB not present.
    public bool is_not_water_but_ocean; // LiquidType related, see below in the MLIQ chunk.
    public bool IsMountAllowed;
    public bool SMOGroupCVERTS2; // Has two MOCV chunks: Just add two or don't set 0x4 to only use cverts2.
    public bool SMOGroupTVERTS2; // Has two MOTV chunks: Just add two.
    public bool SMOGroupANTIPORTAL; // Just call CMapObjGroup::CreateOccluders() independent of groupname being "antiportal". requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE.
    public bool unknown; // requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE.
    public bool SMOGroupEXTERIOR_CULL;
    public bool SMOGroupTVERTS3; // Has three MOTV chunks, eg. for MOMT with shader 18.
    //1 unknown flag follows
}

public struct TriangleMaterialFlags
{
    public bool Unknown1; // F_UNK_0x01
    public bool NoCameraCollide; // F_NOCAMCOLLIDE 
    public bool Detail; // F_DETAIL 
    public bool HasCollision; // F_COLLISION 
    public bool Hint; // F_HINT 
    public bool Render; // F_RENDER 
    public bool Unknown2; // F_UNK_0x40 
    public bool CollideHit; // F_COLLIDE_HIT 

    public bool isTransFace; // { return F_UNK_0x01 && (F_DETAIL || F_RENDER); }
    public bool isColor; // { return !F_COLLISION; }
    public bool isRenderFace; // { return F_RENDER && !F_DETAIL; }
    public bool isCollidable; // { return F_COLLISION || isRenderFace(); }
}
