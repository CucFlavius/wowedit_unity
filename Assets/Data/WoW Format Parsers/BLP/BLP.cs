using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

public class BLPinfo
{
    public TextureFormat textureFormat;
    public byte encoding;
    public byte alphaDepth;
    public byte alphaEncoding;
    public bool hasMipmaps;
    public int width;
    public int height;
    public uint[] mipmapOffsets;
    public uint[] mipmapSize;
    public int mipMapCount;
}

public struct ARGBColor8
{
    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;

    public void ConvertToBGRA(byte[] pixel)
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

public class BLP
{
    private TextureFormat textureFormat; 
    private byte encoding; // 1 = Uncompressed, 2 = DirectX Compressed
    private byte alphaDepth; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
    private byte alphaEncoding; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
    private byte hasMipmaps; // If true (1), then there are Mipmaps
    private int width; // X Resolution of the biggest Mipmap
    private int height; // Y Resolution of the biggest Mipmap
    private uint[] mipmapOffsets = new uint[16]; // Offset for every Mipmap level. If 0 = no more mipmap level
    private uint[] mipmapSize = new uint[16]; // Size for every level
    private int MipMapCount;

    private ARGBColor8[] paletteBGRA = new ARGBColor8[256];
    private Stream str; // Reference of the stream

    public BLPinfo Info()
    {
        BLPinfo blpInfo = new BLPinfo();

        blpInfo.textureFormat   = textureFormat;
        blpInfo.encoding        = encoding;
        blpInfo.alphaDepth      = alphaDepth;
        blpInfo.alphaEncoding   = alphaEncoding;

        if (hasMipmaps > 0)
            blpInfo.hasMipmaps  = true;

        if (hasMipmaps == 0)
            blpInfo.hasMipmaps  = false;

        blpInfo.width           = width;
        blpInfo.height          = height;
        blpInfo.mipmapOffsets   = mipmapOffsets;
        blpInfo.mipmapSize      = mipmapSize;
        blpInfo.mipMapCount     = MipMapCount;
        
        return blpInfo;
    }

    public TextureFormat TxFormat()
    {
        if (encoding == 2)
        {
            if (alphaEncoding >= 1) return TextureFormat.DXT5;
            if (alphaEncoding == 0) return TextureFormat.DXT1;
        }
        if (encoding == 1)
        {
            return TextureFormat.RGBA32;
        }
        return TextureFormat.RGBA32;
    }

    public int GetMipMapCount()
    {
        int i = 0;
        while (mipmapOffsets[i] != 0) i++;
        MipMapCount = i;
        return MipMapCount;
    }

    public byte[] GetUncompressed(Stream stream, bool mipmaps = true)
    {
        ClearData();
        if (!mipmaps)
        {
            ParseHeaderInfo(stream);
            GetMipMapCount();
            byte[] imageBytes = GetImageBytes(0);
            return imageBytes;
        }
        else if (mipmaps)
        {
            ParseHeaderInfo(stream);
            GetMipMapCount();
            List<byte> allDataList = new List<byte>();
            for (int miplvl = 0; miplvl < MipMapCount; miplvl++)
            {
                byte[] buffer = GetImageBytes(miplvl);
                allDataList.AddRange(buffer);
            }
            byte[] alldata = allDataList.ToArray();
            return alldata;
        }
        return null;
    }

    private void ClearData ()
    {
        mipmapOffsets = new uint[16]; // Offset for every Mipmap level. If 0 = no more mipmap level
        mipmapSize = new uint[16]; // Size for every level
        MipMapCount = 0;
    }

    private void ParseHeaderInfo (Stream stream)
    {
        str = stream;
        byte[] buffer = new byte[4];
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
            mipmapSize[i] = BitConverter.ToUInt32(buffer, 0);
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
        textureFormat = TxFormat();
    }

    private byte[] GetImageBytes(int mipmapLevel)
	{
		switch (encoding)
		{
			case 1:
                byte[] data = new byte[mipmapSize[mipmapLevel]];
				str.Position = mipmapOffsets[mipmapLevel];
				str.Read(data, 0, data.Length);
				return ExtractPalettizedImageData((int)(width/Mathf.Pow(2, mipmapLevel)), (int)(height/Mathf.Pow(2, mipmapLevel)), data);
			case 2:
				byte[] data0 = new byte[mipmapSize[mipmapLevel]];
				str.Position = mipmapOffsets[mipmapLevel];
				str.Read(data0, 0, data0.Length);
				return data0;
			case 3:
				byte[] data1 = new byte[mipmapSize[mipmapLevel]];
				str.Position = mipmapOffsets[mipmapLevel];
				str.Read(data1, 0, data1.Length);
				return data1;
			default:
				return new byte[0];
		}
	}

    private byte[] ExtractPalettizedImageData (int w, int h, byte[] data)
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
            case 1:
                byte b = data[alphaStart + (index / 8)];
                return (byte)((b & (0x01 << (index % 8))) == 0 ? 0x00 : 0xff);
            case 4:
                byte b1 = data[alphaStart + (index / 2)];
                return (byte)(index % 2 == 0 ? (b1 & 0x0F) << 4 : b1 & 0xF0);
            case 8:
                return data[alphaStart + index];
            default:
                return 0xFF;
        }
    }

    public void Close()
    {
        str.Close();
        str = null;
    }

    private string ReadFourCCReverse(Stream stream) // 4 byte to 4 chars
    {
        string str = "";
        for (int i = 1; i <= 4; i++)
        {
            int b = stream.ReadByte();
            try
            {
                var s = System.Convert.ToChar(b);
                if (s != '\0')
                {
                    str = str + s;
                }
            }
            catch
            {
                Debug.Log("Couldn't convert Byte to Char: " + b);
            }
        }
        return str;
    }
}
