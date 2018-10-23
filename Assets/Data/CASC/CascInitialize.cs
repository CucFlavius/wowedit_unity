using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CascInitialize  {

    public static bool ThreadRunning;
    public static bool Initialized;
    public static bool Working = false;
    public static bool Working_InitializationFinished;
    public static string CurrentWorkerText;
    public static string PreviousWorkerText;
    public static float CurrentWorkerPercent;
    public static float PreviousWorkerPercent;
    public static System.Threading.Thread CASCThread;
    public static string CurrentDataVersion;

    public static void Reset()
    {
        Initialized = false;
        //Working = false;
        Working_InitializationFinished = false;
    }

    public static void Start ()
    {
        CASCThread = new System.Threading.Thread(CASCInitThread);
        CASCThread.IsBackground = true;
        CASCThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        if (Initialized == false)
        {
            ThreadRunning = true;
            //CASCThread.Start();
            CASCInitThread();
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
        CurrentDataVersion = Settings.Data[3];
        Working = true;

        EncryptionKeys.ParseKeyFile(Settings.ApplicationPath + @"\ListFiles\keys1.txt"); /// will need more keys maybe ///

        Casc.ClearData();
            
        CurrentWorkerText = "Reading WoW Folder";
        Casc.ReadWoWFolder();                    // check for correct wow data path
        CurrentWorkerPercent = .05f;

        CurrentWorkerText = "Find Build Config";
        Casc.FindWoWBuildConfig();
        CurrentWorkerPercent = .10f;

        CurrentWorkerText = "Reading IDX files";
        Casc.ReadWoWIDXfiles();              // read the IDX files
        Debug.Log("LocalIndexData Size : " + IndexBlockParser.LocalIndexData.Count);
        CurrentWorkerPercent = .25f;

        CurrentWorkerText = "Loading Encoding File";
        Casc.LoadEncodingFile();                // Locate and extract encoding file from the data files using the MD5 found in build configuration file
        Debug.Log("Encoding File Size : " + Casc.EncodingData.Count);
        CurrentWorkerPercent = .40f;

        CurrentWorkerText = "Loading Root File";
        Casc.LoadWoWRootFile();
        Debug.Log("Root Data Size : " + Casc.MyRootData.Count);
        CurrentWorkerPercent = .55f;

        CurrentWorkerText = "Loading the Filelist";
        Casc.LoadFilelist();
        CurrentWorkerPercent = .75f;

        CurrentWorkerText = "Sorting the Filelist";
        Casc.LoadTreeData();
        CurrentWorkerPercent = .90f;

        Working_InitializationFinished = true;
        Initialized = true;
        TerrainImport.Initialized = false;
        CASCThread.Abort();
        CASCThread = null;
        ThreadRunning = false;

        DB2.Initialize();
    }
}
