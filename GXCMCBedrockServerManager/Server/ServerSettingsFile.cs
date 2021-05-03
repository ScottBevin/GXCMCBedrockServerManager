using System;

namespace GXCMCBedrockServerManager.Server
{
    public class ServerSettingsFile : Utils.JsonFile<ServerSettingsFile>
    {
        bool TestSetting { get; set; } = false;

        public static ServerSettingsFile Load(string path, string fileName)
        {
            return LoadJsonFile(path, fileName, true);
        }

        public bool Save(string path, string fileName)
        {
            return SaveJsonFile(this, path, fileName, true);
        }
    }
}
