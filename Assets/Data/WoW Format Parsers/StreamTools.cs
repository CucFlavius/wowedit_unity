using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class StreamTools
{
    public int getUintFrom4Bits (bool[] bits)
    {
        int v = 0;
        bool a = bits[0];
        bool b = bits[1];
        bool c = bits[2];
        bool d = bits[3];

        if (!a && !b && !c && !d)
            v = 0;
        else if (!a && !b && !c && d)
            v = 1;
        else if(!a && !b && c && !d)
            v = 2;
        else if(!a && !b && c && d)
            v = 3;
        else if (!a && b && !c && !d)
            v = 4;
        else if (!a && b && !c && d)
            v = 5;
        else if (!a && b && c && !d)
            v = 6;
        else if (!a && b && c && d)
            v = 7;
        else if (a && !b && !c && !d)
            v = 8;
        else if (a && !b && !c && d)
            v = 9;
        else if (a && !b && c && !d)
            v = 10;
        else if (a && !b && c && d)
            v = 11;
        else if (a && b && !c && !d)
            v = 12;
        else if (a && b && !c && d)
            v = 13;
        else if (a && b && c && !d)
            v = 14;
        else if (a && b && c && d)
            v = 15;
        return v;
    }

    public uint getIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        uint asUint = unchecked((uint)array[0]);
        return asUint;

    }

    // 4 byte to 4 chars //
    public string ReadFourCC(MemoryStream stream) 
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
                    str = s + str;
                }
            }
            catch
            {
                Debug.Log("Couldn't convert Byte to Char: " + b);
            }
        }
        return str;
    }

    // 2 bytes to int //
    public int ReadShort(MemoryStream stream) 
    {
        byte[] bytes = new byte[2];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToInt16(bytes, 0);
        return value;
    }

    // 4 bytes to int //
    public int ReadLong(MemoryStream stream) 
    {
        byte[] bytes = new byte[4];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToInt32(bytes, 0);
        return value;
    }

    // 4 bytes to float //
    public float ReadFloat(MemoryStream stream) 
    {
        byte[] bytes = new byte[4];
        float value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToSingle(bytes, 0);
        return value;
    }

    // 8 bytes to ulong //
    public ulong ReadUint64(MemoryStream stream) 
    {
        byte[] bytes = new byte[8];
        ulong value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToUInt64(bytes, 0);
        return value;
    }

    // bit array to int //
    public int GetIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            Debug.Log("getIntFromBitArray() - Argument length shall be at most 32 bits.");
        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }

    // normalize 0|254 to -1|1
    public float NormalizeValue(float value)
    {
        return (2 * (value / 254)) - 1;
    }

    // Rotate an Alpha8 square array of n size by 90 degrees //
    public byte[] RotateAlpha8(byte[] matrix, int n)
    {
        byte[] ret = new byte[n * n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                ret[i * n + j] = matrix[(n - j - 1) * n + i];
            }
        }
        return ret;
    }

    // Rotate a Color32 square array of n size by 90 degrees //
    public Color32[] RotateMatrix(Color32[] matrix, int n)
    {
        Color32[] ret = new Color32[n * n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                ret[i * n + j] = matrix[(n - j - 1) * n + i];
            }
        }
        return ret;
    }

    // bool array to int //
    public int BoolArrayToInt(bool[] bits)
    {
        uint r = 0;
        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i])
            {
                r |= (uint)(1 << (bits.Length - i));
            }
        }
        return (int)(r / 2);
    }

    // normalize 0|15 to 0|255 //
    public int NormalizeHalfResAlphaPixel(int value)
    {
        return value * 255 / 15;
    }

    // bool array to byte //
    public byte ConvertBoolArrayToByte(bool[] source)
    {
        byte result = 0;
        int index = 8 - source.Length;
        foreach (bool b in source)
        {
            if (b)
                result |= (byte)(1 << (7 - index));
            index++;
        }
        return result;
    }

    // read string that ends in byte 0 //
    public string ReadNullTerminatedString(MemoryStream stream)
    {
        StringBuilder sb = new StringBuilder();
        char c;
        while ((c = Convert.ToChar(stream.ReadByte())) != 0)
        {
            sb.Append(c);
        }
        return sb.ToString();
    }

    // read bounding box //
    public BoundingBox ReadBoundingBox(MemoryStream stream)
    {
        BoundingBox box = new BoundingBox
        {
            min = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream)),
            max = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream))
        };
        return box;
    }

    public M2Array ReadM2Array(MemoryStream stream)
    {
        M2Array m2array = new M2Array
        {
            size = ReadLong(stream),
            offset = ReadLong(stream)
        };

        return m2array;
    }

    /*
    public M2Vertex ReadM2Vertex(MemoryStream stream)
    {
        M2Vertex m2vertex = new M2Vertex
        {
            pos = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream)),
            bone_weights = new float[] { stream.ReadByte() / 255.0f, stream.ReadByte() / 255.0f, stream.ReadByte() / 255.0f, stream.ReadByte() / 255.0f },
            bone_indices = new int[] { stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte() },
            normal = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream)),
            tex_coords = new Vector2(ReadFloat(stream), ReadFloat(stream)),
            tex_coords2 = new Vector2(ReadFloat(stream), ReadFloat(stream))
        };

        return m2vertex;
    }
    */
}
