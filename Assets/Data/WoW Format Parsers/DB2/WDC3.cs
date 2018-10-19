using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static partial class WDC3
{
    public static void Read(string fileName, byte[] fileData)
    {
        using (MemoryStream ms = new MemoryStream(fileData))
        {
            StreamTools s = new StreamTools();
            DB2.wdc3_db2_header header = ReadHeader(ms);
            DB2.wdc3_db2_section_header section_header = ReadSectionHeader(ms);
        }
    }

    private static DB2.wdc3_db2_header ReadHeader(MemoryStream ms)
    {
        StreamTools s = new StreamTools();

        DB2.wdc3_db2_header header = new DB2.wdc3_db2_header();
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

    private static DB2.wdc3_db2_section_header ReadSectionHeader(MemoryStream ms)
    {
        StreamTools s = new StreamTools();

        DB2.wdc3_db2_section_header header = new DB2.wdc3_db2_section_header();

        header.tact_key_hash = s.ReadUint64(ms);
        header.file_offset = s.ReadUint32(ms);
        header.record_count = s.ReadUint32(ms);
        header.string_table_size = s.ReadUint32(ms);
        header.offset_records_end = s.ReadUint32(ms);
        header.id_list_size = s.ReadUint32(ms);
        header.relationship_data_size = s.ReadUint32(ms);
        header.offset_map_id_count = s.ReadUint32(ms);
        header.copy_table_count = s.ReadUint32(ms);

        return header;
    }
}
