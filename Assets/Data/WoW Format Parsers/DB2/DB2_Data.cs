using Assets.Const;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DB2
{
    [Serializable]
    public class DB2Storage<T> : Dictionary<uint, T> where T : new()
    {
        public bool HasRecord(uint id)
        {
            return ContainsKey(id);
        }
    }
}
