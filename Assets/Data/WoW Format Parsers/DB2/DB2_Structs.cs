using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public static partial class DB2
{
    public struct wdc1_db2_header
    {
        public int magic;                  // 'WDC1'
        public int record_count;
        public int field_count;
        public int record_size;
        public int string_table_size;
        public int table_hash;             // hash of the table name
        public int layout_hash;            // this is a hash field that changes only when the structure of the data changes
        public int min_id;
        public int max_id;
        public int locale;                 // as seen in TextWowEnum
        public int copy_table_size;
        public DB2Flags flags;                  // possible values are listed in Known Flag Meanings
        public int id_index;               // this is the index of the field containing ID values; this is ignored if flags & 0x04 != 0
        public int total_field_count;      // from WDC1 onwards, this value seems to always be the same as the 'field_count' value
        public int bitpacked_data_offset;  // relative position in record where bitpacked data begins; not important for parsing the file
        public int lookup_column_count;
        public int offset_map_offset;      // Offset to array of struct {public int offset; uint16_t size;}[max_id - min_id + 1];
        public int id_list_size;           // List of ids present in the DB file
        public int field_storage_info_size;
        public int common_data_size;
        public int pallet_data_size;
        public int relationship_data_size;
    };

    public struct wdc2_db2_header
    {
        public int magic;                   // 'WDC2'
        public int record_count;            // This is for all sections combined now
        public int field_count;
        public int record_size;
        public int string_table_size;       // This is for all sections combined now
        public int table_hash;              // Hash of the table name
        public int layout_hash;             // This is a hash field that changes only when the structure of the data changes
        public int min_id;
        public int max_id;
        public int locale;                  // As seen in TextWowEnum
        public DB2Flags flags;              // Possible values are listed in Known Flag Meanings
        public int id_index;                // This is the index of the field containing the ID values; this is ignored if flags & 0x04 != 0
        public int totale_field_count;      // From WDC1 onwards, this value seems to always be the same as the 'field_count' value
        public int bitpacked_data_offset;   // Relative position in record where bitpacked data begins; not important for parsing the file
        public int lookup_column_count;
        public int field_storage_info_size;
        public int common_data_size;
        public int pallet_data_size;
        public int section_count;           // New to WDC2, this number of sections of data
    }

    public struct wdc2_db2_section_header
    {
        public ulong tact_key_hash;           // TactKeyLookup hash
        public int file_offset;             // Absolute position to the beginning of the section
        public int record_count;            // 'record_count' for the section
        public int string_table_size;       // 'string_table_size' for the section
        public int copy_table_size;
        public int offset_map_offset;       // Offset to array of struct {uint32_t offset; uint16_t size;} 
                                            // [max_id - min_id + 1];
        public int id_list_size;            // Size of the list if ids present in the section
        public int relationship_data_size;
    }

    public struct wdc3_db2_header
    {
        public int magic;                   // 'WDC3'
        public int record_count;            // This is for all sections combined now
        public int field_count;
        public int record_size;
        public int string_table_size;       // This is for all sections combined now
        public int table_hash;              // Hash of the table name
        public int layout_hash;             // This is a hash field that changes only when the structure of the data changes
        public int min_id;
        public int max_id;
        public int locale;                  // As seen in TextWowEnum
        public DB2Flags flags;              // Possible values are listed in Known Flag Meanings
        public int id_index;                // This is the index of the field containing the ID values; this is ignored if flags & 0x04 != 0
        public int totale_field_count;      // From WDC1 onwards, this value seems to always be the same as the 'field_count' value
        public int bitpacked_data_offset;   // Relative position in record where bitpacked data begins; not important for parsing the file
        public int lookup_column_count;
        public int field_storage_info_size;
        public int common_data_size;
        public int pallet_data_size;
        public int section_count;           // New to WDC2, this number of sections of data
    }

    public struct wdc3_db2_section_header
    {
        public ulong tact_key_hash;           // TactKeyLookup hash
        public int file_offset;             // Absolute position to the beginning of the section
        public int record_count;            // 'record_count' for the section
        public int string_table_size;       // 'string_table_size' for the section
        public int offset_records_end;      // Offset to the spot were the records end in a file with an offset map structure;
        public int id_list_size;            // Size of the list of ids present in the section
        public int relationship_data_size;  // Size of the relationshop data in the section
        public int offset_map_id_count;     // Count of ids present in the offset map in the section
        public int copy_table_count;        // Count of the number of deduplication entries (you can multiply b8 to mimic the old 'copy_table_size' field)
    }

    public enum DB2Flags
    {
        None = 0x0,
        Sparse = 0x1,
        SecondaryKey = 0x2,
        Index = 0x4,
        Unknown1 = 0x8,
        Unknown2 = 0x10
    }

    public struct field_structure
    {
        public int size;
        public int offset;
    };

    public struct record_data
    {
        public char[] data;
    };

    public struct offset_map_entry
    {
        public uint offset;
        public ushort size;
    };

    public struct Value32
    {
        unsafe fixed byte Value[4];

        public T GetValue<T>() where T : struct
        {
            unsafe
            {
                fixed (byte* ptr = Value)
                    return FastStruct<T>.ArrayToStructure(ref ptr[0]);
            }
        }
    }

    public struct Value64
    {
        unsafe fixed byte Value[8];

        public T GetValue<T>() where T : struct
        {
            unsafe
            {
                fixed (byte* ptr = Value)
                    return FastStruct<T>.ArrayToStructure(ref ptr[0]);
            }
        }
    }

    public enum CompressionType
    {
        None = 0,
        Immediate = 1,
        Common = 2,
        Pallet = 3,
        PalletArray = 4,
        SignedImmediate = 5
    }

    public struct ColumnCompressionData_Immediate
    {
        public int BitOffset;
        public int BitWidth;
        public int Flags; // 0x1 signed
    }

    public struct ColumnCompressionData_Pallet
    {
        public int BitOffset;
        public int BitWidth;
        public int Cardinality;
    }

    public struct ColumnCompressionData_Common
    {
        public Value32 DefaultValue;
        public int B;
        public int C;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ColumnMetaData
    {
        [FieldOffset(0)]
        public int RecordOffset;
        [FieldOffset(2)]
        public int Size;
        [FieldOffset(4)]
        public int AdditionalDataSize;
        [FieldOffset(8)]
        public CompressionType CompressionType;
        [FieldOffset(12)]
        public ColumnCompressionData_Immediate Immediate;
        [FieldOffset(12)]
        public ColumnCompressionData_Pallet Pallet;
        [FieldOffset(12)]
        public ColumnCompressionData_Common Common;
    }

    public struct copy_table_entry
    {
        public int id_of_new_row;
        public int id_of_copied_row;
    };

    public struct relationship_entry
    {
        // This is the id of the foreign key for the record, e.g. SpellID in
        // SpellX* tables.
        public int foreign_id;
        // This is the index of the record in record_data.  Note that this is
        // *not* the record's own ID.
        public int record_index;
    };

    public class ReferenceData
    {
        public int NumRecords { get; set; }
        public int MinId { get; set; }
        public int MaxId { get; set; }
        public ReferenceEntry[] Entries { get; set; }
    }

    public struct ReferenceEntry
    {
        public int Id;
        public int Index;
    }

    public class FieldCache<T>
    {
        public FieldInfo Field;
        public int ArraySize = 0;
        public Action<T, object> Setter;

        public FieldCache(FieldInfo field, int arraySize, Action<T, object> setter)
        {
            this.Field = field;
            this.ArraySize = arraySize;
            this.Setter = setter;
        }
    }

    public interface IDB2Row
    {
        int Id { get; set; }
        int RecordIndex { get; set; }
        void GetFields<T>(FieldCache<T>[] fields, T entry);
        IDB2Row Clone();
    }

    public struct field_storage_info
    {
        public ushort field_offset_bits;
        public ushort field_size_bits; // very important for reading bitpacked fields; size is the sum of all array pieces in bits - for example, uint32[3] will appear here as '96'
        // additional_data_size is the size in bytes of the corresponding section in
        // common_data or pallet_data.  These sections are in the same order as the
        // field_info, so to find the offset, add up the additional_data_size of any
        // previous fields which are stored in the same block (common_data or
        // pallet_data).
        public int additional_data_size;
        public int storage_type;
        public int val1;
        public int val2;
        public int val3;
    }
}
