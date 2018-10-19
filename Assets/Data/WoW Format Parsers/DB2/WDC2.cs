using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class WDC2
{
    private const int headerSize = 0;

    private static DB2.field_structure[] fields;
    private static int[] m_indexData;
    private static DB2.ColumnMetaData[] m_columnMeta;
    private static DB2.Value32[][] m_palletData;
    private static Dictionary<int, DB2.Value32>[] m_commonData;

    // Normal Records Data //
    private static byte[] recordsData;
    private static Dictionary<long, string> m_stringsTable;

    public static void Read(string fileName, byte[] fileData)
    {
        using (MemoryStream ms = new MemoryStream(fileData))
        {
            // Header //
            StreamTools s = new StreamTools();
            DB2.wdc2_db2_header header = ReadHeader(ms);
            DB2.wdc2_db2_section_header section_header = ReadSectionHeader(ms);

            // Field Meta Data //
            fields = new DB2.field_structure[header.totale_field_count];
            for (int f = 0; f < header.totale_field_count; f++)
            {
                DB2.field_structure field = new DB2.field_structure();
                field.size = s.ReadShort(ms);
                field.offset = s.ReadShort(ms);
                fields[f] = field;
            }

            if (!header.flags.HasFlag(DB2.DB2Flags.Sparse))
            {
                /// Normal Records ///

                // Read Records Data //
                recordsData = new byte[header.record_count * header.record_size];
                ms.Read(recordsData, 0, header.record_count * header.record_size);
                Array.Resize(ref recordsData, recordsData.Length + 8);

                // Read String Data //
                m_stringsTable = new Dictionary<long, string>();
                for (int i = 0; i < header.string_table_size;)
                {
                    long oldPos = ms.Position;

                    m_stringsTable[i] = s.ReadCString(ms, Encoding.UTF8);
                    Debug.Log(m_stringsTable[i]);

                    i += (int)(ms.Position - oldPos);
                }
            }
            else
            {
                
            }
        }
    }

    private static DB2.wdc2_db2_header ReadHeader(MemoryStream ms)
    {
        StreamTools s = new StreamTools();

        DB2.wdc2_db2_header header = new DB2.wdc2_db2_header();
        header.magic = s.ReadUint32(ms);
        header.record_count = s.ReadUint32(ms);
        header.field_count = s.ReadUint32(ms);
        header.record_size = s.ReadUint32(ms);
        header.string_table_size = s.ReadUint32(ms);
        header.table_hash = s.ReadUint32(ms);
        header.layout_hash = s.ReadUint32(ms);
        header.min_id = s.ReadUint32(ms);
        header.max_id = s.ReadUint32(ms);
        header.locale = s.ReadUint32(ms);
        header.flags = (DB2.DB2Flags)s.ReadUint16(ms);
        header.id_index = s.ReadUint16(ms);
        header.totale_field_count = s.ReadUint32(ms);
        header.bitpacked_data_offset = s.ReadUint32(ms);
        header.lookup_column_count = s.ReadUint32(ms);
        header.field_storage_info_size = s.ReadUint32(ms);
        header.common_data_size = s.ReadUint32(ms);
        header.pallet_data_size = s.ReadUint32(ms);
        header.section_count = s.ReadUint32(ms);

        return header;
    }

    private static DB2.wdc2_db2_section_header ReadSectionHeader(MemoryStream ms)
    {
        StreamTools s = new StreamTools();
        DB2.wdc2_db2_section_header header = new DB2.wdc2_db2_section_header();

        header.tact_key_hash = s.ReadUint64(ms);
        header.file_offset = s.ReadUint32(ms);
        header.record_count = s.ReadUint32(ms);
        header.string_table_size = s.ReadUint32(ms);
        header.copy_table_size = s.ReadUint32(ms);
        header.offset_map_offset = s.ReadUint32(ms);
        header.id_list_size = s.ReadUint32(ms);
        header.relationship_data_size = s.ReadUint32(ms);

        return header;
    }
}
