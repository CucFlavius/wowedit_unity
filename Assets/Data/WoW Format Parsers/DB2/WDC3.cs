using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public partial class DB2
{
    public class WDC3Row : IDBRow
    {
        private BitReader m_data;
        private BaseReader m_reader;
        private readonly int m_dataOffset;
        private readonly int m_dataPosition;
        private readonly int m_recordsOffset;
        private readonly int m_recordIndex;

        public int Id { get; set; }
        public BitReader Data { get => m_data; set => m_data = value; }

        private readonly FieldMetaData[] m_fieldMeta;
        private readonly ColumnMetaData[] m_columnMeta;
        private readonly Value32[][] m_palletData;
        private readonly Dictionary<int, Value32>[] m_commonData;
        private ReferenceEntry? m_refData;

        public WDC3Row(BaseReader reader, BitReader data, int recordsOffset, int id, ReferenceEntry? refData, int recordIndex)
        {
            m_reader = reader;
            m_data = data;
            m_recordsOffset = recordsOffset;
            m_recordIndex = recordIndex;

            m_dataOffset = m_data.Offset;
            m_dataPosition = m_data.Position;

            m_fieldMeta = reader.Meta;
            m_columnMeta = reader.ColumnMeta;
            m_palletData = reader.PalletData;
            m_commonData = reader.CommonData;
            m_refData = refData;

            Id = id;
        }

        private static Dictionary<Type, Func<int, BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, BaseReader, object>> simpleReaders = new Dictionary<Type, Func<int, BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, BaseReader, object>>
        {
            [typeof(ulong)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<ulong>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(long)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<long>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(float)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<float>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(int)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<int>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(uint)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<uint>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(short)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<short>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(ushort)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<ushort>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(sbyte)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<sbyte>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(byte)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => GetFieldValue<byte>(id, data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(string)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => header.Flags.HasFlagExt(DB2Flags.Sparse) ? data.ReadCString() :
                stringTable[(recordsOffset + data.Offset + (data.Position >> 3)) + GetFieldValue<int>(id, data, fieldMeta, columnMeta, palletData, commonData)],
        };

        private static Dictionary<Type, Func<BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, object>> arrayReaders = new Dictionary<Type, Func<BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, object>>
        {
            [typeof(ulong[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<ulong>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(long[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<long>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(float[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<float>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(int[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<int>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(uint[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<uint>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(ulong[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<ulong>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(short[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<short>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(ushort[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<ushort>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(byte[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<byte>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(sbyte[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueArray<sbyte>(data, fieldMeta, columnMeta, palletData, commonData),
            [typeof(string[])] = (data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable) => GetFieldValueStringArray(data, fieldMeta, columnMeta, recordsOffset, stringTable),
        };

        public void GetFields<T>(FieldCache<T>[] fields, T entry)
        {
            int indexFieldOffSet = 0;

            m_data.Position = m_dataPosition;
            m_data.Offset = m_dataOffset;

            for (int i = 0; i < fields.Length; i++)
            {
                FieldCache<T> info = fields[i];
                if (i == m_reader.IdFieldIndex)
                {
                    if (Id != -1)
                        indexFieldOffSet++;
                    else
                        Id = GetFieldValue<int>(0, m_data, m_fieldMeta[i], m_columnMeta[i], m_palletData[i], m_commonData[i]);

                    info.Setter(entry, Convert.ChangeType(Id, info.Field.FieldType));
                    continue;
                }

                object value = null;
                int fieldIndex = i - indexFieldOffSet;

                if (fieldIndex >= m_reader.Meta.Length)
                {
                    value = m_refData?.Id ?? 0;
                    info.Setter(entry, Convert.ChangeType(value, info.Field.FieldType));
                    continue;
                }

                if (info.IsArray)
                {
                    if (arrayReaders.TryGetValue(info.Field.FieldType, out var reader))
                        value = reader(m_data, m_recordsOffset, m_fieldMeta[fieldIndex], m_columnMeta[fieldIndex], m_palletData[fieldIndex], m_commonData[fieldIndex], m_reader.StringTable);
                    else
                        throw new Exception("Unhandled array type: " + typeof(T).Name);
                }
                else
                {
                    if (simpleReaders.TryGetValue(info.Field.FieldType, out var reader))
                        value = reader(Id, m_data, m_recordsOffset, m_fieldMeta[fieldIndex], m_columnMeta[fieldIndex], m_palletData[fieldIndex], m_commonData[fieldIndex], m_reader.StringTable, m_reader);
                    else
                        throw new Exception("Unhandled field type: " + typeof(T).Name);
                }

                info.Setter(entry, value);
            }
        }

        public static T GetFieldValue<T>(int Id, BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, Value32[] palletData, Dictionary<int, Value32> commonData) where T : struct
        {
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    {
                        int bitSize = 32 - fieldMeta.Bits;
                        if (bitSize <= 0)
                            bitSize = columnMeta.Immediate.BitWidth;

                        return r.ReadValue64(bitSize).GetValue<T>();
                    }
                case CompressionType.SignedImmediate:
                    {
                        return r.ReadValue64Signed(columnMeta.Immediate.BitWidth).GetValue<T>();
                    }
                case CompressionType.Immediate:
                    {
                        return r.ReadValue64(columnMeta.Immediate.BitWidth).GetValue<T>();
                    }
                case CompressionType.Common:
                    {
                        if (commonData.TryGetValue(Id, out Value32 val))
                            return val.GetValue<T>();
                        return columnMeta.Common.DefaultValue.GetValue<T>();
                    }
                case CompressionType.Pallet:
                    {
                        uint palletIndex = r.ReadUInt32(columnMeta.Pallet.BitWidth);
                        return palletData[palletIndex].GetValue<T>();
                    }
            }

            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        public static T[] GetFieldValueArray<T>(BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, Value32[] palletData, Dictionary<int, Value32> commonData) where T : struct
        {
            T[] array;
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    {
                        int bitSize = 32 - fieldMeta.Bits;
                        if (bitSize <= 0)
                            bitSize = columnMeta.Immediate.BitWidth;

                        array = new T[columnMeta.Size / (FastStruct<T>.Size * 8)];
                        for (int i = 0; i < array.Length; i++)
                            array[i] = r.ReadValue64(bitSize).GetValue<T>();

                        return array;
                    }
                case CompressionType.PalletArray:
                    {
                        int cardinality = columnMeta.Pallet.Cardinality;
                        uint palletArrayIndex = r.ReadUInt32(columnMeta.Pallet.BitWidth);

                        array = new T[cardinality];
                        for (int i = 0; i < array.Length; i++)
                            array[i] = palletData[i + cardinality * (int)palletArrayIndex].GetValue<T>();

                        return array;
                    }
            }

            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        public static string[] GetFieldValueStringArray(BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, int recordsOffset, Dictionary<long, string> stringTable)
        {
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    int bitSize = 32 - fieldMeta.Bits;
                    if (bitSize <= 0)
                        bitSize = columnMeta.Immediate.BitWidth;

                    string[] array = new string[columnMeta.Size / (FastStruct<int>.Size * 8)];
                    for (int i = 0; i < array.Length; i++)
                    {
                        int offSet = recordsOffset + r.Offset + (r.Position >> 3);
                        array[i] = stringTable[offSet + r.ReadValue64(bitSize).GetValue<int>()];
                    }

                    return array;
            }

            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        public IDBRow Clone()
        {
            return (IDBRow)MemberwiseClone();
        }
    }

    public class WDC3 : BaseReader
    {
        private const int HeaderSize = 72;
        private const uint WDC3FmtSig = 0x33434457;
        private Func<long, bool> hasTactKeyFunc;

        public Dictionary<int, WDC3Row> _records = new Dictionary<int, WDC3Row>();

        public WDC3(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                if (reader.BaseStream.Length < HeaderSize)
                    Debug.Log("WDC3 File is corrupted or empty!");

                uint magic = reader.ReadUInt32();

                if (magic != WDC3FmtSig)
                    Debug.Log("WDC3 File is corrupted");

                RecordsCount            = reader.ReadInt32();
                FieldsCount             = reader.ReadInt32();
                RecordSize              = reader.ReadInt32();
                StringTableSize         = reader.ReadInt32();
                TableHash               = reader.ReadUInt32();
                LayoutHash              = reader.ReadUInt32();
                MinIndex                = reader.ReadInt32();
                MaxIndex                = reader.ReadInt32();
                int locale              = reader.ReadInt32();
                Flags                   = (DB2Flags)reader.ReadUInt16();
                IdFieldIndex            = reader.ReadUInt16();
                int totalFieldsCount    = reader.ReadInt32();
                int packedDataOffset    = reader.ReadInt32(); // Offset within the field where packed data starts
                int lookupColumnCount   = reader.ReadInt32(); // count of lookup columns
                int columnMetaDataSize  = reader.ReadInt32(); // 24 * NumFields bytes, describes column bit packing, {ushort recordOffset, ushort size, uint additionalDataSize, uint compressionType, uint packedDataOffset or commonvalue, uint cellSize, uint cardinality}[NumFields], sizeof(DBC2CommonValue) == 8
                int commonDataSize      = reader.ReadInt32();
                int palletDataSize      = reader.ReadInt32(); // in bytes, sizeof(DBC2PalletValue) == 4
                int sectionsCount       = reader.ReadInt32();

                if (sectionsCount == 0 || RecordsCount == 0)
                    return;

                SectionHeaderWDC3[] sections = reader.ReadArray<SectionHeaderWDC3>(sectionsCount);

                // field meta data
                m_meta = reader.ReadArray<FieldMetaData>(FieldsCount);

                // column meta data
                m_columnMeta = reader.ReadArray<ColumnMetaData>(FieldsCount);

                // pallet data
                m_palletData = new Value32[m_columnMeta.Length][];
                for (int i = 0; i < m_columnMeta.Length; i++)
                {
                    if (m_columnMeta[i].CompressionType == CompressionType.Pallet || m_columnMeta[i].CompressionType == CompressionType.PalletArray)
                    {
                        m_palletData[i] = reader.ReadArray<Value32>((int)m_columnMeta[i].AdditionalDataSize / 4);
                    }
                }

                // common data
                m_commonData = new Dictionary<int, Value32>[m_columnMeta.Length];
                for (int i = 0; i < m_columnMeta.Length; i++)
                {
                    if (m_columnMeta[i].CompressionType == CompressionType.Common)
                    {
                        var commonValues = new Dictionary<int, Value32>((int)m_columnMeta[i].AdditionalDataSize / 8);
                        m_commonData[i] = commonValues;

                        for (int j = 0; j < m_columnMeta[i].AdditionalDataSize / 8; j++)
                            commonValues[reader.ReadInt32()] = reader.Read<Value32>();
                    }
                }

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    if (sections[sectionIndex].TactKeyLookup != 0)
                        continue;

                    reader.BaseStream.Position = sections[sectionIndex].FileOffset;

                    if (!Flags.HasFlagExt(DB2Flags.Sparse))
                    {
                        // records data
                        recordsData = reader.ReadBytes(sections[sectionIndex].NumRecords * RecordSize);

                        Array.Resize(ref recordsData, recordsData.Length + 8); // pad with extra zeros so we don't crash when reading

                        // string data
                        m_stringsTable = new Dictionary<long, string>(sections[sectionIndex].StringTableSize / 0x20);
                        long stringDataOffset = (RecordsCount - sections[sectionIndex].NumRecords) * RecordSize;
                        for (int i = 0; i < sections[sectionIndex].StringTableSize;)
                        {
                            long oldPos = reader.BaseStream.Position;
                            m_stringsTable[oldPos + stringDataOffset] = reader.ReadCString();
                            i += (int)(reader.BaseStream.Position - oldPos);
                        }
                    }
                    else
                    {
                        // sparse data with inlined strings
                        recordsData = reader.ReadBytes(sections[sectionIndex].OffsetRecordsEndOffset - sections[sectionIndex].FileOffset);

                        if (reader.BaseStream.Position != sections[sectionIndex].OffsetRecordsEndOffset)
                            throw new Exception("reader.BaseStream.Position != sections[sectionIndex].OffsetRecordsEndOffset");
                    }

                    // index data
                    m_indexData = reader.ReadArray<int>(sections[sectionIndex].IndexDataSize / 4);

                    // duplicate rows data
                    if (sections[sectionIndex].CopyTableCount > 0)
                    {
                        if (m_copyData == null)
                            m_copyData = new Dictionary<int, int>();

                        for (int i = 0; i < sections[sectionIndex].CopyTableCount; i++)
                            m_copyData[reader.ReadInt32()] = reader.ReadInt32();
                    }

                    if (sections[sectionIndex].OffsetMapIDCount > 0)
                    {
                        // HACK unittestsparse is malformed and has sparseIndexData first
                        if (TableHash == 145293629)
                            reader.BaseStream.Position += 4 * sections[sectionIndex].OffsetMapIDCount;

                        SparseEntries = reader.ReadArray<SparseEntry>(sections[sectionIndex].OffsetMapIDCount);
                    }

                    // reference data
                    ReferenceData refData = null;
                    if (sections[sectionIndex].ParentLookupDataSize > 0)
                    {
                        refData = new ReferenceData
                        {
                            NumRecords = reader.ReadInt32(),
                            MinId = reader.ReadInt32(),
                            MaxId = reader.ReadInt32()
                        };

                        refData.Entries = reader.ReadArray<ReferenceEntry>(refData.NumRecords);
                    }

                    if (sections[sectionIndex].OffsetMapIDCount > 0)
                    {
                        int[] sparseIndexData = reader.ReadArray<int>(sections[sectionIndex].OffsetMapIDCount);
                        if (sections[sectionIndex].IndexDataSize > 0 && m_indexData.Length != sparseIndexData.Length)
                            throw new Exception("m_indexData.Length != sparseIndexData.Length");
                        m_indexData = sparseIndexData;
                    }

                    bool indexDataNotEmpty = sections[sectionIndex].IndexDataSize != 0 && !m_indexData.GroupBy(i => i).Any(d => d.Count() > 1);

                    int position = 0;
                    for (int i = 0; i < sections[sectionIndex].NumRecords; i++)
                    {
                        BitReader bitReader = new BitReader(recordsData) { Position = 0 };

                        if (Flags.HasFlagExt(DB2Flags.Sparse))
                        {
                            bitReader.Position = position;
                            position += SparseEntries[i].Size * 8;
                        }
                        else
                            bitReader.Offset = i * RecordSize;

                        IDBRow rec = new WDC3Row(this, bitReader, sections[sectionIndex].FileOffset, indexDataNotEmpty ? m_indexData[i] : -1, refData?.Entries.ElementAtOrDefault(i), i);
                        _Records.Add(i, rec);
                    }
                }
            }
        }
    }
}
