using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public static class ADTTexData
    {
        //public static Queue<TextureBlockData> TextureBlockDataQueue = new Queue<TextureBlockData>();
        public static Queue<TextureBlockData> TextureBlockDataQueue = new Queue<TextureBlockData>();
        public static TextureBlockData textureBlockData = new TextureBlockData();

        public class TextureBlockData
        {
            // Per chunk data //
            public List<TextureChunkData> textureChunksData = new List<TextureChunkData>();

            // Terrain textures //
            public List<uint> terrainTextureFileDataIds = new List<uint>();
            public Dictionary<uint, Texture2Ddata> terrainTextures = new Dictionary<uint, Texture2Ddata>();
            public List<byte[]> atlassedAlphaLayers = new List<byte[]>();

            // MTXP data //
            public bool MTXP = false;
            public Dictionary<uint, Flags.TerrainTextureFlag> textureFlags = new Dictionary<uint, Flags.TerrainTextureFlag>();
            public Dictionary<uint, float> heightScales = new Dictionary<uint, float>();
            public Dictionary<uint, float> heightOffsets = new Dictionary<uint, float>();
            public Dictionary<uint, Texture2Ddata> terrainHTextures = new Dictionary<uint, Texture2Ddata>();
        }

        public class TextureChunkData
        {
            // texture data //
            public uint NumberOfTextureLayers; // number of texture layers in this chunk 1=no alpha
            public int[] textureIds = new int[6]; // texture ID from terrainTextures per layer
            public bool[] alpha_map_compressed = new bool[4]; // flag that decides alpha compression per layer
            public int[] LayerOffsetsInMCAL = new int[4]; // offsets for each alpha layer data
            public List<byte[]> alphaLayers = new List<byte[]>(); // up to 3 x bytearrays per list

            // shadow map //
            public bool[] shadowMap = new bool[64 * 64];
            public byte[] shadowMapTexture = new byte[64 * 64];
        }

        public class Texture2Ddata
        {
            public byte[] TextureData;
            public int width;
            public int height;
            public bool hasMipmaps;
            public TextureFormat textureFormat;
        }
    }
}