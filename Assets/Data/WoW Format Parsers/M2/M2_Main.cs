using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class M2
{
    public static void ReadMD21(MemoryStream ms, M2Data m2Data)
    {
        long md20position = ms.Position;

        StreamTools s = new StreamTools();
        int MD20 = s.ReadLong(ms);                                      // "MD20". Legion uses a chunked file format starting with MD21.                              
        int version = s.ReadLong(ms);

        M2Array name = s.ReadM2Array(ms);                               // should be globally unique, used to reload by name in internal clients

        var flags = s.ReadLong(ms);

        M2Array global_loops = s.ReadM2Array(ms);                       // Timestamps used in global looping animations.
        M2Array sequences = s.ReadM2Array(ms);                          // Information about the animations in the model.
        M2Array sequences_lookups = s.ReadM2Array(ms);                  // Mapping of sequence IDs to the entries in the Animation sequences block.
        M2Array bones = s.ReadM2Array(ms);                              // MAX_BONES = 0x100 => Creature\SlimeGiant\GiantSlime.M2 has 312 bones(Wrath)
        M2Array key_bone_lookup = s.ReadM2Array(ms);                    // Lookup table for key skeletal bones.
        M2Array vertices = s.ReadM2Array(ms);
        M2Array colors = s.ReadM2Array(ms);                             // Color and alpha animations definitions.
        M2Array textures = s.ReadM2Array(ms);
        M2Array texture_weights = s.ReadM2Array(ms);                    // Transparency of textures.
        M2Array texture_transforms = s.ReadM2Array(ms);
        M2Array replaceable_texture_lookup = s.ReadM2Array(ms);
        M2Array materials = s.ReadM2Array(ms);                          // Blending modes / render flags.
        M2Array bone_lookup_table = s.ReadM2Array(ms);
        M2Array texture_lookup_table = s.ReadM2Array(ms);
        M2Array tex_unit_lookup_table = s.ReadM2Array(ms);              // ≥ Cata: unused
        M2Array transparency_lookup_table = s.ReadM2Array(ms);
        M2Array texture_transforms_lookup_table = s.ReadM2Array(ms);

        BoundingBox bounding_box = s.ReadBoundingBox(ms);               // min/max( [1].z, 2.0277779f ) - 0.16f seems to be the maximum camera height
        float bounding_sphere_radius = s.ReadFloat(ms);                 // detail doodad draw dist = clamp (bounding_sphere_radius * detailDoodadDensityFade * detailDoodadDist, …)
        BoundingBox collision_box = s.ReadBoundingBox(ms);
        float collision_sphere_radius = s.ReadFloat(ms);

        M2Array collision_triangles = s.ReadM2Array(ms);
        M2Array collision_vertices = s.ReadM2Array(ms);
        M2Array collision_normals = s.ReadM2Array(ms);
        M2Array attachments = s.ReadM2Array(ms);                        // position of equipped weapons or effects
        M2Array attachment_lookup_table = s.ReadM2Array(ms);
        M2Array events = s.ReadM2Array(ms);                             // Used for playing sounds when dying and a lot else.
        M2Array lights = s.ReadM2Array(ms);                             // Lights are mainly used in loginscreens but in wands and some doodads too.
        M2Array cameras = s.ReadM2Array(ms);                            // The cameras are present in most models for having a model in the character tab. 
        M2Array camera_lookup_table = s.ReadM2Array(ms);
        M2Array ribbon_emitters = s.ReadM2Array(ms);                    // Things swirling around. See the CoT-entrance for light-trails.
        M2Array particle_emitters = s.ReadM2Array(ms);

        ms.Position = vertices.offset + md20position;

        m2Data.meshData = new MeshData();

        for (int v = 0; v < vertices.size; v++)
        {
            m2Data.meshData.pos.Add(new Vector3(s.ReadFloat(ms), s.ReadFloat(ms), s.ReadFloat(ms)));
            m2Data.meshData.bone_weights.Add(new float[] { ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f });
            m2Data.meshData.bone_indices.Add(new int[] { ms.ReadByte(), ms.ReadByte(), ms.ReadByte(), ms.ReadByte() });
            m2Data.meshData.normal.Add(new Vector3(s.ReadFloat(ms), s.ReadFloat(ms), s.ReadFloat(ms)));
            m2Data.meshData.tex_coords.Add(new Vector2(s.ReadFloat(ms), s.ReadFloat(ms)));
            m2Data.meshData.tex_coords2.Add(new Vector2(s.ReadFloat(ms), s.ReadFloat(ms)));
        }
    }

    public static void SkipUnknownChunk(MemoryStream ms, int chunkID, int chunkSize)
    {
        ms.Seek(chunkSize, SeekOrigin.Current);
    }
}
