using System.Collections.Generic;
using System.IO;

public partial class DB2
{
    public class Storage<T> : SortedDictionary<int, T> where T : class, new()
    {
        public Storage(DB2Reader dbReader) => dbReader.PopulateRecords(this);
    }
}
