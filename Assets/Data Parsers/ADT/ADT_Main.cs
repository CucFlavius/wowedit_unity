using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT {

    // global flags //
    private static int MH2Ooffset;
    private static int MFBOoffset;
    private static List<bool> has_mcsh;

    
    private static void ReadMVER(MemoryStream ADTstream)
    {
        //int ADTfileversion = ReadLong(ADTstream);
        ADTstream.Position += 4;
    }
    
    private static void ReadMHDR(MemoryStream ADTstream)
    {
        StreamTools s = new StreamTools();
        int flags = s.ReadLong(ADTstream);
        int mcin = s.ReadLong(ADTstream);  // Cata+: obviously gone. probably all offsets gone, except mh2o(which remains in root file).
        int mtex = s.ReadLong(ADTstream);
        int mmdx = s.ReadLong(ADTstream);
        int mmid = s.ReadLong(ADTstream);
        int mwmo = s.ReadLong(ADTstream);
        int mwid = s.ReadLong(ADTstream);
        int mddf = s.ReadLong(ADTstream);
        int modf = s.ReadLong(ADTstream);
        MFBOoffset = s.ReadLong(ADTstream); // this is only set if flags & mhdr_MFBO.
        MH2Ooffset = s.ReadLong(ADTstream);
        int mtxf = s.ReadLong(ADTstream);
        int mamp_value = ADTstream.ReadByte(); // Cata+, explicit MAMP chunk overrides data
        int[] padding = new int[3];
        padding[0] = ADTstream.ReadByte();
        padding[1] = ADTstream.ReadByte();
        padding[2] = ADTstream.ReadByte();
        int[] unused = new int[3];
        unused[0] = s.ReadLong(ADTstream);
        unused[1] = s.ReadLong(ADTstream);
        unused[2] = s.ReadLong(ADTstream);
    }
    
    private static void ReadMH2O(MemoryStream ADTstream, int MH2Osize)
    {
        StreamTools s = new StreamTools();
        long chunkStartPosition = ADTstream.Position;

        // header - SMLiquidChunk
        for (int a = 0; a < 256; a++)
        {
            int offset_instances = s.ReadLong(ADTstream);       // points to SMLiquidInstance[layer_count]
            int layer_count = s.ReadLong(ADTstream);            // 0 if the chunk has no liquids. If > 1, the offsets will point to arrays.
            int offset_attributes = s.ReadLong(ADTstream);      // points to mh2o_chunk_attributes, can be ommitted for all-0
            if (offset_instances >= 0)
            {
                // instances @24bytes
                ADTstream.Seek(chunkStartPosition + offset_instances, SeekOrigin.Begin);
                int liquid_type = s.ReadShort(ADTstream); //DBC - foreign_keyⁱ<uint16_t, &LiquidTypeRec::m_ID> liquid_type;
                int liquid_object_or_lvf = s.ReadShort(ADTstream);    //DBC -  foreign_keyⁱ<uint16_t, &LiquidObjectRec::m_ID> liquid_object_or_lvf;        
                                                                    // if > 41, an id into DB/LiquidObject. If below, LiquidVertexFormat, used in ADT/v18#instance_vertex_data Note hardcoded LO ids below.
                                                                    // if >= 42, look up via DB/LiquidType and DB/LiquidMaterial, otherwise use liquid_object_or_lvf as LVF
                                                                    // also see below for offset_vertex_data: if that's 0 and lt ≠ 2 → lvf = 2
                float min_height_level = s.ReadFloat(ADTstream);  // used as height if no heightmap given and culling ᵘ
                float max_height_level = s.ReadFloat(ADTstream);  // ≥ WoD ignores value and assumes to both be 0.0 for LVF = 2! ᵘ
                int x_offset = ADTstream.ReadByte();    // The X offset of the liquid square (0-7)
                int y_offset = ADTstream.ReadByte();    // The Y offset of the liquid square (0-7)
                int width = ADTstream.ReadByte();   // The width of the liquid square (1-8)
                int height = ADTstream.ReadByte();  // The height of the liquid square (1-8)
                                                    // The above four members are only used if liquid_object_or_lvf <= 41. Otherwise they are assumed 0, 0, 8, 8. (18179) 
                int offset_exists_bitmap = s.ReadLong(ADTstream);   // not all tiles in the instances need to be filled. always 8*8 bits.
                                                                  // offset can be 0 for all-exist. also see (and extend) Talk:ADT/v18#SMLiquidInstance
                int offset_vertex_data = s.ReadLong(ADTstream);     // actual data format defined by LiquidMaterialRec::m_LVF via LiquidTypeRec::m_materialID
                                                                  // if offset = 0 and liquidType ≠ 2, then let LVF = 2, i.e. some ocean shit

            }
            //attributes
            if (offset_attributes >= 0)
            {
                ADTstream.Seek(chunkStartPosition + offset_attributes, SeekOrigin.Begin);
                ulong fishable = s.ReadUint64(ADTstream);               // seems to be usable as visibility information.
                ulong deep = s.ReadUint64(ADTstream);
            }
        }
        ADTstream.Seek(chunkStartPosition + MH2Osize, SeekOrigin.Begin); // set stream location to right after MH2O
    }
  
    
    private static void ReadMCNK(MemoryStream ADTstream, int MCNKchunkNumber, int MCNKsize)
    {
        StreamTools s = new StreamTools();
        Flags f = new Flags();
        MeshChunkData chunkData = new MeshChunkData();
        long MCNKchnkPos = ADTstream.Position;
        // <Header> - 128 bytes
        chunkData.flags = f.ReadMCNKflags(ADTstream);

        chunkData.IndexX = s.ReadLong(ADTstream);
        chunkData.IndexY = s.ReadLong(ADTstream);
        chunkData.nLayers = s.ReadLong(ADTstream);  // maximum 4
        int nDoodadRefs = s.ReadLong(ADTstream);
        ulong holes_high_res = s.ReadUint64(ADTstream);  // only used with flags.high_res_holes
        int ofsLayer = s.ReadLong(ADTstream);
        int ofsRefs = s.ReadLong(ADTstream);
        int ofsAlpha = s.ReadLong(ADTstream);
        int sizeAlpha = s.ReadLong(ADTstream);
        int ofsShadow = s.ReadLong(ADTstream);  // only with flags.has_mcsh
        int sizeShadow = s.ReadLong(ADTstream);
        int areaid = s.ReadLong(ADTstream);  // in alpha: both zone id and sub zone id, as uint16s.
        int nMapObjRefs = s.ReadLong(ADTstream);
        int holes_low_res = s.ReadShort(ADTstream);
        int unknown_but_used = s.ReadShort(ADTstream);  // in alpha: padding
        byte[] ReallyLowQualityTextureingMap = new byte[16];  // uint2_t[8][8] "predTex", It is used to determine which detail doodads to show. Values are an array of two bit 
        for (int b = 0; b<16; b++)
        {
            ReallyLowQualityTextureingMap[b] = (byte)ADTstream.ReadByte();
        }
        // unsigned integers, naming the layer.
        ulong noEffectDoodad = s.ReadUint64(ADTstream);                    // WoD: may be an explicit MCDD chunk
        int ofsSndEmitters = s.ReadLong(ADTstream);
        int nSndEmitters = s.ReadLong(ADTstream);                       // will be set to 0 in the client if ofsSndEmitters doesn't point to MCSE!
        int ofsLiquid = s.ReadLong(ADTstream);
        int sizeLiquid = s.ReadLong(ADTstream);                          // 8 when not used; only read if >8.
        // in alpha, remainder is padding but unused.
        chunkData.MeshPosition = new Vector3(s.ReadFloat(ADTstream), s.ReadFloat(ADTstream), s.ReadFloat(ADTstream));
        int ofsMCCV = s.ReadLong(ADTstream);                             // only with flags.has_mccv, had uint32_t textureId; in ObscuR's structure.
        int ofsMCLV = s.ReadLong(ADTstream);                             // introduced in Cataclysm
        int unused = s.ReadLong(ADTstream);                              // currently unused
        // </header>

        if (!chunkData.flags.has_mccv)
            FillMCCV(chunkData); // fill vertex shading with 127...

        long streamPosition = ADTstream.Position;
        while (streamPosition < MCNKchnkPos + MCNKsize)
        {
            ADTstream.Position = streamPosition;
            int chunkID = s.ReadLong(ADTstream);
            int chunkSize = s.ReadLong(ADTstream);
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
        meshBlockData.meshChunksData.Add(chunkData);
    }
    
    private static void ReadMFBO(MemoryStream ADTstream)
    {
        StreamTools s = new StreamTools();
        short[,] planeMax = new short[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                planeMax[x, y] = (short)s.ReadShort(ADTstream);
            }
        }
        short[,] planeMin = new short[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                planeMin[x, y] = (short)s.ReadShort(ADTstream);
            }
        }
    }
    
    ////////////////////////////
    ////// MCNK Subchunks //////
    ////////////////////////////

    private static void ReadMCVT(MemoryStream ADTstream, MeshChunkData chunkData)
    {
        StreamTools s = new StreamTools();
        for (int v = 1; v <= 145; v++)
        {
            chunkData.VertexHeights.Add(s.ReadFloat(ADTstream));
        }
    }
 
    private static void ReadMCLV(MemoryStream ADTstream, MeshChunkData chunkData)
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
    
    private static void ReadMCCV(MemoryStream ADTstream, MeshChunkData chunkData)
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
    
    private static void FillMCCV(MeshChunkData chunkData)
    {
        chunkData.VertexColors = new Color32[145];
        for (int col = 0; col < 145; col++)
        {
            Color32 colorBGRA = new Color32(127, 127, 127, 127);
            chunkData.VertexColors[col] = colorBGRA;
        }
    } // fill vertex shading with 127

    private static void ReadMCNR(MemoryStream ADTstream, MeshChunkData chunkData)
    {
        StreamTools s = new StreamTools();
        chunkData.VertexNormals = new Vector3[145];
        for (int n = 0; n < 145; n++)
        {
            Vector3 normsRaw = new Vector3(ADTstream.ReadByte(), ADTstream.ReadByte(), ADTstream.ReadByte());

            var calcX = s.NormalizeValue(normsRaw.x); if (calcX <= 0) { calcX = 1 + calcX; } else if (calcX > 0) { calcX = (1 - calcX) * (-1); }
            var calcY = s.NormalizeValue(normsRaw.y); if (calcY <= 0) { calcY = 1 + calcY; } else if (calcY > 0) { calcY = (1 - calcY) * (-1); }
            var calcZ = s.NormalizeValue(normsRaw.z); if (calcZ <= 0) { calcZ = 1 + calcZ; } else if (calcZ > 0) { calcZ = (1 - calcZ) * (-1); }

            chunkData.VertexNormals[n] = new Vector3(calcX, calcZ, calcY);
        }
        // skip unused 13 byte padding //
        ADTstream.Seek(13, SeekOrigin.Current);
    }  // normals
    
    private static void ReadMCSE(MemoryStream ADTstream, MeshChunkData chunkData, int MCSEsize)
    {
        if (MCSEsize != 0)
        {
            Debug.Log("MCSE found " + MCSEsize + " ----------I have info to parse it now");
            ADTstream.Seek(MCSEsize, SeekOrigin.Current); // skip for now
        }
    }

    private static void ReadMCBB(MemoryStream ADTstream, MeshChunkData chunkData, int MCBBsize) // blend batches. max 256 per MCNK
    {
        //Debug.Log(MCBB + "found " + MCBBsize + " ----------I have info to parse it now");
        // skip for now
        ADTstream.Seek(MCBBsize, SeekOrigin.Current);
    }

    private static void ReadMCDD(MemoryStream ADTstream, MeshChunkData chunkData, int MCDDsize) // there seems to be a high-res (?) mode which is not taken into account 
                                                    // in live clients (32 bytes instead of 8) (?). if inlined to MCNK is low-res.
    {
        //Debug.Log(MCDD + "found " + MCDDsize + " ----------I have info to parse it now");

        // skip for now
        ADTstream.Seek(MCDDsize, SeekOrigin.Current);
    }
}