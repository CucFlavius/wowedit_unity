using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class M2
{
    public static void ReadMD21(MemoryStream ms, M2Data m2Data, M2Texture m2Tex)
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
        int num_skin_profiles = s.ReadLong(ms);
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

        // Vertices //
        ms.Position = vertices.offset + md20position;

        m2Data.meshData = new MeshData();

        for (int v = 0; v < vertices.size; v++)
        {
            Vector3 rawPosition = new Vector3(s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale, s.ReadFloat(ms) / Settings.worldScale);
            m2Data.meshData.pos.Add(new Vector3(-rawPosition.x, rawPosition.z, -rawPosition.y));
            m2Data.meshData.bone_weights.Add(new float[] { ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f, ms.ReadByte() / 255.0f });
            m2Data.meshData.bone_indices.Add(new int[] { ms.ReadByte(), ms.ReadByte(), ms.ReadByte(), ms.ReadByte() });
            Vector3 rawnormal = new Vector3(s.ReadFloat(ms) * Settings.worldScale, s.ReadFloat(ms) * Settings.worldScale, s.ReadFloat(ms) * Settings.worldScale);
            m2Data.meshData.normal.Add(new Vector3(-rawnormal.x, rawnormal.z, -rawnormal.y));
            m2Data.meshData.tex_coords.Add(new Vector2(s.ReadFloat(ms), s.ReadFloat(ms)));
            m2Data.meshData.tex_coords2.Add(new Vector2(s.ReadFloat(ms), s.ReadFloat(ms)));
        }

        // Textures //
        ms.Position = textures.offset + md20position;

        for (int t = 0; t < textures.size; t++)
        {
            M2Texture m2Texture = new M2Texture();

            m2Texture.type = s.ReadLong(ms);
            m2Texture.flags = s.ReadLong(ms);

            M2Array filename = s.ReadM2Array(ms);

            // seek to filename and read //
            long savePosition = ms.Position;
            ms.Position = filename.offset + md20position;
            string fileNameString = "";
            for (int n = 0; n < filename.size; n++)
            {
                fileNameString += Convert.ToChar(ms.ReadByte());
            }
            ms.Position = savePosition;

            string fileNameStringFix = fileNameString.TrimEnd(fileNameString[fileNameString.Length - 1]);
            m2Texture.filename = fileNameStringFix;

            Texture2Ddata texture2Ddata = new Texture2Ddata();

            if (fileNameStringFix.Length > 1)
            {
                if (!LoadedBLPs.Contains(fileNameStringFix))
                {
                    string extractedPath = Casc.GetFile(fileNameStringFix);
                    Stream stream = File.Open(extractedPath, FileMode.Open);
                    BLP blp = new BLP();
                    byte[] data = blp.GetUncompressed(stream, true);
                    BLPinfo info = blp.Info();
                    texture2Ddata.hasMipmaps = info.hasMipmaps;
                    texture2Ddata.width = info.width;
                    texture2Ddata.height = info.height;
                    texture2Ddata.textureFormat = info.textureFormat;
                    texture2Ddata.TextureData = data;
                    m2Texture.texture2Ddata = texture2Ddata;
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                    LoadedBLPs.Add(fileNameString);
                }
            }
            m2Data.m2Tex.Add(m2Texture);
        }

        // texture_lookup_table //
        ms.Position = texture_lookup_table.offset + md20position;

        for (int tl = 0; tl < texture_lookup_table.size; tl++)
        {
            m2Data.textureLookupTable.Add(s.ReadUint16(ms));
        }

    }

    public static void SkipUnknownChunk(MemoryStream ms, int chunkID, int chunkSize)
    {
        ms.Seek(chunkSize, SeekOrigin.Current);
    }
}
