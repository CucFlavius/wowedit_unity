using Assets.Data.WoW_Format_Parsers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public class ADTRoot
    {
        // global flags //
        public uint MH2Ooffset;
        public uint MFBOoffset;
        public List<bool> has_mcsh;

        public void ReadMVER(BinaryReader ADTstream)
        {
            ADTstream.BaseStream.Position += 4;
        }

        public void ReadMHDR(BinaryReader ADTstream)
        {
            uint flags      = ADTstream.ReadUInt32();
            uint mcin       = ADTstream.ReadUInt32();  // Cata+: obviously gone. probably all offsets gone, except mh2o(which remains in root file).
            uint mtex       = ADTstream.ReadUInt32();
            uint mmdx       = ADTstream.ReadUInt32();
            uint mmid       = ADTstream.ReadUInt32();
            uint mwmo       = ADTstream.ReadUInt32();
            uint mwid       = ADTstream.ReadUInt32();
            uint mddf       = ADTstream.ReadUInt32();
            uint modf       = ADTstream.ReadUInt32();
            MFBOoffset      = ADTstream.ReadUInt32(); // this is only set if flags & mhdr_MFBO.
            MH2Ooffset      = ADTstream.ReadUInt32();
            uint mtxf       = ADTstream.ReadUInt32();
            uint mamp_value = ADTstream.ReadByte(); // Cata+, explicit MAMP chunk overrides data
            byte[] padding  = new byte[3];
            padding[0]      = ADTstream.ReadByte();
            padding[1]      = ADTstream.ReadByte();
            padding[2]      = ADTstream.ReadByte();
            uint[] unused   = new uint[3];
            unused[0]       = ADTstream.ReadUInt32();
            unused[1]       = ADTstream.ReadUInt32();
            unused[2]       = ADTstream.ReadUInt32();
        }

        public void ReadMH2O(BinaryReader reader, int MH2Osize)
        {
            long chunkStartPosition = reader.BaseStream.Position;

            // header - SMLiquidChunk
            for (int a = 0; a < 256; a++)
            {
                uint offset_instances    = reader.ReadUInt32();                         // points to SMLiquidInstance[layer_count]
                uint layer_count         = reader.ReadUInt32();                         // 0 if the chunk has no liquids. If > 1, the offsets will point to arrays.
                uint offset_attributes   = reader.ReadUInt32();                         // points to mh2o_chunk_attributes, can be ommitted for all-0

                if (offset_instances >= 0)
                {
                    // instances @24bytes
                    reader.BaseStream.Seek(chunkStartPosition + offset_instances, SeekOrigin.Begin);
                    ushort liquid_type          = reader.ReadUInt16();                  //DBC - foreign_keyⁱ<uint16_t, &LiquidTypeRec::m_ID> liquid_type;
                    ushort liquid_object_or_lvf = reader.ReadUInt16();                  //DBC -  foreign_keyⁱ<uint16_t, &LiquidObjectRec::m_ID> liquid_object_or_lvf;        
                                                                                        // if > 41, an id into DB/LiquidObject. If below, LiquidVertexFormat, used in ADT/v18#instance_vertex_data Note hardcoded LO ids below.
                                                                                        // if >= 42, look up via DB/LiquidType and DB/LiquidMaterial, otherwise use liquid_object_or_lvf as LVF
                                                                                        // also see below for offset_vertex_data: if that's 0 and lt ≠ 2 → lvf = 2
                    float min_height_level      = reader.ReadSingle();                  // used as height if no heightmap given and culling ᵘ
                    float max_height_level      = reader.ReadSingle();                  // ≥ WoD ignores value and assumes to both be 0.0 for LVF = 2! ᵘ
                    byte x_offset               = reader.ReadByte();                    // The X offset of the liquid square (0-7)
                    byte y_offset               = reader.ReadByte();                    // The Y offset of the liquid square (0-7)
                    byte width                  = reader.ReadByte();                    // The width of the liquid square (1-8)
                    byte height                 = reader.ReadByte();                    // The height of the liquid square (1-8)
                                                                                        // The above four members are only used if liquid_object_or_lvf <= 41. Otherwise they are assumed 0, 0, 8, 8. (18179) 
                    uint offset_exists_bitmap   = reader.ReadUInt32();                  // not all tiles in the instances need to be filled. always 8*8 bits.
                                                                                        // offset can be 0 for all-exist. also see (and extend) Talk:ADT/v18#SMLiquidInstance
                    uint offset_vertex_data     = reader.ReadUInt32();                  // actual data format defined by LiquidMaterialRec::m_LVF via LiquidTypeRec::m_materialID
                                                                                        // if offset = 0 and liquidType ≠ 2, then let LVF = 2, i.e. some ocean shit

                }
                //attributes
                if (offset_attributes >= 0)
                {
                    reader.BaseStream.Seek(chunkStartPosition + offset_attributes, SeekOrigin.Begin);
                    ulong fishable  = reader.ReadUInt64();                               // seems to be usable as visibility information.
                    ulong deep      = reader.ReadUInt64();
                }
            }
            reader.BaseStream.Seek(chunkStartPosition + MH2Osize, SeekOrigin.Begin); // set stream location to right after MH2O
        }


        public void ReadMCNK(BinaryReader reader, int MCNKchunkNumber, int MCNKsize)
        {
            Flags f                     = new Flags();
            ADTRootData.MeshChunkData chunkData = new ADTRootData.MeshChunkData();
            long MCNKchnkPos            = reader.BaseStream.Position;
            // <Header> - 128 bytes
            chunkData.flags             = f.ReadMCNKflags(reader);

            chunkData.IndexX            = reader.ReadUInt32();
            chunkData.IndexY            = reader.ReadUInt32();
            chunkData.nLayers           = reader.ReadUInt32();                          // maximum 4
            uint nDoodadRefs            = reader.ReadUInt32();
            chunkData.holes_high_res    = reader.ReadUInt64();                          // only used with flags.high_res_holes
            uint ofsLayer               = reader.ReadUInt32();
            uint ofsRefs                = reader.ReadUInt32();
            uint ofsAlpha               = reader.ReadUInt32();
            uint sizeAlpha              = reader.ReadUInt32();
            uint ofsShadow              = reader.ReadUInt32();                          // only with flags.has_mcsh
            uint sizeShadow             = reader.ReadUInt32();
            uint areaid                 = reader.ReadUInt32();                          // in alpha: both zone id and sub zone id, as uint16s.
            uint nMapObjRefs            = reader.ReadUInt32();
            chunkData.holes_low_res     = reader.ReadUInt16();
            ushort unknown_but_used     = reader.ReadUInt16();                          // in alpha: padding
            byte[] ReallyLowQualityTextureingMap = new byte[16];                        // uint2_t[8][8] "predTex", It is used to determine which detail doodads to show. Values are an array of two bit 
            for (int b = 0; b < 16; b++)
            {
                ReallyLowQualityTextureingMap[b] = reader.ReadByte();
            }
            // unsigned integers, naming the layer.
            ulong noEffectDoodad        = reader.ReadUInt64();                          // WoD: may be an explicit MCDD chunk
            int ofsSndEmitters          = reader.ReadInt32();
            int nSndEmitters            = reader.ReadInt32();                           // will be set to 0 in the client if ofsSndEmitters doesn't point to MCSE!
            int ofsLiquid               = reader.ReadInt32();
            int sizeLiquid              = reader.ReadInt32();                           // 8 when not used; only read if >8.
                                                                                        // in alpha, remainder is padding but unused.
            chunkData.MeshPosition      = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            int ofsMCCV                 = reader.ReadInt32();                           // only with flags.has_mccv, had uint32_t textureId; in ObscuR's structure.
            int ofsMCLV                 = reader.ReadInt32();                           // introduced in Cataclysm
            int unused                  = reader.ReadInt32();                           // currently unused
                                                                                        // </header>

            if (!chunkData.flags.has_mccv)
                FillMCCV(chunkData);                                                    // fill vertex shading with 127...

            long streamPosition = reader.BaseStream.Position;
            while (streamPosition < MCNKchnkPos + MCNKsize)
            {
                reader.BaseStream.Position = streamPosition;
                ADTChunkId chunkId = (ADTChunkId)reader.ReadInt32();
                int chunkSize = reader.ReadInt32();
                streamPosition = reader.BaseStream.Position + chunkSize;
                switch (chunkId)
                {
                    case ADTChunkId.MCVT:
                        ReadMCVT(reader, chunkData); // vertex heights
                        break;
                    case ADTChunkId.MCLV:
                        ReadMCLV(reader, chunkData); // chunk lighting
                        break;
                    case ADTChunkId.MCCV:
                        ReadMCCV(reader, chunkData); // vertex shading
                        break;
                    case ADTChunkId.MCNR:
                        ReadMCNR(reader, chunkData); // normals
                        break;
                    case ADTChunkId.MCSE:
                        ReadMCSE(reader, chunkData, chunkSize); // sound emitters
                        break;
                    case ADTChunkId.MCBB:
                        ReadMCBB(reader, chunkData, chunkSize);
                        break;
                    case ADTChunkId.MCDD:
                        ReadMCDD(reader, chunkData, chunkSize);
                        break;
                    default:
                        SkipUnknownChunk(reader, chunkId, chunkSize);
                        break;
                }
            }
            ADTRootData.meshBlockData.meshChunksData.Add(chunkData);
        }

        public void ReadMFBO(BinaryReader reader)
        {
            short[,] planeMax = new short[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    planeMax[x, y] = (short)reader.ReadUInt16();
                }
            }
            short[,] planeMin = new short[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    planeMin[x, y] = (short)reader.ReadUInt16();
                }
            }
        }

        ////////////////////////////
        ////// MCNK Subchunks //////
        ////////////////////////////

        public void ReadMCVT(BinaryReader reader, ADTRootData.MeshChunkData chunkData)
        {
            for (int v = 1; v <= 145; v++)
            {
                chunkData.VertexHeights.Add(reader.ReadSingle());
            }
        }

        public void ReadMCLV(BinaryReader reader, ADTRootData.MeshChunkData chunkData)
        {
            for (int v = 1; v <= 145; v++)
            {
                byte[] ARGB = new byte[4];
                for (int b = 0; b < 4; b++)
                {
                    ARGB[b] = reader.ReadByte();
                }
                chunkData.VertexLighting.Add(ARGB);
            }
            // Alpha is ignored.
            // In contrast to MCCV does not only color but also lightens up the vertices.
            // Result of baking level-designer placed omni lights. With WoD, they added the actual lights to do live lighting.
        }   // chunk lighting

        public void ReadMCCV(BinaryReader reader, ADTRootData.MeshChunkData chunkData)
        {
            chunkData.VertexColors = new Color32[145];

            List<int> vertcolors = new List<int>();
            for (int col = 0; col < 145; col++)
            {
                int channelR = reader.ReadByte();
                vertcolors.Add(channelR);
                int channelG = reader.ReadByte();
                vertcolors.Add(channelG);
                int channelB = reader.ReadByte();
                vertcolors.Add(channelB);
                int channelA = reader.ReadByte();
                vertcolors.Add(channelA);

                Color32 colorsRGBA = new Color32((byte)channelR, (byte)channelG, (byte)channelB, (byte)channelA);
                Color32 colorBGRA = new Color32(colorsRGBA.b, colorsRGBA.g, colorsRGBA.r, colorsRGBA.a);
                chunkData.VertexColors[col] = colorBGRA;
            }
        } // vertex shading

        public void FillMCCV(ADTRootData.MeshChunkData chunkData)
        {
            chunkData.VertexColors = new Color32[145];
            for (int col = 0; col < 145; col++)
            {
                Color32 colorBGRA = new Color32(127, 127, 127, 127);
                chunkData.VertexColors[col] = colorBGRA;
            }
        } // fill vertex shading with 127

        public void ReadMCNR(BinaryReader reader, ADTRootData.MeshChunkData chunkData)
        {
            chunkData.VertexNormals = new Vector3[145];
            for (int n = 0; n < 145; n++)
            {
                Vector3 normsRaw = new Vector3(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());

                var calcX = reader.NormalizeValue(normsRaw.x); if (calcX <= 0) { calcX = 1 + calcX; } else if (calcX > 0) { calcX = (1 - calcX) * (-1); }
                var calcY = reader.NormalizeValue(normsRaw.y); if (calcY <= 0) { calcY = 1 + calcY; } else if (calcY > 0) { calcY = (1 - calcY) * (-1); }
                var calcZ = reader.NormalizeValue(normsRaw.z); if (calcZ <= 0) { calcZ = 1 + calcZ; } else if (calcZ > 0) { calcZ = (1 - calcZ) * (-1); }

                chunkData.VertexNormals[n] = new Vector3(calcX, calcZ, calcY);
            }
            // skip unused 13 byte padding //
            reader.BaseStream.Seek(13, SeekOrigin.Current);
        }   // normals

        public void ReadMCSE(BinaryReader ADTstream, ADTRootData.MeshChunkData chunkData, int MCSEsize)
        {
            if (MCSEsize != 0)
            {
                Debug.Log("MCSE found " + MCSEsize + " ----------I have info to parse it now");
                ADTstream.BaseStream.Seek(MCSEsize, SeekOrigin.Current); // skip for now
            }
        }

        public void ReadMCBB(BinaryReader ADTstream, ADTRootData.MeshChunkData chunkData, int MCBBsize) // blend batches. max 256 per MCNK
        {
            //Debug.Log(MCBB + "found " + MCBBsize + " ----------I have info to parse it now");
            // skip for now
            ADTstream.BaseStream.Seek(MCBBsize, SeekOrigin.Current);
        }

        public void ReadMCDD(BinaryReader ADTstream, ADTRootData.MeshChunkData chunkData, int MCDDsize) // there seems to be a high-res (?) mode which is not taken into account 
                                                                                                        // in live clients (32 bytes instead of 8) (?). if inlined to MCNK is low-res.
        {
            //Debug.Log(MCDD + "found " + MCDDsize + " ----------I have info to parse it now");

            // skip for now
            ADTstream.BaseStream.Seek(MCDDsize, SeekOrigin.Current);
        }

        // Move the stream forward upon finding unknown chunks //
        public static void SkipUnknownChunk(BinaryReader reader, ADTChunkId chunkID, int chunkSize)
        {
            Debug.Log("Missing chunk ID : " + chunkID);
            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }

    }
}