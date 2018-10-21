using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using UnityEngine;

public static partial class DB2
{
    private const int HeaderSize = 72;
    private const uint WDC2FmtSig = 0x32434457;     // WDC2

    public static void Read(string fileName, Stream stream)
    {
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            if (reader.BaseStream.Length < HeaderSize)
                Debug.Log("WDC2 File is corrupted or empty!");

            uint magic = reader.ReadUInt32();

            if (magic != WDC2FmtSig)
                Debug.Log("WDC2 File is corrupted");

            DB2Reader.RecordsCount = reader.ReadInt32();
            DB2Reader.FieldsCount = reader.ReadInt32();
            DB2Reader.RecordSize = reader.ReadInt32();
            DB2Reader.StringTableSize = reader.ReadInt32();
            DB2Reader.TableHash = reader.ReadUInt32();
            DB2Reader.LayoutHash = reader.ReadUInt32();
            DB2Reader.MinIndex = reader.ReadInt32();
            DB2Reader.MaxIndex = reader.ReadInt32();
            int Locale = reader.ReadInt32();
            DB2Reader.Flags = (DB2Flags)reader.ReadUInt16();
            DB2Reader.IdFieldIndex = reader.ReadUInt16();

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
            DB2Reader.m_meta = reader.ReadArray<FieldMetaData>(DB2Reader.FieldsCount);

            // Column meta data
            m_columnMeta = reader.ReadArray<ColumnMetaData>(DB2Reader.FieldsCount);

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

                if (!DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap))
                {
                    // Records Data
                    recordsData = reader.ReadBytes(sections[sectionIndex].NumRecords * DB2Reader.RecordSize);
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
                    for (int i = 0; i < (DB2Reader.MaxIndex - DB2Reader.MinIndex + 1); i++)
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

                for (int i = 0; i < DB2Reader.RecordsCount; i++)
                {
                    BitReader bitReader = new BitReader(recordsData) { Position = 0 };

                    if (DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap))
                    {
                        bitReader.Position = pos;
                        pos += sparseEntries[i].size * 8;
                    }
                    else
                        bitReader.Offset = i * DB2Reader.RecordSize;

                    ///< Todo : Find a fix for this.
                    //IDB2Row rec = new WDC2Row(this, bitReader, sections[sectionIndex].FileOffset, indexDataNotEmpty ? m_indexData[i] : -1, refData?.Entries[i], i);

                    //if (indexDataNotEmpty)
                    //    DB2Reader._Records.Add((int)m_indexData[i], rec);
                    //else
                    //    DB2Reader._Records.Add(rec.Id, rec);
                }

                foreach (var copyRow in copyData)
                {
                    IDB2Row rec = DB2Reader._Records[copyRow.Value].Clone();
                    rec.Data = new BitReader(recordsData);

                    rec.Data.Position = DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap) ? DB2Reader._Records[copyRow.Value].Data.Position : 0;
                    rec.Data.Offset = DB2Reader.Flags.HasFlagExt(DB2Flags.OffsetMap) ? 0 : DB2Reader._Records[copyRow.Value].Data.Offset;

                    rec.Id = copyRow.Key;
                    DB2Reader._Records.Add(copyRow.Key, rec);
                }
            }
        }
    }
}
