﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CASCLib
{
    public static class Extensions
    {
        public static int ReadInt32BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(4);
            return val[3] | val[2] << 8 | val[1] << 16 | val[0] << 24;
        }

        public static long ReadInt40BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(5);
            return val[4] | val[3] << 8 | val[2] << 16 | val[1] << 24 | val[0] << 32;
        }

        public static void Skip(this BinaryReader reader, int bytes)
        {
            reader.BaseStream.Position += bytes;
        }

        public static uint ReadUInt32BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(4);
            return (uint)(val[3] | val[2] << 8 | val[1] << 16 | val[0] << 24);
        }

        public static Action<T, V> GetSetter<T, V>(this FieldInfo fieldInfo)
        {
            var paramExpression = Expression.Parameter(typeof(T));
            var fieldExpression = Expression.Field(paramExpression, fieldInfo);
            var valueExpression = Expression.Parameter(fieldInfo.FieldType);
            var assignExpression = Expression.Assign(fieldExpression, valueExpression);

            return Expression.Lambda<Action<T, V>>(assignExpression, paramExpression, valueExpression).Compile();
        }

        public static Func<T, V> GetGetter<T, V>(this FieldInfo fieldInfo)
        {
            var paramExpression = Expression.Parameter(typeof(T));
            var fieldExpression = Expression.Field(paramExpression, fieldInfo);

            return Expression.Lambda<Func<T, V>>(fieldExpression, paramExpression).Compile();
        }

        public static T Read<T>(this BinaryReader reader) where T : unmanaged
        {
            byte[] result = reader.ReadBytes(Unsafe.SizeOf<T>());

            return Unsafe.ReadUnaligned<T>(ref result[0]);
        }

        public static T[] ReadArray<T>(this BinaryReader reader) where T : unmanaged
        {
            int numBytes = (int)reader.ReadInt64();

            byte[] source = reader.ReadBytes(numBytes);

            reader.BaseStream.Position += (0 - numBytes) & 0x07;

            return source.CopyTo<T>();
        }

        public static T[] ReadArray<T>(this BinaryReader reader, int size) where T : unmanaged
        {
            int numBytes = Unsafe.SizeOf<T>() * size;

            byte[] source = reader.ReadBytes(numBytes);

            return source.CopyTo<T>();
        }

        public static unsafe T[] CopyTo<T>(this byte[] src) where T : unmanaged
        {
            T[] result = new T[src.Length / Unsafe.SizeOf<T>()];

            if (src.Length > 0)
                Unsafe.CopyBlockUnaligned(Unsafe.AsPointer(ref result[0]), Unsafe.AsPointer(ref src[0]), (uint)src.Length);

            return result;
        }

        public static short ReadInt16BE(this BinaryReader reader)
        {
            byte[] val = reader.ReadBytes(2);
            return (short)(val[1] | val[0] << 8);
        }

        public static void CopyBytes(this Stream input, Stream output, int bytes)
        {
            byte[] buffer = new byte[0x4000];
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }

        public static void CopyToStream(this Stream src, Stream dst, long len)
        {
            long done = 0;

            // TODO: Span<byte>+stackalloc
            byte[] buf = new byte[0x10000];

            int count;
            do
            {
                count = src.Read(buf, 0, buf.Length);
                dst.Write(buf, 0, count);

                done += count;
            } while (count > 0);
        }

        public static void ExtractToFile(this Stream input, string path, string name)
        {
            string fullPath = Path.Combine(path, name);
            string dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var fileStream = File.Open(fullPath, FileMode.Create))
            {
                input.Position = 0;
                input.CopyTo(fileStream);
            }
        }

        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        public static bool EqualsTo(this byte[] hash, byte[] other)
        {
            if (hash.Length != other.Length)
                return false;
            for (var i = 0; i < hash.Length; ++i)
                if (hash[i] != other[i])
                    return false;
            return true;
        }

        public static bool EqualsToIgnoreLength(this byte[] array, byte[] other)
        {
            for (var i = 0; i < array.Length; ++i)
                if (array[i] != other[i])
                    return false;
            return true;
        }

        public static byte[] Copy(this byte[] array, int len)
        {
            byte[] ret = new byte[len];
            for (int i = 0; i < len; ++i)
                ret[i] = array[i];
            return ret;
        }

        public static string ToBinaryString(this BitArray bits)
        {
            StringBuilder sb = new StringBuilder(bits.Length);

            for (int i = 0; i < bits.Length; ++i)
            {
                sb.Append(bits[i] ? '1' : '0');
            }

            return sb.ToString();
        }

        public static unsafe bool EqualsTo(this MD5Hash key, byte[] array)
        {
            if (array.Length != 16)
                return false;

            MD5Hash other;

            fixed (byte* ptr = array)
                other = *(MD5Hash*)ptr;

            for (int i = 0; i < 2; ++i)
            {
                ulong keyPart = *(ulong*)(key.Value + i * 8);
                ulong otherPart = *(ulong*)(other.Value + i * 8);

                if (keyPart != otherPart)
                    return false;
            }
            return true;
        }

        public static unsafe bool EqualsTo9(this MD5Hash key, byte[] array)
        {
            if (array.Length < 9)
                return false;

            MD5Hash other;

            fixed (byte* ptr = array)
                other = *(MD5Hash*)ptr;

            ulong keyPart = *(ulong*)(key.Value);
            ulong otherPart = *(ulong*)(other.Value);

            if (keyPart != otherPart)
                return false;

            if (key.Value[8] != other.Value[8])
                return false;

            //for (int i = 0; i < 2; ++i)
            //{
            //    ulong keyPart = *(ulong*)(key.Value + i * 8);
            //    ulong otherPart = *(ulong*)(other.Value + i * 8);

            //    if (keyPart != otherPart)
            //        return false;
            //}

            return true;
        }

        public static unsafe bool EqualsTo(this MD5Hash key, MD5Hash other)
        {
            for (int i = 0; i < 2; ++i)
            {
                ulong keyPart = *(ulong*)(key.Value + i * 8);
                ulong otherPart = *(ulong*)(other.Value + i * 8);

                if (keyPart != otherPart)
                    return false;
            }
            return true;
        }

        public static unsafe string ToHexString(this MD5Hash key)
        {
            byte[] array = new byte[16];

            fixed (byte* aptr = array)
            {
                *(MD5Hash*)aptr = key;
            }

            return array.ToHexString();
        }

        public static unsafe bool IsZeroed(this MD5Hash key)
        {
            for (var i = 0; i < 16; ++i)
                if (key.Value[i] != 0)
                    return false;
            return true;
        }

        public static unsafe MD5Hash ToMD5(this byte[] array)
        {
            if (array.Length != 16)
                throw new ArgumentException("array size != 16");

            fixed (byte* ptr = array)
            {
                return *(MD5Hash*)ptr;
            }
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

        public static void WriteCString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            writer.Write(bytes);
            writer.Write((byte)0);
        }

        public static byte[] ToByteArray(this string str)
        {
            str = str.Replace(" ", string.Empty);

            var res = new byte[str.Length / 2];
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return res;
        }
    }
}
