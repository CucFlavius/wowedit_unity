using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ADT_ProcessData {

    // mesh handler //
    public static void GenerateMeshArrays()
    {
        foreach (ADT.MeshChunkData chunkData in ADT.meshBlockData.meshChunksData)
        {
            // vertices array //

            /*
            chunkData.VertexArray = new Vector3[145];
            int currentVertex = 0;

            chunkData.VertexArray = ADT.Chunk_Vertices;
            for (int i = 0; i < 17; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        chunkData.VertexArray[currentVertex].y = chunkData.VertexHeights[currentVertex] / Settings.worldScale;
                        currentVertex++;
                    }
                }
                if (i % 2 == 1)
                {
                    for (int j1 = 0; j1 < 8; j1++)
                    {
                        chunkData.VertexArray[currentVertex].y = chunkData.VertexHeights[currentVertex] / Settings.worldScale;
                        currentVertex++;
                    }
                }
            }
            currentVertex = 0;
            */
            
            chunkData.VertexArray = new Vector3[145];
            int currentVertex = 0;
            for (int i = 0; i < 17; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        chunkData.VertexArray[currentVertex] = new Vector3((float)(-i * 2.08333125) / Settings.worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / Settings.worldScale,
                                                                            (float)(-j * 4.1666625) / Settings.worldScale);
                        currentVertex++;
                    }
                }
                if (i % 2 == 1)
                {
                    for (int j1 = 0; j1 < 8; j1++)
                    {
                        chunkData.VertexArray[currentVertex] = new Vector3((float)(-i * 2.08333125) / Settings.worldScale,
                                                                            chunkData.VertexHeights[currentVertex] / Settings.worldScale,
                                                                            (float)((-j1 - 0.5) * 4.1666625) / Settings.worldScale);
                        currentVertex++;
                    }
                }
            }
            currentVertex = 0;


            // triangles array //
            chunkData.TriangleArray = ADT.Chunk_Triangles;

            // UVW array //
            chunkData.UVArray = ADT.Chunk_UVs;

            // scale chunk positions to worldScale //
            Vector3 newMapPosition = new Vector3(chunkData.MeshPosition.x / Settings.worldScale,
                                                 chunkData.MeshPosition.z / Settings.worldScale,
                                                 chunkData.MeshPosition.y / Settings.worldScale);
            chunkData.MeshPosition = newMapPosition;
            
        }
    }

    public static void AdjustAlphaBasedOnShadowmap (string mapname)
    {
        // can't run, missing a flag that resides in ADT main
        /*
        foreach (ADT.TextureChunkData chunkData in ADT.textureBlockData.textureChunksData)
        {
            if (chunkData.flags.do_not_fix_alpha_map)
            {
                if (WDT.Flags[mapname].adt_has_height_texturing || WDT.Flags[mapname].adt_has_big_alpha)
                {
                    foreach (byte[] alphaLayer in chunkData.alphaLayers)
                    {
                        for (int b = 0; b < 4096; b++)
                        {
                            if (chunkData.shadowMap[b])
                            {
                                alphaLayer[b] = (byte)((int)alphaLayer[b] * 0.7f);
                            }
                        }
                    }
                }
            }
        }
        */
    }

    public static void Load_hTextures ()
    {
        if (ADT.textureBlockData.MTXP)
        {
            ADT.textureBlockData.terrainHTextures = new Dictionary<string, ADT.Texture2Ddata>();
            foreach (string texturePath in ADT.textureBlockData.terrainTexturePaths)
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
                    ADT.textureBlockData.terrainHTextures.Add(texturePath, texture2Ddata);
                    stream.Close();
                    stream = null;
                }
            }
        }
    }

    public static void AtlassAlphaMaps ()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {

            }
        }
    }


}
