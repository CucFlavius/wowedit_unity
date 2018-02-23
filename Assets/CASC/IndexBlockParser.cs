using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class IndexEntry
{
	public int Index;
	public int Offset;
	public int Size;
}

public static class IndexBlockParser  {

	public static Dictionary<string, IndexEntry> LocalIndexData = new Dictionary<string, IndexEntry>();

	public static int ReadInt32BE(this BinaryReader reader)
	{
		byte[] val = reader.ReadBytes(4);
		return val[3] | val[2] << 8 | val[1] << 16 | val[0] << 24;
	}	
	
	
	public static void ParseIndex(string idx)
	{
		using (var fs = new FileStream(idx, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		using (var br = new BinaryReader(fs))
		{
			int h2Len = br.ReadInt32();
			int h2Check = br.ReadInt32();
			byte[] h2 = br.ReadBytes(h2Len);

			long padPos = (8 + h2Len + 0x0F) & 0xFFFFFFF0;
			fs.Position = padPos;

			int dataLen = br.ReadInt32();
			int dataCheck = br.ReadInt32();

			int numBlocks = dataLen / 18;

			for (int i = 0; i < numBlocks; i++)
			{
				IndexEntry info = new IndexEntry();
				byte[] keyBytes = br.ReadBytes(9);
				string keyString = Convert.ToBase64String(keyBytes);
				byte indexHigh = br.ReadByte();
				int indexLow = ReadInt32BE(br);

				info.Index = (indexHigh << 2 | (byte)((indexLow & 0xC0000000) >> 30));
				info.Offset = (indexLow & 0x3FFFFFFF);
				info.Size = br.ReadInt32();

				if (!LocalIndexData.ContainsKey(keyString)) // use first key
					LocalIndexData.Add(keyString, info);
			}

			padPos = (dataLen + 0x0FFF) & 0xFFFFF000;
			fs.Position = padPos;

			fs.Position += numBlocks * 18;
		}
	}
}
