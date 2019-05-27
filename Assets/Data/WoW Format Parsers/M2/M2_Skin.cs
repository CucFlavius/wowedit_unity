using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using static Assets.Data.WoW_Format_Parsers.M2.M2_Data;
using Assets.Data;
using Assets.Data.WoW_Format_Parsers.M2;
using Assets.WoWEditSettings;

public static partial class M2
{
    public static void ParseSkin(BinaryReader reader, M2Data m2Data)
    {
        string magic        = reader.ReadFourCC();              // 'SKIN'
        M2Array vertices    = reader.ReadM2Array();
        M2Array indices     = reader.ReadM2Array();
        M2Array bones       = reader.ReadM2Array();
        M2Array submeshes   = reader.ReadM2Array();
        M2Array batches     = reader.ReadM2Array();             // nTexture_units
        int boneCountMax    = reader.ReadInt32();               // WoW takes this and divides it by the number of bones in each submesh, then stores the biggest one.
                                                                // Maximum number of bones per drawcall for each view. Related to (old) GPU numbers of registers. 
                                                                // Values seen : 256, 64, 53, 21
        M2Array shadow_batches = reader.ReadM2Array();

        /// Read Batches ///
        reader.BaseStream.Seek(batches.Offset, SeekOrigin.Begin);
        for (var batch = 0; batch < batches.Size; batch++)
        {
            M2BatchIndices m2BatchIndices = new M2BatchIndices();

            m2BatchIndices.M2Batch_flags                        = reader.ReadByte();        // Usually 16 for static textures, and 0 for animated textures. &0x1: materials invert something; &0x2: transform &0x4: projected texture; &0x10: something batch compatible; &0x20: projected texture?; &0x40: use textureWeights
            m2BatchIndices.M2Batch_priorityPlane                = reader.ReadByte(); 
            m2BatchIndices.M2Batch_shader_id                    = reader.ReadUInt16();      // See below.
            m2BatchIndices.M2Batch_skinSectionIndex             = reader.ReadUInt16();      // A duplicate entry of a submesh from the list above.
            m2BatchIndices.M2Batch_geosetIndex                  = reader.ReadUInt16();      // See below.
            m2BatchIndices.M2Batch_color_index                  = reader.ReadUInt16();      // A Color out of the Colors-Block or -1 if none.
            m2BatchIndices.M2Batch_materialIndex                = reader.ReadUInt16();      // The renderflags used on this texture-unit.
            m2BatchIndices.M2Batch_materialLayer                = reader.ReadUInt16();      // Capped at 7 (see CM2Scene::BeginDraw)
            m2BatchIndices.M2Batch_textureCount                 = reader.ReadUInt16();      // 1 to 4. See below. Also seems to be the number of textures to load, starting at the texture lookup in the next field (0x10).
            m2BatchIndices.M2Batch_textureComboIndex            = reader.ReadUInt16();      // Index into Texture lookup table
            m2BatchIndices.M2Batch_textureCoordComboIndex       = reader.ReadUInt16();      // Index into the texture unit lookup table.
            m2BatchIndices.M2Batch_textureWeightComboIndex      = reader.ReadUInt16();      // Index into transparency lookup table.
            m2BatchIndices.M2Batch_textureTransformComboIndex   = reader.ReadUInt16();      // Index into uvanimation lookup table. 

            m2Data.m2BatchIndices.Add(m2BatchIndices);
        }

        // Read SubMesh Data //
        int[] Indices                           = new int[vertices.Size];            // Three indices which make up a triangle.
        int[] Triangles                         = new int[indices.Size];          // Bone indices (Index into BoneLookupTable)

        int[] skinSectionId                     = new int[submeshes.Size];          // Mesh part ID, see below.
        int[] submesh_StartVertex               = new int[submeshes.Size];          // Starting vertex number.
        int[] submesh_NbrVerts                  = new int[submeshes.Size];          // Number of vertices.
        int[] submesh_StartTriangle             = new int[submeshes.Size];          // Starting triangle index (that's 3* the number of triangles drawn so far).
        int[] submesh_NbrTris                   = new int[submeshes.Size];          // Number of triangle indices.

        int[] submesh_boneCount                 = new int[submeshes.Size];          // Number of elements in the bone lookup table. Max seems to be 256 in Wrath. Shall be ≠ 0.
        int[] submesh_boneComboIndex            = new int[submeshes.Size];          // Starting index in the bone lookup table.
        int[] submesh_boneInfluences            = new int[submeshes.Size];          // <= 4
                                                                                    // from <=BC documentation: Highest number of bones needed at one time in this Submesh --Tinyn (wowdev.org) 
                                                                                    // In 2.x this is the amount of of bones up the parent-chain affecting the submesh --NaK
                                                                                    // Highest number of bones referenced by a vertex of this submesh. 3.3.5a and suspectedly all other client revisions. -- Skarn
        int[] submesh_centerBoneIndex           = new int[submeshes.Size];
        Vector3[] submesh_centerPosition        = new Vector3[submeshes.Size];      // Average position of all the vertices in the sub mesh.
        Vector3[] submesh_sortCenterPosition    = new Vector3[submeshes.Size];      // The center of the box when an axis aligned box is built around the vertices in the submesh.
        float[] submesh_sortRadius              = new float[submeshes.Size];        // Distance of the vertex farthest from CenterBoundingBox.

        /// Indices ///
        reader.BaseStream.Seek(vertices.Offset, SeekOrigin.Begin);
        for (var ind = 0; ind < vertices.Size; ind++)
        {
            Indices[ind] = reader.ReadUInt16();
        }

        /// triangles ///
        reader.BaseStream.Seek(indices.Offset, SeekOrigin.Begin);
        for (var tri = 0; tri < indices.Size; tri++)
        {
            Triangles[tri] = reader.ReadUInt16();
        }

        /// submeshes ///
        reader.BaseStream.Seek(submeshes.Offset, SeekOrigin.Begin);
        for (var sub = 0; sub < submeshes.Size; sub++)
        {
            skinSectionId[sub]              = reader.ReadUInt16();
            int Level                       = reader.ReadUInt16();              // (level << 16) is added (|ed) to startTriangle and alike to avoid having to increase those fields to uint32s.
            submesh_StartVertex[sub]        = reader.ReadUInt16() + (Level << 16);
            submesh_NbrVerts[sub]           = reader.ReadUInt16();
            submesh_StartTriangle[sub]      = reader.ReadUInt16() + (Level << 16);
            submesh_NbrTris[sub]            = reader.ReadUInt16();

            submesh_boneCount[sub]          = reader.ReadUInt16();
            submesh_boneComboIndex[sub]     = reader.ReadUInt16();
            submesh_boneInfluences[sub]     = reader.ReadUInt16();
            submesh_centerBoneIndex[sub]    = reader.ReadUInt16();

            Vector3 canterPosition          = new Vector3(reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale);
            submesh_centerPosition[sub]     = new Vector3(-canterPosition.x, canterPosition.z, -canterPosition.y);
            Vector3 sortCenterPosition      = new Vector3(reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale);
            submesh_sortCenterPosition[sub] = new Vector3(-sortCenterPosition.x, sortCenterPosition.z, -sortCenterPosition.y);

            submesh_sortRadius[sub]         = reader.ReadSingle();
        }

        /// Assemble Submeshes ///
        m2Data.submeshData = new List<SubmeshData>();
        for (int sm = 0; sm < submeshes.Size; sm++)
        {
            Vector3[] vertList  = new Vector3[submesh_NbrVerts[sm]];
            Vector3[] normsList = new Vector3[submesh_NbrVerts[sm]];
            Vector2[] uvsList   = new Vector2[submesh_NbrVerts[sm]];
            Vector2[] uvs2List  = new Vector2[submesh_NbrVerts[sm]];

            BoneWeights[] boneWeights = new BoneWeights[submesh_NbrVerts[sm]];

            for (int vn = 0; vn < submesh_NbrVerts[sm]; vn++)
            {
                vertList[vn]    = m2Data.meshData.pos[vn + submesh_StartVertex[sm]];
                normsList[vn]   = m2Data.meshData.normal[vn + submesh_StartVertex[sm]];
                uvsList[vn]     = m2Data.meshData.tex_coords[vn + submesh_StartVertex[sm]];
                uvs2List[vn]    = m2Data.meshData.tex_coords2[vn + submesh_StartVertex[sm]];

                BoneWeights boneWeightVert = new BoneWeights();
                int[] boneIndex = new int[4];
                float[] boneWeight = new float[4];

                for (int bn = 0; bn < 4; bn++)
                {
                    boneIndex[bn]   = m2Data.meshData.bone_indices[vn + submesh_boneComboIndex[sm]][bn];
                    boneWeight[bn]  = m2Data.meshData.bone_weights[vn + submesh_boneComboIndex[sm]][bn];
                }
                boneWeightVert.boneIndex    = boneIndex;
                boneWeightVert.boneWeight   = boneWeight;
                boneWeights[vn]             = boneWeightVert;
            }

            int[] triList = new int[submesh_NbrTris[sm]];
            for (var t = 0; t < submesh_NbrTris[sm]; t++)
            {
                //triList[t] = Triangles[t + submesh_StartTriangle[sm]] - submesh_StartVertex[sm];  // using Separate Meshes, reset first triangle to index 0;
                triList[t] = Triangles[t + submesh_StartTriangle[sm]];                              // using Unity Submeshes, don't reset first triangle to index 0;
            }


            SubmeshData submeshData = new SubmeshData();

            submeshData.ID                  = skinSectionId[sm];
            submeshData.vertList            = vertList;
            submeshData.normsList           = normsList;
            submeshData.uvsList             = uvsList;
            submeshData.uvs2List            = uvs2List;
            Array.Reverse(triList);
            submeshData.triList             = triList;
            submeshData.submesh_StartVertex = submesh_StartVertex[sm];
            submeshData.boneWeights         = boneWeights;
            submeshData.submesh_boneCount   = submesh_boneCount[sm];
            submeshData.submesh_boneInfluences = submesh_boneInfluences[sm];
            m2Data.submeshData.Add(submeshData);
        }

        /// Read Bone Data ///
        // byte[] Properties = new byte[bones.Size];
        // reader.BaseStream.Seek(bones.Offset, SeekOrigin.Current);
        // for (var bone = 0; bone < bones.Size; bone++)
        // {
        //     Properties[bone] = reader.ReadByte();
        // }
    }
}