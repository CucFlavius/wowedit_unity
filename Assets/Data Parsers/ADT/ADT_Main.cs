using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT {

    // global flags //
    private static int MH2Ooffset;
    private static int MFBOoffset;
    private static List<bool> has_mcsh;

    private static void ReadMVER(Stream ADTstream)
    {
        int ADTfileversion = ReadLong(ADTstream);
    }

    private static void ReadMHDR(Stream ADTstream)
    {
        int flags = ReadLong(ADTstream);
        int mcin = ReadLong(ADTstream);  // Cata+: obviously gone. probably all offsets gone, except mh2o(which remains in root file).
        int mtex = ReadLong(ADTstream);
        int mmdx = ReadLong(ADTstream);
        int mmid = ReadLong(ADTstream);
        int mwmo = ReadLong(ADTstream);
        int mwid = ReadLong(ADTstream);
        int mddf = ReadLong(ADTstream);
        int modf = ReadLong(ADTstream);
        MFBOoffset = ReadLong(ADTstream); // this is only set if flags & mhdr_MFBO.
        MH2Ooffset = ReadLong(ADTstream);
        int mtxf = ReadLong(ADTstream);
        int mamp_value = ADTstream.ReadByte(); // Cata+, explicit MAMP chunk overrides data
        int[] padding = new int[3];
        padding[0] = ADTstream.ReadByte();
        padding[1] = ADTstream.ReadByte();
        padding[2] = ADTstream.ReadByte();
        int[] unused = new int[3];
        unused[0] = ReadLong(ADTstream);
        unused[1] = ReadLong(ADTstream);
        unused[2] = ReadLong(ADTstream);
    }

    private static void ReadMH2O(Stream ADTstream, int MH2Osize)
    {
        long chunkStartPosition = ADTstream.Position;

        // header - SMLiquidChunk
        for (int a = 0; a < 256; a++)
        {
            int offset_instances = ReadLong(ADTstream);       // points to SMLiquidInstance[layer_count]
            int layer_count = ReadLong(ADTstream);            // 0 if the chunk has no liquids. If > 1, the offsets will point to arrays.
            int offset_attributes = ReadLong(ADTstream);      // points to mh2o_chunk_attributes, can be ommitted for all-0
            if (offset_instances >= 0)
            {
                // instances @24bytes
                ADTstream.Seek(chunkStartPosition + offset_instances, SeekOrigin.Begin);
                int liquid_type = ReadShort(ADTstream); //DBC - foreign_keyⁱ<uint16_t, &LiquidTypeRec::m_ID> liquid_type;
                int liquid_object_or_lvf = ReadShort(ADTstream);    //DBC -  foreign_keyⁱ<uint16_t, &LiquidObjectRec::m_ID> liquid_object_or_lvf;        
                                                                    // if > 41, an id into DB/LiquidObject. If below, LiquidVertexFormat, used in ADT/v18#instance_vertex_data Note hardcoded LO ids below.
                                                                    // if >= 42, look up via DB/LiquidType and DB/LiquidMaterial, otherwise use liquid_object_or_lvf as LVF
                                                                    // also see below for offset_vertex_data: if that's 0 and lt ≠ 2 → lvf = 2
                float min_height_level = ReadFloat(ADTstream);  // used as height if no heightmap given and culling ᵘ
                float max_height_level = ReadFloat(ADTstream);  // ≥ WoD ignores value and assumes to both be 0.0 for LVF = 2! ᵘ
                int x_offset = ADTstream.ReadByte();    // The X offset of the liquid square (0-7)
                int y_offset = ADTstream.ReadByte();    // The Y offset of the liquid square (0-7)
                int width = ADTstream.ReadByte();   // The width of the liquid square (1-8)
                int height = ADTstream.ReadByte();  // The height of the liquid square (1-8)
                                                    // The above four members are only used if liquid_object_or_lvf <= 41. Otherwise they are assumed 0, 0, 8, 8. (18179) 
                int offset_exists_bitmap = ReadLong(ADTstream);   // not all tiles in the instances need to be filled. always 8*8 bits.
                                                                  // offset can be 0 for all-exist. also see (and extend) Talk:ADT/v18#SMLiquidInstance
                int offset_vertex_data = ReadLong(ADTstream);     // actual data format defined by LiquidMaterialRec::m_LVF via LiquidTypeRec::m_materialID
                                                                  // if offset = 0 and liquidType ≠ 2, then let LVF = 2, i.e. some ocean shit

            }
            //attributes
            if (offset_attributes >= 0)
            {
                ADTstream.Seek(chunkStartPosition + offset_attributes, SeekOrigin.Begin);
                ulong fishable = ReadUint64(ADTstream);               // seems to be usable as visibility information.
                ulong deep = ReadUint64(ADTstream);
            }
        }
        ADTstream.Seek(chunkStartPosition + MH2Osize, SeekOrigin.Begin); // set stream location to right after MH2O
    }

    private static void ReadMCNK(Stream ADTstream, int MCNKchunkNumber, int MCNKsize)
    {
        ChunkData chunkData = new ChunkData();
        long MCNKchnkPos = ADTstream.Position;
        // <Header> - 128 bytes
        chunkData.flags = ReadMCNKflags(ADTstream);

        chunkData.IndexX = ReadLong(ADTstream);
        chunkData.IndexY = ReadLong(ADTstream);
        chunkData.nLayers = ReadLong(ADTstream);  // maximum 4
        int nDoodadRefs = ReadLong(ADTstream);
        ulong holes_high_res = ReadUint64(ADTstream);  // only used with flags.high_res_holes
        int ofsLayer = ReadLong(ADTstream);
        int ofsRefs = ReadLong(ADTstream);
        int ofsAlpha = ReadLong(ADTstream);
        int sizeAlpha = ReadLong(ADTstream);
        int ofsShadow = ReadLong(ADTstream);  // only with flags.has_mcsh
        int sizeShadow = ReadLong(ADTstream);
        int areaid = ReadLong(ADTstream);  // in alpha: both zone id and sub zone id, as uint16s.
        int nMapObjRefs = ReadLong(ADTstream);
        int holes_low_res = ReadShort(ADTstream);
        int unknown_but_used = ReadShort(ADTstream);  // in alpha: padding
        byte[] ReallyLowQualityTextureingMap = new byte[16];  // uint2_t[8][8] "predTex", It is used to determine which detail doodads to show. Values are an array of two bit 
        for (int b = 0; b<16; b++)
        {
            ReallyLowQualityTextureingMap[b] = (byte)ADTstream.ReadByte();
        }
        // unsigned integers, naming the layer.
        ulong noEffectDoodad = ReadUint64(ADTstream);                    // WoD: may be an explicit MCDD chunk
        int ofsSndEmitters = ReadLong(ADTstream);
        int nSndEmitters = ReadLong(ADTstream);                       // will be set to 0 in the client if ofsSndEmitters doesn't point to MCSE!
        int ofsLiquid = ReadLong(ADTstream);
        int sizeLiquid = ReadLong(ADTstream);                          // 8 when not used; only read if >8.
        // in alpha, remainder is padding but unused.
        chunkData.MeshPosition = new Vector3(ReadFloat(ADTstream), ReadFloat(ADTstream), ReadFloat(ADTstream));
        int ofsMCCV = ReadLong(ADTstream);                             // only with flags.has_mccv, had uint32_t textureId; in ObscuR's structure.
        int ofsMCLV = ReadLong(ADTstream);                             // introduced in Cataclysm
        int unused = ReadLong(ADTstream);                              // currently unused
        // </header>

        if (!chunkData.flags.has_mccv)
            FillMCCV(ADTstream, chunkData); // fill vertex shading with 127...

        long streamPosition = ADTstream.Position;
        while (streamPosition < MCNKchnkPos + MCNKsize)
        {
            ADTstream.Position = streamPosition;
            int chunkID = ReadLong(ADTstream);
            int chunkSize = ReadLong(ADTstream);
            streamPosition = ADTstream.Position + chunkSize;
            switch (chunkID)
            {
                case (int)ADTchunkID.MCVT:
                    ReadMCVT(ADTstream, chunkData); // vertex heights
                    break;
                case (int)ADTchunkID.MCLV:
                    ReadMCLV(ADTstream, chunkData); // chunk lighting
                    break;
                case (int)ADTchunkID.MCCV:
                    ReadMCCV(ADTstream, chunkData); // vertex shading
                    break;
                case (int)ADTchunkID.MCNR:
                    ReadMCNR(ADTstream, chunkData); // normals
                    break;
                case (int)ADTchunkID.MCSE:
                    ReadMCSE(ADTstream, chunkData, chunkSize); // sound emitters
                    break;
                case (int)ADTchunkID.MCBB:
                    ReadMCBB(ADTstream, chunkData, chunkSize);
                    break;
                case (int)ADTchunkID.MCDD:
                    ReadMCDD(ADTstream, chunkData, chunkSize);
                    break;
                default:
                    SkipUnknownChunk(ADTstream, chunkID, chunkSize);
                    break;
            }
        }
        blockData.ChunksData.Add(chunkData);
    }

    private static void ReadMFBO(Stream ADTstream)
    {
        short[,] planeMax = new short[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                planeMax[x, y] = (short)ReadShort(ADTstream);
            }
        }
        short[,] planeMin = new short[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                planeMin[x, y] = (short)ReadShort(ADTstream);
            }
        }
    }


    ////////////////////////////
    ////// MCNK Subchunks //////
    ////////////////////////////

    private static void ReadMCVT(Stream ADTstream, ChunkData chunkData)
    {
        for (int v = 1; v <= 145; v++)
        {
            chunkData.VertexHeights.Add(ReadFloat(ADTstream));
        }
    }  //saved
    
    private static void ReadMCLV(Stream ADTstream, ChunkData chunkData)
    {
        for (int v = 1; v <= 145; v++)
        {
            byte[] ARGB = new byte[4];
            for (int b = 0; b < 4; b++)
            {
                ARGB[b] = (byte)ADTstream.ReadByte();
            }
            chunkData.VertexLighting.Add(ARGB);
        }
        // Alpha is ignored.
        // In contrast to MCCV does not only color but also lightens up the vertices.
        // Result of baking level-designer placed omni lights. With WoD, they added the actual lights to do live lighting.
    } // chunk lighting

    private static void ReadMCCV(Stream ADTstream, ChunkData chunkData)
    {
        chunkData.VertexColors = new Color32[145];

        List<int> vertcolors = new List<int>();
        for (int col = 0; col < 145; col++)
        {
            int channelR = ADTstream.ReadByte();
            vertcolors.Add(channelR);
            int channelG = ADTstream.ReadByte();
            vertcolors.Add(channelG);
            int channelB = ADTstream.ReadByte();
            vertcolors.Add(channelB);
            int channelA = ADTstream.ReadByte();
            vertcolors.Add(channelA);

            Color32 colorsRGBA = new Color32((byte)channelR, (byte)channelG, (byte)channelB, (byte)channelA);
            Color32 colorBGRA = new Color32(colorsRGBA.b, colorsRGBA.g, colorsRGBA.r, colorsRGBA.a);
            chunkData.VertexColors[col] = colorBGRA;
        }
    } // vertex shading

    private static void FillMCCV(Stream ADTstrea, ChunkData chunkData)
    {
        chunkData.VertexColors = new Color32[145];
        for (int col = 0; col < 145; col++)
        {
            Color32 colorBGRA = new Color32(127, 127, 127, 127);
            chunkData.VertexColors[col] = colorBGRA;
        }
    } // fill vertex shading with 127

    private static void ReadMCNR(Stream ADTstream, ChunkData chunkData)
    {
        chunkData.VertexNormals = new Vector3[145];

        for (int n = 0; n < 145; n++)
        {
            Vector3 normsRaw = new Vector3(ADTstream.ReadByte(), ADTstream.ReadByte(), ADTstream.ReadByte());

            var calcX = NormalizeValue(normsRaw.x); if (calcX <= 0) { calcX = 1 + calcX; } else if (calcX > 0) { calcX = (1 - calcX) * (-1); }
            var calcY = NormalizeValue(normsRaw.y); if (calcY <= 0) { calcY = 1 + calcY; } else if (calcY > 0) { calcY = (1 - calcY) * (-1); }
            var calcZ = NormalizeValue(normsRaw.z); if (calcZ <= 0) { calcZ = 1 + calcZ; } else if (calcZ > 0) { calcZ = (1 - calcZ) * (-1); }

            chunkData.VertexNormals[n] = new Vector3(calcX, calcZ, calcY);
        }
        // skip unused 13 byte padding //
        ADTstream.Seek(13, SeekOrigin.Current);
    }  // normals

    private static void ReadMCSE(Stream ADTstream, ChunkData chunkData, int MCSEsize)
    {
        if (MCSEsize != 0)
        {
            Debug.Log("MCSE found " + MCSEsize + " ----------I have info to parse it now");
            ADTstream.Seek(MCSEsize, SeekOrigin.Current); // skip for now
        }
    }

    private static void ReadMCBB(Stream ADTstream, ChunkData chunkData, int MCBBsize) // blend batches. max 256 per MCNK
    {
        //Debug.Log(MCBB + "found " + MCBBsize + " ----------I have info to parse it now");
        // skip for now
        ADTstream.Seek(MCBBsize, SeekOrigin.Current);
    }

    private static void ReadMCDD(Stream ADTstream, ChunkData chunkData, int MCDDsize) // there seems to be a high-res (?) mode which is not taken into account 
                                                    // in live clients (32 bytes instead of 8) (?). if inlined to MCNK is low-res.
    {
        //Debug.Log(MCDD + "found " + MCDDsize + " ----------I have info to parse it now");

        // skip for now
        ADTstream.Seek(MCDDsize, SeekOrigin.Current);
    }
}