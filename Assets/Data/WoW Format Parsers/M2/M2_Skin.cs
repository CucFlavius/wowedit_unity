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

        /// Read Batch Indices ///

        int[] M2Batch_flags = new int[batches.size];
        int[] M2Batch_shader_id = new int[batches.size];
        int[] M2Batch_submesh_index = new int[batches.size];
        int[] M2Batch_submesh_index2 = new int[batches.size];
        int[] M2Batch_color_index = new int[batches.size];
        int[] M2Batch_render_flags = new int[batches.size];
        int[] M2Batch_layer = new int[batches.size];
        int[] M2Batch_op_count = new int[batches.size];
        int[] M2Batch_texture = new int[batches.size];
        int[] M2Batch_tex_unit_number2 = new int[batches.size];
        int[] M2Batch_transparency = new int[batches.size];
        int[] M2Batch_texture_anim = new int[batches.size];

        ms.Seek(batches.offset, SeekOrigin.Begin);
        for (var batch = 0; batch < batches.size; batch++)
        {
            M2Batch_flags[batch] = s.ReadShort(ms);                 // probably two uint8_t? -- Usually 16 for static textures, and 0 for animated textures. &0x1: materials invert something; &0x2: transform &0x4: projected texture; &0x10: something batch compatible; &0x20: projected texture?; &0x40: transparency something
            M2Batch_shader_id[batch] = s.ReadShort(ms);             // See below.
            M2Batch_submesh_index[batch] = s.ReadShort(ms);         // A duplicate entry of a submesh from the list above.
            M2Batch_submesh_index2[batch] = s.ReadShort(ms);        // See below.
            M2Batch_color_index[batch] = s.ReadShort(ms);           // A Color out of the Colors-Block or -1 if none.
            M2Batch_render_flags[batch] = s.ReadShort(ms);          // The renderflags used on this texture-unit.
            M2Batch_layer[batch] = s.ReadShort(ms);                 //
            M2Batch_op_count[batch] = s.ReadShort(ms);              // 1 to 4. See below. Also seems to be the number of textures to load, starting at the texture lookup in the next field (0x10).
            M2Batch_texture[batch] = s.ReadShort(ms);               // Index into Texture lookup table
            M2Batch_tex_unit_number2[batch] = s.ReadShort(ms);      // Index into the texture unit lookup table.
            M2Batch_transparency[batch] = s.ReadShort(ms);          // Index into transparency lookup table.
            M2Batch_texture_anim[batch] = s.ReadShort(ms);          // Index into uvanimation lookup table. 
        }

        /// Read Mesh Data ///

        int[] Indices = new int[indices.size];
        int[] Triangles = new int[triangles.size];

        int[] skinSectionId = new int[submeshes.size];
        int[] submesh_StartVertex = new int[submeshes.size];
        int[] submesh_NbrVerts = new int[submeshes.size];
        int[] submesh_StartTriangle = new int[submeshes.size];
        int[] submesh_NbrTris = new int[submeshes.size];

        // indices //
        ms.Seek(indices.offset, SeekOrigin.Begin);
        for (var ind = 0; ind < indices.size; ind++)
        {
            Indices[ind] = s.ReadShort(ms);
        }

        // triangles //
        ms.Seek(triangles.offset, SeekOrigin.Begin);
        for (var tri = 0; tri < triangles.size; tri++)
        {
            Triangles[tri] = s.ReadShort(ms);
        }

        // submeshes //
        ms.Seek(submeshes.offset, SeekOrigin.Begin);
        for (var sub = 0; sub < submeshes.size; sub++)
        {
            skinSectionId[sub] = s.ReadShort(ms);                   // Mesh part ID, see below.
            int Level = s.ReadShort(ms);                            // (level << 16) is added (|ed) to startTriangle and alike to avoid having to increase those fields to uint32s.
            submesh_StartVertex[sub] = s.ReadShort(ms) + (Level << 16);
            submesh_NbrVerts[sub] = s.ReadShort(ms);
            submesh_StartTriangle[sub] = s.ReadUint16(ms) + (Level << 16);
            submesh_NbrTris[sub] = s.ReadShort(ms);

            ms.Position += 36;
        }

        /// Assemble Submeshes ///

        m2Data.submeshData = new List<SubmeshData>();

        for (int sm = 0; sm < submeshes.size; sm++)
        {
            Vector3[] vertList = new Vector3[submesh_NbrVerts[sm]];
            Vector3[] normsList = new Vector3[submesh_NbrVerts[sm]];
            Vector2[] uvsList = new Vector2[submesh_NbrVerts[sm]];
            Vector2[] uvs2List = new Vector2[submesh_NbrVerts[sm]];
            
            for (int vn = 0; vn < submesh_NbrVerts[sm]; vn++)
            {
                vertList[vn] = m2Data.meshData.pos[vn + submesh_StartVertex[sm]];
                normsList[vn] = m2Data.meshData.normal[vn + submesh_StartVertex[sm]];
                uvsList[vn] = m2Data.meshData.tex_coords[vn + submesh_StartVertex[sm]];
                uvs2List[vn] = m2Data.meshData.tex_coords2[vn + submesh_StartVertex[sm]];
            }

            int[] triList = new int[submesh_NbrTris[sm]];
            for (var t = 0; t < submesh_NbrTris[sm]; t++)
            {
                triList[t] = Triangles[t + submesh_StartTriangle[sm]] - submesh_StartVertex[sm];
            }

            SubmeshData submeshData = new SubmeshData();

            submeshData.ID = skinSectionId[sm];
            submeshData.vertList = vertList;
            submeshData.normsList = normsList;
            submeshData.uvsList = uvsList;
            submeshData.uvs2List = uvs2List;
            Array.Reverse(triList);
            submeshData.triList = triList;

            m2Data.submeshData.Add(submeshData);
        }
    }
}