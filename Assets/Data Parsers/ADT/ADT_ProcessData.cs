using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                //chunkData.UVArray[u] = new Vector2(chunkData.VertexArray[u].x / ((33.3333f) / Settings.worldScale) + (((16.6666f - chunkData.VertexArray[u].x) / 33.3333f) * 0.034f ) - 0.017f,
                //                                    chunkData.VertexArray[u].z / ((33.3333f) / Settings.worldScale) + (((16.6666f - chunkData.VertexArray[u].z) / 33.3333f) * 0.034f ) - 0.017f);
                chunkData.UVArray[u] = new Vector2(chunkData.VertexArray[u].x / (33.3333f / Settings.worldScale), chunkData.VertexArray[u].z / (33.3333f / Settings.worldScale));


            }

            // scale chunk positions to worldScale //
            Vector3 newMapPosition = new Vector3(chunkData.MeshPosition.x / Settings.worldScale, chunkData.MeshPosition.z / Settings.worldScale, chunkData.MeshPosition.y / Settings.worldScale);
            chunkData.MeshPosition = newMapPosition;

        }
    }

    public static void Load_hTextures ()
    {
        if (ADT.blockData.MTXP)
        {
            ADT.blockData.terrainHTextures = new Dictionary<string, ADT.Texture2Ddata>();
            foreach (string texturePath in ADT.blockData.terrainTexturePaths)
            {
                string noExtension = Path.GetFileNameWithoutExtension(texturePath);
                string directoryPath = Path.GetDirectoryName(texturePath);
                string hTexturePath = directoryPath + @"\" + noExtension + "_h" + ".blp";
                if (Casc.FileExists(hTexturePath))
                {
                    string extractedPath = Casc.GetFile(hTexturePath);
                    Stream stream = File.Open(extractedPath, FileMode.Open);
                    byte[] data = BLP.GetUncompressed(stream, true);
                    BLPinfo info = BLP.Info();
                    ADT.Texture2Ddata texture2Ddata = new ADT.Texture2Ddata();
                    texture2Ddata.hasMipmaps = info.hasMipmaps;
                    texture2Ddata.width = info.width;
                    texture2Ddata.height = info.height;
                    if (info.width != info.height) // Unity doesn't support nonsquare mipmaps // sigh
                        texture2Ddata.hasMipmaps = false;
                    texture2Ddata.textureFormat = info.textureFormat;
                    texture2Ddata.TextureData = data;
                    ADT.blockData.terrainHTextures.Add(texturePath, texture2Ddata);
                    stream.Close();
                    stream = null;
                }
            }
        }
        
    }


}
