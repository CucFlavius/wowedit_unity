using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.WoWEditSettings
{
    /// <summary>
    /// Handles .ini file section-data
    /// </summary>
    public class INISection
    {
        public string Name { get; set; }
        private Dictionary<string, string> keyValues = new Dictionary<string, string>();

        public void AddKeyValue(string key, string value) => keyValues.Add(key, value);
        public void SetValueOfKey(string key, string value) => keyValues[key] = value;

        public string GetString(string key)
        {
            foreach (var sub in keyValues)
            {
                if (sub.Key == key)
                    return sub.Value;
            }
            return null;
        }

        public bool GetBool(string key)
        {
            return (bool.Parse(GetString(key)));
        }

        public int GetInt(string key)
        {
            return (int.Parse(GetString(key)));
        }

        public Dictionary<string, string> GetKeyValues()
        {
            return keyValues;
        }
    }
}
