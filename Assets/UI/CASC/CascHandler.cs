using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using CASCLib;
using System.Threading;
using Assets.Tools.CSV;

namespace Assets.UI.CASC
{
    public class CascHandler : MonoBehaviour
    {
        public CASCHandler cascHandler;
        public ListfileLoader listfileLoader = new ListfileLoader();

        public void InitCasc(CASCConfig config)
        {
            new Thread(() => {
                cascHandler = CASCHandler.OpenStorage(config);
                cascHandler.Root.SetFlags(LocaleFlags.None, false, false);

                Debug.Log("Loading Listfile...");
                cascHandler.Root.LoadListFile();
                listfileLoader.LoadListfile();

                cascHandler.Root.SetFlags(LocaleFlags.None, false);
            }).Start();
        }
    }
}
