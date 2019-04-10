using Assets.Data.CASC;
using Assets.WoWEditSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public static class ADT_ProcessData
    {

        // mesh handler //
        public static void GenerateMeshArrays()
        {
            foreach (ADTRootData.MeshChunkData chunkData in ADTRootData.meshBlockData.meshChunksData)
            {
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
                if (chunkData.holes_low_res == 0 && chunkData.holes_high_res == 0)
                {
                    chunkData.TriangleArray = ADT.Chunk_Triangles;
                }
                else
                {
                    if (chunkData.flags.high_res_holes)
                    {
                        // high res holes //
                        byte[] bytes = BitConverter.GetBytes(chunkData.holes_high_res);
                        BitArray bits = new BitArray(bytes);
                        bool[,] bitmap = new bool[8, 8];
                        for (int i = 0; i < 8; i++)
                            for (int j = 0; j < 8; j++)
                                bitmap[i, j] = bits[(j * 8) + i];
                        int[] triangles = new int[256 * 3];
                        int triOffset = 0;
                        //create 8 strips//
                        for (int strip = 0; strip < 8; strip++)
                        {
                            //   case \/   //
                            for (int t = 0; t < 8; t++)
                            {
                                if (!bitmap[t, strip])
                                {
                                    triangles[triOffset + 0] = t + strip * 17;
                                    triangles[triOffset + 1] = t + 1 + strip * 17;
                                    triangles[triOffset + 2] = t + 9 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case /\   //
                            for (int t1 = 0; t1 < 8; t1++)
                            {
                                if (!bitmap[t1, strip])
                                {
                                    triangles[triOffset + 0] = t1 + (strip + 1) * 17;
                                    triangles[triOffset + 1] = t1 - 8 + (strip + 1) * 17;
                                    triangles[triOffset + 2] = t1 + 1 + (strip + 1) * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case >   //
                            for (int t2 = 0; t2 < 8; t2++)
                            {
                                if (!bitmap[t2, strip])
                                {
                                    triangles[triOffset + 0] = t2 + strip * 17;
                                    triangles[triOffset + 1] = t2 + 9 + strip * 17;
                                    triangles[triOffset + 2] = t2 + 17 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case <   //
                            for (int t3 = 0; t3 < 8; t3++)
                            {
                                if (!bitmap[t3, strip])
                                {
                                    triangles[triOffset + 0] = t3 + 9 + strip * 17;
                                    triangles[triOffset + 1] = t3 + 1 + strip * 17;
                                    triangles[triOffset + 2] = t3 + 18 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                        }
                        chunkData.TriangleArray = triangles;
                    }
                    else
                    {
                        // low res holes //
                        byte[] bytes = BitConverter.GetBytes(chunkData.holes_low_res);
                        BitArray bits = new BitArray(bytes);
                        bool[,] bitmap = new bool[4, 4];
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                                bitmap[i, j] = bits[(j * 4) + i];
                        int[] triangles = new int[256 * 3];
                        int triOffset = 0;
                        //create 8 strips//
                        for (int strip = 0; strip < 8; strip++)
                        {
                            //   case \/   //
                            for (int t = 0; t < 8; t++)
                            {
                                if (!bitmap[t / 2, strip / 2])
                                {
                                    triangles[triOffset + 0] = t + strip * 17;
                                    triangles[triOffset + 1] = t + 1 + strip * 17;
                                    triangles[triOffset + 2] = t + 9 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case /\   //
                            for (int t1 = 0; t1 < 8; t1++)
                            {
                                if (!bitmap[t1 / 2, strip / 2])
                                {
                                    triangles[triOffset + 0] = t1 + (strip + 1) * 17;
                                    triangles[triOffset + 1] = t1 - 8 + (strip + 1) * 17;
                                    triangles[triOffset + 2] = t1 + 1 + (strip + 1) * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case >   //
                            for (int t2 = 0; t2 < 8; t2++)
                            {
                                if (!bitmap[t2 / 2, strip / 2])
                                {
                                    triangles[triOffset + 0] = t2 + strip * 17;
                                    triangles[triOffset + 1] = t2 + 9 + strip * 17;
                                    triangles[triOffset + 2] = t2 + 17 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                            //   case <   //
                            for (int t3 = 0; t3 < 8; t3++)
                            {
                                if (!bitmap[t3 / 2, strip / 2])
                                {
                                    triangles[triOffset + 0] = t3 + 9 + strip * 17;
                                    triangles[triOffset + 1] = t3 + 1 + strip * 17;
                                    triangles[triOffset + 2] = t3 + 18 + strip * 17;
                                    triOffset = triOffset + 3;
                                }
                            }
                        }
                        chunkData.TriangleArray = triangles;
                    }
                }

                // UVW array //
                chunkData.UVArray = ADT.Chunk_UVs;

                // scale chunk positions to worldScale //
                Vector3 newMapPosition = new Vector3(chunkData.MeshPosition.x / SettingsManager<Configuration>.Config.WorldSettings.WorldScale,
                                                     chunkData.MeshPosition.z / SettingsManager<Configuration>.Config.WorldSettings.WorldScale,
                                                     chunkData.MeshPosition.y / SettingsManager<Configuration>.Config.WorldSettings.WorldScale);
                chunkData.MeshPosition = newMapPosition;
            }
        }

        public static void AdjustAlphaBasedOnShadowmap(string mapname)
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

        public static void Load_hTextures()
        {
            if (ADTTexData.textureBlockData.MTXP)
            {
                ADTTexData.textureBlockData.terrainHTextures = new Dictionary<string, ADTTexData.Texture2Ddata>();
                foreach (string texturePath in ADTTexData.textureBlockData.terrainTexturePaths)
                {
                    string noExtension      = Path.GetFileNameWithoutExtension(texturePath);
                    string directoryPath    = Path.GetDirectoryName(texturePath);
                    string hTexturePath     = directoryPath + @"\" + noExtension + "_h" + ".blp";
                    if (Casc.FileExists(hTexturePath))
                    {
                        string extractedPath        = Casc.GetFile(hTexturePath);
                        Stream stream               = File.Open(extractedPath, FileMode.Open);
                        BLP blp                     = new BLP();
                        byte[] data                 = blp.GetUncompressed(stream, true);
                        BLPinfo info                = blp.Info();
                        ADTTexData.Texture2Ddata texture2Ddata = new ADTTexData.Texture2Ddata();
                        texture2Ddata.hasMipmaps    = info.hasMipmaps;
                        texture2Ddata.width         = info.width;
                        texture2Ddata.height        = info.height;
                        if (info.width != info.height) // Unity doesn't support nonsquare mipmaps // sigh
                            texture2Ddata.hasMipmaps = false;
                        texture2Ddata.textureFormat = info.textureFormat;
                        texture2Ddata.TextureData   = data;
                        ADTTexData.textureBlockData.terrainHTextures.Add(texturePath, texture2Ddata);
                        stream.Close();
                        stream = null;
                    }
                    else
                    {
                        Debug.Log($"Filepath does not exists: {hTexturePath}");
                    }
                }
            }
        }

        public static void AtlassAlphaMaps()
        {
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {

                }
            }
        }
    }
}
