using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using CASCLib;
using System.Threading;
using System.IO;

namespace Assets.UI.CASC
{
    public class CascHandler : MonoBehaviour
    {
        private LocaleFlags installedLocalesMask;
        private LocaleFlags firstInstalledLocale;
        public CASCHandler cascHandler;

        public void InitCasc(CASCConfig config)
        {
            uint localeMask = 0;

            string[] tagLines = config.BuildInfo[0]["Tags"].Split(' ');
            foreach (var line in tagLines)
            {
                if (!Enum.TryParse(line, true, out LocaleFlags locale))
                    continue;

                localeMask = localeMask | Convert.ToUInt32(locale);
            }

            installedLocalesMask = (LocaleFlags)localeMask;
            firstInstalledLocale = LocaleFlags.None;

            for (Locale i = 0; i < Locale.Total; ++i)
            {
                if (i == Locale.None)
                    continue;

                if (!Convert.ToBoolean(installedLocalesMask & SharedConst.WowLocaleToCascLocaleFlags[(int)i]))
                    continue;

                firstInstalledLocale = SharedConst.WowLocaleToCascLocaleFlags[(int)i];
                break;
            }

            if (firstInstalledLocale < LocaleFlags.None)
                Debug.Log("No locales detected.");

            // Opens Storage
            cascHandler = CASCHandler.OpenStorage(config);
            cascHandler.Root.SetFlags(firstInstalledLocale, false);
        }

        public List<string> ReadBuildInfo(string path)
        {
            var wowInstalls = new List<string>();

            // Pretty self explanatory what this does :P
            if (File.Exists(Path.Combine(path, ".build.info")))
            {
                using (var sr = new StreamReader(Path.Combine(path, ".build.info")))
                {
                    // Read the first useless line.
                    sr.ReadLine();

                    while(!sr.EndOfStream)
                    {
                        // Reading the line and splitting it
                        var line = sr.ReadLine();
                        var split = line.Split('|');

                        // Check if build is active.
                        if (split[1] == "1")
                            wowInstalls.Add(split[13]);
                        else
                            Debug.Log($"INFO: Product '{split[13]}' ({split[0]} {split[2]}) is not active.");
                    }
                }
            }
            else
                throw new Exception($"ERROR: '.build.info' is not in {path}");

            return wowInstalls;
        }
    }
}
