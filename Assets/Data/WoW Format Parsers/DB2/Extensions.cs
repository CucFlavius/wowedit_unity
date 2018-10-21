using System;
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
    public static string ReadCString(this BinaryReader reader)
    {
        return reader.ReadCString();
    }
}