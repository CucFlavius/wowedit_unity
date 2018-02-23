//using System.Drawing;
//using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class BLPinfo
{
    public TextureFormat textureFormat;
    public byte encoding;
    public byte alphaDepth;
    public byte alphaEncoding;
    public byte hasMipmaps;
    public int width;
    public int height;
    public uint[] mipmapOffsets;
    public uint[] mipmapSize;
}

public class ARGBColor8
{

    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;

    /// Converts the given Pixel-Array into the BGRA-Format
    /// This will also work vice versa

    public static void ConvertToBGRA(byte[] pixel)
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

public static class BLP
{

    private static TextureFormat textureFormat; 
    private static byte encoding; // 1 = Uncompressed, 2 = DirectX Compressed
    private static byte alphaDepth; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
    private static byte alphaEncoding; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
    private static byte hasMipmaps; // If true (1), then there are Mipmaps
    private static int width; // X Resolution of the biggest Mipmap
    private static int height; // Y Resolution of the biggest Mipmap
    private static uint[] mipmapOffsets = new uint[16]; // Offset for every Mipmap level. If 0 = no more mipmap level
    private static uint[] mipmapSize = new uint[16]; // Size for every level

    private static ARGBColor8[] paletteBGRA = new ARGBColor8[256];
    private static Stream str; // Reference of the stream

    public static BLPinfo Info()
    {
        BLPinfo blpInfo = new BLPinfo();
        blpInfo.textureFormat = textureFormat;
        blpInfo.encoding = encoding;
        blpInfo.alphaDepth = alphaDepth;
        blpInfo.alphaEncoding = alphaEncoding;
        blpInfo.hasMipmaps = hasMipmaps;
        blpInfo.width = width;
        blpInfo.height = height;
        blpInfo.mipmapOffsets = mipmapOffsets;
        blpInfo.mipmapSize = mipmapSize;
        return blpInfo;
    }

    public static TextureFormat TxFormat()
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

    public static byte[] GetUncompressed(Stream stream, int mipmapLevel = 0)
    {
        ParseHeaderInfo(stream);
        byte[] imageBytes = GetImageBytes(mipmapLevel);
        return imageBytes;
    }

    private static void ParseHeaderInfo (Stream stream)
    {
        str = stream;
        byte[] buffer = new byte[4];
        str.Read(buffer, 0, 4);

        // check for Magic Code //
        if (System.BitConverter.ToUInt32(buffer, 0) != 0x32504c42)
            Debug.Log("Invalid BLP Format");

        // Read type
        str.Read(buffer, 0, 4);
        uint type = System.BitConverter.ToUInt32(buffer, 0);
        if (type != 1)
            Debug.Log("Invalid BLP-Type! Should be 1 but " + type + " was found");

        // Read encoding, alphaBitDepth, alphaEncoding and hasMipmaps
        str.Read(buffer, 0, 4);
        encoding = buffer[0];
        alphaDepth = buffer[1];
        alphaEncoding = buffer[2];
        hasMipmaps = buffer[3];

        // Read width
        str.Read(buffer, 0, 4);
        width = System.BitConverter.ToInt32(buffer, 0);

        // Read height
        str.Read(buffer, 0, 4);
        height = System.BitConverter.ToInt32(buffer, 0);

        // Read MipmapOffset Array
        for (int i = 0; i < 16; i++)
		{
            stream.Read(buffer, 0, 4);
            mipmapOffsets[i] = System.BitConverter.ToUInt32(buffer, 0);
        }

        // Read MipmapSize Array
        for (int i1 = 0; i1 < 16; i1++)
		{
            str.Read(buffer, 0, 4);
            mipmapSize[i1] = System.BitConverter.ToUInt32(buffer, 0);
        }

        // When encoding is 1, there is no image compression and we have to read a color palette
        if (encoding == 1)
        {
            // Reading palette
            for (int i2 = 0; i2 < 256; i2++)
			{
                byte[] color = new byte[4];
                str.Read(color, 0, 4);
                paletteBGRA[i2].blue = color[0];
                paletteBGRA[i2].green = color[1];
                paletteBGRA[i2].red = color[2];
                paletteBGRA[i2].alpha = color[3];
            }
        }
    }

    private static byte[] GetImageBytes(int mipmapLevel)
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

    private static byte[] ExtractPalettizedImageData (int w, int h, byte[] data)
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

    private static byte GetAlpha (byte[] data, int index, int alphaStart)
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

    public static void Close()
    {
        str.Close();
        str = null;
    }
}
