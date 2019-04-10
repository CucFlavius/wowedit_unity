﻿using System;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Data.CASC
{
    public static partial class Casc
    {
        public class md5Hash
        {
            public byte[] Value = new byte[16];
        }

        public struct MD5Hash
        {
            public md5Hash Hash;
        }

        public struct RootFile
        {
            public MultiDictionary<ulong, RootEntry> Entries;
        }

        public struct RootEntry
        {
            public ContentFlags contentFlags;
            public LocaleFlags localeFlags;
            public ulong lookup;
            public uint fileDataId;
            public MD5Hash md5;
        }

        [Flags]
        public enum LocaleFlags : uint
        {
            All = 0xFFFFFFFF,
            None = 0,
            //Unk_1 = 0x1,
            enUS = 0x2,
            koKR = 0x4,
            //Unk_8 = 0x8,
            frFR = 0x10,
            deDE = 0x20,
            zhCN = 0x40,
            esES = 0x80,
            zhTW = 0x100,
            enGB = 0x200,
            enCN = 0x400,
            enTW = 0x800,
            esMX = 0x1000,
            ruRU = 0x2000,
            ptBR = 0x4000,
            itIT = 0x8000,
            ptPT = 0x10000,
            enSG = 0x20000000, // custom
            plPL = 0x40000000, // custom
            All_WoW = enUS | koKR | frFR | deDE | zhCN | esES | zhTW | enGB | esMX | ruRU | ptBR | itIT | ptPT
        }

        [Flags]
        public enum ContentFlags : uint
        {
            None = 0,
            F00000001 = 0x1,
            F00000002 = 0x2,
            F00000004 = 0x4,
            F00000008 = 0x8, // added in 7.2.0.23436
            F00000010 = 0x10, // added in 7.2.0.23436
            LowViolence = 0x80, // many models have this flag
            F10000000 = 0x10000000,
            F20000000 = 0x20000000, // added in 21737
            Bundle = 0x40000000,
            NoCompression = 0x80000000 // sounds have this flag
        }
    }
}
