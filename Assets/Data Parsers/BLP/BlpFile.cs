/*
 * Copyright (c) <2011> <by Xalcon @ mmowned.com-Forum>
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SereniaBLPLib
{
    // Some Helper Struct to store Color-Data
    public struct ARGBColor8
    {
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        /// <summary>
        /// Converts the given Pixel-Array into the BGRA-Format
        /// This will also work vice versa
        /// </summary>
        /// <param name="pixel"></param>
        public static void convertToBGRA(byte[] pixel)
        {
            byte tmp;
            for (int i = 0; i < pixel.Length; i += 4)
            {
                tmp = pixel[i]; // store red
                pixel[i] = pixel[i + 2]; // Write blue into red
                pixel[i + 2] = tmp; // write stored red into blue
            }
        }
    }

    public sealed class BlpFile : IDisposable
    {
        //uint type; // compression: 0 = JPEG Compression, 1 = Uncompressed or DirectX Compression
        byte encoding; // 1 = Uncompressed, 2 = DirectX Compressed
        byte alphaDepth; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
        byte alphaEncoding; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
        byte hasMipmaps; // If true (1), then there are Mipmaps
        int width; // X Resolution of the biggest Mipmap
        int height; // Y Resolution of the biggest Mipmap

        uint[] mipmapOffsets = new uint[16]; // Offset for every Mipmap level. If 0 = no more mitmap level
        uint[] mippmapSize = new uint[16]; // Size for every level
        ARGBColor8[] paletteBGRA = new ARGBColor8[256]; // The color-palette for non-compressed pictures

        Stream str; // Reference of the stream

        /// <summary>
        /// Extracts the palettized Image-Data from the given Mipmap and returns a byte-Array in the 32Bit RGBA-Format
        /// </summary>
        /// <param name="mipmap">The desired Mipmap-Level. If the given level is invalid, the smallest available level is choosen</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="data"></param>
        /// <returns>Pixel-data</returns>
        private byte[] GetPictureUncompressedByteArray(int w, int h, byte[] data)
        {
            int length = w * h;
            byte[] pic = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                pic[i * 4] = paletteBGRA[data[i]].red;
                pic[i * 4 + 1] = paletteBGRA[data[i]].green;
                pic[i * 4 + 2] = paletteBGRA[data[i]].blue;
                pic[i * 4 + 3] = GetAlpha(data, i, length);
            }
            return pic;
        }

        private byte GetAlpha(byte[] data, int index, int alphaStart)
        {
            switch (alphaDepth)
            {
                default:
                    return 0xFF;
                case 1:
                    {
                        byte b = data[alphaStart + (index / 8)];
                        return (byte)((b & (0x01 << (index % 8))) == 0 ? 0x00 : 0xff);
                    }
                case 4:
                    {
                        byte b = data[alphaStart + (index / 2)];
                        return (byte)(index % 2 == 0 ? (b & 0x0F) << 4 : b & 0xF0);
                    }
                case 8:
                    return data[alphaStart + index];
            }
        }

        /// <summary>
        /// Returns the raw Mipmap-Image Data. This data can either be compressed or uncompressed, depending on the Header-Data
        /// </summary>
        /// <param name="mipmapLevel"></param>
        /// <returns></returns>
        public byte[] GetPictureData(int mipmapLevel)
        {
            if (str != null)
            {
                byte[] data = new byte[mippmapSize[mipmapLevel]];
                str.Position = mipmapOffsets[mipmapLevel];
                str.Read(data, 0, data.Length);
                return data;
            }
            return null;
        }

        /// <summary>
        /// Returns the amount of Mipmaps in this BLP-File
        /// </summary>
        public int MipMapCount
        {
            get
            {
                int i = 0;
                while (mipmapOffsets[i] != 0) i++;
                return i;
            }
        }

        public BlpFile(Stream stream)
        {
            str = stream;
            byte[] buffer = new byte[4];
            // Well, have to fix this... looks weird o.O
            str.Read(buffer, 0, 4);

            // Checking for correct Magic-Code
            if (BitConverter.ToUInt32(buffer, 0) != 0x32504c42)
                throw new Exception("Invalid BLP Format");

            // Reading type
            str.Read(buffer, 0, 4);
            uint type = BitConverter.ToUInt32(buffer, 0);
            if (type != 1)
                throw new Exception("Invalid BLP-Type! Should be 1 but " + type + " was found");

            // Reading encoding, alphaBitDepth, alphaEncoding and hasMipmaps
            str.Read(buffer, 0, 4);
            encoding = buffer[0];
            alphaDepth = buffer[1];
            alphaEncoding = buffer[2];
            hasMipmaps = buffer[3];

            // Reading width
            str.Read(buffer, 0, 4);
            width = BitConverter.ToInt32(buffer, 0);

            // Reading height
            str.Read(buffer, 0, 4);
            height = BitConverter.ToInt32(buffer, 0);

            // Reading MipmapOffset Array
            for (int i = 0; i < 16; i++)
            {
                stream.Read(buffer, 0, 4);
                mipmapOffsets[i] = BitConverter.ToUInt32(buffer, 0);
            }

            // Reading MipmapSize Array
            for (int i = 0; i < 16; i++)
            {
                str.Read(buffer, 0, 4);
                mippmapSize[i] = BitConverter.ToUInt32(buffer, 0);
            }

            // When encoding is 1, there is no image compression and we have to read a color palette
            if (encoding == 1)
            {
                // Reading palette
                for (int i = 0; i < 256; i++)
                {
                    byte[] color = new byte[4];
                    str.Read(color, 0, 4);
                    paletteBGRA[i].blue = color[0];
                    paletteBGRA[i].green = color[1];
                    paletteBGRA[i].red = color[2];
                    paletteBGRA[i].alpha = color[3];
                }
            }
        }

        /// <summary>
        /// Returns the uncompressed image as a bytarray in the 32pppRGBA-Format
        /// </summary>
        private byte[] GetImageBytes(int w, int h, byte[] data)
        {
            switch (encoding)
            {
                case 1:
                    return GetPictureUncompressedByteArray(w, h, data);
                case 2:
                    DXTDecompression.DXTFlags flag = (alphaDepth > 1) ? ((alphaEncoding == 7) ? DXTDecompression.DXTFlags.DXT5 : DXTDecompression.DXTFlags.DXT3) : DXTDecompression.DXTFlags.DXT1;
                    return DXTDecompression.DecompressImage(w, h, data, flag);
                case 3:
                    return data;
                default:
                    return new byte[0];
            }
        }


        /// <summary>
        /// Runs close()
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Closes the Memorystream
        /// </summary>
        public void Close()
        {
            str.Close();
            str = null;
        }
    }
}