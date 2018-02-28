using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ADT_ProcessData {

    // mesh handler //
    public static void GenerateMeshArrays()
    {
        foreach (ADT.ChunkData chunkData in ADT.blockData.ChunksData)
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
                        chunkData.VertexArray[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / Settings.worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / Settings.worldScale,
                                                                            (float)(-j * 0.208333125 * 20) / Settings.worldScale);
                        currentVertex++;
                    }
                }
                if (i % 2 == 1)
                {
                    for (int j1 = 0; j1 < 8; j1++)
                    {
                        chunkData.VertexArray[currentVertex] = new Vector3((float)((-i * 0.5) * 0.208333125 * 20) / Settings.worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / Settings.worldScale,
                                                                            (float)((-j1 - 0.5) * 0.208333125 * 20) / Settings.worldScale);
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
                chunkData.UVArray[u] = new Vector2(chunkData.VertexArray[u].x / ((33.3333f) / Settings.worldScale) + (((16.6666f - chunkData.VertexArray[u].x) / 33.3333f) * 0.034f ) - 0.017f,
                                                    chunkData.VertexArray[u].z / ((33.3333f) / Settings.worldScale) + (((16.6666f - chunkData.VertexArray[u].z) / 33.3333f) * 0.034f ) - 0.017f);
            }

            // scale chunk positions to worldScale //
            Vector3 newMapPosition = new Vector3(chunkData.MeshPosition.x / Settings.worldScale, chunkData.MeshPosition.z / Settings.worldScale, chunkData.MeshPosition.y / Settings.worldScale);
            chunkData.MeshPosition = newMapPosition;

            // Vertex color fill if missing //
            if (chunkData.VertexColors == null)
            {
                chunkData.VertexColors = new Color32[145];
                for (int v = 0; v < 145; v++)
                {
                    //Color32 colorBGRA = new Color32(127, 127, 127, 127);
                    //chunkData.VertexColors[v] = colorBGRA;
                }
            }
        }
            
    }


}
