using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class BLP {

    private static string ReadFourCCReverse(Stream stream) // 4 byte to 4 chars
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
                    str = str + s;
                }
            }
            catch
            {
                Debug.Log("Couldn't convert Byte to Char: " + b);
            }
        }
        return str;
    }
}
