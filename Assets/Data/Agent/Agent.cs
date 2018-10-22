using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Agent
{
    public static List<string> FindWowInstalls()
    {
        string agentPath = GetPath();
        string productDBPath = Path.GetDirectoryName(Path.GetDirectoryName(agentPath)) + @"\product.db";
        ReadProductDB(productDBPath);

        return null;
    }

    private static void ReadProductDB(string path)
    {
        byte[] productDBData = File.ReadAllBytes(path);
        using (MemoryStream ms = new MemoryStream(productDBData))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                br.BaseStream.Position += 4;    // skip unknown 4
                int NameSize = br.ReadByte();
                string Name = new string(br.ReadChars(NameSize));
                int skipPosition0 = br.ReadByte();
                int IDSize = br.ReadByte();
                string ID = new string(br.ReadChars(IDSize));
                br.BaseStream.Position += 3;    // skip unknown 3
                int pathSize = br.ReadByte();
                string productPath = new string(br.ReadChars(pathSize));
                br.BaseStream.Position += 1;    // skip unknown 1
                int localeSize = br.ReadByte();
                string locale = new string(br.ReadChars(localeSize));
                int skipPosition1 = br.ReadByte();
                int unkSize0 = br.ReadByte();
                br.BaseStream.Position += skipPosition1;    // skip unknown
                br.BaseStream.Position += 2; // skip unk 2
                br.BaseStream.Position += 5; // skip J.R.Z
                int acctSize = br.ReadByte();
                string acct = new string(br.ReadChars(acctSize));
                br.BaseStream.Position += 1; // skip unk 1
                int geoIPSize = br.ReadByte();
                string geoIP = new string(br.ReadChars(geoIPSize));
                br.BaseStream.Position += 18; // skip 18
                int versionSize = br.ReadByte();
                string version = new string(br.ReadChars(versionSize));
                br.BaseStream.Position += 2; // skip unk 2
                br.BaseStream.Position += 1; // skip unk 1
                int locale2Size = br.ReadByte();
                string locale2 = new string(br.ReadChars(locale2Size));
                br.BaseStream.Position += 1; // skip unk 1
                int buildKeySize = br.ReadByte();
                string buildKey = new string(br.ReadChars(buildKeySize));
                br.BaseStream.Position += 3; // skip unk 3
                int lastSize = br.ReadByte();
                string last = new string(br.ReadChars(lastSize));
                br.BaseStream.Position += 1; // skip unk 1
                int buildKey2Size = br.ReadByte();
                string buildKey2 = new string(br.ReadChars(buildKey2Size));
                br.BaseStream.Position += 3; // skip unk 3
                int locale3Size = br.ReadByte();
                string locale3 = new string(br.ReadChars(locale3Size));

            }
        }
    }

    private static string GetPath()
    {
        System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcessesByName("Agent");
        return localByName[0].MainModule.FileName.ToString();
    }
}
