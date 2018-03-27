using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Collections;

public static partial class WMO
{
    /*
 * Helper Methods for reading byte data
 */

    private static string ReadFourCC(Stream stream) // 4 byte to 4 chars
    {
        string str = "";
        for (int i = 1; i <= 4; i++)
        {
            int b = stream.ReadByte();
            try
            {
                var s = System.Convert.ToChar(b);
                if (s != '\0')
                {
                    str = s + str;
                }
            }
            catch
            {
                Debug.Log("Couldn't convert Byte to Char: " + b);
            }
        }
        return str;
    }

    public static string ReadNullTerminatedString(Stream stream)
    {
        StringBuilder sb = new StringBuilder();

        char c;
        while ((c = System.Convert.ToChar(stream.ReadByte())) != 0)
        {
            sb.Append(c);
        }

        return sb.ToString();
    }


    private static uint ReadUint(Stream stream) // 2 bytes to int
    {
        byte[] bytes = new byte[2];
        uint value;
        bytes[0] = (byte)stream.ReadByte();
        bytes[1] = (byte)stream.ReadByte();
        value = System.BitConverter.ToUInt16(bytes, 0);
        return value;
    }

    private static int ReadShort(Stream stream) // 2 bytes to int
    {
        byte[] bytes = new byte[2];
        int value;
        bytes[0] = (byte)stream.ReadByte();
        bytes[1] = (byte)stream.ReadByte();
        value = System.BitConverter.ToInt16(bytes, 0);
        return value;
    }

    private static uint ReadUintLong(Stream stream) // 4 bytes to int
    {
        byte[] bytes = new byte[4];
        uint value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToUInt32(bytes, 0);
        return value;
    }

    private static int ReadLong(Stream stream) // 4 bytes to int
    {
        byte[] bytes = new byte[4];
        int value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToInt32(bytes, 0);
        return value;
    }

    private static float ReadFloat(Stream stream) // 4 bytes to float
    {
        byte[] bytes = new byte[4];
        float value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToSingle(bytes, 0);
        return value;
    }

