using Assets.Data;
using Assets.Data.WoW_Format_Parsers;
using Assets.Data.WoW_Format_Parsers.M2;
using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.Tools.CSV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Assets.Data.WoW_Format_Parsers.M2.M2_Data;

public static partial class M2
{
    public static void ReadMD21(BinaryReader br, M2Data m2Data, M2Texture m2Tex)
    {
        long md20position = br.BaseStream.Position;

        int MD20                                = br.ReadInt32();                                      // "MD20". Legion uses a chunked file format starting with MD21.                              
        int version                             = br.ReadInt32();
        M2Array name                            = br.ReadM2Array();                               // should be globally unique, used to reload by name in internal clients
        var flags                               = br.ReadInt32();
        M2Array global_loops                    = br.ReadM2Array();                       // Timestamps used in global looping animations.
        M2Array sequences                       = br.ReadM2Array();                          // Information about the animations in the model.
        M2Array sequences_lookups               = br.ReadM2Array();                  // Mapping of sequence IDs to the entries in the Animation sequences block.
        M2Array bones                           = br.ReadM2Array();                              // MAX_BONES = 0x100 => Creature\SlimeGiant\GiantSlime.M2 has 312 bones(Wrath)
        M2Array key_bone_lookup                 = br.ReadM2Array();                    // Lookup table for key skeletal bones.
        M2Array vertices                        = br.ReadM2Array();
        int num_skin_profiles                   = br.ReadInt32();
        M2Array colors                          = br.ReadM2Array();                             // Color and alpha animations definitions.
        M2Array textures                        = br.ReadM2Array();
        M2Array texture_weights                 = br.ReadM2Array();                    // Transparency of textures.
        M2Array texture_transforms              = br.ReadM2Array();
        M2Array replaceable_texture_lookup      = br.ReadM2Array();
        M2Array materials                       = br.ReadM2Array();                          // Blending modes / render flags.
        M2Array bone_lookup_table               = br.ReadM2Array();
        M2Array texture_lookup_table            = br.ReadM2Array();
        M2Array tex_unit_lookup_table           = br.ReadM2Array();              // ≥ Cata: unused
        M2Array transparency_lookup_table       = br.ReadM2Array();
        M2Array texture_transforms_lookup_table = br.ReadM2Array();

        m2Data.bounding_box                     = br.ReadBoundingBoxes();                    // min/max( [1].z, 2.0277779f ) - 0.16f seems to be the maximum camera height
        float bounding_sphere_radius            = br.ReadSingle();                 // detail doodad draw dist = clamp (bounding_sphere_radius * detailDoodadDensityFade * detailDoodadDist, …)
        BoundingBox collision_box               = br.ReadBoundingBoxes();
        float collision_sphere_radius           = br.ReadSingle();

        M2Array collision_triangles             = br.ReadM2Array();
        M2Array collision_vertices              = br.ReadM2Array();
        M2Array collision_normals               = br.ReadM2Array();
        M2Array attachments                     = br.ReadM2Array();                        // position of equipped weapons or effects
        M2Array attachment_lookup_table         = br.ReadM2Array();
        M2Array events                          = br.ReadM2Array();                             // Used for playing sounds when dying and a lot else.
        M2Array lights                          = br.ReadM2Array();                             // Lights are mainly used in loginscreens but in wands and some doodads too.
        M2Array cameras                         = br.ReadM2Array();                            // The cameras are present in most models for having a model in the character tab. 
        M2Array camera_lookup_table             = br.ReadM2Array();
        M2Array ribbon_emitters                 = br.ReadM2Array();                    // Things swirling around. See the CoT-entrance for light-trails.
        M2Array particle_emitters               = br.ReadM2Array();

        // Name //
        br.BaseStream.Position = name.Offset + md20position;
        for (int n = 0; n < name.Size; n++)
            m2Data.name += Convert.ToChar(br.ReadByte());

        // Bones //
        br.BaseStream.Position = bones.Offset + md20position;
        M2TrackBase[] translationM2track = new M2TrackBase[bones.Size];
        M2TrackBase[] rotationM22track = new M2TrackBase[bones.Size];
        M2TrackBase[] scaleM22track = new M2TrackBase[bones.Size];
        for (int cb = 0; cb < bones.Size; cb++)
        {
            M2CompBone m2CompBone = new M2CompBone();

            m2CompBone.key_bone_id          = br.ReadInt32();                    // Back-reference to the key bone lookup table. -1 if this is no key bone.
            m2CompBone.flags                = br.ReadInt32();
            m2CompBone.parent_bone          = br.ReadInt16();
            m2CompBone.submesh_id           = br.ReadUInt16();
            m2CompBone.uDistToFurthDesc     = br.ReadUInt16();
            m2CompBone.uZRatioOfChain       = br.ReadUInt16();

            translationM2track[cb]          = br.ReadM2Track();
            rotationM22track[cb]            = br.ReadM2Track();
            scaleM22track[cb]               = br.ReadM2Track();

            Vector3 pivotRaw = new Vector3(br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale);
            m2CompBone.pivot = new Vector3(-pivotRaw.x, pivotRaw.z, -pivotRaw.y);

            m2Data.m2CompBone.Add(m2CompBone);
        }


        // Animations //
        int numberOfAnimations = 0;
        for (int ab = 0; ab < bones.Size; ab++)
        {
            List<Animation_Vector3> bone_position_animations    = new List<Animation_Vector3>();
            List<Animation_Quaternion> bone_rotation_animations = new List<Animation_Quaternion>();
            List<Animation_Vector3> bone_scale_animations       = new List<Animation_Vector3>();

            // Position //
            int numberOfPositionAnimations = translationM2track[ab].Timestamps.Size;
            if (numberOfAnimations < numberOfPositionAnimations) numberOfAnimations = numberOfPositionAnimations;
            for (int at = 0; at < numberOfPositionAnimations; at++)
            {
                Animation bone_animation = new Animation();
                Animation_Vector3 positions = new Animation_Vector3();

                // Timestamps //
                List<int> timeStamps        = new List<int>();
                br.BaseStream.Position  = translationM2track[ab].Timestamps.Offset + md20position;
                M2Array m2AnimationOffset   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationOffset.Offset;
                for (int t = 0; t < m2AnimationOffset.Size; t++)
                {
                    timeStamps.Add(br.ReadInt32());
                }
                positions.timeStamps = timeStamps;

                // Values //
                List<Vector3> values        = new List<Vector3>();
                br.BaseStream.Position  = translationM2track[ab].Values.Offset + md20position;
                M2Array m2AnimationValues   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationValues.Offset;
                for (int t = 0; t < m2AnimationValues.Size; t++)
                {
                    Vector3 rawPosition = new Vector3(br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale);
                    values.Add(new Vector3(-rawPosition.x, rawPosition.z, -rawPosition.y));
                }
                positions.values = values;
                bone_position_animations.Add(positions);
            }


            // Rotation //
            int numberOfRotationAnimations = rotationM22track[ab].Timestamps.Size;
            if (numberOfAnimations < numberOfRotationAnimations) numberOfAnimations = numberOfRotationAnimations;
            for (int ar = 0; ar < numberOfRotationAnimations; ar++)
            {
                Animation_Quaternion rotations = new Animation_Quaternion();

                // Timestamps //
                List<int> timeStamps        = new List<int>();
                br.BaseStream.Position  = rotationM22track[ab].Timestamps.Offset + md20position;
                M2Array m2AnimationOffset   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationOffset.Offset;
                for (int t = 0; t < m2AnimationOffset.Size; t++)
                {
                    timeStamps.Add(br.ReadInt32());
                }
                rotations.timeStamps = timeStamps;

                // Values //
                List<Quaternion> values     = new List<Quaternion>();
                br.BaseStream.Position  = rotationM22track[ab].Values.Offset + md20position;
                M2Array m2AnimationValues   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationValues.Offset;
                for (int t = 0; t < m2AnimationValues.Size; t++)
                {
                    Quaternion rawRotation = br.ReadQuaternion();
                    values.Add(new Quaternion(rawRotation.x, rawRotation.y, rawRotation.z, rawRotation.w));
                }
                rotations.values = values;
                bone_rotation_animations.Add(rotations);
            }

            // Scale //
            int numberOfScaleAnimations = scaleM22track[ab].Timestamps.Size;
            if (numberOfAnimations < numberOfScaleAnimations) numberOfAnimations = numberOfScaleAnimations;
            for (int aS = 0; aS < numberOfScaleAnimations; aS++)
            {
                Animation_Vector3 scales = new Animation_Vector3();

                // Timestamps //
                List<int> timeStamps        = new List<int>();
                br.BaseStream.Position  = scaleM22track[ab].Timestamps.Offset + md20position;
                M2Array m2AnimationOffset   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationOffset.Offset;
                for (int t = 0; t < m2AnimationOffset.Size; t++)
                {
                    timeStamps.Add(br.ReadInt32());
                }
                scales.timeStamps = timeStamps;

                // Values //
                List<Vector3> values        = new List<Vector3>();
                br.BaseStream.Position  = scaleM22track[ab].Values.Offset + md20position;
                M2Array m2AnimationValues   = br.ReadM2Array();
                br.BaseStream.Position  = m2AnimationValues.Offset;
                for (int t = 0; t < m2AnimationValues.Size; t++)
                {
                    Vector3 rawScale = new Vector3(br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale);
                    values.Add(new Vector3(-rawScale.x, rawScale.z, -rawScale.y));
                }
                scales.values = values;
                bone_scale_animations.Add(scales);
            }
            //Debug.Log(numberOfPositionAnimations + " " + numberOfRotationAnimations + " " + numberOfScaleAnimations);
            m2Data.position_animations.Add(bone_position_animations);
            m2Data.rotation_animations.Add(bone_rotation_animations);
            m2Data.scale_animations.Add(bone_scale_animations);
        }
        m2Data.numberOfAnimations = numberOfAnimations;

        // Bone Lookup Table //
        br.BaseStream.Position = bone_lookup_table.Offset + md20position;
        for (int blt = 0; blt < key_bone_lookup.Size; blt++)
        {
            m2Data.bone_lookup_table.Add(br.ReadUInt16());
        }

        // Key-Bone Lookup //
        br.BaseStream.Position = key_bone_lookup.Offset + md20position;
        for (int kbl = 0; kbl < key_bone_lookup.Size; kbl++)
        {
            m2Data.key_bone_lookup.Add(br.ReadUInt16());
        }

        // Vertices //
        br.BaseStream.Position = vertices.Offset + md20position;
        m2Data.meshData = new MeshData();
        for (int v = 0; v < vertices.Size; v++)
        {
            Vector3 rawPosition = new Vector3(br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale, br.ReadSingle() / Settings.worldScale);
            m2Data.meshData.pos.Add(new Vector3(-rawPosition.x, rawPosition.z, -rawPosition.y));
            m2Data.meshData.bone_weights.Add(new float[] { br.ReadByte() / 255.0f, br.ReadByte() / 255.0f, br.ReadByte() / 255.0f, br.ReadByte() / 255.0f });
            m2Data.meshData.bone_indices.Add(new int[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() });
            //Debug.Log(m2Data.meshData.bone_indices[v][0] + " " + m2Data.meshData.bone_indices[v][1] + " " + m2Data.meshData.bone_indices[v][2] + " " + m2Data.meshData.bone_indices[v][3]);
            Vector3 rawnormal = new Vector3(br.ReadSingle() * Settings.worldScale, br.ReadSingle() * Settings.worldScale, br.ReadSingle() * Settings.worldScale);
            m2Data.meshData.normal.Add(new Vector3(-rawnormal.x, rawnormal.z, -rawnormal.y));
            m2Data.meshData.tex_coords.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
            m2Data.meshData.tex_coords2.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
        }

        // texture_lookup_table //
        br.BaseStream.Position = texture_lookup_table.Offset + md20position;
        for (int tl = 0; tl < texture_lookup_table.Size; tl++)
        {
            m2Data.textureLookupTable.Add(br.ReadUInt16());
        }

    }

