using System;
using System.Collections.Generic;

namespace GXCMCBedrockServerManager.Server
{
    public class GlobalSettingsFile : Utils.JsonFile<GlobalSettingsFile>
    {
        static string SystemSettingsFileName { get { return "Settings"; } }

        public string ServerSettingsFileName { get; set; } = "GXCMCServerSettings";

        public List<string> ServerLocations { get; set; } = new List<string>();

        public static GlobalSettingsFile Load()
        {
            return LoadJsonFile("./", SystemSettingsFileName, true);
        }

        public bool Save()
        {
            return SaveJsonFile(this, "./", SystemSettingsFileName, true);
        }
    }
}
