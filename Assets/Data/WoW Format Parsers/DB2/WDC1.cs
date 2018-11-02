using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static partial class DB2
{
    public static class WDC1
    {
        private const int HeaderSize = 84;
        private const uint WDC1FmtSig = 0x31434457; // WDC1

        public static void Read(string fileName, Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                if (reader.BaseStream.Length < HeaderSize)
                    Debug.Log("WDC1 File is corrupted or empty!");

                uint magic = reader.ReadUInt32();

                if (magic != WDC1FmtSig)
                    Debug.Log("WDC1 File is corrupted");

                DB2Reader.RecordsCount = reader.ReadInt32();
                DB2Reader.FieldsCount = reader.ReadInt32();
                DB2Reader.RecordSize = reader.ReadInt32();
                DB2Reader.StringTableSize = reader.ReadInt32();

                DB2Reader.TableHash = reader.ReadUInt32();
                DB2Reader.LayoutHash = reader.ReadUInt32();
                DB2Reader.MinIndex = reader.ReadInt32();
                DB2Reader.MaxIndex = reader.ReadInt32();
                int locale = reader.ReadInt32();
                int copyTableSize = reader.ReadInt32();
                DB2Reader.Flags = (DB2Flags)reader.ReadUInt16();
                DB2Reader.IdFieldIndex = reader.ReadUInt16();

                int totalFieldsCount = reader.ReadInt32();
                int packedDataOffset = reader.ReadInt32();      // Offset within the field where packed data starts
                int lookupColumnCount = reader.ReadInt32();     // count of lookup columns
                int sparseTableOffset = reader.ReadInt32();     // absolute value, {uint offset, ushort size}[MaxId - MinId + 1]
                int indexDataSize = reader.ReadInt32();         // int indexData[IndexDataSize / 4]
                int columnMetaDataSize = reader.ReadInt32();    // 24 * NumFields bytes, describes column bit packing, {ushort recordOffset, ushort size, uint additionalDataSize, uint compressionType, uint packedDataOffset or commonvalue, uint cellSize, uint cardinality}[NumFields], sizeof(DBC2CommonValue) == 8
                int commonDataSize = reader.ReadInt32();
                int palletDataSize = reader.ReadInt32();        // in bytes, sizeof(DBC2PalletValue) == 4
                int referenceDataSize = reader.ReadInt32();     // uint NumRecords, uint minId, uint maxId, {uint id, uint index}[NumRecords], questionable usefulness...

                // Field meta data
                DB2Reader.m_meta = reader.ReadArray<FieldMetaData>(DB2Reader.FieldsCount);

                if (!DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap))
                {
                    // records data
                    DB2Reader.recordsData = reader.ReadBytes(DB2Reader.RecordsCount * DB2Reader.RecordSize);

                    Array.Resize(ref DB2Reader.recordsData, DB2Reader.recordsData.Length + 8); // pad with extra zeros so we don't crash when reading

                    // string data
                    DB2Reader.m_stringsTable = new Dictionary<long, string>();

                    for (int i = 0; i < DB2Reader.StringTableSize;)
                    {
                        long oldPos = reader.BaseStream.Position;

                        DB2Reader.m_stringsTable[i] = reader.ReadCString();

                        i += (int)(reader.BaseStream.Position - oldPos);
                    }
                }
                else
                {
                    // sparse data with inlined strings
                    DB2Reader.recordsData = reader.ReadBytes(sparseTableOffset - HeaderSize - Marshal.SizeOf<FieldMetaData>() * DB2Reader.FieldsCount);

                    if (reader.BaseStream.Position != sparseTableOffset)
                        throw new Exception("r.BaseStream.Position != sparseTableOffset");

                    Dictionary<uint, int> offSetKeyMap = new Dictionary<uint, int>();
                    List<offset_map_entry> tempSparseEntries = new List<offset_map_entry>();
                    for (int i = 0; i < (DB2Reader.MaxIndex - DB2Reader.MinIndex + 1); i++)
                    {
                        offset_map_entry sparse = reader.Read<offset_map_entry>();

                        if (sparse.offset == 0 || sparse.size == 0)
                            continue;

                        // special case, may contain duplicates in the offset map that we don't want
                        if (copyTableSize == 0)
                        {
                            if (offSetKeyMap.ContainsKey(sparse.offset))
                                continue;
                        }

                        tempSparseEntries.Add(sparse);
                        offSetKeyMap.Add(sparse.offset, 0);
                    }

                    DB2Reader.sparseEntries = tempSparseEntries.ToArray();
                }

                // index data
                DB2Reader.m_indexData = reader.ReadArray<int>(indexDataSize / 4);

                // duplicate rows data
                Dictionary<int, int> copyData = new Dictionary<int, int>();

                for (int i = 0; i < copyTableSize / 8; i++)
                    copyData[reader.ReadInt32()] = reader.ReadInt32();

                // column meta data
                DB2Reader.m_columnMeta = reader.ReadArray<ColumnMetaData>(DB2Reader.FieldsCount);

                // pallet data
                DB2Reader.m_palletData = new Value32[DB2Reader.m_columnMeta.Length][];

                for (int i = 0; i < DB2Reader.m_columnMeta.Length; i++)
                {
                    if (DB2Reader.m_columnMeta[i].CompressionType == CompressionType.Pallet || DB2Reader.m_columnMeta[i].CompressionType == CompressionType.PalletArray)
                    {
                        DB2Reader.m_palletData[i] = reader.ReadArray<Value32>((int)DB2Reader.m_columnMeta[i].AdditionalDataSize / 4);
                    }
                }

                // common data
                DB2Reader.m_commonData = new Dictionary<int, Value32>[DB2Reader.m_columnMeta.Length];

                for (int i = 0; i < DB2Reader.m_columnMeta.Length; i++)
                {
                    if (DB2Reader.m_columnMeta[i].CompressionType == CompressionType.Common)
                    {
                        Dictionary<int, Value32> commonValues = new Dictionary<int, Value32>();
                        DB2Reader.m_commonData[i] = commonValues;

                        for (int j = 0; j < DB2Reader.m_columnMeta[i].AdditionalDataSize / 8; j++)
                            commonValues[reader.ReadInt32()] = reader.Read<Value32>();
                    }
                }

                // reference data
                ReferenceData refData = null;

                if (referenceDataSize > 0)
                {
                    refData = new ReferenceData
                    {
                        NumRecords = reader.ReadInt32(),
                        MinId = reader.ReadInt32(),
                        MaxId = reader.ReadInt32()
                    };

                    refData.Entries = reader.ReadArray<ReferenceEntry>(refData.NumRecords);
                }

                int position = 0;

                for (int i = 0; i < DB2Reader.RecordsCount; ++i)
                {
                    BitReader bitReader = new BitReader(DB2Reader.recordsData) { Position = 0 };

                    if (DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap))
                    {
                        bitReader.Position = position;
                        position += DB2Reader.sparseEntries[i].size * 8;
                    }
                    else
                        bitReader.Offset = i * DB2Reader.RecordSize;
                }

                foreach (var copyRow in copyData)
                {
                    IDB2Row rec = DB2Reader._Records[copyRow.Value].Clone();
                    rec.Data = new BitReader(DB2Reader.recordsData);

                    rec.Data.Position = DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap) ? DB2Reader._Records[copyRow.Value].Data.Position : 0;
                    rec.Data.Offset = DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap) ? 0 : DB2Reader._Records[copyRow.Value].Data.Offset;

                    rec.Id = copyRow.Key;
                    DB2Reader._Records.Add(copyRow.Key, rec);
                }
            }
        }
    }
}
