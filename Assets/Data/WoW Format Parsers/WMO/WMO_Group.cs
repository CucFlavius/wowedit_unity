using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.WoWEditSettings;

namespace Assets.Data.WoW_Format_Parsers.WMO
{
    public static partial class WMO
    {
        // group file header //
        public static void ReadMOGP(BinaryReader reader)
        {
            int groupName                       = reader.ReadInt32();       // offset into MOGN
            if (wmoData.MOGNgroupnames.ContainsKey(groupName))
                groupDataBuffer.groupName = wmoData.MOGNgroupnames[groupName];
            else
                groupDataBuffer.groupName = "-noname-";

            int descriptiveGroupName            = reader.ReadInt32();       // offset into MOGN
            if (wmoData.MOGNgroupnames.ContainsKey(descriptiveGroupName))
                groupDataBuffer.descriptiveGroupName = wmoData.MOGNgroupnames[descriptiveGroupName];
            else
                groupDataBuffer.descriptiveGroupName = "-nodescriptivename-";

            groupDataBuffer.flags           = reader.ReadGroupFlags();
            //Debug.Log(groupDataBuffer.flags.SMOGroupTVERTS2);
            groupDataBuffer.boundingBox     = reader.ReadBoundingBoxes();   // as with flags, same as in corresponding MOGI entry
            groupDataBuffer.portalStart     = reader.ReadInt16();           // index into MOPR
            groupDataBuffer.portalCount     = reader.ReadInt16();           // number of MOPR items used after portalStart
            groupDataBuffer.transBatchCount = reader.ReadInt16();           // RenderBatchCountA
            groupDataBuffer.intBatchCount   = reader.ReadInt16();           // RenderBatchCountInterior
            groupDataBuffer.extBatchCount   = reader.ReadInt16();           // RenderBatchCountExterior
            int padding_or_batch_type_d     = reader.ReadInt16();           // probably padding, but might be data?
            groupDataBuffer.fogIds = new List<int>();                       // ids in MFOG
            for (int i = 0; i < 4; i++)
            {
                groupDataBuffer.fogIds.Add(reader.ReadByte());
            }
            groupDataBuffer.groupLiquid     = reader.ReadInt32();           // see below in the MLIQ chunk // LiquidType
            groupDataBuffer.uniqueID        = reader.ReadInt32();           // foreign_keyⁱ<uint32_t, &WMOAreaTableRec::m_WMOGroupID>
            int flags2                      = reader.ReadInt32();
            int unused                      = reader.ReadInt32();
        }   // loaded

        // Material info for triangles, two bytes per triangle. //
        // So size of this chunk in bytes is twice the number of triangles in the WMO group. //
        public static void ReadMOPY(BinaryReader reader, int MOPYsize)
        {
            groupDataBuffer.MOPYtriangleMaterialFlags = new List<TriangleMaterialFlags>();
            groupDataBuffer.MOPYtriangleMaterialIndex = new List<int>();
            for (int i = 0; i < MOPYsize / 2; i++)
            {
                groupDataBuffer.MOPYtriangleMaterialFlags.Add(reader.ReadTriangleMaterialFlags());
                groupDataBuffer.MOPYtriangleMaterialIndex.Add(reader.ReadByte());
            }
        }   // loaded

        // Vertex indices for triangles //
        public static void ReadMOVI(BinaryReader reader, int MOVIsize)
        {
            int nIndices = MOVIsize / 2;
            groupDataBuffer.triangles = new uint[nIndices];
            for (int i = 0; i < nIndices; i++)
            {
                groupDataBuffer.triangles[i] = reader.ReadUInt32();
            }
        } // loaded

