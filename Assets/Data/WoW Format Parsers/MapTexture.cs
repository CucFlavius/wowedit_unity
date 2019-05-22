using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Assets.Data.WoW_Format_Parsers.ADT.ADTTexData;
using CASCLib;

namespace Assets.Data.WoW_Format_Parsers
{
    public static class MapTexture
    {
        // Threading //
        public static bool MapTextureThreadRunning = false;

        // Data //
        public static LockFreeQueue<MapTextureBlock> MapTextureDataQueue = new LockFreeQueue<MapTextureBlock>();

        public class MapTextureBlock
        {
            public string dataPath;
            public string mapName;
            public Vector2 coords;
            public Texture2Ddata data;
        }

        public static void Load(string mapName, Vector2 coords)
        {
            string dataPath = @"world\maptextures\" + mapName + @"\" + mapName + "_" + coords.x + "_" + coords.y + ".blp";
            MapTextureBlock mapTextureBlock = new MapTextureBlock();
            Texture2Ddata texture2Ddata     = new Texture2Ddata();
            // int fdid = Casc.GetFileDataIdByName(dataPath);
            // string extractedTexturePath = Casc.GetFile(fdid);
            // using (Stream stream = File.Open(extractedTexturePath, FileMode.Open))
            // {
            //     BLP blp                     = new BLP();
            //     byte[] data                 = blp.GetUncompressed(stream, true);
            //     BLPinfo info                = blp.Info();
            //     texture2Ddata.hasMipmaps    = info.hasMipmaps;
            //     texture2Ddata.height        = info.height;
            //     texture2Ddata.width         = info.width;
            //     texture2Ddata.textureFormat = info.textureFormat;
            //     texture2Ddata.TextureData   = data;
            //     mapTextureBlock.dataPath    = dataPath;
            //     mapTextureBlock.mapName     = mapName;
            //     mapTextureBlock.coords      = coords;
            //     mapTextureBlock.data        = texture2Ddata;
            //     if (ADT.ADT.working)
            //         MapTextureDataQueue.Enqueue(mapTextureBlock);
            //     MapTextureThreadRunning = false;
            // }
        }
    }
}