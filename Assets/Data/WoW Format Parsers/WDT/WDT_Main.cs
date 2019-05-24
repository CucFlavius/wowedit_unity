using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT {

    private static void ReadMVER(BinaryReader br)
    {
        uint WDTfileversion = br.ReadUInt32();
    }

    private static void ReadMPHD (BinaryReader br, WDTflagsdata WDTflags)
    {
        byte[] arrayOfBytes = new byte[4];
        br.Read(arrayOfBytes, 0, 4);
        BitArray flags = new BitArray(arrayOfBytes);

        // <flags>
        bool wdt_uses_global_map_obj                = flags[0]; // Use global map object definition.
        bool adt_has_mccv                           = flags[1]; // ≥ Wrath adds color: ADT.MCNK.MCCV. with this flag every ADT in the map _must_ have 
                                                                // MCCV chunk at least with default values, else only base texture layer is rendered on such ADTs.
        WDTflags.adt_has_big_alpha                  = flags[2]; // shader = 2. Decides whether to use _env terrain shaders or not: funky and if MCAL has 4096 instead of 2048(?)
        bool adt_has_doodadrefs_sorted_by_size_cat  = flags[3]; // if enabled, the ADT's MCRF(m2 only)/MCRD chunks need to be sorted by size category
        bool adt_has_mclv                           = flags[4]; // ≥ Cata adds second color: ADT.MCNK.MCLV
        bool adt_has_upside_down_ground             = flags[5]; // ≥ Cata Flips the ground display upside down to create a ceiling
        bool unk_0x0040                             = flags[6]; // ≥ Mists ??? -- Only found on Firelands2.wdt (but only since MoP) before Legion
        WDTflags.adt_has_height_texturing           = flags[7]; // ≥ Mists shader = 6. Decides whether to influence alpha maps by _h+MTXP: (without with)
                                                                // also changes MCAL size to 4096 for uncompressed entries
        bool unk_0x0100                             = flags[8]; // ≥ Legion implicitly sets 0x8000
        bool unk_0x0200                             = flags[9];
        bool unk_0x0400                             = flags[10];
        bool unk_0x0800                             = flags[11];
        bool unk_0x1000                             = flags[12];
        bool unk_0x2000                             = flags[13];
        bool unk_0x4000                             = flags[14];
        bool unk_0x8000                             = flags[15]; // ≥ Legion implicitly set for map ids 0, 1, 571, 870, 1116 (continents). Affects the rendering of _lod.adt
        bool mask_vertex_buffer_format              = flags[16]; // = adt_has_mccv | adt_has_mclv,                    // CMap::LoadWdt
        bool mask_render_chunk_something            = flags[17]; // = adt_has_height_texturing | adt_has_big_alpha,   // CMapArea::PrepareRenderChunk, CMapChunk::ProcessIffChunks

        uint lgtFileDataID = br.ReadUInt32();
        uint occFileDataID = br.ReadUInt32();
        uint fogsFileDataID = br.ReadUInt32();
        uint mpvFileDataID = br.ReadUInt32();
        uint texFileDataID = br.ReadUInt32();
        uint wdlFileDataID = br.ReadUInt32();
        uint pd4FileDataID = br.ReadUInt32();
    }

    private static void ReadMAIN (BinaryReader br, WDTflagsdata WDTflags)
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                // 8 bytes //
                byte[] arrayOfBytes = new byte[4];
                br.Read(arrayOfBytes, 0, 4);
                BitArray flags      = new BitArray(arrayOfBytes);

                // <flags>
                WDTflags.HasADT[x,y]    = flags[0]; // flags 0 and 1 always alternate, when one is true other is false.
                bool Flag_AllWater      = flags[1];
                bool Flag_Loaded        = flags[2]; // always false (only set during runtime?)
                // </flags>

                uint asyncId = br.ReadUInt32();
            }
        }
    }

    private static void ReadMAID (BinaryReader br)
    {
        for (int i = 0; i < 128; i++)
        {
            WDTEntry Entry = new WDTEntry()
            {
                RootADT         = br.ReadUInt32(),
                OBJ0ADT         = br.ReadUInt32(),
                OBJ1ADT         = br.ReadUInt32(),
                TEX0ADT         = br.ReadUInt32(),
                LODADT          = br.ReadUInt32(),
                MapTexture      = br.ReadUInt32(),
                MapTextureN     = br.ReadUInt32(),
                MiniMapTexture  = br.ReadUInt32(),
            };

            WDTEntries.Add(Entry);
        }
    }
}
