using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class DB2
{
    public abstract class BaseReader
    {
        public int RecordsCount { get; protected set; }
        public int FieldsCount { get; protected set; }
        public int RecordSize { get; protected set; }
        public int StringTableSize { get; protected set; }
        public uint TableHash { get; protected set; }
        public uint LayoutHash { get; protected set; }
        public int MinIndex { get; protected set; }
        public int MaxIndex { get; protected set; }
        public int IdFieldIndex { get; protected set; }
        public DB2Flags Flags { get; protected set; }

        #region Data

        protected FieldMetaData[] m_meta;
        public FieldMetaData[] Meta => m_meta;

        protected int[] m_indexData;
        public int[] IndexData => m_indexData;

        protected ColumnMetaData[] m_columnMeta;
        public ColumnMetaData[] ColumnMeta => m_columnMeta;

        protected Value32[][] m_palletData;
        public Value32[][] PalletData => m_palletData;

        protected Dictionary<int, Value32>[] m_commonData;
        public Dictionary<int, Value32>[] CommonData => m_commonData;

        protected Dictionary<long, string> m_stringsTable;
        public Dictionary<long, string> StringTable => m_stringsTable;

        protected Dictionary<int, int> m_copyData;

        protected byte[] recordsData;
        protected Dictionary<int, IDBRow> _Records = new Dictionary<int, IDBRow>();

        public SparseEntry[] SparseEntries;

        #endregion

        #region Helpers

        public void Enumerate(Action<IDBRow> action)
        {
            Parallel.ForEach(_Records.Values, action);
            Parallel.ForEach(GetCopyRows(), action);
        }

        private IEnumerable<IDBRow> GetCopyRows()
        {
            if (m_copyData == null || m_copyData.Count == 0)
                yield break;

            // fix temp ids
            _Records = _Records.ToDictionary(x => x.Value.Id, x => x.Value);

            foreach (var copyRow in m_copyData)
            {
                IDBRow rec = _Records[copyRow.Value].Clone();
                rec.Data = new BitReader(recordsData);
                rec.Id = copyRow.Key;
                _Records[rec.Id] = rec;
                yield return rec;
            }

            m_copyData.Clear();
        }

        #endregion
    }
}