    public static void ReadTXID(BinaryReader br, M2Data m2Data)
    {
        br.BaseStream.Position -= 4;
        var size = br.ReadUInt32();
        var numTextures = size / 4;

        for (int i = 0; i < numTextures; i++)
        {
            uint texture = br.ReadUInt32();
            string Filename = CSVReader.LookupId(texture);

            M2Texture m2Texture = new M2Texture();
            m2Texture.filename = Filename;
            Texture2Ddata texture2Ddata = new Texture2Ddata();
            if (!LoadedBLPs.Contains(Filename))
            {
                string extractedPath = Casc.GetFile(Filename);
                Stream stream = File.Open(extractedPath, FileMode.Open);
                BLP blp = new BLP();
                byte[] data = blp.GetUncompressed(stream, true);
                BLPinfo info = blp.Info();
                texture2Ddata.hasMipmaps    = info.hasMipmaps;
                texture2Ddata.width         = info.width;
                texture2Ddata.height        = info.height;
                texture2Ddata.textureFormat = info.textureFormat;
                texture2Ddata.TextureData   = data;
                m2Texture.texture2Ddata     = texture2Ddata;
                stream.Close();
                stream.Dispose();
                LoadedBLPs.Add(Filename);
            }
            m2Data.m2Tex.Add(m2Texture);
        }
    }

    public static void SkipUnknownChunk(MemoryStream ms, M2ChunkId chunkID, int chunkSize)
    {
        ms.Seek(chunkSize, SeekOrigin.Current);
    }
}