        // Vertices chunk //
        public static void ReadMOVT(BinaryReader reader, int MOVTsize)
        {
            int nVertices = (MOVTsize / 4) / 3;
            groupDataBuffer.vertices = new Vector3[nVertices];
            for (int i = 0; i < nVertices; i++)
            {
                Vector3 positions = new Vector3(reader.ReadSingle() / SettingsManager<Configuration>.Config.WorldSettings.WorldScale, 
                    reader.ReadSingle() / SettingsManager<Configuration>.Config.WorldSettings.WorldScale, 
                    reader.ReadSingle() / SettingsManager<Configuration>.Config.WorldSettings.WorldScale);
                groupDataBuffer.vertices[i] = new Vector3(-positions.x, positions.z, -positions.y);
            }
        } // loaded

        // Normals chunk //
        public static void ReadMONR(BinaryReader reader, int MONRsize)
        {
            int nNormals = (MONRsize / 4) / 3;
            groupDataBuffer.normals = new Vector3[nNormals];
            for (int i = 0; i < nNormals; i++)
            {
                Vector3 readNormals = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                groupDataBuffer.normals[i] = new Vector3(-readNormals.x, readNormals.z, -readNormals.y);
            }
        } // loaded

        // Texture coordinates, 2 floats per vertex in (X,Y) order. //
        public static void ReadMOTV(BinaryReader reader, int MOTVsize)
        {
            if (MOTVsize != 0)
            {
                int nTextureCoordinates = (MOTVsize / 4) / 2;
                groupDataBuffer.UVs = new Vector2[nTextureCoordinates];
                for (int i = 0; i < nTextureCoordinates; i++)
                {
                    groupDataBuffer.UVs[i] = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                }
            }
        } // loaded

        // Render batches. Records of 24 bytes. //
        public static void ReadMOBA(BinaryReader reader, int MOBAsize)
        {
            groupDataBuffer.batch_StartIndex    = new List<uint>();
            groupDataBuffer.batch_Count         = new List<uint>();
            groupDataBuffer.batch_MinIndex      = new List<uint>();
            groupDataBuffer.batch_MaxIndex      = new List<uint>();
            groupDataBuffer.batchMaterialIDs    = new List<int>();

            var nBatches                = MOBAsize / 24;
            groupDataBuffer.nBatches    = nBatches;
            for (int i = 0; i < nBatches; i++)
            {
                sbyte[] box = new sbyte[10];
                for (int j = 0; j < box.Length; j++)
                    box[j] = reader.ReadSByte();

                int material_id_large               = reader.ReadInt16();           // used if flag_use_uint16_t_material is set

                groupDataBuffer.batch_StartIndex.Add(reader.ReadUInt32());          // index of the first face index used in MOVI
                groupDataBuffer.batch_Count.Add(reader.ReadUInt16());               // number of MOVI indices used
                groupDataBuffer.batch_MinIndex.Add(reader.ReadUInt16());            // index of the first vertex used in MOVT
                groupDataBuffer.batch_MaxIndex.Add(reader.ReadUInt16());            // index of the last vertex used (batch includes this one)
                bool flag_use_material_id_large     = reader.ReadBoolean();         // ≥Legion // instead of material_id use material_id_large // if byte==0 flag false I think
                int material_id                     = reader.ReadByte();

                if (flag_use_material_id_large == false)
                    groupDataBuffer.batchMaterialIDs.Add(material_id);
                else
                    groupDataBuffer.batchMaterialIDs.Add(material_id_large);
            }
        }   // loaded

        public static void ReadMOCV(BinaryReader reader, int MOCVsize)
        {
            groupDataBuffer.vertexColors = new List<Color32>();
            int count = MOCVsize / 4;
            for (int i = 0; i < count; i++)
            {
                BGRA bgra = reader.ReadBGRA();
                Color32 color = new Color32(bgra.R, bgra.G, bgra.B, bgra.A);
                groupDataBuffer.vertexColors.Add(color);
            }
        }


        public static void ReadMOCV2(BinaryReader reader, int chunkSize)
        {
            //Debug.Log("Skipping MOCV2");
            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }

        public static void ReadMOTV2(BinaryReader reader, int chunkSize)
        {
            //Debug.Log("Skipping MOTV2");
            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }

        public static void ReadMOTV3(BinaryReader reader, int chunkSize)
        {
            //Debug.Log("Skipping MOTV3");
            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }

    }
}