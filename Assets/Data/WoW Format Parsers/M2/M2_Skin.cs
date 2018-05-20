using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static partial class M2
{
    public static void ParseSkin(MemoryStream ms, M2Data m2Data)
    {
        StreamTools s = new StreamTools();

        string magic = s.ReadFourCC(ms);            // 'SKIN'
        M2Array vertices = s.ReadM2Array(ms);
        M2Array indices = s.ReadM2Array(ms);
        M2Array bones = s.ReadM2Array(ms);
        M2Array submeshes = s.ReadM2Array(ms);
        M2Array batches = s.ReadM2Array(ms);
        int boneCountMax = s.ReadLong(ms);          // WoW takes this and divides it by the number of bones in each submesh, then stores the biggest one.
                                                    // Maximum number of bones per drawcall for each view. Related to (old) GPU numbers of registers. 
                                                    // Values seen : 256, 64, 53, 21
        M2Array shadow_batches = s.ReadM2Array(ms);
    }
}