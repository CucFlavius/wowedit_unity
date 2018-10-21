using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static partial class DB2
{
    public static class Data
    {
        public static Dictionary<string, List<object>> Database = new Dictionary<string, List<object>>();

        public static void Sort(string fileName)
        {
            Type obj = DB2.DefinitionStructs["_" + fileName.Substring(0, fileName.Length - 4)];
            var fieldValues = obj.GetFields().ToList();

            if (magic == "WDC1")
            {
                int columns = fieldValues.Count;
                long rows = WDC1.header.record_count;

                #region method1
                /*
                using (MemoryStream ms = new MemoryStream(WDC1.recordsData))
                {
                    StreamTools s = new StreamTools();
                    for (int r = 0; r < rows/100; r++)
                    {
                        int id = 0;
                        string rowTest = "";

                        for (int c = 0; c < columns; c++)
                        {
                            if (WDC1.m_columnMeta[c].CompressionType == CompressionType.None)
                            {
                                if (fieldValues[c].FieldType.Name == "String")
                                {
                                    //rowTest += WDC1.m_stringsTable[s.ReadLong(ms)] + " ";
                                    rowTest += s.ReadLong(ms) + " ";
                                }
                                else if (fieldValues[c].FieldType.Name == "Int32")
                                {
                                    rowTest += s.ReadLong(ms) + " ";
                                }
                                else if (fieldValues[c].FieldType.Name == "UInt16")
                                {
                                    rowTest += s.ReadUint16(ms) + " ";
                                }
                                else if (fieldValues[c].FieldType.Name == "Byte")
                                {
                                    rowTest += ms.ReadByte() + " ";
                                }
                                else if (fieldValues[c].FieldType.Name == "Int16")
                                {
                                    rowTest += s.ReadShort(ms) + " ";
                                }
                                else if (fieldValues[c].FieldType.Name == "UInt32")
                                {
                                    rowTest += s.ReadUint32(ms) + " ";
                                }
                            }
                            else if (WDC1.m_columnMeta[c].CompressionType == CompressionType.Immediate)
                            {
                                int bitOffset = WDC1.m_columnMeta[c].Immediate.BitOffset;
                                int bitWidth = WDC1.m_columnMeta[c].Immediate.BitWidth;

                                Debug.Log(bitOffset + " " + bitWidth);

                                if (!WDC1.header.flags.HasFlag(DB2Flags.Index) && c == WDC1.header.id_index)
                                {

                                }
                                else
                                {

                                }

                                //int size = bitWidth[]
                                ms.ReadByte();

                                
                            }
                        }
                        Debug.Log(rowTest);
                    }
                }
                */
                #endregion

                #region method2

                //BitReader br = new BitReader(WDC1.recordsData);
                BitReader br = new BitReader();
                br.Initialize(WDC1.recordsData);

                //Debug.Log(columns + " " + WDC1.m_columnMeta.Length);

                int index = WDC1.header.flags.HasFlag(DB2Flags.Index) ? 1 : 0;

                for (int r = 0; r < rows; r++)
                {
                    string rowTest = "";

                    if (index == 1)
                    {
                        rowTest += WDC1.m_indexData[r] + " | ";
                    }

                    for (int c = 0; c < columns-index; c++)
                    {

                        if (WDC1.m_columnMeta[c].CompressionType == CompressionType.None)
                        {
                            if (fieldValues[c].FieldType.Name == "String")
                            {
                                rowTest += WDC1.m_stringsTable[br.ReadInt32()] + " | ";
                            }
                            else if (fieldValues[c].FieldType.Name == "Int32")
                            {
                                rowTest += br.ReadInt32() + " | ";
                            }
                            else if (fieldValues[c].FieldType.Name == "UInt16")
                            {
                                rowTest += br.ReadUInt16() + " | ";
                            }
                            else if (fieldValues[c].FieldType.Name == "Byte")
                            {
                                rowTest += br.ReadByte() + " | ";
                            }
                            else if (fieldValues[c].FieldType.Name == "Int16")
                            {
                                rowTest += br.ReadInt16() + " | ";
                            }
                            else if (fieldValues[c].FieldType.Name == "UInt32")
                            {
                                rowTest += br.ReadUInt32() + " | ";
                            }
                        }
                        else if (WDC1.m_columnMeta[c].CompressionType == CompressionType.Immediate)
                        {
                            int bitOffset = WDC1.m_columnMeta[c].Immediate.BitOffset;
                            int bitWidth = WDC1.m_columnMeta[c].Immediate.BitWidth;

                            // skip data
                            br.SeekBits(bitWidth);
                        }
                    }
                    Debug.Log(rowTest);
                }

                #endregion
            }
        }
        //public static Dictionary<int, Definitions.AnimationData> AnimationData;
    }

}
