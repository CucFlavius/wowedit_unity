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
    public class ListfileLoader
    {
        private string ListFile = "listfile.csv";
        public Dictionary<string, uint> ListFileEntry;

        /// <summary>
        /// Loads listfile.csv and adds all of the entries into a Dictionary.
        /// </summary>
        public void LoadListfile()
        {
            ListFileEntry = new Dictionary<string, uint>();

            using (var sr = new StreamReader(ListFile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    string[] values = line.Split(';');

                    ListFileEntry.Add(values[1], uint.Parse(values[0]));
                }
                Debug.Log($"ListFileLoader: Loaded {ListFileEntry.Count} file names...");
            }
        }
    }
}
