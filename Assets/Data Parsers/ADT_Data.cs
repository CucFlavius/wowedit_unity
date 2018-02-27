using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ADT {

    // block queue //
    public static Queue<BlockDataType> AllBlockData = new Queue<BlockDataType>();

    // block buffer //
    public static BlockDataType blockData = new BlockDataType();

    // data structures //
    public class BlockDataType
    {
        public List<ChunkData> ChunksData = new List<ChunkData>();
        public List<string> terrainTexturePaths = new List<string>();
        public Dictionary<string, Texture2Ddata> terrainTextures = new Dictionary<string, Texture2Ddata>();
    }

    public class ChunkData
    {
        // object properties //
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

        // texture data //
        public int NumberOfTextureLayers; // number of texture layers in this chunk 1=no alpha
        public int[] textureIds = new int[6]; // texture ID from terrainTextures per layer
        public bool[] alpha_map_compressed = new bool[4]; // flag that decides alpha compression per layer
        public int[] LayerOffsetsInMCAL = new int[4]; // offsets for each alpha layer data
        public List<byte[]> alphaLayers = new List<byte[]>(); // up to 3 x bytearrays per list
    }

    public class Texture2Ddata
    {
        public byte[] TextureData;
        public int width;
        public int height;
        public bool hasMipmaps;
        public TextureFormat textureFormat;
    }

    // data managers //
    public static void ClearBlockData ()
    {
        Debug.Log("Cleared buffer.");
        blockData.terrainTexturePaths.Clear();
        blockData.terrainTextures.Clear();
        blockData.ChunksData.Clear();
    }


}
