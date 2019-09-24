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
        public CASCHandler cascHandler;
        public GameObject blockUI;

        public static bool unblockUI = false;

        public void Start()
        {
            blockUI.SetActive(true);
        }

        public void Update()
        {
            if (unblockUI == true)
            {
                blockUI.SetActive(false);
                unblockUI = false;
            }
        }

        public void InitCasc(CASCConfig config, LocaleFlags firstInstalledLocale)
        {
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
