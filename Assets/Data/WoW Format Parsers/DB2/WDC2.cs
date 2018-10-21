using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

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
}
