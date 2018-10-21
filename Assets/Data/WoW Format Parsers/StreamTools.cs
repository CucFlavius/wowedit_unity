using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class StreamTools
{
    public int getUintFrom4Bits(bool[] bits)
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
        else if (!a && !b && c && !d)
            v = 2;
        else if (!a && !b && c && d)
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

    // 2 bytes to uint //
    public int ReadUint16(MemoryStream stream)
    {
        byte[] bytes = new byte[2];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToUInt16(bytes, 0);
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

    // 4 bytes to uint //
    public int ReadUint32(MemoryStream stream)
    {
        byte[] bytes = new byte[4];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = (int)System.BitConverter.ToUInt32(bytes, 0);
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

    // 8 bytes to ulong //
    public long ReadInt64(MemoryStream stream)
    {
        byte[] bytes = new byte[8];
        long value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToInt64(bytes, 0);
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
        Vector3 rawMin = new Vector3(ReadFloat(stream) / Settings.worldScale, ReadFloat(stream) / Settings.worldScale, ReadFloat(stream) / Settings.worldScale);
        Vector3 rawMax = new Vector3(ReadFloat(stream) / Settings.worldScale, ReadFloat(stream) / Settings.worldScale, ReadFloat(stream) / Settings.worldScale);
        BoundingBox box = new BoundingBox
        {
            min = new Vector3(-rawMin.x, rawMin.z, -rawMin.y),
            max = new Vector3(-rawMax.x, rawMax.z, -rawMax.y)
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

    public M2TrackBase ReadM2Track(MemoryStream stream)
    {
        M2TrackBase m2TrackBase = new M2TrackBase
        {
            interpolationtype = (InterpolationType)ReadUint16(stream),
            GlobalSequenceID = ReadShort(stream),
            Timestamps = ReadM2Array(stream),
            Values = ReadM2Array(stream)
        };
        //Debug.Log(m2TrackBase.Timestamps.size);
        return m2TrackBase;
    }

    public Quaternion ReadQuaternion16(MemoryStream stream)
    {
        short x = (short)ReadShort(stream);
        short y = (short)ReadShort(stream);
        short z = (short)ReadShort(stream);
        short w = (short)ReadShort(stream);

        return new Quaternion
        (
            ShortQuatValueToFloat(x),
            ShortQuatValueToFloat(y),
            ShortQuatValueToFloat(z),
            ShortQuatValueToFloat(w)
        );
    }

    public float ShortQuatValueToFloat(short inShort)
    {
        return inShort / (float)short.MaxValue;
    }

    public string ReadCString(MemoryStream ms)
    {
        return ReadCString(ms, Encoding.UTF8);
    }

    public string ReadCString(MemoryStream ms, Encoding encoding)
    {
        var bytes = new System.Collections.Generic.List<byte>();
        byte b;
        while ((b = (byte)ms.ReadByte()) != 0)
            bytes.Add(b);
        return encoding.GetString(bytes.ToArray());
    }


    public T[] ReadArray<T>(MemoryStream ms) where T : struct
    {
        int numBytes = (int)ReadInt64(ms);

        byte[] result = new byte[numBytes];
        ms.Read(result, 0, numBytes);

        ms.Position += (0 - numBytes) & 0x07;
        return FastStruct<T>.ReadArray(result);
    }

    public T[] ReadArray<T>(MemoryStream ms, int size) where T : struct
    {
        int numBytes = Marshal.SizeOf<T>() * size;

        byte[] result = new byte[numBytes];
        ms.Read(result, 0, numBytes);

        return FastStruct<T>.ReadArray(result);
    }


    public T Read<T>(MemoryStream ms) where T : struct
    {
        int size = FastStruct<T>.Size;
        byte[] result = new byte[size];
        ms.Read(result, 0, size);
        return FastStruct<T>.ArrayToStructure(result);
    }

    private byte[] m_array;
    private int m_readPos;
    private int m_readOffset;

    public uint ReadUInt32(int numBits)
    {
        uint result = FastStruct<uint>.ArrayToStructure(ref m_array[m_readOffset + (m_readPos >> 3)]) << (32 - numBits - (m_readPos & 7)) >> (32 - numBits);
        m_readPos += numBits;
        return result;
    }

    public DB2.Value32 ReadValue32(MemoryStream ms, int numBits)
    {
        unsafe
        {
            ulong result = ReadUInt32(numBits);
            return *(DB2.Value32*)&result;
        }
    }
}
