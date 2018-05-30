using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class M2
{
    public static void ParseSkin(MemoryStream ms, M2Data m2Data)
    {
        StreamTools s = new StreamTools();

        string magic = s.ReadFourCC(ms);            // 'SKIN'
        M2Array indices = s.ReadM2Array(ms);
        M2Array triangles = s.ReadM2Array(ms);
        M2Array bones = s.ReadM2Array(ms);
        M2Array submeshes = s.ReadM2Array(ms);
        M2Array batches = s.ReadM2Array(ms); // nTexture_units
        int boneCountMax = s.ReadLong(ms);          // WoW takes this and divides it by the number of bones in each submesh, then stores the biggest one.
                                                    // Maximum number of bones per drawcall for each view. Related to (old) GPU numbers of registers. 
                                                    // Values seen : 256, 64, 53, 21
        M2Array shadow_batches = s.ReadM2Array(ms);

        /// Read Batches ///
        ms.Seek(batches.offset, SeekOrigin.Begin);
        for (var batch = 0; batch < batches.size; batch++)
        {
            M2BatchIndices m2BatchIndices = new M2BatchIndices();

            m2BatchIndices.M2Batch_flags = s.ReadShort(ms);                 // probably two uint8_t? -- Usually 16 for static textures, and 0 for animated textures. &0x1: materials invert something; &0x2: transform &0x4: projected texture; &0x10: something batch compatible; &0x20: projected texture?; &0x40: transparency something
            m2BatchIndices.M2Batch_shader_id = s.ReadShort(ms);             // See below.
            m2BatchIndices.M2Batch_submesh_index = s.ReadShort(ms);         // A duplicate entry of a submesh from the list above.
            m2BatchIndices.M2Batch_submesh_index2 = s.ReadShort(ms);        // See below.
            m2BatchIndices.M2Batch_color_index = s.ReadShort(ms);           // A Color out of the Colors-Block or -1 if none.
            m2BatchIndices.M2Batch_render_flags = s.ReadShort(ms);          // The renderflags used on this texture-unit.
            m2BatchIndices.M2Batch_layer = s.ReadShort(ms);                 //
            m2BatchIndices.M2Batch_op_count = s.ReadShort(ms);              // 1 to 4. See below. Also seems to be the number of textures to load, starting at the texture lookup in the next field (0x10).
            m2BatchIndices.M2Batch_texture = s.ReadShort(ms);               // Index into Texture lookup table
            m2BatchIndices.M2Batch_tex_unit_number2 = s.ReadShort(ms);      // Index into the texture unit lookup table.
            m2BatchIndices.M2Batch_transparency = s.ReadShort(ms);          // Index into transparency lookup table.
            m2BatchIndices.M2Batch_texture_anim = s.ReadShort(ms);          // Index into uvanimation lookup table. 

            m2Data.m2BatchIndices.Add(m2BatchIndices);
        }

        // Read SubMesh Data //

        int[] Indices = new int[indices.size];
        int[] Triangles = new int[triangles.size];

        int[] skinSectionId = new int[submeshes.size];                          // Mesh part ID, see below.
        int[] submesh_StartVertex = new int[submeshes.size];                    // Starting vertex number.
        int[] submesh_NbrVerts = new int[submeshes.size];                       // Number of vertices.
        int[] submesh_StartTriangle = new int[submeshes.size];                  // Starting triangle index (that's 3* the number of triangles drawn so far).
        int[] submesh_NbrTris = new int[submeshes.size];                        // Number of triangle indices.

        int[] submesh_boneCount = new int[submeshes.size];                      // Number of elements in the bone lookup table. Max seems to be 256 in Wrath. Shall be ≠ 0.
        int[] submesh_boneComboIndex = new int[submeshes.size];                 // Starting index in the bone lookup table.
        int[] submesh_boneInfluences = new int[submeshes.size];                 // <= 4
                                                                                // from <=BC documentation: Highest number of bones needed at one time in this Submesh --Tinyn (wowdev.org) 
                                                                                // In 2.x this is the amount of of bones up the parent-chain affecting the submesh --NaK
                                                                                // Highest number of bones referenced by a vertex of this submesh. 3.3.5a and suspectedly all other client revisions. -- Skarn
        int[] submesh_centerBoneIndex = new int[submeshes.size];
        Vector3[] submesh_centerPosition = new Vector3[submeshes.size];         // Average position of all the vertices in the sub mesh.
        Vector3[] submesh_sortCenterPosition = new Vector3[submeshes.size];     // The center of the box when an axis aligned box is built around the vertices in the submesh.
        float[] submesh_sortRadius = new float[submeshes.size];                 // Distance of the vertex farthest from CenterBoundingBox.

        /// Indices ///
        ms.Seek(indices.offset, SeekOrigin.Begin);
        for (var ind = 0; ind < indices.size; ind++)
        {
            Indices[ind] = s.ReadShort(ms);
        }

        /// triangles ///
        ms.Seek(triangles.offset, SeekOrigin.Begin);
        for (var tri = 0; tri < triangles.size; tri++)
        {
            Triangles[tri] = s.ReadShort(ms);
        }

        /// submeshes ///
        ms.Seek(submeshes.offset, SeekOrigin.Begin);
        for (var sub = 0; sub < submeshes.size; sub++)
        {
            skinSectionId[sub] = s.ReadUint16(ms);
            int Level = s.ReadUint16(ms);                            // (level << 16) is added (|ed) to startTriangle and alike to avoid having to increase those fields to uint32s.
            submesh_StartVertex[sub] = s.ReadUint16(ms) + (Level << 16);
            submesh_NbrVerts[sub] = s.ReadUint16(ms);
            submesh_StartTriangle[sub] = s.ReadUint16(ms) + (Level << 16);
            submesh_NbrTris[sub] = s.ReadUint16(ms);

            submesh_boneCount[sub] = s.ReadUint16(ms);
            submesh_boneComboIndex[sub] = s.ReadUint16(ms);
            submesh_boneInfluences[sub] = s.ReadUint16(ms);
            submesh_centerBoneIndex[sub] = s.ReadUint16(ms);
            Vector3 raw_centerPosition = new Vector3(s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale);
            submesh_centerPosition[sub] = new Vector3(-raw_centerPosition.x, raw_centerPosition.z, -raw_centerPosition.y);
            Vector3 raw_sortCenterPosition = new Vector3(s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale);
            submesh_sortCenterPosition[sub] = new Vector3(-raw_sortCenterPosition.x, raw_sortCenterPosition.z, -raw_sortCenterPosition.y);
            submesh_sortRadius[sub] = s.ReadFloat(ms);
        }

        /// Assemble Submeshes ///
        m2Data.submeshData = new List<SubmeshData>();
        for (int sm = 0; sm < submeshes.size; sm++)
        {
            Vector3[] vertList = new Vector3[submesh_NbrVerts[sm]];
            Vector3[] normsList = new Vector3[submesh_NbrVerts[sm]];
            Vector2[] uvsList = new Vector2[submesh_NbrVerts[sm]];
            Vector2[] uvs2List = new Vector2[submesh_NbrVerts[sm]];

            BoneWeights[] boneWeights = new BoneWeights[submesh_NbrVerts[sm]];

            for (int vn = 0; vn < submesh_NbrVerts[sm]; vn++)
            {
                vertList[vn] = m2Data.meshData.pos[vn + submesh_StartVertex[sm]];
                normsList[vn] = m2Data.meshData.normal[vn + submesh_StartVertex[sm]];
                uvsList[vn] = m2Data.meshData.tex_coords[vn + submesh_StartVertex[sm]];
                uvs2List[vn] = m2Data.meshData.tex_coords2[vn + submesh_StartVertex[sm]];

                BoneWeights boneWeightVert = new BoneWeights();
                int[] boneIndex = new int[4];
                float[] boneWeight = new float[4];

                for (int bn = 0; bn < 4; bn++)
                {
                    boneIndex[bn] = m2Data.meshData.bone_indices[vn + submesh_boneComboIndex[sm]][bn];
                    boneWeight[bn] = m2Data.meshData.bone_weights[vn + submesh_boneComboIndex[sm]][bn];
                }
                boneWeightVert.boneIndex = boneIndex;
                boneWeightVert.boneWeight = boneWeight;
                boneWeights[vn] = boneWeightVert;
            }

            int[] triList = new int[submesh_NbrTris[sm]];
            for (var t = 0; t < submesh_NbrTris[sm]; t++)
            {
                //triList[t] = Triangles[t + submesh_StartTriangle[sm]] - submesh_StartVertex[sm];  // using Separate Meshes, reset first triangle to index 0;
                triList[t] = Triangles[t + submesh_StartTriangle[sm]];                              // using Unity Submeshes, don't reset first triangle to index 0;
            }


            SubmeshData submeshData = new SubmeshData();

            submeshData.ID = skinSectionId[sm];
            submeshData.vertList = vertList;
            submeshData.normsList = normsList;
            submeshData.uvsList = uvsList;
            submeshData.uvs2List = uvs2List;
            Array.Reverse(triList);
            submeshData.triList = triList;
            submeshData.submesh_StartVertex = submesh_StartVertex[sm];
            submeshData.boneWeights = boneWeights;
            submeshData.submesh_boneCount = submesh_boneCount[sm];
            submeshData.submesh_boneInfluences = submesh_boneInfluences[sm];
            m2Data.submeshData.Add(submeshData);
        }

        /// Assemble Bone Data ///
        /// 
    }
}