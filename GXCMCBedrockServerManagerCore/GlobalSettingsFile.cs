﻿using System;
using System.Collections.Generic;

namespace GXCMCBedrockServerManagerCore
{
    public class GlobalSettingsFile : Utils.JsonFile<GlobalSettingsFile>
    {
        static string SystemSettingsFileName { get { return "Settings"; } }

        public string ServerSettingsFileName { get; set; } = "GXCMCServerSettings";
        public string ServerExecutableFileName { get; set; } = "bedrock_server.exe";


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
