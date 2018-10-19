using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class DB2
{
    public static string[] fileNames = new string[] { "animationdata.db2" };
    public static Dictionary<string, bool> availableFiles = new Dictionary<string, bool>();
    private static string dbfilesclient = @"dbfilesclient\";

    public static void Initialize()
    {
        // Check existing DB2 files //
        
        foreach (string fileName in fileNames)
        {
            if (Casc.FileExists (dbfilesclient + fileName))
            {
                availableFiles.Add(fileName, true);
                Read(fileName);
            }
            else
            {
                availableFiles.Add(fileName, false);
                Debug.LogWarning("Warning: " + "Missing " + fileName);
            }
        }
    }

    public static void Read(string fileName)
    {
        string dataPath = dbfilesclient + fileName;
        string path = Casc.GetFile(dataPath);
        byte[] fileData = File.ReadAllBytes(path);

        // Check DB2 Version //

        // WDC //
        if (fileData[0] == Convert.ToByte('W') && fileData[1] == Convert.ToByte('D') && fileData[2] == Convert.ToByte('C'))
        {
            // WDC 1 //
            if (fileData[3] == Convert.ToByte('1'))
            {
                WDC1.Read(fileName, fileData);
            }
            // WDC 2 //
            if (fileData[3] == Convert.ToByte('2'))
            {
                WDC2.Read(fileName, fileData);
            }
            // WDC 3 //
            if (fileData[3] == Convert.ToByte('3'))
            {
                WDC3.Read(fileName, fileData);
            }
        }

        // WDB //
        else
        {
            Debug.LogWarning("Warning: " + "DB2 Format not supported: " + fileData[0] + fileData[1] + fileData[2] + fileData[3] + fileName);
        }
    }

    public class BitReader
    {
        private byte[] m_array;
        private int m_readPos;
        private int m_readOffset;

        public int Position { get { return m_readPos; } set { m_readPos = value; } }
        public int Offset { get { return m_readOffset; }  set { m_readOffset = value; } }
        public byte[] Data { get { return m_array; } set { m_array = value; } }

        public BitReader(byte[] data)
        {
            m_array = data;
        }

        public BitReader(byte[] data, int offset)
        {
            m_array = data;
            m_readOffset = offset;
        }

        public uint ReadUInt32(int numBits)
        {
            uint result = FastStruct<uint>.ArrayToStructure(ref m_array[m_readOffset + (m_readPos >> 3)]) << (32 - numBits - (m_readPos & 7)) >> (32 - numBits);
            m_readPos += numBits;
            return result;
        }

        public ulong ReadUInt64(int numBits)
        {
            ulong result = FastStruct<ulong>.ArrayToStructure(ref m_array[m_readOffset + (m_readPos >> 3)]) << (64 - numBits - (m_readPos & 7)) >> (64 - numBits);
            m_readPos += numBits;
            return result;
        }

        public Value32 ReadValue32(int numBits)
        {
            unsafe
            {
                ulong result = ReadUInt32(numBits);
                return *(Value32*)&result;
            }
        }

        public Value64 ReadValue64(int numBits)
        {
            unsafe
            {
                ulong result = ReadUInt64(numBits);
                return *(Value64*)&result;
            }
        }

        public string ReadCString()
        {
            uint num;
            List<byte> bytes = new List<byte>();
            int byteSize = 8;

            while ((num = ReadUInt32(byteSize)) != 0)
                bytes.Add(Convert.ToByte(num));

            return System.Text.Encoding.UTF8.GetString(bytes.ToArray());
        }
    }

}
