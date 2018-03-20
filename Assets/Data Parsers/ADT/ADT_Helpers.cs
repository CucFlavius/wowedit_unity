using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static partial class ADT {

    /*
     * Helper Methods for reading byte data
     */

    private static string ReadFourCC(MemoryStream stream) // 4 byte to 4 chars
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

    private static int ReadShort(MemoryStream stream) // 2 bytes to int
    {

        byte[] bytes = new byte[2];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToInt16(bytes, 0);
        return value;
    }

    
    private static int ReadLong(MemoryStream stream) // 4 bytes to int
    {
        byte[] bytes = new byte[4];
        int value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToInt32(bytes, 0);
        return value;
    }
    
    private static float ReadFloat(MemoryStream stream) // 4 bytes to float
    {
        byte[] bytes = new byte[4];
        float value;
        stream.Read(bytes, 0, bytes.Length);
        value = System.BitConverter.ToSingle(bytes, 0);
        return value;
    }

    private static ulong ReadUint64(MemoryStream stream) // 8 bytes to ulong
    {
        byte[] bytes = new byte[8];
        ulong value;
        stream.Read(bytes, 0, bytes.Length);
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


    public static byte[] RotateAlpha8(byte[] matrix, int n)
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

    public static string ReadNullTerminatedString(MemoryStream stream)
    {
        StringBuilder sb = new StringBuilder();

        char c;
        while ((c = System.Convert.ToChar(stream.ReadByte())) != 0)
        {
            sb.Append(c);
        }

        return sb.ToString();
    }

    private static BoundingBox ReadBoundingBox(MemoryStream stream)
    {
        BoundingBox box = new BoundingBox();
        box.min = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream));
        box.max = new Vector3(ReadFloat(stream), ReadFloat(stream), ReadFloat(stream));
        return box;
    }



    ///////////////////////////////////
    /////////// Flag Readers///////////
    ///////////////////////////////////


    private static MCNKflags ReadMCNKflags (MemoryStream stream)
    {
        MCNKflags mcnkFlags = new MCNKflags();
        // <Flags> 4 bytes
        byte[] arrayOfBytes = new byte[4];
        stream.Read(arrayOfBytes, 0, 4);
        BitArray flags = new BitArray(arrayOfBytes);
        mcnkFlags.has_mcsh = flags[0]; // if ADTtex has MCSH chunk
        mcnkFlags.impass = flags[1];
        mcnkFlags.lq_river = flags[2];
        mcnkFlags.lq_ocean = flags[3];
        mcnkFlags.lq_magma = flags[4];
        mcnkFlags.lq_slime = flags[5];
        mcnkFlags.has_mccv = flags[6];
        mcnkFlags.unknown_0x80 = flags[7];
        mcnkFlags.do_not_fix_alpha_map = flags[15];  // "fix" alpha maps in MCAL (4 bit alpha maps are 63*63 instead of 64*64).
                                                     // Note that this also means that it *has* to be 4 bit alpha maps, otherwise UnpackAlphaShadowBits will assert.
        mcnkFlags.high_res_holes = flags[16];  // Since ~5.3 WoW uses full 64-bit to store holes for each tile if this flag is set.
        return mcnkFlags;
    }
    
    private static TerrainTextureFlag ReadTerrainTextureFlag (Stream stream)
    {
        byte[] bytes = new byte[4];
        uint value;
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)stream.ReadByte();
        }
        value = System.BitConverter.ToUInt32(bytes, 0);
        return (TerrainTextureFlag)value;
    }

    private static MDDFFlags ReadMDDFFlags (Stream stream)
    {
        byte[] arrayOfBytes = new byte[2];
        stream.Read(arrayOfBytes, 0, 2);
        BitArray flags = new BitArray(arrayOfBytes);
        MDDFFlags MDDFflags = new MDDFFlags();
        MDDFflags.mddf_biodome = flags[0];              // this sets internal flags to | 0x800 (WDOODADDEF.var0xC).
        MDDFflags.mddf_shrubbery = flags[1];            // the actual meaning of these is unknown to me. maybe biodome is for really big M2s.
                                                        // 6.0.1.18179 seems not to check for this flag
        MDDFflags.mddf_unk_4 = flags[2];                // Legion+ᵘ
        MDDFflags.mddf_unk_8 = flags[3];                // Legion+ᵘ
        MDDFflags.Flag_liquidKnown = flags[5];          // Legion+ᵘ // SMDoodadDef::Flag_liquidKnown
        MDDFflags.mddf_entry_is_filedata_id = flags[6]; // Legion+ᵘ nameId is a file data id to directly load
        MDDFflags.mddf_unk_100 = flags[8];              // Legion+ᵘ
        return MDDFflags;
    }

    private static MODFFlags ReadMODFFlags (Stream stream)
    {
        byte[] arrayOfBytes = new byte[2];
        stream.Read(arrayOfBytes, 0, 2);
        BitArray flags = new BitArray(arrayOfBytes);
        MODFFlags MODFflags = new MODFFlags();
        MODFflags.modf_destroyable = flags[0];          // set for destroyable buildings like the tower in DeathknightStart. This makes it a server-controllable game object.
        MODFflags.modf_use_lod = flags[1];              // WoD(?)+: also load _LOD1.WMO for use dependent on distance
        MODFflags.modf_unk_4 = flags[2];                // Legion(?)+: unknown
        MODFflags.modf_entry_is_filedata_id = flags[3]; // Legion+: nameId is a file data id to directly load //SMMapObjDef::FLAG_FILEDATAID
        return MODFflags;
    }
}
