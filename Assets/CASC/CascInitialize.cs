using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CascInitialize  {

    public static bool ThreadRunning;
    public static bool Initialized;
    public static bool Working;
    public static bool Working_InitializationFinished;
    public static string CurrentWorkerText;
    public static string PreviousWorkerText;
    public static float CurrentWorkerPercent;
    public static float PreviousWorkerPercent;
    public static System.Threading.Thread CASCThread = new System.Threading.Thread(CASCInitThread);

    public static void Reset()
    {
        Initialized = false;
        Working = false;
        Working_InitializationFinished = false;
    }

    public static void Start ()
    {
        if (Initialized == false)
        {
            //CASCThread = new System.Threading.Thread(CASCInitThread);
            ThreadRunning = true;
            CASCThread.Start();
        }
        else
        {
            Debug.Log("Already initialized CASC");
        }
    }

    public static void Stop ()
    {
        Initialized = false;
        ThreadRunning = false;
        CASCThread.Abort();
        CASCThread = null;
    }

    private static void CASCInitThread()
    {
        while (ThreadRunning)
        {
            Working = true;

            EncryptionKeys.ParseKeyFile(Settings.ApplicationPath + @"\ListFiles\keys1.txt"); /// will need more keys maybe ///

            CurrentWorkerText = "Reading WoW Folder";
            Casc.ReadWoWFolder();                    // check for correct wow data path
            CurrentWorkerPercent = .15f;

            CurrentWorkerText = "Find Build Config";
            Casc.FindWoWBuildConfig();
            CurrentWorkerPercent = .30f;

            CurrentWorkerText = "Reading IDX files";
            Casc.ReadWoWIDXfiles();              // read the IDX files
            Debug.Log("LocalIndexData Size : " + IndexBlockParser.LocalIndexData.Count);
            CurrentWorkerPercent = .45f;

            CurrentWorkerText = "Loading Encoding File";
            Casc.LoadEncodingFile();                // Locate and extract encoding file from the data files using the MD5 found in build configuration file
            Debug.Log("Encoding File Size : " + Casc.EncodingData.Count);
            CurrentWorkerPercent = .60f;

            CurrentWorkerText = "Loading Root File";
            Casc.LoadWoWRootFile();
            Debug.Log("Root Data Size : " + Casc.MyRootData.Count);
            CurrentWorkerPercent = .75f;

            CurrentWorkerText = "Loading the Filelist";
            Casc.LoadFilelist();
            CurrentWorkerPercent = .90f;

            /*
            CurrentWorkerText = "Sorting the Filelist";
            LoadTreeData();
            CurrentWorkerPercent = .90f;
            */

            Working_InitializationFinished = true;
            Initialized = true;

            CASCThread.Abort();
            CASCThread = null;
            ThreadRunning = false;
        }
    }
}
