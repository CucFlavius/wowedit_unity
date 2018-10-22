using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class Extensions
{
    public static T[] ReadArray<T>(this BinaryReader reader, int size) where T : struct
    {
        int numBytes = Marshal.SizeOf<T>() * size;

        byte[] result = reader.ReadBytes(numBytes);

        return FastStruct<T>.ReadArray(result);
    }

    public static T Read<T>(this BinaryReader reader) where T : struct
    {
        byte[] result = reader.ReadBytes(FastStruct<T>.Size);

        return FastStruct<T>.ArrayToStructure(result);
    }

    public static bool HasFlagExt(this DB2.DB2Flags flag, DB2.DB2Flags valueToCheck)
    {
        return (flag & valueToCheck) == valueToCheck;
    }
}

public static class CStringExtensions
{
    /// <summary> Reads the NULL terminated string from
    /// the current stream and advances the current position of the stream by string length + 1.
    /// <seealso cref="BinaryReader.ReadString"/>
    /// </summary>
    public static string ReadCString(this BinaryReader reader)
    {
        return reader.ReadCString(Encoding.UTF8);
    }

    /// <summary> Reads the NULL terminated string from
    /// the current stream and advances the current position of the stream by string length + 1.
    /// <seealso cref="BinaryReader.ReadString"/>
    /// </summary>
    public static string ReadCString(this BinaryReader reader, Encoding encoding)
    {
        var bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 0)
            bytes.Add(b);
        return encoding.GetString(bytes.ToArray());
    }
}