using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Helper Methods for Byte operations and so on.
/// </summary>

public static partial class Casc{

    public static byte[] ToByteArray(string str)
    {
	    str = str.Replace(" ", String.Empty);

	    var res = new byte[str.Length / 2];
	    for (int i = 0; i < res.Length; ++i)
	    {
		    res[i] = Convert.ToByte(str.Substring(i* 2, 2), 16);
	    }
	    return res;
    }

    public static byte[] Strip9Bytes (byte[] bigArray )
    {
	    if (bigArray != null) {
		    byte[] smallArray = new byte[9];
		    for (int i = 0; i < 9; i++) {
			    smallArray[i] = bigArray[i];
		    }
		    return smallArray;
	    }
	    else return null;
    }

    public static string ByteString (byte[] bytes) 
    {
	    if (bytes != null) {
		    return Convert.ToBase64String(bytes);
	    }
	    else return null;
    }

    public static FileStream GetDatafileStream(int index)
    {
	    if (index >= 0){
	    var fs = new FileStream(WoWDataPath + @"\data\data."+index.ToString("000"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	    return fs;
	    }
	
	    else {
		    Debug.Log ("wrong index : "+index);
		    return null;
	    }
    }

    public static int ReadInt32BE(BinaryReader reader) 
    {
	    byte[] bytes = reader.ReadBytes(4);
	    return bytes[3] | (bytes[2] << 8) | (bytes[1] << 16) | (bytes[0] << 24);
    }

    public static bool EqualsTo(byte[] hash, byte[] other)
    {
	    if (hash.Length != other.Length)
		    return false;
	    for (int i = 0; i < hash.Length; ++i)
		    if (hash[i] != other[i])
			    return false;
	    return true;
    }


    public static void CopyTo(Stream input, Stream destination)
    {
        byte[] array = new byte[81920];
        int count;
        while (true)
        {
            count = input.Read(array, 0, array.Length);
            if (count == 0)
            {
                break;
            }
            destination.Write(array, 0, count);
        }
    }

    public static void StreamToFile(Stream InputStream, string path)
    {
        InputStream.Seek(0, SeekOrigin.Begin);
        var br = new BinaryReader(InputStream);
        byte[] data;
        data = br.ReadBytes((int)InputStream.Length);
        System.IO.File.WriteAllBytes(path, data);
        InputStream.Seek(0, SeekOrigin.Begin);
    }

    public static Stream GetEncodingData (string WoWEncodingKey){
	    if (WoWEncodingKey == null){
		    return null;
	    }
	    if (EncodingData.ContainsKey(WoWEncodingKey)){
		    return OpenWoWFile (EncodingData[WoWEncodingKey].Keys[0]);
	    }
	    else 
	    {
		    Debug.Log ("Error - Encoding Data Not Found.");
		    return null;
	    }
    }

}
