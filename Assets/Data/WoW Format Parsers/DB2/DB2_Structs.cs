using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class DB2
{
    public interface IDB2Row
    {
        int Id { get; set; }
        BitReader Data { get; set; }
        void GetFields<T>(FieldCache<T>[] fields, T entry);
        IDB2Row Clone();
    }

    public class DB2Reader
    {
        public int RecordsCount { get; set; }
        public int FieldsCount { get; set; }
        public int RecordSize { get; set; }
        public int StringTableSize { get; set; }
        public uint TableHash { get; set; }
        public uint LayoutHash { get; set; }
        public int MinIndex { get; set; }
        public int MaxIndex { get; set; }
        public int IdFieldIndex { get; set; }
        public DB2Flags Flags { get; set; }

        public FieldMetaData[] m_meta;
        public FieldMetaData[] Meta => m_meta;

        public int[] m_indexData;
        public int[] IndexData => m_indexData;

        public ColumnMetaData[] m_columnMeta;
        public ColumnMetaData[] ColumnMeta => m_columnMeta;

        public Value32[][] m_palletData;
        public Value32[][] PalletData => m_palletData;

        public Dictionary<int, Value32>[] m_commonData;
        public Dictionary<int, Value32>[] CommonData => m_commonData;

        public Dictionary<long, string> StringTable => m_stringsTable;

        public Dictionary<int, IDB2Row> _Records = new Dictionary<int, IDB2Row>();

        // Normal Records Data
        public byte[] recordsData;
        public Dictionary<long, string> m_stringsTable;

        // Sparse records data
        public offset_map_entry[] sparseEntries;

        public bool HasRow(int id)
        {
            return _Records.ContainsKey(id);
        }
    }

    public struct SectionHeader
    {
        public int unk1;
        public int unk2;
        public int FileOffset;
        public int NumRecords;
        public int StringTableSize;
        public int CopyTableSize;
        public int SparseTableOffset; // CatalogDataOffset, absolute value, {uint offset, ushort size}[MaxId - MinId + 1]
        public int IndexDataSize; // int indexData[IndexDataSize / 4]
        public int ParentLookupDataSize; // uint NumRecords, uint minId, uint maxId, {uint id, uint index}[NumRecords], questionable usefulness...
    }

    public enum DB2Flags : short
    {
        None = 0x0,
        OffsetMap = 0x1,
        RelationshipData = 0x2,
        IndexMap = 0x4,
        Unknown = 0x8,
        Compressed = 0x10
    }

    public struct field_structure
    {
        public int size;
        public int offset;
    };

    public struct FieldMetaData
    {
        public short Bits;
        public short Offset;
    }

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
        public bool IsArray = false;
        public bool IndexMapField = false;

        public Action<T, object> Setter;

        public FieldCache(FieldInfo field, bool isArray, Action<T, object> setter, bool indexMapField)
        {
            Field = field;
            IsArray = isArray;
            Setter = setter;
            IndexMapField = indexMapField;
        }
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
