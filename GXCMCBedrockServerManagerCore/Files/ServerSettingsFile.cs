using System;

namespace GXCMCBedrockServerManagerCore.Files
{
    public class ServerSettingsFile : Utils.JsonFile<ServerSettingsFile>
    {
        public bool ShowOutputConsoleWindow { get; set; } = false;
        public ServerPlayers.ServerPlayersSettings Players { get; set; } = new ServerPlayers.ServerPlayersSettings();
        public ServerBackups.ServerBackupSettings Backups { get; set; } = new ServerBackups.ServerBackupSettings();

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
