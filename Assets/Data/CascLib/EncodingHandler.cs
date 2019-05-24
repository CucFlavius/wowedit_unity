using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace CASCLib
{
    public struct EncodingEntry
    {
        public MD5Hash Key;
        public long Size;
    }

    public class EncodingHandler
    {
        private static readonly MD5HashComparer comparer = new MD5HashComparer();
        private Dictionary<MD5Hash, EncodingEntry> EncodingData = new Dictionary<MD5Hash, EncodingEntry>(comparer);

        private const int CHUNK_SIZE = 4096;

        public int Count => EncodingData.Count;

        public EncodingHandler(BinaryReader stream)
        {
            stream.Skip(2); // EN
            byte Version        = stream.ReadByte(); // must be 1
            byte CKeyLength     = stream.ReadByte();
            byte EKeyLength     = stream.ReadByte();
            int CKeyPageSize    = stream.ReadInt16BE() * 1024; // KB to bytes
            int EKeyPageSize    = stream.ReadInt16BE() * 1024; // KB to bytes
            int CKeyPageCount   = stream.ReadInt32BE();
            int EKeyPageCount   = stream.ReadInt32BE();
            byte unk1           = stream.ReadByte(); // must be 0
            int ESpecBlockSize  = stream.ReadInt32BE();

            // stream.Skip(ESpecBlockSize);
            string[] strings = Encoding.ASCII.GetString(stream.ReadBytes(ESpecBlockSize)).Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

            // stream.Skip(CKeyPageCount * 32);
            ValueTuple<MD5Hash, MD5Hash>[] aEntries = new ValueTuple<MD5Hash, MD5Hash>[CKeyPageCount];
            for (int i = 0; i < CKeyPageCount; ++i)
            {
                var firstHash = stream.Read<MD5Hash>();
                var blockHash = stream.Read<MD5Hash>();
                aEntries[i] = (firstHash, blockHash);
            }

            long chunkStart = stream.BaseStream.Position;

            for (int i = 0; i < CKeyPageCount; ++i)
            {
                byte keysCount;

                while ((keysCount = stream.ReadByte()) != 0)
                {
                    var entry = new EncodingEntry()
                    {
                        Size = stream.ReadInt40BE()
                    };

                    var cKey = stream.Read<MD5Hash>();

                    // how do we handle multiple keys?
                    for (var key = 0; key < keysCount; ++key)
                    {
                        // use first key for now
                        if (key == 0)
                            entry.Key = stream.Read<MD5Hash>();
                        else
                            stream.ReadBytes(16);
                    }

                    EncodingData.Add(cKey, entry);
                }

                // each chunk is 4096 bytes, and zero padding at the end
                long remaining = CHUNK_SIZE - ((stream.BaseStream.Position - chunkStart) % CHUNK_SIZE);

                if (remaining > 0)
                    stream.BaseStream.Position += remaining;
            }

            // stream.Skip(EKeyPageCount * 32);
            for (int i = 0; i < EKeyPageCount; ++i)
            {
                var firstKey  = stream.Read<MD5Hash>();
                var blockHash = stream.Read<MD5Hash>();
            }

            long chunkStart2 = stream.BaseStream.Position;

            while(stream.BaseStream.Position < chunkStart2 + CHUNK_SIZE * EKeyPageCount)
            {
                var remaining = CHUNK_SIZE - (stream.BaseStream.Position - chunkStart2) % CHUNK_SIZE;

                if (remaining < 25)
                    stream.BaseStream.Position += remaining;

                var eKey        = stream.Read<MD5Hash>();
                int eSpecIndex  = stream.ReadInt32BE();
                long fileSize   = stream.ReadInt40BE();

                if (eSpecIndex == int.MaxValue) break;
            }


            // string block till the end of file
        }

        public IEnumerable<KeyValuePair<MD5Hash, EncodingEntry>> Entries
        {
            get
            {
                foreach (var entry in EncodingData)
                    yield return entry;
            }
        }

        public bool GetEntry(MD5Hash md5, out EncodingEntry enc) => EncodingData.TryGetValue(md5, out enc);

        public void Clear()
        {
            EncodingData.Clear();
            EncodingData = null;
        }
    }
}
