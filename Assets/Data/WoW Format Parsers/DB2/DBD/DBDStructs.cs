using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DB2.DBDBuild;
using static DB2.DBDBuildRange;

public static partial class DB2
{
    public static class DBDStructs
    {
        public struct DBDefinition
        {
            public Dictionary<string, ColumnDefinition> columnDefinitions;
            public VersionDefinitions[] versionDefinitions;
        }

        public struct VersionDefinitions
        {
            public Build[] builds;
            public BuildRange[] buildRanges;
            public string[] layoutHashes;
            public string comment;
            public Definition[] definitions;
        }

        public struct Definition
        {
            public int size;
            public int arrLength;
            public string name;
            public bool isID;
            public bool isRelation;
            public bool isNonInline;
            public bool isSigned;
            public string comment;
        }

        public struct ColumnDefinition
        {
            public string type;
            public string foreignTable;
            public string foreignColumn;
            public bool verified;
            public string comment;
        }
    }
}