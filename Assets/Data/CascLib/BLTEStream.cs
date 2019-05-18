﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

namespace CASCLib
{
    [Serializable]
    public class BLTEDecoderException : Exception
    {
        public int ErrorCode { get; }

        public BLTEDecoderException(int error, string message) : base(message)
        {
            ErrorCode = error;
        }

        public BLTEDecoderException(int error, string fmt, params object[] args) : base(string.Format(fmt, args))
        {
            ErrorCode = error;
        }
    }

    class DataBlock
    {
        public int CompSize;
        public int DecompSize;
        public MD5Hash Hash;
        public byte[] Data;
    }

    public class BLTEStream : Stream
    {
        private BinaryReader _reader;
        private MD5 _md5 = MD5.Create();
        private MemoryStream _memStream;
        private DataBlock[] _dataBlocks;
        private Stream _stream;
        private int _blocksIndex;
        private long _length;

        private const byte ENCRYPTION_SALSA20 = 0x53;
        private const byte ENCRYPTION_ARC4 = 0x41;
        private const int BLTE_MAGIC = 0x45544c42;

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _length;

        public override long Position
        {
            get { return _memStream.Position; }
            set
            {
                while (value > _memStream.Length)
                    if (!ProcessNextBlock())
                        break;

                _memStream.Position = value;
            }
        }

        public BLTEStream(Stream src, MD5Hash md5)
        {
            _stream = src;
            _reader = new BinaryReader(src);

            Parse(md5);
        }

        private void Parse(MD5Hash md5)
        {
            int size = (int)_reader.BaseStream.Length;

            if (size < 8)
                throw new BLTEDecoderException(0, "not enough data: {0}", 8);

            int magic = _reader.ReadInt32();

            if (magic != BLTE_MAGIC)
                throw new BLTEDecoderException(0, "frame header mismatch (bad BLTE file)");

            int headerSize = _reader.ReadInt32BE();

            if (CASCConfig.ValidateData)
            {
                long oldPos = _reader.BaseStream.Position;

                _reader.BaseStream.Position = 0;

                byte[] newHash = _md5.ComputeHash(_reader.ReadBytes(headerSize > 0 ? headerSize : size));

                if (!md5.EqualsTo(newHash))
                    throw new BLTEDecoderException(0, "data corrupted");

                _reader.BaseStream.Position = oldPos;
            }

            int numBlocks = 1;

            if (headerSize > 0)
            {
                if (size < 12)
                    throw new BLTEDecoderException(0, "not enough data: {0}", 12);

                byte[] fcbytes = _reader.ReadBytes(4);

                numBlocks = fcbytes[1] << 16 | fcbytes[2] << 8 | fcbytes[3] << 0;

                if (fcbytes[0] != 0x0F || numBlocks == 0)
                    throw new BLTEDecoderException(0, "bad table format 0x{0:x2}, numBlocks {1}", fcbytes[0], numBlocks);

                int frameHeaderSize = 24 * numBlocks + 12;

                if (headerSize != frameHeaderSize)
                    throw new BLTEDecoderException(0, "header size mismatch");

                if (size < frameHeaderSize)
                    throw new BLTEDecoderException(0, "not enough data: {0}", frameHeaderSize);
            }

            _dataBlocks = new DataBlock[numBlocks];

            for (int i = 0; i < numBlocks; i++)
            {
                DataBlock block = new DataBlock();

                if (headerSize != 0)
                {
                    block.CompSize = _reader.ReadInt32BE();
                    block.DecompSize = _reader.ReadInt32BE();
                    block.Hash = _reader.Read<MD5Hash>();
                }
                else
                {
                    block.CompSize = size - 8;
                    block.DecompSize = size - 8 - 1;
                    block.Hash = default;
                }

                _dataBlocks[i] = block;
            }

            _memStream = new MemoryStream(_dataBlocks.Sum(b => b.DecompSize));

            ProcessNextBlock();

            _length = headerSize == 0 ? _memStream.Length : _memStream.Capacity;

            //for (int i = 0; i < _dataBlocks.Length; i++)
            //{
            //    ProcessNextBlock();
            //}
        }

