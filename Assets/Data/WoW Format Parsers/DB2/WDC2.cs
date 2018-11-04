using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using UnityEngine;

public partial class DB2
{
    public class WDC2Row : IDB2Row
    {
        private BitReader m_data;
        private DB2Reader m_reader;
        private int m_dataOffset;
        private int m_recordsOffset;
        private int m_recordIndex;

        public int Id { get; set; }
        public BitReader Data { get => m_data; set => m_data = value; }

        private FieldMetaData[] m_fieldMeta;
        private ColumnMetaData[] m_columnMeta;
        private Value32[][] m_palletData;
        private Dictionary<int, Value32>[] m_commonData;
        private ReferenceEntry? m_refData;

        public WDC2Row(DB2Reader reader, BitReader data, int recordsOffset, int id, ReferenceEntry? refData, int recordIndex)
        {
            m_reader = reader;
            m_data = data;
            m_recordsOffset = recordsOffset;
            m_recordIndex = recordIndex;

            m_dataOffset = m_data.Offset;

            m_fieldMeta = reader.Meta;
            m_columnMeta = reader.ColumnMeta;
            m_palletData = reader.PalletData;
            m_commonData = reader.CommonData;
            m_refData = refData;

            if (id != -1)
                Id = id;
            else
            {
                int idFieldIndex = reader.IdFieldIndex;

                m_data.Position = m_columnMeta[idFieldIndex].RecordOffset;

                Id = GetFieldValue<int>(0, m_data, m_fieldMeta[idFieldIndex], m_columnMeta[idFieldIndex], m_palletData[idFieldIndex], m_commonData[idFieldIndex]);
            }
        }

        private static Dictionary<Type, Func<int, BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, DB2Reader, object>> simpleReaders = new Dictionary<Type, Func<int, BitReader, int, FieldMetaData, ColumnMetaData, Value32[], Dictionary<int, Value32>, Dictionary<long, string>, DB2Reader, object>>
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
            [typeof(string)] = (id, data, recordsOffset, fieldMeta, columnMeta, palletData, commonData, stringTable, header) => header.Flags.HasFlagExt(DB2Flags.OffsetMap) ? data.ReadCString() :
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

