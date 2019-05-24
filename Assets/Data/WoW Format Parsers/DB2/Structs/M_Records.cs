using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Const.SharedConst;

public partial class DB2
{
    public sealed class MapRecord
    {
        public uint Id;
        public string Directory;
        public string MapName;
        public string MapDescription0;                               // Horde
        public string MapDescription1;                               // Alliance
        public string PvpShortDescription;
        public string PvpLongDescription;
        public float[] Corpse = new float[2];                                           // entrance coordinates in ghost mode  (in most cases = normal entrance)
        public byte MapType;
        public byte InstanceType;
        public byte ExpansionID;
        public ushort AreaTableID;
        public ushort LoadingScreenID;
        public ushort TimeOfDayOverride;
        public ushort ParentMapID;
        public ushort CosmeticParentMapID;
        public byte TimeOffset;
        public float MinimapIconScale;
        public ushort CorpseMapID;                                              // map_id of entrance map in ghost mode (continent always and in most cases = normal entrance)
        public byte MaxPlayers;
        public ushort WindSettingsID;
        public uint ZmpFileDataID;
        public uint WdtFileDataID;
        public uint[] Flags = new uint[2];
    }
}
