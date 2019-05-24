using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Const
{
    public class SharedConst
    {
        public const LocaleConstant DefaultLocale = LocaleConstant.enUS;
        public enum LocaleConstant
        {
            enUS = 0,
            koKR = 1,
            frFR = 2,
            deDE = 3,
            zhCN = 4,
            zhTW = 5,
            esES = 6,
            esMX = 7,
            ruRU = 8,
            None = 9,
            ptBR = 10,
            itIT = 11,
            Total = 12
        }

        public enum MapFlags : uint
        {
            CanToggleDifficulty = 0x0100,
            FlexLocking = 0x8000, // All difficulties share completed encounters lock, not bound to a single instance id
                                  // heroic difficulty flag overrides it and uses instance id bind
            Garrison = 0x4000000
        }

        public enum MapTypes : byte
        {
            Common = 0,
            Instance = 1,
            Raid = 2,
            Battleground = 3,
            Arena = 4,
            Scenario = 5
        }

        public enum Expansion
        {
            LevelCurrent = -1,
            Classic = 0,
            BurningCrusade = 1,
            WrathOfTheLichKing = 2,
            Cataclysm = 3,
            MistsOfPandaria = 4,
            WarlordsOfDraenor = 5,
            Legion = 6,
            BattleForAzeroth = 7,
            Max,

            MaxAccountExpansions
        }
    }
}
