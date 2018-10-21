using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public static class WDC2
{
    private const int HeaderSize = 72;
    private const uint WDC2FmtSig = 0x32434457;     // WDC2

    public static void Read(string fileName, Stream stream)
    {
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            if (reader.BaseStream.Length < HeaderSize)
                Debug.Log("WDC2 File is corrupted or empty!");

            uint magic = reader.ReadUInt32();

            if (magic != WDC2FmtSig)
                Debug.Log("WDC2 File is corrupted");

            
        }
    }
=======
public static class WDC2 {

    public static void Read(string fileName, byte[] fileData)
    {

    }

>>>>>>> 4d5993a84f318bd801f9e78e2acf32078f5495a4
}
