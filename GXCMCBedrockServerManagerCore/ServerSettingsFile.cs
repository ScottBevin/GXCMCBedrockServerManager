using System;

namespace GXCMCBedrockServerManagerCore
{
    public class ServerSettingsFile : Utils.JsonFile<ServerSettingsFile>
    {
        public bool ShowOutputConsoleWindow { get; set; } = false;

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
