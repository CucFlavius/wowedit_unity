using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public static class ADTRootData
    {
        public static Queue<MeshBlockData> MeshBlockDataQueue = new Queue<MeshBlockData>();
        public static MeshBlockData meshBlockData = new MeshBlockData();

        public class MeshBlockData
        {
            // Per chunk data //
            public List<MeshChunkData> meshChunksData = new List<MeshChunkData>();
        }

        public class MeshChunkData
        {
            // object properties //
            public Flags.MCNKflags flags;
            public int IndexX;
            public int IndexY;
            public int nLayers; // number of alpha layers
            public Vector3 MeshPosition;

            // mesh data //
            public List<float> VertexHeights = new List<float>();
            public Vector3[] VertexArray;
            public List<byte[]> VertexLighting = new List<byte[]>();
            public Color32[] VertexColors;
            public Vector3[] VertexNormals;
            public int[] TriangleArray;
            public Vector2[] UVArray;

            // mesh holes //
            public int holes_low_res;
            public ulong holes_high_res;
        }
        public struct MCNKflags
        {
            public bool has_mcsh; // if ADTtex has MCSH chunk
            public bool impass;
            public bool lq_river;
            public bool lq_ocean;
            public bool lq_magma;
            public bool lq_slime;
            public bool has_mccv;
            public bool unknown_0x80;
            public bool do_not_fix_alpha_map;  // "fix" alpha maps in MCAL (4 bit alpha maps are 63*63 instead of 64*64).
                                               // Note that this also means that it *has* to be 4 bit alpha maps, otherwise UnpackAlphaShadowBits will assert.
            public bool high_res_holes;  // Since ~5.3 WoW uses full 64-bit to store holes for each tile if this flag is set.
        }
    }
}