using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class M2
{
    public static void ReadMD21(MemoryStream ms)
    {
        StreamTools s = new StreamTools();
        int MD20 = s.ReadLong(ms);
        int MD20Size = s.ReadLong(ms);

        int magic = s.ReadLong(ms);                 // "MD20". Legion uses a chunked file format starting with MD21.
        int version = s.ReadLong(ms);
        M2Array name = s.ReadM2Array(ms);

        var flags = s.ReadLong(ms);

        M2Array global_loops = s.ReadM2Array(ms);
        M2Array sequences = s.ReadM2Array(ms);
        M2Array sequences_lookups = s.ReadM2Array(ms);
        M2Array bones = s.ReadM2Array(ms);
        M2Array key_bone_lookup = s.ReadM2Array(ms);
        M2Array vertices = s.ReadM2Array(ms);
        M2Array colors = s.ReadM2Array(ms);
        M2Array textures = s.ReadM2Array(ms);
        M2Array texture_weights = s.ReadM2Array(ms);
        M2Array texture_transforms = s.ReadM2Array(ms);
        M2Array replaceable_texture_lookup = s.ReadM2Array(ms);
        M2Array materials = s.ReadM2Array(ms);
        M2Array bone_lookup_table = s.ReadM2Array(ms);
        M2Array texture_lookup_table = s.ReadM2Array(ms);
        M2Array tex_unit_lookup_table = s.ReadM2Array(ms);  // M2Array<uint16_tⁱ> tex_unit_lookup_table;    ≥ Cata: unused
        M2Array transparency_lookup_table = s.ReadM2Array(ms);
        M2Array texture_transforms_lookup_table = s.ReadM2Array(ms);

        BoundingBox bounding_box = s.ReadBoundingBox(ms);
        float bounding_sphere_radius = s.ReadFloat(ms);
        BoundingBox collision_box = s.ReadBoundingBox(ms);
        float collision_sphere_radius = s.ReadFloat(ms);

        M2Array collision_triangles = s.ReadM2Array(ms);
        M2Array collision_vertices = s.ReadM2Array(ms);
        M2Array collision_normals = s.ReadM2Array(ms);
        M2Array attachments = s.ReadM2Array(ms);
        M2Array attachment_lookup_table = s.ReadM2Array(ms);
        M2Array events = s.ReadM2Array(ms);
        M2Array lights = s.ReadM2Array(ms);
        M2Array cameras = s.ReadM2Array(ms);
        M2Array camera_lookup_table = s.ReadM2Array(ms);
        M2Array ribbon_emitters = s.ReadM2Array(ms);
        M2Array particle_emitters = s.ReadM2Array(ms);
    }

    public static void SkipUnknownChunk(MemoryStream ms, int chunkID, int chunkSize)
    {
        ms.Seek(chunkSize, SeekOrigin.Current);
    }
}
