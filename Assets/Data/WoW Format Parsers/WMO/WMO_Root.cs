using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class WMO
{
    // root file version //
    public static void ReadMVER(Stream WMOrootstream)
    {
        int WMOfileversion = ReadLong(WMOrootstream);
    }

    // root file header // 64 bytes
    public static void ReadMOHD(Stream WMOrootstream)
    {
        wmoData.Info.nTextures = ReadLong(WMOrootstream);           // number of textures used
        wmoData.Info.nGroups = ReadLong(WMOrootstream);             // number of groups
        wmoData.Info.nPortals = ReadLong(WMOrootstream);            // number of portals
        wmoData.Info.nLights = ReadLong(WMOrootstream);             // number of lights 
                                                                    //Blizzard seems to add one to the MOLT entry count when there are MOLP chunks in the groups (and maybe for MOLS too?)
        int nDoodadNames = ReadLong(WMOrootstream);                 // number of doodad names
        int nDoodadDefs = ReadLong(WMOrootstream);                  // number of doodad definitions
        int nDoodadSets = ReadLong(WMOrootstream);                  // number of doodad sets
        ARGB ambColor = ReadARGB(WMOrootstream);                    // Color settings for base (ambient) color. See the flag at /*03Ch*/.   // ARGB
        wmoData.Info.wmoID = ReadLong(WMOrootstream);                        // < uint32_t, &WMOAreaTableRec::m_WMOID> ID located in DBC
        BoundingBox bounding_box = ReadBoundingBox(WMOrootstream);  // in the alpha, this bounding box was computed upon loading
        // <Flags> 2 bytes                                                            
        byte[] arrayOfBytes = new byte[2];
        WMOrootstream.Read(arrayOfBytes, 0, 2);
        BitArray flags = new BitArray(arrayOfBytes);
        bool flag_attenuate_vertices_based_on_distance_to_portal = flags[0];
        bool flag_skip_base_color = flags[1];                       // do not add base (ambient) color (of MOHD) to MOCVs. apparently does more, e.g. required for multiple MOCVs
        bool flag_use_liquid_type_dbc_id = flags[2];                // use real liquid type ID from DBCs instead of local one. See MLIQ for further reference.
        bool flag_lighten_interiors = flags[3];                     // makes iterior groups much brighter, effects MOCV rendering. Used e.g.in Stormwind for having shiny bright interiors,
        bool Flag_Lod = flags[4];                                   // ≥ Legion (20740)
        // 11 bits unused flags                                     // unused as of Legion (20994)
        // </Flags>
        int numLod = ReadShort(WMOrootstream);                      // ≥ Legion (21108) includes base lod (→ numLod = 3 means '.wmo', 'lod0.wmo' and 'lod1.wmo')


    }

    // texture paths //
    public static void ReadMOTX(Stream WMOrootstream, int MOTXsize)
    {
        long currentPos = WMOrootstream.Position;

        while (WMOrootstream.Position < currentPos + MOTXsize)
        {
            int position = (int)(WMOrootstream.Position - currentPos);
            string path = ReadNullTerminatedString(WMOrootstream);
            if (path != "" )//&& !wmoData.textureData.ContainsKey(path))
            {
                //Debug.Log(path);
                wmoData.texturePaths.Add(position, path);
                string extractedPath = Casc.GetFile(path);
                if (File.Exists(extractedPath))
                {
                    Stream stream = File.Open(extractedPath, FileMode.Open);
                    BLP blp = new BLP();
                    byte[] data = blp.GetUncompressed(stream, true);
                    BLPinfo info = blp.Info();
                    Texture2Ddata texture2Ddata = new Texture2Ddata();
                    texture2Ddata.hasMipmaps = info.hasMipmaps;
                    texture2Ddata.width = info.width;
                    texture2Ddata.height = info.height;
                    texture2Ddata.textureFormat = info.textureFormat;
                    texture2Ddata.TextureData = data;
                    wmoData.textureData[path] = texture2Ddata;
                    stream.Close();
                    stream = null;
                }
            }
        }
    } // loaded

    // materials //
    public static void ReadMOMT(Stream WMOrootstream, int MOMTsize) 
    {
        wmoData.Info.nMaterials = MOMTsize / 64;
        for (int i = 0; i < wmoData.Info.nMaterials; i++)
        {
            WMOMaterial material = new WMOMaterial();

            material.flags = ReadMaterialFlags(WMOrootstream);
            material.shader = ReadLong(WMOrootstream);  // Index into CMapObj::s_wmoShaderMetaData. See below (shader types).
            material.blendMode = (BlendingMode)ReadLong(WMOrootstream);         // Blending: see https://wowdev.wiki/Rendering#EGxBlend

            material.texture1_offset = ReadLong(WMOrootstream);                          // Diffuse Texture; offset into MOTX
            material.color1 = ReadRGBA(WMOrootstream);                                  // emissive color; see below (emissive color)
            material.texture1_flags = ReadMaterialFlags(WMOrootstream);

            material.texture2_offset = ReadLong(WMOrootstream);                          // Environment Texture; envNameIndex; offset into MOTX
            material.color2 = ReadRGBA(WMOrootstream);                                  // diffuse color; CWorldView::GatherMapObjDefGroupLiquids(): geomFactory->SetDiffuseColor((CImVectorⁱ*)(smo+7));
            // environment textures don't need flags

            material.ground_type = ReadLong(WMOrootstream);                              // foreign_keyⁱ< uint32_t, &TerrainTypeRec::m_ID > ground_type; // according to CMapObjDef::GetGroundType 
            material.texture3_offset = ReadLong(WMOrootstream);                          // offset into MOTX
            material.color3 = ReadRGBA(WMOrootstream);
            material.texture3_flags = ReadMaterialFlags(WMOrootstream);

            // skip runtime data //
            WMOrootstream.Seek(16, SeekOrigin.Current);

            wmoData.materials.Add(material);
        }
    } // loaded

    // texture translation animations //
    public static void ReadMOUV(Stream WMOrootstream) 
    {
        // Currently, only a translating animation is possible for two of the texture layers.
        for (int i = 0; i < wmoData.Info.nMaterials; i++)
        {
            Vector2 translation_speed = new Vector2(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream));
        }
        // The formula from translation_speed values to TexMtx translation values is along the lines of:
        // a_i = translation_i ? 1000 / translation_i : 0
        // b_i = a_i ? (a_i < 0 ? (1 - (time ? % -a_i) / -a_i) : ((time ? % a_i) / a_i)) : 0
    }

    // list of group names for the groups in this map object //
    public static void ReadMOGN(Stream WMOrootstream, int MOGNsize)
    {
        long currentPos = WMOrootstream.Position;
        // starts with two 0x00
        WMOrootstream.Seek(2, SeekOrigin.Current);
        while (WMOrootstream.Position < currentPos+MOGNsize)
        {
            wmoData.MOGNgroupnames.Add((int)(WMOrootstream.Position - currentPos), ReadNullTerminatedString(WMOrootstream));
        }
    } // loaded

    // Group information for WMO groups
    public static void ReadMOGI(Stream WMOrootstream)
    {
        for (int a = 0; a < wmoData.Info.nGroups; a++)
        {
            int flags = ReadLong(WMOrootstream);                                            //  see information in in MOGP, they are equivalent
            BoundingBox boundingBox = ReadBoundingBox(WMOrootstream);                       // group bounding box
            //wmoData.MOGIgroupnames.Add(wmoData.MOGNgroupnames[ReadLong(WMOrootstream)]);    // name in MOGN chunk (-1 for no name)
            int nameOffset = ReadLong(WMOrootstream);
        }
    } // exists in group

    // Skybox model filename
    public static void ReadMOSB(Stream WMOrootstream, int MOSBsize) 
    {
        string skyboxName = null;
        for (int a = 0; a < MOSBsize; a++)
        {
            int b = WMOrootstream.ReadByte();
            if (b != 0)
            {
                var stringSymbol = System.Convert.ToChar(b);
                skyboxName = skyboxName + stringSymbol;
            }
            else
            {
                WMOrootstream.Seek(3, SeekOrigin.Current);
            }
        }
    }

    // Portal vertices
    public static void ReadMOPV(Stream WMOrootstream, int MOPVsize)
    {
        long currentPosition = WMOrootstream.Position;
        List<Vector3> PortalVertices = new List<Vector3>();
        while (WMOrootstream.Position < MOPVsize + currentPosition)
        {
            PortalVertices.Add(new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream)));
        }
    }

    // Portal information.
    public static void ReadMOPT(Stream WMOrootstream)
    {
        List<SMOPortal> PortalInfo = new List<SMOPortal>();
        for (int i = 0; i < wmoData.Info.nPortals; ++i)
        {
            SMOPortal portal = new SMOPortal();
            portal.startVertex = ReadShort(WMOrootstream);
            portal.count = ReadShort(WMOrootstream);
            C4Plane plane = new C4Plane();
            plane.normal = new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream));
            plane.distance = ReadFloat(WMOrootstream);
            portal.plane = plane;
            PortalInfo.Add(portal);
        }
    }

    // Map Object Portal References from groups. 
    public static void ReadMOPR(Stream WMOrootstream, int MOPRsize)
    {
        int portalReferenceCount = MOPRsize / 8;
        for (int i = 0; i < portalReferenceCount; ++i)
        {
            int PortalIndex = ReadShort(WMOrootstream); // into MOPR
            int GroupIndex = ReadShort(WMOrootstream);  // the other one
            int Side = ReadShort(WMOrootstream);        // positive or negative.
            int Unknown = ReadShort(WMOrootstream);
        }
    }

    // Visible block vertices
    public static void ReadMOVV(Stream WMOrootstream, int MOVVsize)
    {
        List<Vector3> VisibleVertices = new List<Vector3>();
        int vertexCount = MOVVsize / 12;
        for (int i = 0; i < vertexCount; ++i)
        {
            VisibleVertices.Add(new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream)));
        }
    }

    // Visible block list
    public static void ReadMOVB(Stream WMOrootstream, int MOVBsize)
    {
        List<VisibleBlock> VisibleBlocks = new List<VisibleBlock>();
        int blockCount = MOVBsize / 4;
        for (int i = 0; i < blockCount; ++i)
        {
            VisibleBlock block = new VisibleBlock();
            block.firstVertex = ReadShort(WMOrootstream);
            block.count = ReadShort(WMOrootstream);
            VisibleBlocks.Add(block);
        }
    }

    // Lighting information.
    public static void ReadMOLT(Stream WMOrootstream)
    {
        List<StaticLight> StaticLights = new List<StaticLight>();
        int lightsCount = wmoData.Info.nLights;
        for (int i = 0; i < lightsCount; ++i)
        {
            StaticLight staticLight = new StaticLight();
            staticLight.type = (LightType)WMOrootstream.ReadByte();
            staticLight.useAttenuation = WMOrootstream.ReadByte(); // boolean - true if byte != 0
            staticLight.useUnknown1 = WMOrootstream.ReadByte(); // boolean - true if byte != 0
            staticLight.useUnknown2 = WMOrootstream.ReadByte(); // boolean - true if byte != 0
            staticLight.color = ReadBGRA(WMOrootstream);
            staticLight.position = new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream));
            staticLight.intensity = ReadFloat(WMOrootstream);
            staticLight.attenuationStart = ReadFloat(WMOrootstream);
            staticLight.attenuationEnd = ReadFloat(WMOrootstream);
            staticLight.unknown1StartRadius = ReadFloat(WMOrootstream);
            staticLight.unknown1EndRadius = ReadFloat(WMOrootstream);
            staticLight.unknown2StartRadius = ReadFloat(WMOrootstream);
            staticLight.unknown2EndRadius = ReadFloat(WMOrootstream);
            StaticLights.Add(staticLight);
        }
    }

    // This chunk defines doodad sets.
    public static void ReadMODS(Stream WMOrootstream, int MODSsize)
    {
        int numberOfSets = MODSsize / 32;
        List<DoodadSet> DoodadSets = new List<DoodadSet>();
        for (int i = 0; i < numberOfSets; ++i)
        {
            DoodadSet doodadSet = new DoodadSet();
            byte[] nameBytes = new byte[20];
            WMOrootstream.Read(nameBytes, 0, 20);
            doodadSet.name = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
            doodadSet.firstDoodadInstanceIndex = ReadLong(WMOrootstream);
            doodadSet.doodadInstanceCount = ReadLong(WMOrootstream);
            int Unused = ReadLong(WMOrootstream);
            DoodadSets.Add(doodadSet);
        }
    }

    //List of filenames for M2(They are listed as MDX) models
    public static void ReadMODN(Stream WMOrootstream, int MODNsize)
    {
        string m2Path = "";
        List<string> m2Paths = new List<string>();
        for (int a = 0; a < MODNsize; a++)
        {
            int b = WMOrootstream.ReadByte();
            if (b != 0)
            {
                var stringSymbol = System.Convert.ToChar(b);
                m2Path = m2Path + stringSymbol;
            }
            else if (b == 0)
            {
                if ((WMOrootstream.Position) % 4 == 0)
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
    public static void ReadMODD(Stream WMOrootstream, int MODDsize)
    {
        int numberOfDoodadInstances = MODDsize / 40;
        List<DoodadInstanceInfo> DoodadInstances = new List<DoodadInstanceInfo>();
        for (int i = 0; i < numberOfDoodadInstances; ++i)
        {
            DoodadInstanceInfo doodadInstance = new DoodadInstanceInfo();
            byte[] finalNameBytes = new byte[4];
            byte[] nameOffsetBytes = new byte[3];
            WMOrootstream.Read(nameOffsetBytes, 0, 3);
            Buffer.BlockCopy(nameOffsetBytes, 0, finalNameBytes, 0, 3);
            doodadInstance.nameOffset = BitConverter.ToUInt32(finalNameBytes, 0); // reference offset into MODN

            DoodadInstanceFlags doodadInstanceFlags = new DoodadInstanceFlags();
            byte[] arrayOfBytes = new byte[1];
            WMOrootstream.Read(arrayOfBytes, 0, 1);
            BitArray flags = new BitArray(arrayOfBytes);
            doodadInstanceFlags.AcceptProjectedTexture = flags[0];
            doodadInstanceFlags.Unknown1 = flags[1]; // MapStaticEntity::field_34 |= 1 (if set, MapStaticEntity::AdjustLighting is _not_ called)
            doodadInstanceFlags.Unknown2 = flags[2];
            doodadInstanceFlags.Unknown3 = flags[3];
            doodadInstance.flags = doodadInstanceFlags;

            doodadInstance.position = new Vector3(ReadFloat(WMOrootstream) / Settings.worldScale, ReadFloat(WMOrootstream) / Settings.worldScale, ReadFloat(WMOrootstream) / Settings.worldScale); // (X,Z,-Y)
            doodadInstance.orientation = new Quaternion(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream)); // (X, Y, Z, W)
            doodadInstance.scale = ReadFloat(WMOrootstream); // scale factor
            doodadInstance.staticLightingColor = ReadBGRA(WMOrootstream); // (B,G,R,A) overrides pc_sunColor

            DoodadInstances.Add(doodadInstance);
        }
    }

    // Fog information.
    public static void ReadMFOG(Stream WMOrootstream, int MFOGsize)
    {
        int numberOfFogs = MFOGsize / 48;
        List<FogInstance> FogInstances = new List<FogInstance>();
        for (int i = 0; i < numberOfFogs; ++i)
        {
            FogInstance fogInstance = new FogInstance();

            FogFlags fogFlags = new FogFlags();
            byte[] arrayOfBytes = new byte[1];
            WMOrootstream.Read(arrayOfBytes, 0, 1);
            BitArray flags = new BitArray(arrayOfBytes);
            fogFlags.flag_infinite_radius = flags[0];
            fogFlags.flag_0x10 = flags[4];
            fogInstance.flags = fogFlags;

            fogInstance.position = new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream));
            fogInstance.smaller_radius = ReadFloat(WMOrootstream); // start
            fogInstance.larger_radius = ReadFloat(WMOrootstream); // end
            fogInstance.landFog = ReadFogDefinition(WMOrootstream);
            fogInstance.underwaterFog = ReadFogDefinition(WMOrootstream);

            FogInstances.Add(fogInstance);
        }
    }

    // Convex Volume Planes. Contains blocks of floating-point numbers. 
    public static void ReadMCVP(Stream WMOrootstream, int MCVPsize) // optional
    {
        // These are used to define the volume of when you are inside this WMO. 
        // Important for transports. 
        //If a point is behind all planes (i.e. point-plane distance is negative for all planes), it is inside.
        List<C4Plane> ConvexVolumePlanes = new List<C4Plane>();
        for (int i = 0; i < MCVPsize/16; i++)
        {
            C4Plane convexVolumePlane = new C4Plane();
            convexVolumePlane.normal = new Vector3(ReadFloat(WMOrootstream), ReadFloat(WMOrootstream), ReadFloat(WMOrootstream));
            convexVolumePlane.distance = ReadFloat(WMOrootstream);
            ConvexVolumePlanes.Add(convexVolumePlane);
        }
    }

    // (Legion+) required when WMO is loaded from fileID (e.g. game objects)
    public static void ReadGFID(Stream WMOrootstream, int GFIDsize)
    {
        List<int> IDFlags = new List<int>();
        int flagCount = GFIDsize / sizeof(uint);
        for (int i = 0; i < flagCount; ++i)
        {
            IDFlags.Add(ReadLong(WMOrootstream));
        }
    }

}

