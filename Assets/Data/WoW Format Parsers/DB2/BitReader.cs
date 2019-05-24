using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class DB2
{
    public class BitReader
    {
        private readonly byte[] m_array;
        private int m_readPos;
        private int m_readOffset;

        public int Position { get => m_readPos; set => m_readPos = value; }
        public int Offset { get => m_readOffset; set => m_readOffset = value; }

        public BitReader(byte[] data) => m_array = data;

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

        public Value64 ReadValue64Signed(int numBits)
        {
            unsafe
            {
                ulong result = ReadUInt64(numBits);
                ulong signedShift = (1UL << (numBits - 1));
                result = (signedShift ^ result) - signedShift;
                return *(Value64*)&result;
            }
        }

        public string ReadCString()
        {
            uint num;

            List<byte> bytes = new List<byte>(0x20);
            while ((num = ReadUInt32(8)) != 0)
                bytes.Add((byte)num);

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
