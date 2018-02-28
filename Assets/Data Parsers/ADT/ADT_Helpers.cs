using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT {

    /*
     * Helper Methods for reading byte data
     */

    private static string ReadFourCC(Stream stream) // 4 byte to 4 chars
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
        for (int i = 0; i < 4; i++) {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToInt32(bytes, 0);
        return value;
    }

    private static float ReadFloat(Stream stream) // 4 bytes to float
    {
        byte[] bytes = new byte[4];
        float value;
        for (int i = 0; i < 4; i++) {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToSingle(bytes, 0);
        return value;
    }

    private static ulong ReadUint64(Stream stream) // 8 bytes to ulong
    {
        byte[] bytes = new byte[8];
        ulong value;
        for (int i = 0; i < 8; i++) {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToUInt64(bytes, 0);
        return value;
    }

    private static int GetIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            Debug.Log ("getIntFromBitArray() - Argument length shall be at most 32 bits.");
        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];

    }

    private static float NormalizeValue(float value)
    {
        //			 	    x - minX
        //	[-1,1] = 2 * -------------- -1
        //				   maxX - minX
        // Normalizing 0 - 254
        float normalized = (2 * (value / 254)) - 1;
        return normalized;
    }

    public static Color32[] RotateMatrix(Color32[] matrix , int n)
    {
        Color32[] ret = new Color32[n * n];   
	    for (int i = 0; i<n; i++) {
		    for (int j = 0; j<n; j++) {
			    ret[i * n + j] = matrix[(n - j - 1) * n + i];
		    }
	    }   
	    return ret;
    }

    private static int BoolArrayToInt(bool[] bits)
    {
	    uint r = 0;
	    for(int i = 0; i<bits.Length; i++) {
		    if(bits[i]) {
			    r |= (uint)(1 << (bits.Length - i));
		    }
	    }
	    return (int)(r/2);
    }

    private static int NormalizeHalfResAlphaPixel(int value)
    {
        //					  newMax - newMin
        //	In = (I - Min) * ----------------- + newMin
        //						 Max - Min
        int normalizedValue = value * 255 / 15;
        return normalizedValue;
    }

    private static int Fbooltoint(bool [] barray)
    {
        int number = 0;
        if (barray[0] == false)
        {
            if (barray[1] == false)
            {
                if (barray[2] == false)
                {
                    if (barray[3] == false)
                    {
                        number = 0;
                    }
                    if (barray[3] == true)
                    {
                        number = 1;
                    }
                }
                if (barray[2] == true)
                {
                    if (barray[3] == false)
                    {
                        number = 2;
                    }
                    if (barray[3] == true)
                    {
                        number = 3;
                    }
                }
            }
            if (barray[1] == true)
            {
                if (barray[2] == false)
                {
                    if (barray[3] == false)
                    {
                        number = 4;
                    }
                    if (barray[3] == true)
                    {
                        number = 5;
                    }
                }
                if (barray[2] == true)
                {
                    if (barray[3] == false)
                    {
                        number = 6;
                    }
                    if (barray[3] == true)
                    {
                        number = 7;
                    }
                }
            }
        }
        if (barray[0] == true)
        {
            if (barray[1] == false)
            {
                if (barray[2] == false)
                {
                    if (barray[3] == false)
                    {
                        number = 8;
                    }
                    if (barray[3] == true)
                    {
                        number = 9;
                    }
                }
                if (barray[2] == true)
                {
                    if (barray[3] == false)
                    {
                        number = 10;
                    }
                    if (barray[3] == true)
                    {
                        number = 11;
                    }
                }
            }
            if (barray[1] == true)
            {
                if (barray[2] == false)
                {
                    if (barray[3] == false)
                    {
                        number = 12;
                    }
                    if (barray[3] == true)
                    {
                        number = 13;
                    }
                }
                if (barray[2] == true)
                {
                    if (barray[3] == false)
                    {
                        number = 14;
                    }
                    if (barray[3] == true)
                    {
                        number = 15;
                    }
                }
            }
        }
        return number;
    }

    private static byte ConvertBoolArrayToByte(bool[] source)
    {
        byte result = 0;
        // This assumes the array never contains more than 8 elements!
        int index = 8 - source.Length;

        // Loop through the array
        foreach (bool b in source)
        {
            // if the element is 'true' set the bit at that position
            if (b)
                result |= (byte)(1 << (7 - index));

            index++;
        }

        return result;
    }
}
