using GXCMCBedrockServerManagerCore.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore
{
    public class ServerBackups
    {
        public class ServerBackupSettings
        {
            public string BackupBeginMessage { get; set; } = "World backup starting...";
            public string BackupEndMessage { get; set; } = "World backup competed.";
            public string BackupsPath { get; set; } = "[SERVER_PATH]\\WorldBackups";
            public bool CompressBackups { get; set; } = true;
            public int BackupsToKeep { get; set; } = 10;
            public bool BackupWorldOnAllPlayersLeave { get; set; } = true;
            public int MinutesBetweenAutoBackups { get; set; } = 60 * 4;
            public bool OnlyRunScheduledBackupsWhenSomeoneHasBeenOnline { get; set; } = true;
        }

        public class BackupReceipt : Utils.JsonFile<BackupReceipt>
        {
            public class BackupFile
            {
                public string Path { get; set; }
                public int Size { get; set; }
                public bool CopiedOK { get; set; } = false;
            }

            public string ServerName { get; set; }
            public string LevelName { get; set; }
            public DateTime BackupTime { get; set; }
            public bool IsCompressed { get; set; }
            public List<BackupFile> Files { get; set; } = new List<BackupFile>();

            static string FileName { get; } = "BackupReceipt";

            public static BackupReceipt Load(string path)
            {
                return LoadJsonFile(path, FileName, false);
            }

            public bool Save(string path)
            {
                return SaveJsonFile(this, path, FileName, false);
            }
        }

        ServerInstance Server { get; set; }

        public bool Initialise(ServerInstance parent)
        {
            Server = parent;

            return true;
        }

        public bool TriggerBackup()
        {
            DateTime timestamp = DateTime.Now;

            string timeStampString = $"{timestamp.ToShortDateString()}-{timestamp.ToShortTimeString()}".Replace("\\", "").Replace("/", "").Replace(":", "");

            string path = Path.Combine(
                Server.ServerSettings.Backups.BackupsPath.Replace("[SERVER_PATH]", Server.ServerPath),
                $"{Server.ServerProperties.ServerName}-{Server.ServerProperties.LevelName}-{timeStampString}");

            BackupReceipt receipt = new BackupReceipt()
            {
                ServerName = Server.ServerProperties.ServerName,
                LevelName = Server.ServerProperties.LevelName,
                BackupTime = timestamp,
                IsCompressed = Server.ServerSettings.Backups.CompressBackups,
            };

            var taskHandle = Server.TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
            {
                Task = new SaveBackupTask()
                {
                    TargetDirectory = path,
                    Receipt = receipt
                }
            }) ;

            return (taskHandle != null);
        }
    }
}
