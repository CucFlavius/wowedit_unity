using Assets.Data.WoW_Format_Parsers.M2;
using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.WoWEditSettings;
using CASCLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data
{
    public static class Extensions
    {
        public static BoundingBox ReadBoundingBoxes(this BinaryReader reader)
        {
            Vector3 rawMin = new Vector3(reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale);
            Vector3 rawMax = new Vector3(reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale, reader.ReadSingle() / Settings.WorldScale);
            BoundingBox box = new BoundingBox
            {
                min = new Vector3(-rawMin.x, rawMin.z, -rawMin.y),
                max = new Vector3(-rawMax.x, rawMax.z, -rawMax.y)
            };
            return box;
        }

        public static short ReadInt16BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(2);
            return (short)(val[1] | val[0] << 8);
        }

        public static int ReadInt32BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(4);
            return val[3] | val[2] << 8 | val[1] << 16 | val[0] << 24;
        }

        public static long ReadInt40BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(5);
            return val[4] | val[3] << 8 | val[2] << 16 | val[1] << 24 | val[0] << 32;
        }

        public static void Skip(this BinaryReader reader, int bytes)
        {
            reader.BaseStream.Position += bytes;
        }

        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            StringBuilder sb = new StringBuilder();
            char c;

            while ((c = Convert.ToChar(reader.ReadByte())) != 0)
                sb.Append(c);

            return sb.ToString();
        }

        public static string ReadFourCC(this BinaryReader reader)
        {
            string str = "";
            for (int i = 1; i <= 4; i++)
            {
                int b = reader.ReadByte();
                try
                {
                    var s = System.Convert.ToChar(b);
                    if (s != '\0')
                    {
                        str = s + str;
                    }
                }
                catch
                {
                    Debug.Log("Couldn't convert Byte to Char: " + b);
                }
            }
            return str;
        }

        public static M2Array ReadM2Array(this BinaryReader reader)
        {
            M2Array array = new M2Array()
            {
                Size    = reader.ReadInt32(),
                Offset  = reader.ReadInt32()
            };

            return array;
        }

        public static M2TrackBase ReadM2Track(this BinaryReader reader)
        {
            M2TrackBase trackBase = new M2TrackBase
            {
                interpolationtype   = (InterpolationType)reader.ReadUInt16(),
                GlobalSequenceID    = reader.ReadUInt16(),
                Timestamps          = reader.ReadM2Array(),
                Values              = reader.ReadM2Array(),
            };

            return trackBase;
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            short x = reader.ReadInt16();
            short y = reader.ReadInt16();
            short z = reader.ReadInt16();
            short w = reader.ReadInt16();

            return new Quaternion()
            {
                x = x,
                y = y,
                z = z,
                w = w
            };
        }

        public static ARGB ReadARGB(this BinaryReader reader)
        {
            ARGB argbColor = new ARGB();
            argbColor.A = reader.ReadByte();
            argbColor.R = reader.ReadByte();
            argbColor.G = reader.ReadByte();
            argbColor.B = reader.ReadByte();
            return argbColor;
        }

        public static RGBA ReadRGBA(this BinaryReader reader)
        {
            RGBA RGBAColor = new RGBA()
            {
                R = reader.ReadByte(),
                G = reader.ReadByte(),
                B = reader.ReadByte(),
                A = reader.ReadByte(),
            };
            return RGBAColor;
        }

        public static BGRA ReadBGRA(this BinaryReader reader)
        {
            BGRA BGRAColor = new BGRA()
            {
                B = reader.ReadByte(),
                G = reader.ReadByte(),
                R = reader.ReadByte(),
                A = reader.ReadByte(),
            };
            return BGRAColor;
        }

        public static MaterialFlags ReadMaterialFlags(this BinaryReader reader)
        {
            byte[] ArrayOfBytes = new byte[4];
            reader.Read(ArrayOfBytes, 0, 4);

            BitArray Flags = new BitArray(ArrayOfBytes);
            MaterialFlags materialFlags = new MaterialFlags()
            {
                F_UNLIT     = Flags[0],
                F_UNFOGGED  = Flags[1],
                F_UNCULLED  = Flags[2],
                F_EXTLIGHT  = Flags[3],
                F_SIDN      = Flags[4],
                F_WINDOW    = Flags[5],
                F_CLAMP_S   = Flags[6],
                F_CLAMP_T   = Flags[7],
                flag_0x100  = Flags[8],
            };

            return materialFlags;
        }

        public static FogDefinition ReadFogEditions(this BinaryReader reader)
        {
            FogDefinition fog = new FogDefinition()
            {
                end = reader.ReadSingle(),
                start_scalar = reader.ReadSingle(),
                color = reader.ReadBGRA()
            };

            return fog;
        }

        public static GroupFlags ReadGroupFlags(this BinaryReader reader)
        {
            byte[] arrayOfBytes = new byte[4];
            reader.Read(arrayOfBytes, 0, 4);
            BitArray flags = new BitArray(arrayOfBytes);

            GroupFlags groupFlags               = new GroupFlags();
            groupFlags.HasBSPtree               = flags[0];         // (MOBN and MOBR chunk).
            groupFlags.Haslightmap              = flags[1];         // (MOLM, MOLD). (UNUSED: 20740) possibly: subtract mohd.color in mocv fixing  // SubtractAmbientColour
            groupFlags.Hasvertexolors           = flags[2];         // (MOCV chunk).
            groupFlags.SMOGroupEXTERIOR         = flags[3];         // -- Outdoor - also influences how doodads are culled // is outdoors
            groupFlags.SMOGroupEXTERIOR_LIT     = flags[6];         // -- "Do not use local diffuse lightning". Applicable for both doodads from this wmo group(color from MODD) and water(CWorldView::GatherMapObjDefGroupLiquids). 
            groupFlags.SMOGroupUNREACHABLE      = flags[7];
            groupFlags.Haslights                = flags[9];         // (MOLR chunk)
            groupFlags.SMOGroupLOD              = flags[10];        // Legion+? Also load for LoD != 0 (_lod* groups)
            groupFlags.Hasdoodads               = flags[11];        // (MODR chunk)
            groupFlags.SMOGroupLIQUIDSURFACE    = flags[12];        // -- Has water(MLIQ chunk)
            groupFlags.SMOGroupINTERIOR         = flags[13];        // -- Indoor
            groupFlags.SMOGroupALWAYSDRAW       = flags[16];        // -- clear 0x8 after CMapObjGroup::Create() in MOGP and MOGI // AlwaysDrawEvenIfOutdoors
            groupFlags.HasMORIandMORBchunks     = flags[17];        // Has triangle strips
            groupFlags.Showskybox               = flags[18];        // -- automatically unset if MOSB not present.
            groupFlags.is_not_water_but_ocean   = flags[19];        // LiquidType related, see below in the MLIQ chunk. // is oceanic water
            groupFlags.IsMountAllowed           = flags[21];
            groupFlags.SMOGroupCVERTS2          = flags[24];        // Has two MOCV chunks: Just add two or don't set 0x4 to only use cverts2. // has two vertex shading sets
            groupFlags.SMOGroupTVERTS2          = flags[25];        // Has two MOTV chunks: Just add two. // has two texture coordinate sets
            groupFlags.SMOGroupANTIPORTAL       = flags[26];        // Just call CMapObjGroup::CreateOccluders() independent of groupname being "antiportal". requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE. // create occlusion without clearing bsp
            groupFlags.unknown                  = flags[27];        // requires intBatchCount == 0, extBatchCount == 0, UNREACHABLE. // occlusion related
            groupFlags.SMOGroupEXTERIOR_CULL    = flags[29];
            groupFlags.SMOGroupTVERTS3          = flags[30];        // Has three MOTV chunks, eg. for MOMT with shader 18.
                                                                    // 1 unused flag follows
            return groupFlags;
        }

        public static TriangleMaterialFlags ReadTriangleMaterialFlags(this BinaryReader reader)
        {
            byte[] arrayOfBytes = new byte[1];
            reader.Read(arrayOfBytes, 0, 1);
            BitArray flags = new BitArray(arrayOfBytes);

            TriangleMaterialFlags triangleMaterialFlags = new TriangleMaterialFlags();

            triangleMaterialFlags.Unknown1          = flags[0]; // F_UNK_0x01
            triangleMaterialFlags.NoCameraCollide   = flags[1]; // F_NOCAMCOLLIDE 
            triangleMaterialFlags.Detail            = flags[2]; // F_DETAIL 
            triangleMaterialFlags.HasCollision      = flags[3]; // F_COLLISION 
            triangleMaterialFlags.Hint              = flags[4]; // F_HINT 
            triangleMaterialFlags.Render            = flags[5]; // F_RENDER 
            triangleMaterialFlags.Unknown2          = flags[6]; // F_UNK_0x40 
            triangleMaterialFlags.CollideHit        = flags[7]; // F_COLLIDE_HIT 

            if (flags[0] && (flags[2] || flags[5]))
                triangleMaterialFlags.isTransFace = true;       // { return F_UNK_0x01 && (F_DETAIL || F_RENDER); }
            else
                triangleMaterialFlags.isTransFace = false;

            triangleMaterialFlags.isColor = !flags[3];          // { return !F_COLLISION; }

            if (flags[5] && !flags[2])
                triangleMaterialFlags.isRenderFace = true;      // { return F_RENDER && !F_DETAIL; }
            else
                triangleMaterialFlags.isRenderFace = false;

            if (flags[3] || triangleMaterialFlags.isRenderFace)
                triangleMaterialFlags.isCollidable = true;      // { return F_COLLISION || isRenderFace(); }
            else
                triangleMaterialFlags.isCollidable = false;

            return triangleMaterialFlags;

        }

        public static int GetUIntFrom4Bits(this BinaryReader reader, bool[] bits)
        {
            int v = 0;
            bool a = bits[0];
            bool b = bits[1];
            bool c = bits[2];
            bool d = bits[3];

            if (!a && !b && !c && !d)
                v = 0;
            else if (!a && !b && !c && d)
                v = 1;
            else if (!a && !b && c && !d)
                v = 2;
            else if (!a && !b && c && d)
                v = 3;
            else if (!a && b && !c && !d)
                v = 4;
            else if (!a && b && !c && d)
                v = 5;
            else if (!a && b && c && !d)
                v = 6;
            else if (!a && b && c && d)
                v = 7;
            else if (a && !b && !c && !d)
                v = 8;
            else if (a && !b && !c && d)
                v = 9;
            else if (a && !b && c && !d)
                v = 10;
            else if (a && !b && c && d)
                v = 11;
            else if (a && b && !c && !d)
                v = 12;
            else if (a && b && !c && d)
                v = 13;
            else if (a && b && c && !d)
                v = 14;
            else if (a && b && c && d)
                v = 15;
            return v;
        }

        public static float NormalizeValue(this BinaryReader reader, float value)
        {
            return (2 * (value / 254)) - 1;
        }

        public static int NormalizeHalfResAlphaPixel(this BinaryReader reader, int value)
        {
            return value * 255 / 15;
        }

        public static int BoolArrayToInt(this BinaryReader reader, bool[] bits)
        {
            uint r = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    r |= (uint)(1 << (bits.Length - i));
                }
            }
            return (int)(r / 2);
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }

    public static class CollecionExtensions
    {
        public static TValue LookupByKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, object key)
        {
            TValue val;
            TKey newkey = (TKey)Convert.ChangeType(key, typeof(TKey));
            return dict.TryGetValue(newkey, out val) ? val : default(TValue);
        }

        public static TValue LookupByKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue val;
            return dict.TryGetValue(key, out val) ? val : default;
        }

        public static KeyValuePair<TKey, TValue> Find<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.ContainsKey(key))
                return default(KeyValuePair<TKey, TValue>);

            return new KeyValuePair<TKey, TValue>(key, dict[key]);
        }

        public static bool ContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, object key)
        {
            TKey newkey = (TKey)Convert.ChangeType(key, typeof(TKey));
            return dict.ContainsKey(newkey);
        }
    }
}
