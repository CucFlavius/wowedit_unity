using Assets.World.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tools.CSV
{
    public class CSVReader
    {
        private static WebClient client = new WebClient();
        private static string csvFile = "listfile.csv";
        private static string listUrl = "https://bnet.marlam.in/listfile.php?type=csv";

        public static ListFile listFile = new ListFile();
        public static List<string> TextureFileNames;

        /// <summary>
        /// Load Listfile.CSV from the current directory.
        /// </summary>
        public static void LoadCSV()
        {
            listFile.TexturePath = new Dictionary<uint, string>();

            if (!File.Exists(csvFile))
            {
                client.DownloadFile(listUrl, csvFile);
                using (var streamReader = new StreamReader(csvFile))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        string[] values = line.Split(';');

                        listFile.TexturePath.Add(uint.Parse(values[0]), values[1]);
                    }
                }
            }
            else
            {
                using (var streamReader = new StreamReader(csvFile))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        string[] values = line.Split(';');

                        listFile.TexturePath.Add(uint.Parse(values[0]), values[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Searches the Id referenced in TXID and other FileDataId chunks.
        /// </summary>
        /// <param name="id"></param>
        public static string LookupId(uint id)
        {
            if (listFile.TexturePath.TryGetValue(id, out string Filename))
            {
                Debug.Log($"Id: {id} Filename: {Filename}");
                return Filename;
            }
            else
            {
                if (id != 0)
                {
                    string filename = $"{id}.blp";
                    Debug.Log($"Unknown Filename - {filename}");
                }
                return string.Empty;
            }
        }

        public struct ListFile
        {
            public Dictionary<uint, string> TexturePath;
        }
    }
}
