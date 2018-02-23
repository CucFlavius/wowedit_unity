using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class BuildInfoDataEntry
{
    public string Index;
    public string Type;
    public string Data;
}

public class BLTEChunk
{
    public int CompSize;
    public int DecompSize;
    public byte[] Hash;
    public byte[] Data;
}

public class EncodingEntry
{
    public int Size;
    public List<byte[]> Keys;

	public void EncodingEntryInit()
    {
        Keys = new List<byte[]>();
    }
}

public static partial class Casc
{
    public static string WoWDataPath;
    public static ArrayList buildInfoData = new ArrayList();
    public static string WoWVersion = null;
    public static string WoWEncodingKey = null;
    public static string WoWRootKey = null;
    public static int FileListSize = 0;

    //data
    public static Dictionary<String, IndexEntry> LocalIndexData = new Dictionary<String, IndexEntry>();
    public static Dictionary<String, EncodingEntry> EncodingData = new Dictionary<String, EncodingEntry>();
    public static Dictionary<ulong, String> MyRootData = new Dictionary<ulong, String>();
    public static Dictionary<String, ulong> FileListDictionary = new Dictionary< String, ulong>();

    static readonly Jenkins96 Hasher = new Jenkins96();

    public static void ReadWoWFolder()
    {
        // Check if we're in Data folder //
        WoWDataPath = Settings.Data[3];
        string[] fileInfo = Directory.GetFiles(WoWDataPath);
        foreach (string file in fileInfo)
        {
            if (System.IO.Path.GetFileName(file) == "Wow.exe")
            {
                WoWDataPath = WoWDataPath + @"Data";
            }
        }
    }

    public static void FindWoWBuildConfig()
    {
        string buildInfoFilePath = Settings.Data[3] + @"\.build.info";
        StreamReader buildInfoFileReader = new StreamReader(buildInfoFilePath);
        List<string> buildInfoFileLines = buildInfoFileReader.ReadToEnd().Split("\n"[0]).ToList();
        string buildFilePath = null;
        string[] buildInfoFileData_Index = buildInfoFileLines[0].Split("|"[0]);
        string[] buildInfoFileData_Entry_Locale1 = buildInfoFileLines[1].Split("|"[0]);
        string[] buildInfoFileData_Entry_Locale2 = new string[10];
        if (buildInfoFileLines.Count >= 3)
        {
            buildInfoFileData_Entry_Locale2 = buildInfoFileLines[2].Split("|"[0]);
        }
        for (int entry = 0; entry < buildInfoFileData_Index.Length; entry++)
	    {
            BuildInfoDataEntry bide = new BuildInfoDataEntry();
            bide.Index = buildInfoFileData_Index[entry].Split("!"[0])[0];
            bide.Type = buildInfoFileData_Index[entry].Split("!"[0])[1];
            bide.Data = buildInfoFileData_Entry_Locale2[entry];
            buildInfoData.Add(bide);

            if (bide.Index == "Build Key")
            {
                buildFilePath = WoWDataPath + @"\config\" + bide.Data.Substring(0, 2) + @"\" + bide.Data.Substring(2, 2) + @"\" + bide.Data;
            }
            if (bide.Index == "Version")
            {
                WoWVersion = bide.Data;
            }
        }
        StreamReader BuildReader = new StreamReader(buildFilePath);
        List<String> BuildfileLines = new List<String>();
        BuildfileLines = BuildReader.ReadToEnd().Split("\n"[0]).ToList();
        for (int line = 0; line < BuildfileLines.Count; line++) {
            if (BuildfileLines[line].Split('=')[0] == "encoding ")
            {
                string key = BuildfileLines[line].Split('=')[1];
                string[] strArr = key.Split(' ');
                WoWEncodingKey = strArr[strArr.Length - 1];
            }
            if (BuildfileLines[line].Split('=')[0] == "root ")
            {
                string key1 = BuildfileLines[line].Split('=')[1];
                string[] strArr1 = key1.Split(' ');
                WoWRootKey = strArr1[strArr1.Length - 1];
            }
        }
        BuildReader.Close();
        BuildReader = null;
    }

