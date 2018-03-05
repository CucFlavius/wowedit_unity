using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static partial class WMO
{
    // group file header //
    public static void ReadMOGP(Stream WMOgroupstream)
    {
        int groupName = ReadLong(WMOgroupstream);                   // offset into MOGN
        if (wmoData.MOGNgroupnames.ContainsKey(groupName))
            groupDataBuffer.groupName = wmoData.MOGNgroupnames[groupName];
        else
            groupDataBuffer.groupName = "-noname-";

        int descriptiveGroupName = ReadLong(WMOgroupstream);        // offset into MOGN
        if (wmoData.MOGNgroupnames.ContainsKey(descriptiveGroupName))
            groupDataBuffer.descriptiveGroupName = wmoData.MOGNgroupnames[descriptiveGroupName];
        else
            groupDataBuffer.descriptiveGroupName = "-nodescriptivename-";

        groupDataBuffer.flags = ReadGroupFlags(WMOgroupstream);
        //Debug.Log(groupDataBuffer.flags.SMOGroupTVERTS2);
        groupDataBuffer.boundingBox = ReadBoundingBox(WMOgroupstream);  // as with flags, same as in corresponding MOGI entry
        groupDataBuffer.portalStart = ReadShort(WMOgroupstream);                // index into MOPR
        groupDataBuffer.portalCount = ReadShort(WMOgroupstream);                // number of MOPR items used after portalStart
        groupDataBuffer.transBatchCount = ReadShort(WMOgroupstream);            // RenderBatchCountA
        groupDataBuffer.intBatchCount = ReadShort(WMOgroupstream);              // RenderBatchCountInterior
        groupDataBuffer.extBatchCount = ReadShort(WMOgroupstream);              // RenderBatchCountExterior
        int padding_or_batch_type_d = ReadShort(WMOgroupstream);    // probably padding, but might be data?
        groupDataBuffer.fogIds = new List<int>();                         // ids in MFOG
        for (int i = 0; i < 4; i++)
        {
            groupDataBuffer.fogIds.Add(WMOgroupstream.ReadByte());
        }
        groupDataBuffer.groupLiquid = ReadLong(WMOgroupstream);                 // see below in the MLIQ chunk // LiquidType
        groupDataBuffer.uniqueID = ReadLong(WMOgroupstream);                    // foreign_keyⁱ<uint32_t, &WMOAreaTableRec::m_WMOGroupID>
        int flags2 = ReadLong(WMOgroupstream);
        int unused = ReadLong(WMOgroupstream);
    } // loaded

    // Material info for triangles, two bytes per triangle. //
    // So size of this chunk in bytes is twice the number of triangles in the WMO group. //
    public static void ReadMOPY(Stream WMOgroupstream, int MOPYsize)
    {
        groupDataBuffer.MOPYtriangleMaterialFlags = new List<TriangleMaterialFlags>();
        groupDataBuffer.MOPYtriangleMaterialIndex = new List<int>();
        for (int i = 0; i < MOPYsize / 2; i++)
        {
            groupDataBuffer.MOPYtriangleMaterialFlags.Add(ReadTriangleMaterialFlags(WMOgroupstream));
            groupDataBuffer.MOPYtriangleMaterialIndex.Add(WMOgroupstream.ReadByte());
        }
    } // loaded

    // Vertex indices for triangles //
    public static void ReadMOVI(Stream WMOgroupstream, int MOVIsize)
    {
        int nIndices = MOVIsize / 2;
        groupDataBuffer.triangles = new uint[nIndices];
        for (int i = 0; i < nIndices; i++)
        {
            groupDataBuffer.triangles[i] = ReadUint(WMOgroupstream);
        }
    } // loaded

    // Vertices chunk //
    public static void ReadMOVT(Stream WMOgroupstream, int MOVTsize)
    {
        int nVertices = (MOVTsize / 4) / 3;
        groupDataBuffer.vertices = new Vector3[nVertices];
        for (int i = 0; i < nVertices; i++)
        {
            Vector3 positions = new Vector3(ReadFloat(WMOgroupstream) / Settings.worldScale, ReadFloat(WMOgroupstream) / Settings.worldScale, ReadFloat(WMOgroupstream) / Settings.worldScale);
            groupDataBuffer.vertices[i] = new Vector3(-positions.x, positions.z, -positions.y);
        }
    } // loaded

    // Normals chunk //
    public static void ReadMONR(Stream WMOgroupstream, int MONRsize)
    {
        int nNormals = (MONRsize / 4) / 3;
        groupDataBuffer.normals = new Vector3[nNormals];
        for (int i = 0; i < nNormals; i++)
        {
            Vector3 readNormals = new Vector3(ReadFloat(WMOgroupstream), ReadFloat(WMOgroupstream), ReadFloat(WMOgroupstream));
            groupDataBuffer.normals[i] = new Vector3(-readNormals.x, readNormals.z, -readNormals.y);
        }
    } // loaded

    // Texture coordinates, 2 floats per vertex in (X,Y) order. //
    public static void ReadMOTV(Stream WMOgroupstream, int MOTVsize)
    {
        if (MOTVsize != 0)
        {
            int nTextureCoordinates = (MOTVsize / 4) / 2;
            groupDataBuffer.UVs = new Vector2[nTextureCoordinates];
            for (int i = 0; i < nTextureCoordinates; i++)
            {
                groupDataBuffer.UVs[i] = new Vector2(ReadFloat(WMOgroupstream), ReadFloat(WMOgroupstream));
            }
        }
    } // loaded

    // Render batches. Records of 24 bytes. //
    public static void ReadMOBA(Stream WMOgroupstream, int MOBAsize)
    {
        groupDataBuffer.batch_StartIndex = new List<uint>();
        groupDataBuffer.batch_nIndices = new List<uint>();
        groupDataBuffer.batch_StartVertex = new List<uint>();
        groupDataBuffer.batch_EndVertex = new List<uint>();
        groupDataBuffer.batchMaterialIDs = new List<int>();

        var nBatches = MOBAsize / 24;
        groupDataBuffer.nBatches = nBatches;
        for (int i = 0; i < nBatches; i++)
        {
            // 10x uint8_t unknown
            ReadLong(WMOgroupstream);
            ReadLong(WMOgroupstream);
            ReadShort(WMOgroupstream);
            int material_id_large = ReadShort(WMOgroupstream);  // used if flag_use_uint16_t_material is set

            groupDataBuffer.batch_StartIndex.Add(ReadUintLong(WMOgroupstream));     // index of the first face index used in MOVI
            groupDataBuffer.batch_nIndices.Add(ReadUint(WMOgroupstream));      // number of MOVI indices used
            groupDataBuffer.batch_StartVertex.Add(ReadUint(WMOgroupstream));   // index of the first vertex used in MOVT
            groupDataBuffer.batch_EndVertex.Add(ReadUint(WMOgroupstream));     // index of the last vertex used (batch includes this one)

            int flag_use_material_id_large = WMOgroupstream.ReadByte(); // ≥Legion // instead of material_id use material_id_large // if byte==0 flag false I think
            int material_id = WMOgroupstream.ReadByte();
            if (flag_use_material_id_large == 0)
                groupDataBuffer.batchMaterialIDs.Add(material_id);
            else
                groupDataBuffer.batchMaterialIDs.Add(material_id_large);
        }
    } // loaded

    public static void ReadMOCV(Stream WMOrootstream, int MOCVsize)
    {
        groupDataBuffer.vertexColors = new List<Color32>();
        int count = MOCVsize / 4;
        for (int i = 0; i < count; i++)
        {
            BGRA bgra = ReadBGRA(WMOrootstream);
            Color32 color = new Color32(bgra.R, bgra.G, bgra.B, bgra.A);
            groupDataBuffer.vertexColors.Add(color);
        }
    }


    public static void ReadMOCV2(Stream WMOrootstream, int chunkSize)
    {
        Debug.Log("Skipping MOCV2");
        WMOrootstream.Seek(chunkSize, SeekOrigin.Current);
    }

    public static void ReadMOTV2(Stream WMOrootstream, int chunkSize)
    {
        Debug.Log("Skipping MOTV2");
        WMOrootstream.Seek(chunkSize, SeekOrigin.Current);
    }

    public static void ReadMOTV3(Stream WMOrootstream, int chunkSize)
    {
        Debug.Log("Skipping MOTV3");
        WMOrootstream.Seek(chunkSize, SeekOrigin.Current);
    }

}
