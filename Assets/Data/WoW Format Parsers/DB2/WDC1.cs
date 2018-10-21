using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static partial class WDC1
{
    private const int headerSize = 84;

    private static DB2.field_structure[] fields;
    private static int[] m_indexData;
    private static DB2.ColumnMetaData[] m_columnMeta;
    private static DB2.Value32[][] m_palletData;
    private static Dictionary<int, DB2.Value32>[] m_commonData;

    // Normal Records Data //
    private static byte[] recordsData;
    private static Dictionary<long, string> m_stringsTable;

    // Sparse Records Data //
    public static DB2.offset_map_entry[] sparseEntries;

    public static void Read(string fileName, byte[] fileData)
    {
        using (MemoryStream ms = new MemoryStream(fileData))
        {
            // Header //
            StreamTools s = new StreamTools();
            DB2.wdc1_db2_header header = ReadHeader(ms);

            // Field Meta Data //
            fields = new DB2.field_structure[header.total_field_count];
            for (int f = 0; f < header.total_field_count; f++)
            {
                DB2.field_structure field = new DB2.field_structure();
                field.size = s.ReadShort(ms);
                field.offset = s.ReadShort(ms);
                fields[f] = field;
            }

            if (!header.flags.HasFlag(DB2.DB2Flags.Sparse))
            {
                /// Normal records ///

                // Read Records Data //
                recordsData = new byte[header.record_count * header.record_size];
                ms.Read(recordsData, 0, header.record_count * header.record_size);
                Array.Resize(ref recordsData, recordsData.Length + 8);

                // Read String Data //
                //Debug.Log("header.string_table_size " + header.string_table_size);
                m_stringsTable = new Dictionary<long, string>();
                for (int i = 0; i < header.string_table_size;)
                {
                    long oldPos = ms.Position;

                    m_stringsTable[i] = s.ReadCString(ms, Encoding.UTF8);
                    //Debug.Log(m_stringsTable[i]);

                    i += (int)(ms.Position - oldPos);
                }
            }
            else
            {
                /// Offset map records /// -- these records have null-terminated strings inlined, and
                // since they are variable-length, they are pointed to by an array of 6-byte
                // offset+size pairs.

                ms.Read(recordsData, 0, header.offset_map_offset - headerSize - Marshal.SizeOf<DB2.field_structure>() * header.total_field_count);

                if (ms.Position != header.offset_map_offset)
                    Debug.LogError("Error: r.BaseStream.Position != sparseTableOffset");

                Dictionary<uint, int> offSetKeyMap = new Dictionary<uint, int>();
                List<DB2.offset_map_entry> tempSparseEntries = new List<DB2.offset_map_entry>();

                for (int i = 0; i < (header.max_id - header.min_id + 1); i++)
                {
                    DB2.offset_map_entry sparse = s.Read<DB2.offset_map_entry>(ms);

                    if (sparse.offset == 0 || sparse.size == 0)
                        continue;

                    // special case, may contain duplicates in the offset map that we don't want
                    if (header.copy_table_size == 0)
                    {
                        if (offSetKeyMap.ContainsKey(sparse.offset))
                            continue;
                    }

                    tempSparseEntries.Add(sparse);
                    offSetKeyMap.Add(sparse.offset, 0);
                }
                sparseEntries = tempSparseEntries.ToArray();
            }

            // Index Data //
            m_indexData = new int[header.id_list_size / 4];
            for (int iD = 0; iD < header.id_list_size / 4; iD++)
                m_indexData[iD] = s.ReadLong(ms);

            // Duplicate Rows Data //
            Dictionary<int, int> copyData = new Dictionary<int, int>();
            for (int i = 0; i < header.copy_table_size / 8; i++)
            {
                copyData[s.ReadLong(ms)] = s.ReadLong(ms);
            }

            for (int t = 0; t < header.record_count; t++)
            {
                if (copyData.ContainsKey(t))
                {
                    Debug.Log("copy " + t + " to " + copyData[t]);
                }
            }

            // Column Meta Data //
            m_columnMeta = new DB2.ColumnMetaData[header.total_field_count];
            //Debug.Log("header.total_field_count " + header.total_field_count);
            for (int cmd = 0; cmd < header.total_field_count; cmd++)
            {
                DB2.ColumnMetaData columnData = new DB2.ColumnMetaData();
                columnData.RecordOffset = s.ReadUint16(ms);
                columnData.Size = s.ReadUint16(ms);
                columnData.AdditionalDataSize = s.ReadUint32(ms);
                columnData.CompressionType = (DB2.CompressionType)s.ReadLong(ms);
                switch (columnData.CompressionType)
                {
                    case DB2.CompressionType.Immediate:
                        {
                            DB2.ColumnCompressionData_Immediate Immediate = new DB2.ColumnCompressionData_Immediate();
                            Immediate.BitOffset = s.ReadUint32(ms);
                            Immediate.BitWidth = s.ReadUint32(ms);
                            Immediate.Flags = s.ReadUint32(ms);
                            columnData.Immediate = Immediate;
                            break;
                        }
                    
                    case DB2.CompressionType.Pallet:
                        {
                            DB2.ColumnCompressionData_Pallet Pallet = new DB2.ColumnCompressionData_Pallet();
                            Pallet.BitOffset = s.ReadUint32(ms);
                            Pallet.BitWidth = s.ReadUint32(ms);
                            Pallet.Cardinality = s.ReadUint32(ms);
                            columnData.Pallet = Pallet;
                            break;
                        }

                    case DB2.CompressionType.Common:
                        {
                            DB2.ColumnCompressionData_Common Common = new DB2.ColumnCompressionData_Common();
                            Common.DefaultValue = s.ReadValue32(ms, 32);
                            Common.B = s.ReadUint32(ms);
                            Common.C = s.ReadUint32(ms);
                            columnData.Common = Common;
                            break;
                        }
                    default:
                        {
                            s.ReadUint32(ms);
                            s.ReadUint32(ms);
                            s.ReadUint32(ms);
                            break;
                        }
                }
                m_columnMeta[cmd] = columnData;
            }

            // Pallet Data //
            m_palletData = new DB2.Value32[m_columnMeta.Length][];
            for (int i = 0; i < m_columnMeta.Length; i++)
            {
                if (m_columnMeta[i].CompressionType == DB2.CompressionType.Pallet || m_columnMeta[i].CompressionType == DB2.CompressionType.PalletArray)
                {
                    m_palletData[i] = s.ReadArray<DB2.Value32>(ms, (int)m_columnMeta[i].AdditionalDataSize / 4);
                }
            }

            // Common Data //
            m_commonData = new Dictionary<int, DB2.Value32>[m_columnMeta.Length];
            for (int i = 0; i < m_columnMeta.Length; i++)
            {
                //Debug.Log(m_columnMeta[i].CompressionType);
                if (m_columnMeta[i].CompressionType == DB2.CompressionType.Common)
                {
                    Dictionary<int, DB2.Value32> commonValues = new Dictionary<int, DB2.Value32>();
                    m_commonData[i] = commonValues;

                    for (int j = 0; j < m_columnMeta[i].AdditionalDataSize / 8; j++)
                        commonValues[s.ReadLong(ms)] = s.Read<DB2.Value32>(ms);
                }
            }

            // Reference Data //
            DB2.ReferenceData refData = null;

            if (header.relationship_data_size > 0)
            {
                refData = new DB2.ReferenceData
                {
                    NumRecords = s.ReadLong(ms),
                    MinId = s.ReadLong(ms),
                    MaxId = s.ReadLong(ms)
                };

                refData.Entries = s.ReadArray<DB2.ReferenceEntry>(ms, refData.NumRecords);
            }

            //DB2.BitReader bitReader = new DB2.BitReader(recordsData);

            //int position = 0;
            using (MemoryStream rs = new MemoryStream(recordsData))
            {
                for (int i = 0; i < header.record_count; ++i)
                {
                    int[] values = new int[m_columnMeta.Length];
                    values[0] = s.ReadUint32(rs);
                    values[1] = s.ReadUint16(rs);
                    values[2] = s.ReadUint16(rs);
                    values[3] = s.ReadUint32(rs);

                    //Debug.Log(i + " " + values[0] + " " + values[1] + " " + values[2] + " " + values[3]);
                }
            }


                for (int i = 0; i < header.record_count; ++i)
                {
                    int[] values = new int[m_columnMeta.Length];
                    //values[0] = s.ReadUint32(ms);
                    //values[1] = s.ReadUint16(ms);
                    //values[2] = s.ReadUint16(ms);
                    //values[3] = ms.ReadByte();

                    //values[0] = new recordsData[position]
    
                //Debug.Log(recordsData[])

                    //Debug.Log(i + " " + values[0] + " " + values[1] + " " + values[2] + " " + values[3]);

                    for (int j = 0; j < m_columnMeta.Length; j++)
                    {

                        /*
                        if (m_columnMeta[j].CompressionType == DB2.CompressionType.Common)
                            values[j] = m_commonData[j][i].GetValue<int>();
                        if (m_columnMeta[j].CompressionType == DB2.CompressionType.Pallet)
                            values[j] = m_palletData[j][i].GetValue<int>();
                        */
                    }

                    /*
                    bitReader.Position = 0;
                    bitReader.Offset = i * header.record_size;

                    DB2.IDB2Row rec = new WDC1Row(this, bitReader, indexDataSize != 0 ? m_indexData[i] : -1, refData?.Entries[i]);
                    rec.RecordIndex = i;

                    if (indexDataSize != 0)
                        _Records.Add(m_indexData[i], rec);
                    else
                        _Records.Add(rec.Id, rec);
                    */
                }
        }
    }

    private static DB2.wdc1_db2_header ReadHeader(MemoryStream ms)
    {
        StreamTools s = new StreamTools();

        DB2.wdc1_db2_header header = new DB2.wdc1_db2_header();
        header.magic = s.ReadUint32(ms);                    // 'WDC1'
        header.record_count = s.ReadUint32(ms);
        header.field_count = s.ReadUint32(ms);
        header.record_size = s.ReadUint32(ms);
        header.string_table_size = s.ReadUint32(ms);
        header.table_hash = s.ReadUint32(ms);               // hash of the table name
        header.layout_hash = s.ReadUint32(ms);              // this is a hash field that changes only when the structure of the data changes
        header.min_id = s.ReadUint32(ms);
        header.max_id = s.ReadUint32(ms);
        header.locale = s.ReadUint32(ms);                   // as seen in TextWowEnum
        header.copy_table_size = s.ReadUint32(ms);
        header.flags = (DB2.DB2Flags)s.ReadUint16(ms);      // possible values are listed in Known Flag Meanings
        header.id_index = s.ReadUint16(ms);                 // this is the index of the field containing ID values; this is ignored if flags & 0x04 != 0
        header.total_field_count = s.ReadUint32(ms);        // from WDC1 onwards, this value seems to always be the same as the 'field_count' value
        header.bitpacked_data_offset = s.ReadUint32(ms);    // relative position in record where bitpacked data begins; not important for parsing the file
        header.lookup_column_count = s.ReadUint32(ms);
        header.offset_map_offset = s.ReadUint32(ms);        // Offset to array of struct {header.offset; uint16_t size;}[max_id - min_id + 1];
        header.id_list_size = s.ReadUint32(ms);             // List of ids present in the DB file
        header.field_storage_info_size = s.ReadUint32(ms);  // 24 * NumFields bytes, describes column bit packing, {ushort recordOffset, ushort size, uint additionalDataSize, uint compressionType, uint packedDataOffset or commonvalue, uint cellSize, uint cardinality}[NumFields], sizeof(DBC2CommonValue) == 8
        header.common_data_size = s.ReadUint32(ms);
        header.pallet_data_size = s.ReadUint32(ms);         // in bytes, sizeof(DBC2PalletValue) == 4
        header.relationship_data_size = s.ReadUint32(ms);   // referenceDataSize - uint NumRecords, uint minId, uint maxId, {uint id, uint index}[NumRecords]

        return header;
    }

}
