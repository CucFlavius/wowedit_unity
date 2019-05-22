using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Assets.WoWEditSettings;
using UnityEngine;

namespace Assets.Data.DataLocal
{
    public class DataLocalHandler
    {
        public static string GetFile(string fileNameRaw)
        {
            string fileLocation = null;
            string fileName = string.Empty;

            if (fileNameRaw[0] == @"/"[0] || fileNameRaw[0] == @"\"[0])
                fileName = fileName.Remove(0, 1);
            else
                fileName = fileNameRaw;

            if (File.Exists(Settings.GetSection("path").GetString("extractedpath") + $@"\{fileName}"))
                fileLocation = Settings.GetSection("path").GetString("extractedpath") + $@"\{fileName}";
            else
                Debug.Log($@"Missing Extracted File : {Settings.GetSection("path").GetString("extractedpath")}\{fileName}");

            return fileLocation;
        }

        public static Stream GetFileStream(string filename)
        {
            Stream stream = File.OpenRead(GetFile(filename));
            return stream;
        }
    }
}
