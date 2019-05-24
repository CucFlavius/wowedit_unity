using CASCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UI.CASC
{
    public class SharedConst
    {
        public static LocaleFlags[] WowLocaleToCascLocaleFlags =
        {
            LocaleFlags.enUS | LocaleFlags.enGB,
            LocaleFlags.koKR,
            LocaleFlags.frFR,
            LocaleFlags.deDE,
            LocaleFlags.zhCN,
            LocaleFlags.zhTW,
            LocaleFlags.esES,
            LocaleFlags.esMX,
            LocaleFlags.ruRU,
            0,
            LocaleFlags.ptBR | LocaleFlags.ptPT,
            LocaleFlags.itIT
        };
    }
}
