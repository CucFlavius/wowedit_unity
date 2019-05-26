using CASCLib;
using Assets.WoWEditSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.WMO
{
    public static partial class WMO
    {
        // root file version //
        public static void ReadMVER(BinaryReader reader)
        {
            int WMOfileversion = reader.ReadInt32();
        }

        // root file header // 64 bytes
        public static void ReadMOHD(BinaryReader reader)
        {
            wmoData.Info.nTextures      = reader.ReadInt32();                   // number of textures used
            wmoData.Info.nGroups        = reader.ReadInt32();                   // number of groups
            wmoData.Info.nPortals       = reader.ReadInt32();                   // number of portals
            wmoData.Info.nLights        = reader.ReadInt32();                   // number of lights 
                                                                                //Blizzard seems to add one to the MOLT entry count when there are MOLP chunks in the groups (and maybe for MOLS too?)
            int nDoodadNames            = reader.ReadInt32();                   // number of doodad names
            int nDoodadDefs             = reader.ReadInt32();                   // number of doodad definitions
            int nDoodadSets             = reader.ReadInt32();                   // number of doodad sets
            ARGB ambColor               = reader.ReadARGB();                    // Color settings for base (ambient) color. See the flag at /*03Ch*/.   // ARGB
            wmoData.Info.wmoID          = reader.ReadInt64();                   // < uint32_t, &WMOAreaTableRec::m_WMOID> ID located in DBC
            BoundingBox bounding_box    = reader.ReadBoundingBoxes();           // in the alpha, this bounding box was computed upon loading
                                                                                // <Flags> 2 bytes                                                            
            byte[] arrayOfBytes = new byte[2];
            reader.Read(arrayOfBytes, 0, 2);
            BitArray flags = new BitArray(arrayOfBytes);
            bool flag_attenuate_vertices_based_on_distance_to_portal = flags[0];
            bool flag_skip_base_color = flags[1];                           // do not add base (ambient) color (of MOHD) to MOCVs. apparently does more, e.g. required for multiple MOCVs
            bool flag_use_liquid_type_dbc_id = flags[2];                    // use real liquid type ID from DBCs instead of local one. See MLIQ for further reference.
            bool flag_lighten_interiors = flags[3];                         // makes iterior groups much brighter, effects MOCV rendering. Used e.g.in Stormwind for having shiny bright interiors,
            bool Flag_Lod = flags[4];                                       // ≥ Legion (20740)
                                                                            // 11 bits unused flags unused as of Legion (20994)
                                                                            // </Flags>
            short numLod = reader.ReadInt16();                              // ≥ Legion (21108) includes base lod (→ numLod = 3 means '.wmo', 'lod0.wmo' and 'lod1.wmo')
        }

        // materials //
        public static void ReadMOMT(BinaryReader reader, int MOMTsize, CASCHandler Handler)
        {
            wmoData.Info.nMaterials = MOMTsize / 64;
            for (int i = 0; i < wmoData.Info.nMaterials; i++)
            {
                WMOMaterial material = new WMOMaterial();

                material.flags              = reader.ReadMaterialFlags();
                material.ShaderType         = (WMOFragmentShader)reader.ReadUInt32();   // Index into CMapObj::s_wmoShaderMetaData. See below (shader types).
                material.BlendMode          = (BlendingMode)reader.ReadUInt32();        // Blending: see https://wowdev.wiki/Rendering#EGxBlend

                material.TextureId1         = reader.ReadUInt32();                      // offset into MOTX; ≥ Battle (8.1.0.27826) No longer references MOTX but is a filedata id directly.
                material.SidnColor          = reader.ReadBGRA();                        // emissive color; see below (emissive color)
                material.FrameSidnColor     = reader.ReadBGRA();                        // sidn emissive color; set at runtime; gets sidn-manipulated emissive color; see below (emissive color)
                material.TextureId2         = reader.ReadUInt32();                      // Environment Texture; envNameIndex; offset into MOTX
                material.DiffColor          = reader.ReadBGRA();                        // diffuse color; CWorldView::GatherMapObjDefGroupLiquids(): geomFactory->SetDiffuseColor((CImVectorⁱ*)(smo+7));
                                                                                        // environment textures don't need flags

                material.GroundType         = reader.ReadUInt32();                      // foreign_keyⁱ< uint32_t, &TerrainTypeRec::m_ID > ground_type; // according to CMapObjDef::GetGroundType 
                material.TextureId3         = reader.ReadUInt32();                      // offset into MOTX
                material.Color              = reader.ReadBGRA();
                material.texture3_flags     = reader.ReadMaterialFlags();

                // skip runtime data //
                reader.BaseStream.Seek(16, SeekOrigin.Current);

                wmoData.materials.Add(material);
                if (!wmoData.texturePaths.ContainsKey(material.TextureId1))
                    wmoData.texturePaths.Add(material.TextureId1, material.TextureId1);

                if (material.TextureId1 != 0)
                {
                    Texture2Ddata textureData   = new Texture2Ddata();
                    Stream stream               = Handler.OpenFile(material.Texture1);
                    BLP blp                     = new BLP();
                    byte[] data                 = blp.GetUncompressed(stream, true);
                    BLPinfo info                = blp.Info();
                    textureData.hasMipmaps      = info.hasMipmaps;
                    textureData.width           = info.width;
                    textureData.height          = info.height;
                    textureData.textureFormat   = info.textureFormat;
                    textureData.TextureData     = data;
                    stream.Close();
                    stream.Dispose();
                    LoadedBLPs.Add(material.Texture1);

                    if (!wmoData.textureData.ContainsKey(material.TextureId1))
                        wmoData.textureData.Add(material.TextureId1, textureData);
                }
            }
        }   // loaded

        // texture translation animations //
        public static void ReadMOUV(BinaryReader reader)
        {
            // Currently, only a translating animation is possible for two of the texture layers.
            for (int i = 0; i < wmoData.Info.nMaterials; i++)
            {
                Vector2 translation_speed = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            }
            // The formula from translation_speed values to TexMtx translation values is along the lines of:
            // a_i = translation_i ? 1000 / translation_i : 0
            // b_i = a_i ? (a_i < 0 ? (1 - (time ? % -a_i) / -a_i) : ((time ? % a_i) / a_i)) : 0
        }

        // list of group names for the groups in this map object //
        public static void ReadMOGN(BinaryReader reader, int MOGNsize)
        {
            long currentPos = reader.BaseStream.Position;
            // starts with two 0x00
            reader.BaseStream.Seek(2, SeekOrigin.Current);
            while (reader.BaseStream.Position < currentPos + MOGNsize)
            {
                wmoData.MOGNgroupnames.Add((int)(reader.BaseStream.Position - currentPos), reader.ReadNullTerminatedString());
            }
        }   // loaded

        // Group information for WMO groups
        public static void ReadMOGI(BinaryReader reader)
        {
            for (int a = 0; a < wmoData.Info.nGroups; a++)
            {
                uint flags              = reader.ReadUInt32();                          //  see information in in MOGP, they are equivalent
                BoundingBox boundingBox = reader.ReadBoundingBoxes();                   // group bounding box
                // wmoData.MOGIgroupnames.Add(wmoData.MOGNgroupnames[ReadLong(WMOrootstream)]);
                int nameOffset          = reader.ReadInt32();                           // name in MOGN chunk (-1 for no name)
            }
        }   // exists in group

        // Skybox model filename
        public static void ReadMOSB(BinaryReader reader, int MOSBsize)
        {
            string skyboxName = null;
            for (int a = 0; a < MOSBsize; a++)
            {
                int b = reader.ReadByte();
                if (b != 0)
                {
                    var stringSymbol = System.Convert.ToChar(b);
                    skyboxName = skyboxName + stringSymbol;
                }
                else
                {
                    reader.BaseStream.Seek(3, SeekOrigin.Current);
                }
            }
        }

        // Portal vertices
        public static void ReadMOPV(BinaryReader reader, int MOPVsize)
        {
            long currentPosition = reader.BaseStream.Position;
            List<Vector3> PortalVertices = new List<Vector3>();
            while (reader.BaseStream.Position < MOPVsize + currentPosition)
            {
                PortalVertices.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        // Portal information.
        public static void ReadMOPT(BinaryReader reader)
        {
            List<SMOPortal> PortalInfo = new List<SMOPortal>();
            for (int i = 0; i < wmoData.Info.nPortals; ++i)
            {
                SMOPortal portal    = new SMOPortal();
                portal.startVertex  = reader.ReadUInt16();
                portal.count        = reader.ReadUInt16();
                C4Plane plane       = new C4Plane();
                plane.normal        = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                plane.distance      = reader.ReadSingle();
                portal.plane        = plane;
                PortalInfo.Add(portal);
            }
        }

        // Map Object Portal References from groups. 
        public static void ReadMOPR(BinaryReader reader, int MOPRsize)
        {
            int portalReferenceCount = MOPRsize / 8;
            for (int i = 0; i < portalReferenceCount; ++i)
            {
                ushort PortalIndex  = reader.ReadUInt16();      // into MOPR
                ushort GroupIndex   = reader.ReadUInt16();      // the other one
                short Side          = reader.ReadInt16();       // positive or negative.
                ushort Filler       = reader.ReadUInt16();
            }
        }

        // Visible block vertices
        public static void ReadMOVV(BinaryReader reader, int MOVVsize)
        {
            List<Vector3> VisibleVertices = new List<Vector3>();
            int vertexCount = MOVVsize / 12;
            for (int i = 0; i < vertexCount; ++i)
            {
                VisibleVertices.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        // Visible block list
        public static void ReadMOVB(BinaryReader reader, int MOVBsize)
        {
            List<VisibleBlock> VisibleBlocks = new List<VisibleBlock>();
            int blockCount = MOVBsize / 4;
            for (int i = 0; i < blockCount; ++i)
            {
                VisibleBlock block  = new VisibleBlock();
                block.firstVertex   = reader.ReadUInt16();
                block.count         = reader.ReadUInt16();
                VisibleBlocks.Add(block);
            }
        }

        // Lighting information.
        public static void ReadMOLT(BinaryReader reader)
        {
            List<StaticLight> StaticLights = new List<StaticLight>();
            int lightsCount = wmoData.Info.nLights;
            for (int i = 0; i < lightsCount; ++i)
            {
                StaticLight staticLight         = new StaticLight();
                staticLight.type                = (LightType)reader.ReadByte();
                staticLight.useAttenuation      = reader.ReadByte(); // boolean - true if byte != 0
                staticLight.useUnknown1         = reader.ReadByte(); // boolean - true if byte != 0
                staticLight.useUnknown2         = reader.ReadByte(); // boolean - true if byte != 0
                staticLight.color               = reader.ReadBGRA();
                staticLight.position            = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                staticLight.intensity           = reader.ReadSingle();
                staticLight.attenuationStart    = reader.ReadSingle();
                staticLight.attenuationEnd      = reader.ReadSingle();
                staticLight.unknown1StartRadius = reader.ReadSingle();
                staticLight.unknown1EndRadius   = reader.ReadSingle();
                staticLight.unknown2StartRadius = reader.ReadSingle();
                staticLight.unknown2EndRadius   = reader.ReadSingle();
                StaticLights.Add(staticLight);
            }
        }

        // This chunk defines doodad sets.
        public static void ReadMODS(BinaryReader reader, int MODSsize)
        {
            int numberOfSets = MODSsize / 32;
            List<DoodadSet> DoodadSets = new List<DoodadSet>();
            for (int i = 0; i < numberOfSets; ++i)
            {
                DoodadSet doodadSet = new DoodadSet();
                byte[] nameBytes = new byte[20];
                reader.Read(nameBytes, 0, 20);
                doodadSet.name                      = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
                doodadSet.firstDoodadInstanceIndex  = reader.ReadUInt32();
                doodadSet.doodadInstanceCount       = reader.ReadUInt32();
                char[] Unused = reader.ReadChars(4);
                DoodadSets.Add(doodadSet);
            }
        }

        //List of filenames for M2(They are listed as MDX) models
        public static void ReadMODN(BinaryReader reader, int MODNsize)
        {
            string m2Path = "";
            List<string> m2Paths = new List<string>();
            for (int a = 0; a < MODNsize; a++)
            {
                int b = reader.ReadByte();
                if (b != 0)
                {
                    var stringSymbol = Convert.ToChar(b);
                    m2Path = m2Path + stringSymbol;
                }
                else if (b == 0)
                {
                    if (reader.BaseStream.Position % 4 == 0)
                    {
                        if (m2Path.Length > 0)
                        {
                            m2Paths.Add(m2Path);
                            m2Path = null;
                        }
                    }
                }
            }
        }

        // Information for doodad instances. 
        public static void ReadMODD(BinaryReader reader, int MODDsize)
        {
            int numberOfDoodadInstances = MODDsize / 40;
            List<DoodadInstanceInfo> DoodadInstances = new List<DoodadInstanceInfo>();
            for (int i = 0; i < numberOfDoodadInstances; ++i)
            {
                DoodadInstanceInfo doodadInstance   = new DoodadInstanceInfo();
                byte[] finalNameBytes               = new byte[4];
                byte[] nameOffsetBytes              = new byte[3];
                reader.Read(nameOffsetBytes, 0, 3);
                Buffer.BlockCopy(nameOffsetBytes, 0, finalNameBytes, 0, 3);
                doodadInstance.nameOffset           = BitConverter.ToUInt32(finalNameBytes, 0); // reference offset into MODN

                DoodadInstanceFlags doodadInstanceFlags = new DoodadInstanceFlags();
                byte[] arrayOfBytes                     = new byte[1];
                reader.Read(arrayOfBytes, 0, 1);
                BitArray flags                          = new BitArray(arrayOfBytes);
                doodadInstanceFlags.AcceptProjectedTexture  = flags[0];
                doodadInstanceFlags.Unknown1                = flags[1]; // MapStaticEntity::field_34 |= 1 (if set, MapStaticEntity::AdjustLighting is _not_ called)
                doodadInstanceFlags.Unknown2                = flags[2];
                doodadInstanceFlags.Unknown3                = flags[3];
                doodadInstance.flags                        = doodadInstanceFlags;

                doodadInstance.position             = new Vector3(reader.ReadSingle() / Settings.WorldScale,  // X
                                                                  reader.ReadSingle() / Settings.WorldScale,  // Z
                                                                  reader.ReadSingle() / Settings.WorldScale); // Y
                doodadInstance.orientation          = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()); // (X, Y, Z, W)
                doodadInstance.scale                = reader.ReadSingle();      // scale factor
                doodadInstance.staticLightingColor  = reader.ReadBGRA();        // (B,G,R,A) overrides pc_sunColor

                DoodadInstances.Add(doodadInstance);
            }
        }

        // Fog information.
        public static void ReadMFOG(BinaryReader reader, int MFOGsize)
        {
            int numberOfFogs = MFOGsize / 48;
            List<FogInstance> FogInstances = new List<FogInstance>();
            for (int i = 0; i < numberOfFogs; ++i)
            {
                FogInstance fogInstance = new FogInstance();

                FogFlags fogFlags               = new FogFlags();
                byte[] arrayOfBytes             = new byte[1];
                reader.Read(arrayOfBytes, 0, 1);
                BitArray flags                  = new BitArray(arrayOfBytes);
                fogFlags.flag_infinite_radius   = flags[0];
                fogFlags.flag_0x10              = flags[4];
                fogInstance.flags               = fogFlags;

                fogInstance.position            = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                fogInstance.smaller_radius      = reader.ReadSingle(); // start
                fogInstance.larger_radius       = reader.ReadSingle(); // end
                fogInstance.landFog             = reader.ReadFogEditions();
                fogInstance.underwaterFog       = reader.ReadFogEditions();

                FogInstances.Add(fogInstance);
            }
        }

        // Convex Volume Planes. Contains blocks of floating-point numbers. 
        public static void ReadMCVP(BinaryReader reader, int MCVPsize) // optional
        {
            // These are used to define the volume of when you are inside this WMO. 
            // Important for transports. 
            //If a point is behind all planes (i.e. point-plane distance is negative for all planes), it is inside.
            List<C4Plane> ConvexVolumePlanes = new List<C4Plane>();
            for (int i = 0; i < MCVPsize / 16; i++)
            {
                C4Plane convexVolumePlane   = new C4Plane();
                convexVolumePlane.normal    = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                convexVolumePlane.distance  = reader.ReadSingle();
                ConvexVolumePlanes.Add(convexVolumePlane);
            }
        }

        // (Legion+) required when WMO is loaded from fileID (e.g. game objects)
        public static void ReadGFID(BinaryReader reader, int GFIDsize)
        {
            List<float> IDFlags = new List<float>();
            int flagCount = GFIDsize / sizeof(uint);
            for (int i = 0; i < flagCount; ++i)
            {
                IDFlags.Add(reader.ReadSingle());
            }
        }

    }
}