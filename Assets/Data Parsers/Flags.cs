using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Flags
{
    ///////////////////////////////////
    #region Flag Structs

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

    #endregion
    ///////////////////////////////////


    ///////////////////////////////////
    #region Flag Readers

    public Flags.MCNKflags ReadMCNKflags(MemoryStream stream)
    {
        Flags.MCNKflags mcnkFlags = new Flags.MCNKflags();
        // <Flags> 4 bytes
        byte[] arrayOfBytes = new byte[4];
        stream.Read(arrayOfBytes, 0, 4);
        BitArray flags = new BitArray(arrayOfBytes);
        mcnkFlags.has_mcsh = flags[0]; // if ADTtex has MCSH chunk
        mcnkFlags.impass = flags[1];
        mcnkFlags.lq_river = flags[2];
        mcnkFlags.lq_ocean = flags[3];
        mcnkFlags.lq_magma = flags[4];
        mcnkFlags.lq_slime = flags[5];
        mcnkFlags.has_mccv = flags[6];
        mcnkFlags.unknown_0x80 = flags[7];
        mcnkFlags.do_not_fix_alpha_map = flags[15];  // "fix" alpha maps in MCAL (4 bit alpha maps are 63*63 instead of 64*64).
                                                     // Note that this also means that it *has* to be 4 bit alpha maps, otherwise UnpackAlphaShadowBits will assert.
        mcnkFlags.high_res_holes = flags[16];  // Since ~5.3 WoW uses full 64-bit to store holes for each tile if this flag is set.
        return mcnkFlags;
    }

    public TerrainTextureFlag ReadTerrainTextureFlag(Stream stream)
    {
        byte[] bytes = new byte[4];
        uint value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToUInt32(bytes, 0);
        return (TerrainTextureFlag)value;
    }

    public MDDFFlags ReadMDDFFlags(Stream stream)
    {
        byte[] arrayOfBytes = new byte[2];
        stream.Read(arrayOfBytes, 0, 2);
        BitArray flags = new BitArray(arrayOfBytes);
        MDDFFlags MDDFflags = new MDDFFlags();
        MDDFflags.mddf_biodome = flags[0];              // this sets internal flags to | 0x800 (WDOODADDEF.var0xC).
        MDDFflags.mddf_shrubbery = flags[1];            // the actual meaning of these is unknown to me. maybe biodome is for really big M2s.
                                                        // 6.0.1.18179 seems not to check for this flag
        MDDFflags.mddf_unk_4 = flags[2];                // Legion+ᵘ
        MDDFflags.mddf_unk_8 = flags[3];                // Legion+ᵘ
        MDDFflags.Flag_liquidKnown = flags[5];          // Legion+ᵘ // SMDoodadDef::Flag_liquidKnown
        MDDFflags.mddf_entry_is_filedata_id = flags[6]; // Legion+ᵘ nameId is a file data id to directly load
        MDDFflags.mddf_unk_100 = flags[8];              // Legion+ᵘ
        return MDDFflags;
    }

    public MODFFlags ReadMODFFlags(Stream stream)
    {
        byte[] arrayOfBytes = new byte[2];
        stream.Read(arrayOfBytes, 0, 2);
        BitArray flags = new BitArray(arrayOfBytes);
        MODFFlags MODFflags = new MODFFlags();
        MODFflags.modf_destroyable = flags[0];          // set for destroyable buildings like the tower in DeathknightStart. This makes it a server-controllable game object.
        MODFflags.modf_use_lod = flags[1];              // WoD(?)+: also load _LOD1.WMO for use dependent on distance
        MODFflags.modf_unk_4 = flags[2];                // Legion(?)+: unknown
        MODFflags.modf_entry_is_filedata_id = flags[3]; // Legion+: nameId is a file data id to directly load //SMMapObjDef::FLAG_FILEDATAID
        return MODFflags;
    }

    #endregion
    ///////////////////////////////////
}

