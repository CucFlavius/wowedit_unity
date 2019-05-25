using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using CASCLib;
using Assets.UI.CASC;
using System.Runtime.CompilerServices;
using Assets.Data;
using System.Linq;

public partial class DB2
{
    public class DB2Reader
    {
        private BaseReader _reader;
        private CASCHandler Casc;

        public DB2Reader(uint FileDataId)
        {
            Casc = GameObject.Find("[CASC]").GetComponent<CascHandler>().cascHandler;
            var stream = Casc.OpenFile(FileDataId);
            using (var bin = new BinaryReader(stream))
            {
                var identifier = new string(bin.ReadChars(4));
                stream.Position = 0;
                switch (identifier)
                {
                    case "WDC3":
                        _reader = new WDC3(stream);
                        break;
                    default:
                        Debug.Log("DBC Type " + identifier + " is not supported");
                        break;
                }
            }
        }

        public Storage<T> GetRecords<T>() where T : class, new() => new Storage<T>(this);

        public void PopulateRecords<T>(IDictionary<int, T> storage) where T : class, new() => ReadRecords(storage);

        public virtual void ReadRecords<T>(IDictionary<int, T> storage) where T : class, new()
        {
            var fieldCache = typeof(T).GetFields().Select(x => new FieldCache<T>(x)).ToArray();

            _reader.Enumerate((row) =>
            {
                T entry = new T();
                row.GetFields(fieldCache, entry);
                lock (storage)
                    storage.Add(row.Id, entry);
            });
        }
    }
}
