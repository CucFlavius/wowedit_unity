using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ChunkID
{
    public enum ADT // blame Fabi
    {
        MVER = 0x4d564552,
        MHDR = 0x4d484452,
        MMID = 0x4d4d4944,
        MWMO = 0x4d574d4f,
        MWID = 0x4d574944,
        MDDF = 0x4d444446,
        MODF = 0x4d4f4446,
        MH2O = 0x4d48324f,
        MCNK = 0x4d434e4b,
        MCVT = 0x4d435654,
        MCLV = 0x4d434c56,
        MCCV = 0x4d434356,
        MCRF = 0x4d435246,
        MCRD = 0x4d435244,
        MCRW = 0x4d435257,
        MCLQ = 0x4d434c51,
        MCNR = 0x4d434e52,
        MCSE = 0x4d435345,
        MCBB = 0x4d434242,
        MCDD = 0x4d434444,
        MFBO = 0x4d46424f,
        MBMH = 0x4d424d48,
        MBBB = 0x4d424242,
        MBNV = 0x4d424e56,
        MBMI = 0x4d424d49,
        MTEX = 0x4d544558,
        MCLY = 0x4d434c59,
        MCSH = 0x4d435348,
        MCAL = 0x4d43414c,
        MCMT = 0x4d434d54,
        MTXF = 0x4d545846,
        MTXP = 0x4d545850,
        MAMP = 0x4d414d50,
        MLHD = 0x4d4c4844,
        MLVH = 0x4d4c5648,
        MLVI = 0x4d4c5649,
        MLLL = 0x4d4c4c4c,
        MLND = 0x4d4c4e44,
        MLSI = 0x4d4c5349,
        MLLD = 0x4d4c4c44,
        MLLN = 0x4d4c4c4e,
        MLLV = 0x4d4c4c56,
        MLLI = 0x4d4c4c49,
        MLMD = 0x4d4c4d44,
        MLMX = 0x4d4c4d58,
        MLDD = 0x4d4c4444,
        MLDX = 0x4d4c4458,
        MLDL = 0x4d4c444c,
        MLFD = 0x4d4c4644,
        MBMB = 0x4d424d42,
        MMDX = 0x4d4d4458
    }
}