    public static void ReadWoWIDXfiles()
    {
        String[] idxFiles = Directory.GetFiles(WoWDataPath + @"\data\", "*.idx");
	    foreach (string idxfile in idxFiles)
        {
            IndexBlockParser.ParseIndex(idxfile);
        }
    }

    public static void LoadEncodingFile()
    {
        string encodingFilePath = Settings.Data[0] + @"\WoW_Encoding_" + WoWVersion + ".bin";
        // if not cached //
        if (!File.Exists(encodingFilePath))
        {
            // convert encoding key string to byte array
            byte[] WoWEncodingKeyBytes = ToByteArray(WoWEncodingKey);
            //// Extract Encoding File from BLTE and Read its Data ////
            var fs = OpenWoWFile(WoWEncodingKeyBytes);
            // cache //
            StreamToFile(fs, encodingFilePath);
            // read //
            ReadEncodingFile(fs);
        }
        // if cached //
        else if (File.Exists(encodingFilePath))
        {
            FileStream fs1 = File.OpenRead(encodingFilePath);
            // read //
            ReadEncodingFile(fs1);
        }
    }

    public static Stream OpenWoWFile (byte[] key) {
	    if (key == null) {
		    return null;
	    }
	    // trim the byte array encoding key to 9 bytes //
	    var ninebyte = Strip9Bytes(key);
	    var newString = ByteString(ninebyte);
	    if (IndexBlockParser.LocalIndexData.ContainsKey(newString))
	    {
		    var FileIndex = IndexBlockParser.LocalIndexData[newString].Index;
		    var FileOffset = IndexBlockParser.LocalIndexData[newString].Offset;
		    var FileSize = IndexBlockParser.LocalIndexData[newString].Size;
		    var stream = GetDatafileStream(FileIndex);
		    if (stream == null){return null;}
		    stream.Position = FileOffset;
		    stream.Position += 30;
		    var blte = BLTE.OpenFile(stream, FileSize - 30);
		    return blte;
	    }
	    else
	    {
		    Debug.Log ("Error : local index missing");
		    return null;
	    }
    }

    public static void ReadEncodingFile(Stream fs)
    {
        if (fs != null)
        {
            BinaryReader br = new BinaryReader(fs);
            br.ReadBytes(2); // EN
            byte b1 = br.ReadByte();
            byte b2 = br.ReadByte();
            byte b3 = br.ReadByte();
            ushort s1 = br.ReadUInt16();
            ushort s2 = br.ReadUInt16();
            int numEntries = ReadInt32BE(br);
            int i1 = ReadInt32BE(br);
            byte b4 = br.ReadByte();
            int entriesOfs = ReadInt32BE(br);
            fs.Position += entriesOfs; // skip strings
            fs.Position += numEntries * 32;
            for (int i = 0; i < numEntries; ++i)
		    {
                ushort keysCount;
                while (true)
                {
                    keysCount = br.ReadUInt16();
                    if (keysCount == 0)
                    {
                        break;
                    }
                    int fileSize = ReadInt32BE(br);
                    byte[] md5 = br.ReadBytes(16);
                    EncodingEntry entry = new EncodingEntry();
                    entry.EncodingEntryInit();
                    entry.Size = fileSize;
                    List<byte[]> entryKeysList = new List<byte[]>();
                    for (int ki = 0; ki < keysCount; ++ki)
				    {
                        byte[] key = br.ReadBytes(16);
                        entryKeysList.Add(key);
                    }
                    entry.Keys = entryKeysList;
                    EncodingData.Add(ByteString(md5), entry);
                }
                while (br.PeekChar() == 0)
                {
                    fs.Position++;
                }
            }
            fs.Close();
            fs = null;
        }
        else
        {
            Debug.Log("ReadEncodingFile null");
        }
    }

    public static void LoadWoWRootFile()
    {
        var rootFilePath = Settings.Data[0] + @"\Root_" + WoWVersion + ".bin";
        // not cached //
        if (!File.Exists(rootFilePath))
        {
            if (WoWRootKey == null)
            {
                Debug.Log("Error - WoWRootKey null");
                return;
            }
            // convert root key string to byte array
            byte[] WoWRootKeyByte = ToByteArray(WoWRootKey);
            if (WoWRootKeyByte == null)
            {
                Debug.Log("Error - WoWRootKey null");
                return;
            }
            //// Extract Root File from BLTE and Read its Data ////
            var fs = GetEncodingData(ByteString(WoWRootKeyByte));
            StreamToFile(fs, rootFilePath);
            ReadRootFile(fs);
        }
        // cached //
        else if (File.Exists(rootFilePath))
        {
            FileStream fs1 = File.OpenRead(rootFilePath);
            ReadRootFile(fs1);
        }
    }

    public static void ReadRootFile(Stream fs)
    {
        if (fs != null)
        {
            BinaryReader br = new BinaryReader(fs);
            while (fs.Position < fs.Length)
            {
                int count = br.ReadInt32();
                br.ReadBytes(8 + count * 4);
                for (var i = 0; i < count; ++i)
                {
                    byte[] MD5hash = br.ReadBytes(16);
                    string MD5hashString = ByteString(MD5hash);
                    ulong hash = br.ReadUInt64();
                    if (!MyRootData.ContainsKey(hash))
                    {
                        MyRootData.Add(hash, MD5hashString);
                    }
                    else
                    {
                        MyRootData[hash] = MD5hashString;
                    }
                }
            }
        }
        fs.Close();
        fs = null;
    }

    public static void LoadFilelist()
    {
        if (!Directory.Exists(Settings.ApplicationPath + @"\ListFiles\"))
        {
            Directory.CreateDirectory(Settings.ApplicationPath + @"\ListFiles\");
        }
        if (File.Exists(Settings.ApplicationPath + @"\ListFiles\Listfile.txt"))
        {
            StreamReader sr = File.OpenText(Settings.ApplicationPath + @"\ListFiles\Listfile.txt");
            string[] testFile = sr.ReadToEnd().Split("\n"[0]);
            FileListSize = testFile.Length;
            int position = 0;
            Debug.Log("FileList Lines : " + testFile.Length);
            foreach (string item in testFile)
            {
                if (item != null)
                {
                    ulong hashUlong = Hasher.ComputeHash(item);
                    FileListDictionary.Add(item, hashUlong);
                    position++;
                }
            }
        }
        else
        {
            Debug.Log("Missing Listfile");
        }
    }

    public static string ExtractFileToCache (ulong md5, string path)
    {
	    var fs = ExtractFileToMemory(md5);
	    if (fs != null) {
		    Directory.CreateDirectory(Path.GetDirectoryName(Settings.Data[0] + @"\" + path));
		    StreamToFile(fs, Settings.Data[0] + @"\" + path);
		    fs.Close();
		    fs = null;
		    return Settings.Data[0] + @"\" + path;
	    }
	    else
	    {
		    return null;
	    }
    }

    public static Stream ExtractFileToMemory(ulong key)
    {
	    var WoWEncodingKey = SearchRootFile(key);
        var fs = GetEncodingData(WoWEncodingKey);
	    return fs;
    }

    public static string SearchRootFile(ulong key) {
        string value;
	    if (MyRootData.TryGetValue(key, out value))
	    {
		    return value;
	    }
	    else 
	    {
	        Debug.Log ("Error : Root Key Missing");
	        return null;
	    }
    }

}