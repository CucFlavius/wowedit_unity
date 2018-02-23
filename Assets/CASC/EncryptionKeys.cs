using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EncryptionKeys {

    public static Dictionary<string, byte[]> keys = new Dictionary<string, byte[]>();


    // [0xF381BFA8B60C68FB] = "62D90EFA7F36D71C398AE2F1FE37BDB9".ToByteArray(),
    //key - f381bfa8b60c68fb = 62d90efa7f36d71c398ae2f1fe37bdb9


    public static byte[] GetKey(string keyName)
    {
        if (keys.ContainsKey(keyName))
        {
            byte[] key = keys[keyName];
            //keys.TryGetValue(keyName, out byte[] key);
            return key;
        }
        else
        {
            return null;
        }
    }

    private static Salsa20 salsa = new Salsa20();

    //public static Salsa20 SalsaInstance => salsa;
    public static Salsa20 SalsaInstance { get { return salsa; } }

    public static void ParseKeyFile (String path)
    {
        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string[] elements = line.Split(" "[0]);

            string key = (elements[0].Split("-"[0]))[1].ToUpper();
            string value = elements[2].ToUpper();
            EncryptionKeys.keys[key] = ToByteArray(value);
        }
    }

    private static byte[] ToByteArray(String str)
    {
        str = str.Replace(" ", String.Empty);

        byte[] res = new byte[str.Length / 2];
        for (int i = 0; i < res.Length; ++i)
        {
            res[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
        }
        return res;
    }

}


