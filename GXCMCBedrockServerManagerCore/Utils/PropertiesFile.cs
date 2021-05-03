using System;
using System.Collections.Generic;
using System.IO;

namespace GXCMCBedrockServerManagerCore.Utils
{
    class PropertiesFile
    {
        Dictionary<string, string> Entries = new Dictionary<string, string>();

        public bool TryGetValue<T>(string key, ref T value)
        {
            string valueString = "";
            if(Entries.TryGetValue(key, out valueString))
            {
                try
                {
                    if (typeof(T).IsEnum)
                    {
                        value = (T)Enum.Parse(typeof(T), valueString);
                    }
                    else
                    {
                        value = (T)Convert.ChangeType(valueString, typeof(T));
                    }

                    return true;
                }
                catch
                {

                }
            }

            return false;
        }

        public bool Contains(string key)
        {
            return Entries.ContainsKey(key);
        }

        public static PropertiesFile Readfile(string path)
        {
            try
            {
                PropertiesFile file = new PropertiesFile();

                string[] contents = File.ReadAllLines(path);

                foreach(string line in contents)
                {
                    if(line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] splitString = line.Split('=');

                    string key = splitString[0];
                    string value = splitString.Length > 1 ? splitString[1] : "";

                    file.Entries.Add(key, value);
                }

                return file;
            }
            catch 
            {
                return null;   
            }
        }
    }
}
