using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class DB2
{
    public interface IDBRow
    {
        int Id { get; set; }
        BitReader Data { get; set; }
        void GetFields<T>(FieldCache<T>[] fields, T entry);
        IDBRow Clone();
    }
}