            for (int i = 0; i < fields.Length; ++i)
            {
                FieldCache<T> info = fields[i];
                if (info.IndexMapField)
                {
                    indexFieldOffSet++;
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

                if (!m_reader.Flags.HasFlagExt(DB2Flags.OffsetMap))
                {
                    m_data.Position = m_columnMeta[fieldIndex].RecordOffset;
                    m_data.Offset = m_dataOffset;
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

        private static T GetFieldValue<T>(int Id, BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, Value32[] palletData, Dictionary<int, Value32> commonData) where T : struct
        {
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    int bitSize = 32 - fieldMeta.Bits;
                    if (bitSize > 0)
                        return r.ReadValue64(bitSize).GetValue<T>();
                    else
                        return r.ReadValue64(columnMeta.Immediate.BitWidth).GetValue<T>();
                case CompressionType.SignedImmediate:
                    return r.ReadValue64Signed(columnMeta.Immediate.BitWidth).GetValue<T>();
                case CompressionType.Immediate:
                    return r.ReadValue64(columnMeta.Immediate.BitWidth).GetValue<T>();
                case CompressionType.Common:
                    if (commonData.TryGetValue(Id, out Value32 val))
                        return val.GetValue<T>();
                    else
                        return columnMeta.Common.DefaultValue.GetValue<T>();
                case CompressionType.Pallet:
                    uint palletIndex = r.ReadUInt32(columnMeta.Pallet.BitWidth);

                    T val1 = palletData[palletIndex].GetValue<T>();

                    return val1;
            }
            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        private static T[] GetFieldValueArray<T>(BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, Value32[] palletData, Dictionary<int, Value32> commonData) where T : struct
        {
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    int bitSize = 32 - fieldMeta.Bits;

                    T[] arr1 = new T[columnMeta.Size / (FastStruct<T>.Size * 8)];

                    for (int i = 0; i < arr1.Length; i++)
                    {
                        if (bitSize > 0)
                            arr1[i] = r.ReadValue64(bitSize).GetValue<T>();
                        else
                            arr1[i] = r.ReadValue64(columnMeta.Immediate.BitWidth).GetValue<T>();
                    }

                    return arr1;
                case CompressionType.PalletArray:
                    int cardinality = columnMeta.Pallet.Cardinality;

                    uint palletArrayIndex = r.ReadUInt32(columnMeta.Pallet.BitWidth);

                    T[] arr3 = new T[cardinality];

                    for (int i = 0; i < arr3.Length; i++)
                        arr3[i] = palletData[i + cardinality * (int)palletArrayIndex].GetValue<T>();

                    return arr3;
            }
            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        private static string[] GetFieldValueStringArray(BitReader r, FieldMetaData fieldMeta, ColumnMetaData columnMeta, int recordsOffset, Dictionary<long, string> stringTable)
        {
            switch (columnMeta.CompressionType)
            {
                case CompressionType.None:
                    int bitSize = 32 - fieldMeta.Bits;

                    string[] arr1 = new string[columnMeta.Size / (FastStruct<int>.Size * 8)];

                    for (int i = 0; i < arr1.Length; i++)
                    {
                        int offSet = recordsOffset + r.Offset + (r.Position >> 3);

                        if (bitSize > 0)
                            arr1[i] = stringTable[offSet + r.ReadValue64(bitSize).GetValue<int>()];
                        else
                            arr1[i] = stringTable[offSet + r.ReadValue64(columnMeta.Immediate.BitWidth).GetValue<int>()];
                    }

                    return arr1;
            }
            throw new Exception(string.Format("Unexpected compression type {0}", columnMeta.CompressionType));
        }

        public IDB2Row Clone()
        {
            return (IDB2Row)MemberwiseClone();
        }
    }
    public class WDC2 : DB2Reader
    {
        private const int HeaderSize = 72;
        private const uint WDC2FmtSig = 0x32434457;     // WDC2

        public WDC2(string fileName, Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                if (reader.BaseStream.Length < HeaderSize)
                    Debug.Log("WDC2 File is corrupted or empty!");

                uint magic = reader.ReadUInt32();

                if (magic != WDC2FmtSig)
                    Debug.Log("WDC2 File is corrupted");

                RecordsCount = reader.ReadInt32();
                FieldsCount = reader.ReadInt32();
                RecordSize = reader.ReadInt32();
                StringTableSize = reader.ReadInt32();
                TableHash = reader.ReadUInt32();
                LayoutHash = reader.ReadUInt32();
                MinIndex = reader.ReadInt32();
                MaxIndex = reader.ReadInt32();
                int Locale = reader.ReadInt32();
                Flags = (DB2Flags)reader.ReadUInt16();
                IdFieldIndex = reader.ReadUInt16();

                int totalFieldsCount = reader.ReadInt32();
                int packedDataOffset = reader.ReadInt32();              // Offset within the field where packed data starts
                int lookupColumnCount = reader.ReadInt32();             // Count of lookup columns
                int columnMetaDataSize = reader.ReadInt32();            // 24 * NumFields bytes, describes column bit packet.
                int commonDataSize = reader.ReadInt32();
                int palletDataSize = reader.ReadInt32();                // In bytes, sizeof(DBC2PalletValue) == 4
                int sectionsCount = reader.ReadInt32();

                if (sectionsCount > 1)
                    Debug.Log("sectionsCount > 1");
                else if (sectionsCount == 0)
                    return;

                SectionHeader[] sections = reader.ReadArray<SectionHeader>(sectionsCount);

                // Field meta data
                m_meta = reader.ReadArray<FieldMetaData>(FieldsCount);

                // Column meta data
                m_columnMeta = reader.ReadArray<ColumnMetaData>(FieldsCount);

                // Pallet data
                m_palletData = new Value32[m_columnMeta.Length][];
                for (int i = 0; i < m_columnMeta.Length; i++)
                {
                    if (m_columnMeta[i].CompressionType == CompressionType.Pallet || m_columnMeta[i].CompressionType == CompressionType.PalletArray)
                        m_palletData[i] = reader.ReadArray<Value32>((int)m_columnMeta[i].AdditionalDataSize / 4);
                }

                // Common Data
                m_commonData = new Dictionary<int, Value32>[m_columnMeta.Length];
                for (int i = 0; i < m_columnMeta.Length; i++)
                {
                    if (m_columnMeta[i].CompressionType == CompressionType.Common)
                    {
                        Dictionary<int, Value32> commonValues = new Dictionary<int, Value32>();
                        m_commonData[i] = commonValues;

                        for (int j = 0; j < m_columnMeta[i].AdditionalDataSize / 8; j++)
                            commonValues[reader.ReadInt32()] = reader.Read<Value32>();
                    }
                }

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    reader.BaseStream.Position = sections[sectionIndex].FileOffset;

                    if (!Flags.HasFlagExt(DB2Flags.OffsetMap))
                    {
                        // Records Data
                        recordsData = reader.ReadBytes(sections[sectionIndex].NumRecords * RecordSize);
                        Array.Resize(ref recordsData, recordsData.Length + 8);  // Pad with extra zeros so we don't crash when reading

                        // string data
                        m_stringsTable = new Dictionary<long, string>();
                        for (int i = 0; i < sections[sectionIndex].StringTableSize;)
                        {
                            long oldPos = reader.BaseStream.Position;
                            m_stringsTable[oldPos] = reader.ReadCString();
                            i += (int)(reader.BaseStream.Position - oldPos);
                        }
                    }
                    else
                    {
                        // Sparse data with inlined strings
                        recordsData = reader.ReadBytes(sections[sectionIndex].SparseTableOffset - sections[sectionIndex].FileOffset);

                        if (reader.BaseStream.Position != sections[sectionIndex].SparseTableOffset)
                            Debug.Log("reader.BaseStream.Position != sections[sectionIndex].SparseTableOffset");

                        Dictionary<uint, int> offSetKeyMap = new Dictionary<uint, int>();
                        List<offset_map_entry> tempSparseEntries = new List<offset_map_entry>();
                        for (int i = 0; i < (MaxIndex - MinIndex + 1); i++)
                        {
                            offset_map_entry sparse = reader.Read<offset_map_entry>();
                            if (sparse.offset == 0 || sparse.size == 0)
                                continue;

                            // Special case, may contain duplicates in the offset map we don't want
                            if (sections[sectionIndex].CopyTableSize == 0)
                            {
                                if (offSetKeyMap.ContainsKey(sparse.offset))
                                    continue;
                            }

                            tempSparseEntries.Add(sparse);
                            offSetKeyMap.Add(sparse.offset, 0);
                        }
                        sparseEntries = tempSparseEntries.ToArray();
                    }

                    // Index data
                    m_indexData = reader.ReadArray<int>(sections[sectionIndex].IndexDataSize / 4);

                    // duplicate rows data
                    Dictionary<int, int> copyData = new Dictionary<int, int>();
                    for (int i = 0; i < sections[sectionIndex].CopyTableSize / 8; i++)
                        copyData[reader.ReadInt32()] = reader.ReadInt32();

                    // Ref data
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

                    int pos = 0;
                    bool indexDataNotEmpty = sections[sectionIndex].IndexDataSize != 0 && m_indexData.GroupBy(i => i).Where(d => d.Count() > 1).Count() == 0;

                    for (int i = 0; i < RecordsCount; i++)
                    {
                        BitReader bitReader = new BitReader(recordsData) { Position = 0 };

                        if (Flags.HasFlagExt(DB2Flags.OffsetMap))
                        {
                            bitReader.Position = pos;
                            pos += sparseEntries[i].size * 8;
                        }
                        else
                            bitReader.Offset = i * RecordSize;

                        IDB2Row rec = new WDC2Row(this, bitReader, sections[sectionIndex].FileOffset, indexDataNotEmpty ? m_indexData[i] : -1, refData?.Entries[i], i);

                        if (indexDataNotEmpty)
                            _Records.Add((int)m_indexData[i], rec);
                        else
                            _Records.Add(rec.Id, rec);
                    }

                    foreach (var copyRow in copyData)
                    {
                        IDB2Row rec = _Records[copyRow.Value].Clone();
                        rec.Data = new BitReader(recordsData);

                        rec.Data.Position = Flags.HasFlagExt(DB2Flags.OffsetMap) ? _Records[copyRow.Value].Data.Position : 0;
                        rec.Data.Offset = Flags.HasFlagExt(DB2Flags.OffsetMap) ? 0 : _Records[copyRow.Value].Data.Offset;

                        rec.Id = copyRow.Key;
                        _Records.Add(copyRow.Key, rec);
                    }
                }
            }
        }
    }
}