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
            public uint FileDataId;
            public Vector2 coords;
            public Texture2Ddata data;
        }

        public static void Load(Vector2 coords, CASCHandler Handler)
        {
            MapTextureBlock mapTextureBlock = new MapTextureBlock();
            Texture2Ddata texture2Ddata     = new Texture2Ddata();
            using (Stream stream = Handler.OpenFile(WDT.WDTEntries[((int)coords.x, (int)coords.y)].MapTexture))
            {
                BLP blp                     = new BLP();
                byte[] data                 = blp.GetUncompressed(stream, true);
                BLPinfo info                = blp.Info();
                texture2Ddata.hasMipmaps    = info.hasMipmaps;
                texture2Ddata.height        = info.height;
                texture2Ddata.width         = info.width;
                texture2Ddata.textureFormat = info.textureFormat;
                texture2Ddata.TextureData   = data;
                mapTextureBlock.FileDataId  = WDT.WDTEntries[((int)coords.x, (int)coords.y)].MapTexture;
                mapTextureBlock.coords      = coords;
                mapTextureBlock.data        = texture2Ddata;
                if (ADT.ADT.working)
                    MapTextureDataQueue.Enqueue(mapTextureBlock);
                MapTextureThreadRunning = false;
            }
        }
    }
}