        private void HandleDataBlock(byte[] data, int index)
        {
            switch (data[0])
            {
                case 0x45: // E (encrypted)
                    (byte[] decryptedData, bool isDecrypted) = Decrypt(data, index);
                    if (isDecrypted)
                        HandleDataBlock(decryptedData, index);
                    else
                        _memStream.Write(new byte[_dataBlocks[index].DecompSize], 0, _dataBlocks[index].DecompSize);
                    break;
                case 0x46: // F (frame, recursive)
                    throw new BLTEDecoderException(1, "DecoderFrame not implemented");
                case 0x4E: // N (not compressed)
                    _memStream.Write(data, 1, data.Length - 1);
                    break;
                case 0x5A: // Z (zlib compressed)
                    Decompress(data, _memStream);
                    break;
                default:
                    throw new BLTEDecoderException(1, "unknown BLTE block type {0} (0x{1:X2})!", (char)data[0], data[0]);
            }
        }

        private static (byte[] data, bool isDecrypted) Decrypt(byte[] data, int index)
        {
            byte keyNameSize = data[1];

            if (keyNameSize == 0 || keyNameSize != 8)
                throw new BLTEDecoderException(2, "keyNameSize == 0 || keyNameSize != 8");

            byte[] keyNameBytes = new byte[keyNameSize];
            Array.Copy(data, 2, keyNameBytes, 0, keyNameSize);

            ulong keyName = BitConverter.ToUInt64(keyNameBytes, 0);

            byte IVSize = data[keyNameSize + 2];

            if (IVSize != 4 || IVSize > 0x10)
                throw new BLTEDecoderException(2, "IVSize != 4 || IVSize > 0x10");

            byte[] IVpart = new byte[IVSize];
            Array.Copy(data, keyNameSize + 3, IVpart, 0, IVSize);

            if (data.Length < IVSize + keyNameSize + 4)
                throw new BLTEDecoderException(2, "data.Length < IVSize + keyNameSize + 4");

            int dataOffset = keyNameSize + IVSize + 3;

            byte encType = data[dataOffset];

            if (encType != ENCRYPTION_SALSA20 && encType != ENCRYPTION_ARC4) // 'S' or 'A'
                throw new BLTEDecoderException(2, "encType != ENCRYPTION_SALSA20 && encType != ENCRYPTION_ARC4");

            dataOffset++;

            // expand to 8 bytes
            byte[] IV = new byte[8];
            Array.Copy(IVpart, IV, IVpart.Length);

            // magic
            for (int shift = 0, i = 0; i < sizeof(int); shift += 8, i++)
            {
                IV[i] ^= (byte)((index >> shift) & 0xFF);
            }

            byte[] key = KeyService.GetKey(keyName);

            bool hasKey = key != null;

            if (key == null)
            {
                key = new byte[16];
                if (CASCConfig.ThrowOnMissingDecryptionKey && index == 0)
                    throw new BLTEDecoderException(3, "unknown keyname {0:X16}", keyName);
                //return null;
            }

            if (encType == ENCRYPTION_SALSA20)
            {
                ICryptoTransform decryptor = KeyService.SalsaInstance.CreateDecryptor(key, IV);

                return (decryptor.TransformFinalBlock(data, dataOffset, data.Length - dataOffset), hasKey);
            }
            else
            {
                // ARC4 ?
                throw new BLTEDecoderException(2, "encType ENCRYPTION_ARC4 not implemented");
            }
        }

        private static void Decompress(byte[] data, Stream outStream)
        {
            // skip first 3 bytes (zlib)
            using (var ms = new MemoryStream(data, 3, data.Length - 3))
            using (var dfltStream = new DeflateStream(ms, CompressionMode.Decompress))
            {
                dfltStream.CopyTo(outStream);
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_memStream.Position + count > _memStream.Length && _blocksIndex < _dataBlocks.Length)
            {
                if (!ProcessNextBlock())
                    return 0;

                return Read(buffer, offset, count);
            }
            else
            {
                return _memStream.Read(buffer, offset, count);
            }
        }

        private bool ProcessNextBlock()
        {
            if (_blocksIndex == _dataBlocks.Length)
                return false;

            long oldPos = _memStream.Position;
            _memStream.Position = _memStream.Length;

            DataBlock block = _dataBlocks[_blocksIndex];

            block.Data = _reader.ReadBytes(block.CompSize);

            if (!block.Hash.IsZeroed() && CASCConfig.ValidateData)
            {
                byte[] blockHash = _md5.ComputeHash(block.Data);

                if (!block.Hash.EqualsTo(blockHash))
                    throw new BLTEDecoderException(0, "MD5 mismatch");
            }

            HandleDataBlock(block.Data, _blocksIndex);
            _blocksIndex++;

            _memStream.Position = oldPos;

            return true;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _stream?.Dispose();
                    _reader?.Dispose();
                    _memStream?.Dispose();
                }
            }
            finally
            {
                _stream = null;
                _reader = null;
                _memStream = null;

                base.Dispose(disposing);
            }
        }
    }
}
