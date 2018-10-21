using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BitReader
{
    private Stream stream;
    private byte[] byteBuffer;
    private byte currentByte;
    private int currentBit;
    private long offset;

    public void Initialize(byte[] buffer)
    {
        stream = new MemoryStream(buffer);
        currentBit = 0;
        byteBuffer = buffer;
        currentByte = byteBuffer[0];
    }

    #region Read Variables

    public byte ReadByte()
    {
        return ReadBytes(1)[0];
    }

    public Int16 ReadInt16() // ReadShort 
    {
        byte[] bytes = ReadBytes(2);
        return System.BitConverter.ToInt16(bytes, 0);
    } 

    public Int32 ReadInt32() // ReadLong
    {
        byte[] bytes = ReadBytes(4);
        return System.BitConverter.ToInt32(bytes, 0);
    }

    public Int64 ReadInt64()
    {
        byte[] bytes = ReadBytes(8);
        return System.BitConverter.ToInt64(bytes, 0);
    }

    public UInt16 ReadUInt16()
    {
        byte[] bytes = ReadBytes(2);
        return System.BitConverter.ToUInt16(bytes, 0);
    }

    public UInt32 ReadUInt32()
    {
        byte[] bytes = ReadBytes(4);
        return System.BitConverter.ToUInt32(bytes, 0);
    }

    public UInt64 ReadUInt64()
    {
        byte[] bytes = ReadBytes(8);
        return System.BitConverter.ToUInt64(bytes, 0);
    }

    public byte[] ReadBitsInt (int n)
    {
        int offsetBytes = (int)Mathf.Floor(n / 8);
        int totalBits = currentBit + n;
        byte[] values = new byte[offsetBytes + 1];
        //BitArray bits = new BitArray(values);

        byte[] bytes = new byte[n + 1];
        stream.Read(bytes, 0, n + 1);
        bytes[0] = currentByte;
        BitArray bits = new BitArray(bytes);
        BitArray valueBits = new BitArray(values);

        for (int v = 0; v < valueBits.Length; v++)
        {
            valueBits[v] = bits[v + currentBit];
        }
        values = BitArrayToByteArray(valueBits);

        stream.Position -= 1;
        currentByte = byteBuffer[stream.Position];
        currentBit = totalBits - (offsetBytes * 8) + 1;
        return values;
    }

    #endregion

    #region Seek

    public void SeekBits(int bits)
    {
        // Seek Normally //
        if (currentBit == 0 && bits % 8 == 0)
        {
            stream.Position += bits / 8;
        }
        // Seek with bit offset //
        else
        {
            int totalBits = currentBit + bits;
            int offsetBytes = (int)Mathf.Floor(totalBits / 8);
            stream.Position += offsetBytes;
            currentBit = totalBits - (offsetBytes * 8) + 1;
        }
        currentByte = byteBuffer[stream.Position];
    }

    public void SeekBytes(int bytes)
    {
        // Seek Normally //
        if (currentBit == 0)
        {
            stream.Position += bytes;
        }
        // Seek with bit offset //
        else
        {
            int totalBits = currentBit + (bytes*8);
            int offsetBytes = (int)Mathf.Floor(totalBits / 8);
            stream.Position += offsetBytes;
            currentBit = totalBits - (offsetBytes * 8);
        }
        currentByte = byteBuffer[stream.Position];
    }

    #endregion

    #region Helpers

    public byte[] ReadBytes(int n)
    {
        byte[] value = new byte[n];

        if (currentBit == 0)
            stream.Read(value, 0, n);
        else
        {
            byte[] bytes = new byte[n + 1];
            stream.Read(bytes, 0, n + 1);
            bytes[0] = currentByte;
            BitArray bits = new BitArray(bytes);
            BitArray valueBits = new BitArray(value);

            for (int v = 0; v < valueBits.Length; v++)
            {
                valueBits[v] = bits[v + currentBit];
            }
            value = BitArrayToByteArray(valueBits);

            stream.Position -= 1;
            currentByte = byteBuffer[stream.Position];
        }
        return value;
    }

    public byte ConvertToByte(BitArray bits)
    {
        if (bits.Count != 8)
        {
            throw new ArgumentException("bits");
        }
        byte[] bytes = new byte[1];
        bits.CopyTo(bytes, 0);
        return bytes[0];
    }

    public static byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }

    #endregion
}