    private static BoundingBox ReadBoundingBox (Stream stream)
    {
        BoundingBox box = new BoundingBox();
        box.min = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream));
        box.max = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream));
        return box;
    }

    private static ARGB ReadARGB (Stream stream)
    {
        ARGB argbColor = new ARGB();                              
        argbColor.A = (byte)stream.ReadByte();
        argbColor.R = (byte)stream.ReadByte();
        argbColor.G = (byte)stream.ReadByte();
        argbColor.B = (byte)stream.ReadByte();
        return argbColor;
    }

    private static RGBA ReadRGBA (Stream stream)
    {
        RGBA RGBAColor = new RGBA();                         
        RGBAColor.R = (int)stream.ReadByte();
        RGBAColor.G = (int)stream.ReadByte();
        RGBAColor.B = (int)stream.ReadByte();
        RGBAColor.A = (int)stream.ReadByte();
        return RGBAColor;
    }

    private static BGRA ReadBGRA(Stream stream)
    {
        BGRA BGRAColor = new BGRA();
        BGRAColor.B = (byte)stream.ReadByte();
        BGRAColor.G = (byte)stream.ReadByte();
        BGRAColor.R = (byte)stream.ReadByte();
        BGRAColor.A = (byte)stream.ReadByte();
        return BGRAColor;
    }

    private static MaterialFlags ReadMaterialFlags (Stream stream)
    {
        byte[] arrayOfBytes = new byte[4];
        stream.Read(arrayOfBytes, 0, 4);
        BitArray flags = new BitArray(arrayOfBytes);
        MaterialFlags materialFlags = new MaterialFlags();
        materialFlags.F_UNLIT = flags[0];      // disable lighting logic in shader (but can still use vertex colors)
        materialFlags.F_UNFOGGED = flags[1];    // disable fog shading (rarely used)
        materialFlags.F_UNCULLED = flags[2];     // two-sided
        materialFlags.F_EXTLIGHT = flags[3];     // darkened, the intern face of windows are flagged 0x08
        materialFlags.F_SIDN = flags[4];         // (bright at night, unshaded) (used on windows and lamps in Stormwind, for example) (see emissive color)
        materialFlags.F_WINDOW = flags[5];       // lighting related (flag checked in CMapObj::UpdateSceneMaterials)
        materialFlags.F_CLAMP_S = flags[6];      // tex clamp S (force this material's textures to use clamp s addressing) //TextureWrappingClamp
        materialFlags.F_CLAMP_T = flags[7];      // tex clamp T (force this material's textures to use clamp t addressing) //TextureWrappingRepeat
        materialFlags.flag_0x100 = flags[8];

        return materialFlags;
    }

    private static FogDefinition ReadFogDefinition(Stream stream)
    {
        FogDefinition fogDefinition = new FogDefinition();
        fogDefinition.end = ReadFloat(stream);
        fogDefinition.start_scalar = ReadFloat(stream);
        fogDefinition.color = ReadBGRA(stream);
        return fogDefinition;
    }

    private static GroupFlags ReadGroupFlags(Stream stream)
    {
        byte[] arrayOfBytes = new byte[4];
        stream.Read(arrayOfBytes, 0, 4);
        BitArray flags = new BitArray(arrayOfBytes);

        GroupFlags groupFlags = new GroupFlags();
        groupFlags.HasBSPtree = flags[0]; // (MOBN and MOBR chunk).
        groupFlags.Haslightmap = flags[1]; // (MOLM, MOLD). (UNUSED: 20740) possibly: subtract mohd.color in mocv fixing  // SubtractAmbientColour
        groupFlags.Hasvertexolors = flags[2]; // (MOCV chunk).
        groupFlags.SMOGroupEXTERIOR = flags[3]; // -- Outdoor - also influences how doodads are culled // is outdoors
        groupFlags.SMOGroupEXTERIOR_LIT = flags[6]; // -- "Do not use local diffuse lightning". Applicable for both doodads from this wmo group(color from MODD) and water(CWorldView::GatherMapObjDefGroupLiquids). 
        groupFlags.SMOGroupUNREACHABLE = flags[7];
        groupFlags.Haslights = flags[9]; // (MOLR chunk)
        groupFlags.SMOGroupLOD = flags[10]; // Legion+? Also load for LoD != 0 (_lod* groups)
        groupFlags.Hasdoodads = flags[11]; // (MODR chunk)
        groupFlags.SMOGroupLIQUIDSURFACE = flags[12]; // -- Has water(MLIQ chunk)
        groupFlags.SMOGroupINTERIOR = flags[13]; // -- Indoor
        groupFlags.SMOGroupALWAYSDRAW = flags[16]; // -- clear 0x8 after CMapObjGroup::Create() in MOGP and MOGI // AlwaysDrawEvenIfOutdoors
        groupFlags.HasMORIandMORBchunks = flags[17]; // Has triangle strips
        groupFlags.Showskybox = flags[18]; // -- automatically unset if MOSB not present.
        groupFlags.is_not_water_but_ocean = flags[19]; // LiquidType related, see below in the MLIQ chunk. // is oceanic water
        groupFlags.IsMountAllowed = flags[21];
        groupFlags.SMOGroupCVERTS2 = flags[24]; // Has two MOCV chunks: Just add two or don't set 0x4 to only use cverts2. // has two vertex shading sets
        groupFlags.SMOGroupTVERTS2 = flags[25]; // Has two MOTV chunks: Just add two. // has two texture coordinate sets
        groupFlags.SMOGroupANTIPORTAL = flags[26]; // Just call CMapObjGroup::CreateOccluders() independent of groupname being "antiportal". requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE. // create occlusion without clearing bsp
        groupFlags.unknown = flags[27]; // requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE. // occlusion related
        groupFlags.SMOGroupEXTERIOR_CULL = flags[29];
        groupFlags.SMOGroupTVERTS3 = flags[30]; // Has three MOTV chunks, eg. for MOMT with shader 18.
        // 1 unused flag follows
        return groupFlags;
    }

    private static TriangleMaterialFlags ReadTriangleMaterialFlags(Stream stream)
    {
        byte[] arrayOfBytes = new byte[1];
        stream.Read(arrayOfBytes, 0, 1);
        BitArray flags = new BitArray(arrayOfBytes);

        TriangleMaterialFlags triangleMaterialFlags = new TriangleMaterialFlags();

        triangleMaterialFlags.Unknown1 = flags[0]; // F_UNK_0x01
        triangleMaterialFlags.NoCameraCollide = flags[1]; // F_NOCAMCOLLIDE 
        triangleMaterialFlags.Detail = flags[2]; // F_DETAIL 
        triangleMaterialFlags.HasCollision = flags[3]; // F_COLLISION 
        triangleMaterialFlags.Hint = flags[4]; // F_HINT 
        triangleMaterialFlags.Render = flags[5]; // F_RENDER 
        triangleMaterialFlags.Unknown2 = flags[6]; // F_UNK_0x40 
        triangleMaterialFlags.CollideHit = flags[7]; // F_COLLIDE_HIT 

        if (flags[0] && (flags[2] || flags[5]))
            triangleMaterialFlags.isTransFace = true; // { return F_UNK_0x01 && (F_DETAIL || F_RENDER); }
        else
            triangleMaterialFlags.isTransFace = false;

        triangleMaterialFlags.isColor = !flags[3]; // { return !F_COLLISION; }

        if (flags[5] && !flags[2])
            triangleMaterialFlags.isRenderFace = true; // { return F_RENDER && !F_DETAIL; }
        else
            triangleMaterialFlags.isRenderFace = false;

        if (flags[3] || triangleMaterialFlags.isRenderFace)
            triangleMaterialFlags.isCollidable = true; // { return F_COLLISION || isRenderFace(); }
        else
            triangleMaterialFlags.isCollidable = false;

        return triangleMaterialFlags;
    }
}

