using Assets.Data.CASC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class BLTE {

    public static MemoryStream OpenFile (Stream stream, int size)
    {
        MemoryStream memStream = new MemoryStream();
        //Debug.Log(stream.Position + " " + stream.Length);
	    memStream = ExtractData(stream, size);
	    if (memStream != null) {
		    memStream.Position = 0;
		    return memStream;
	    }
	    else return null;
    }

    public static MemoryStream ExtractData(Stream stream, int size)
    {
	    if (size < 8) {
		    Debug.Log ("Error : BLTE block too small");
		    return null;
	    }   
	    BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
        MD5 md5 = MD5.Create();
        MemoryStream Mstream = new MemoryStream(); // write to this one
        int magic = reader.ReadInt32(); // BLTE (raw)
	    if (magic != 0x45544c42) 
	    {
		    Debug.Log ("Error: BLTE_ExtractData - Wrong BLTE Magic");
		    return null;
	    }
	    else
	    {
            int frameHeaderSize = Casc.ReadInt32BE(reader);
            int chunkCount = 0;
            int totalSize = 0;
		    if (frameHeaderSize == 0)
		    {
			    totalSize = size - 8;
			    chunkCount = 1;
		    }
		    else
		    {
			    byte unk1 = reader.ReadByte(); // byte
			    if (unk1 != 0x0F) {
				    Debug.Log ("Error: BLTE_ExtractData - unk1 != 0x0F");
				    return null;
			    }
                byte v1 = reader.ReadByte();
                byte v2 = reader.ReadByte();
                byte v3 = reader.ReadByte();
			    chunkCount = v1 << 16 | v2 << 8 | v3 << 0; // 3-byte
		    }
		    if (chunkCount < 0) {
			    Debug.Log ("Error: BLTE_ExtractData - Possible error ({0}) at offset: {1:X8}");
			    return null;
		    }
            BLTEChunk[] chunks = new BLTEChunk[chunkCount];
		    for (int i = 0; i < chunkCount; i++) {
			    chunks[i] = new BLTEChunk();
			    if (frameHeaderSize != 0)
			    {
				    chunks[i].CompSize = Casc.ReadInt32BE(reader);
				    chunks[i].DecompSize = Casc.ReadInt32BE(reader);
				    chunks[i].Hash = reader.ReadBytes(16);
			    }
			    else
			    {
			    chunks[i].CompSize = totalSize;
			    chunks[i].DecompSize = totalSize - 1;
			    chunks[i].Hash = null;	
			    }
		    }
		    for (int i1 = 0; i1 < chunkCount; i1++){
			    chunks[i1].Data = reader.ReadBytes(chunks[i1].CompSize);
			    if (chunks[i1].Data.Length != chunks[i1].CompSize)
			    {
				    Debug.Log ("Error: BLTE_ExtractData - chunks[i1].data.Length != chunks[i1].compSize"); //might need a return
			    }
			    if (frameHeaderSize != 0)
			    {
                    byte[] hh = md5.ComputeHash(chunks[i1].Data);
				    if (!Casc.EqualsTo(hh,chunks[i1].Hash)) {
					    Debug.Log ("Error: BLTE_ExtractData - MD5 missmatch!"); //might need return
				    }
			    }
			    HandleDataBlock(chunks[i1].Data, i1, Mstream);
		    }
	    }
        Mstream.Position = 0;
	    return Mstream;
    }

    public static void HandleDataBlock (byte[] data, int index, Stream stream)
    {
	    switch (data[0])
	    {
		    case 0x4E: // N
			    stream.Write(data, 1, data.Length - 1);
			    break;
		    case 0x5A: // Z
			    Casc.CopyTo(BLTE_Decompress(data),stream);
			    break;
            case 0x45: // E (encrypted)
                byte[] decrypted = BLTE.Decrypt(data, index);
			    if (decrypted == null)
			    {
				    break;
			    }
                HandleDataBlock(decrypted, index, stream);
                break;
		    default:
			    Debug.Log ("Error: BLTE_ExtractData - Unknown byte at switch case : "+ data[0]); //might need return
                break;
	    }
    }

    public static Stream BLTE_Decompress (byte[] data) {
	    //byte[] buf = new byte[0x80000];
        MemoryStream ms = new MemoryStream(data, 3, data.Length - 3);
        DeflateStream dStream = new DeflateStream(ms, CompressionMode.Decompress);
	    return dStream;
    }

    public static byte[] Decrypt(byte[] data, int index)
    {
        const byte ENCRYPTION_SALSA20 = 0x53;
        const byte ENCRYPTION_ARC4 = 0x41;
        //const int BLTE_MAGIC = 0x45544c42;
        byte keyNameSize = data[1];
        if (keyNameSize == 0 || keyNameSize != 8)
            Debug.Log("error");
        byte[] keyNameBytes = new byte[keyNameSize];
        Array.Copy(data, 2, keyNameBytes, 0, keyNameSize);
        string keyName = BitConverter.ToString(keyNameBytes).Replace("-", String.Empty);
        byte IVSize = data[keyNameSize + 2];
        if (IVSize != 4 || IVSize > 0x10)
            Debug.Log("error");
        byte[] IVpart = new byte[IVSize];
        Array.Copy(data, keyNameSize + 3, IVpart, 0, IVSize);
        if (data.Length < IVSize + keyNameSize + 4)
            Debug.Log("error");
        int dataOffset = keyNameSize + IVSize + 3;
        byte encType = data[dataOffset];
        if (encType != ENCRYPTION_SALSA20 && encType != ENCRYPTION_ARC4) // 'S' or 'A'
            Debug.Log("error");
        dataOffset++;
        // expand to 8 bytes
        byte[] IV = new byte[8];
        Array.Copy(IVpart, IV, IVpart.Length);
        // magic
        for (int shift = 0, i = 0; i < sizeof(int); shift += 8, i++)
        {
            IV[i] ^= (byte)((index >> shift) & 0xFF);
        }
        Debug.Log("K " + keyName.ToString());
        byte[] key = EncryptionKeys.GetKey(keyName.ToString());
        if (key == null)
        {
            Debug.Log("error");
            return null;
        }
        if (encType == ENCRYPTION_SALSA20)
        {
            ICryptoTransform decryptor = EncryptionKeys.SalsaInstance.CreateDecryptor(key, IV);
            return decryptor.TransformFinalBlock(data, dataOffset, data.Length - dataOffset);
        }
        else
        {
            // ARC4 ?
            Debug.Log("error");
            return null;
        }
    }
}
