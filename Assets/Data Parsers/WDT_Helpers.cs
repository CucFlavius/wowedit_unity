using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class WDT {
    /*
 * Helper Methods for reading byte data
 */

    private static string ReadFourCC(Stream stream) // 4 byte to 4 chars
    {

        string str = "";
        for (int i = 1; i <= 4; i++)
        {
            int b = stream.ReadByte();
            var s = System.Convert.ToChar(b);
            if (s != '\0')
            {
                str = s + str;
            }
        }
        return str;
    }

    private static int ReadShort(Stream stream) // 2 bytes to int
    {

        byte[] bytes = new byte[2];
        int value;
        bytes[0] = (byte)stream.ReadByte();
        bytes[1] = (byte)stream.ReadByte();
        value = System.BitConverter.ToInt16(bytes, 0);
        return value;
    }

    private static int ReadLong(Stream stream) // 4 bytes to int
    {
        byte[] bytes = new byte[4];
        int value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToInt32(bytes, 0);
        return value;
    }

    private static float ReadFloat(Stream stream) // 4 bytes to float
    {
        byte[] bytes = new byte[4];
        float value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToSingle(bytes, 0);
        return value;
    }

    private static ulong ReadUint64(Stream stream) // 8 bytes to ulong
    {
        byte[] bytes = new byte[8];
        ulong value;
        for (int i = 0; i < 8; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToUInt64(bytes, 0);
        return value;
    }

    private static int GetIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            Debug.Log("getIntFromBitArray() - Argument length shall be at most 32 bits.");
        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];

    }
}
