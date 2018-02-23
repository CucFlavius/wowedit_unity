using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ADT {

    public static float worldScale = 10.0f;

    // data //
    public static Queue<List<ChunkData>> AllADTdata = new Queue<List<ChunkData>>();
    public static List<ChunkData> ADTdata = new List<ChunkData>();


    // data structures //
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
        public List<int[]> VertexColors = new List<int[]>();
        public List<Vector3> VertexNormals = new List<Vector3>();
        public int[] TriangleArray;
        public Vector2[] UVArray;
    }

    // data managers //
    public static void ClearADTdata ()
    {
        Debug.Log("Cleared buffer.");
        ADTdata.Clear();
    }

    // mesh handlers //
    private static void GenerateMeshArrays()
    {
        //Debug.Log("AllADTdata " + AllADTdata.Count);
        foreach (ChunkData chunkData in ADTdata)
        {
            // vertices array //
            chunkData.VertexArray = new Vector3[145];
            int currentVertex = 0;
            for (int i = 0; i < 17; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        chunkData.VertexArray[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / worldScale,
                                                                            (float)(-j * 0.208333125 * 20) / worldScale);
                        currentVertex++;
                    }
                }
                if (i % 2 == 1)
                {
                    for (int j1 = 0; j1 < 8; j1++)
                    {
                        chunkData.VertexArray[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / worldScale,
                                                                            (float)((-j1 - 0.5) * 0.208333125 * 20) / worldScale);
                        currentVertex++;
                    }
                }
            }
            currentVertex = 0;

            // triangles array //
            chunkData.TriangleArray = new int[256 * 3];
            int triOffset = 0;
            //create 8 strips//
            for (int strip = 0; strip < 8; strip++)
            {
                //   case \/   //
                for (int t = 0; t < 8; t++)
                {
                    chunkData.TriangleArray[triOffset + 0] = t + strip * 17;
                    chunkData.TriangleArray[triOffset + 1] = t + 1 + strip * 17;
                    chunkData.TriangleArray[triOffset + 2] = t + 9 + strip * 17;
                    triOffset = triOffset + 3;
                }
                //   case /\   //
                for (int t1 = 0; t1 < 8; t1++)
                {
                    chunkData.TriangleArray[triOffset + 0] = t1 + (strip + 1) * 17;
                    chunkData.TriangleArray[triOffset + 1] = t1 - 8 + (strip + 1) * 17;
                    chunkData.TriangleArray[triOffset + 2] = t1 + 1 + (strip + 1) * 17;
                    triOffset = triOffset + 3;
                }
                //   case >   //
                for (int t2 = 0; t2 < 8; t2++)
                {
                    chunkData.TriangleArray[triOffset + 0] = t2 + strip * 17;
                    chunkData.TriangleArray[triOffset + 1] = t2 + 9 + strip * 17;
                    chunkData.TriangleArray[triOffset + 2] = t2 + 17 + strip * 17;
                    triOffset = triOffset + 3;
                }
                //   case <   //
                for (int t3 = 0; t3 < 8; t3++)
                {
                    chunkData.TriangleArray[triOffset + 0] = t3 + 9 + strip * 17;
                    chunkData.TriangleArray[triOffset + 1] = t3 + 1 + strip * 17;
                    chunkData.TriangleArray[triOffset + 2] = t3 + 18 + strip * 17;
                    triOffset = triOffset + 3;
                }
            }

            // UVW array //
            chunkData.UVArray = new Vector2[145];
            for (int u = 0; u < 145; u++)
            {
                chunkData.UVArray[u] = new Vector2((float)((chunkData.VertexArray[u].x + (chunkData.VertexArray[u].x * 0.200)) / 2),
                                                    (float)((chunkData.VertexArray[u].z + (chunkData.VertexArray[u].z * 0.200)) / 2));
            }

            // Normals //

            // scale chunk positions to worldScale //
            Vector3 newMapPosition = new Vector3(chunkData.MeshPosition.x / worldScale, chunkData.MeshPosition.z / worldScale, chunkData.MeshPosition.y / worldScale);
            chunkData.MeshPosition = newMapPosition;
        }
    }
}
