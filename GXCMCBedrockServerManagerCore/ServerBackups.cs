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
            public double MinutesBetweenAutoBackups { get; set; } = 60.0 * 4.0;
            public double BackupCooldownMinutes { get; set; } = 60.0;
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

            public string FullPath { get; private set; }

            static string FileName { get; } = "BackupReceipt";

            public static BackupReceipt Load(string path)
            {
                BackupReceipt br = LoadJsonFile(path, FileName, false);
                br.FullPath = Path.Combine(path, FileName);
                return br;
            }

            public bool Save(string path)
            {
                FullPath = Path.Combine(path, FileName);
                return SaveJsonFile(this, path, FileName, false);
            }
        }

        ServerInstance Server { get; set; }

        bool AnyPlayersOnlineSinceLastBackup { get; set; }
        DateTime LastBackupTime { get; set; } = DateTime.Now;

        object RequestLock { get; set; } = new object();

        public bool Initialise(ServerInstance parent)
        {
            Server = parent;

            Server.Players.OnPlayerJoined += OnPlayerJoined;
            Server.Players.OnPlayerLeft += OnPlayerLeft;

            return true;
        }

        public bool TriggerBackup()
        {
            lock (RequestLock)
            {
                DateTime timestamp = DateTime.Now;

                if ((timestamp - LastBackupTime).TotalMinutes >= Server.ServerSettings.Backups.BackupCooldownMinutes &&
                    AnyPlayersOnlineSinceLastBackup)
                {
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
                        },
                        OnSuccessTask = new ServerTaskController.TaskCreationParams()
                        {
                            Task = new BackupsPruneTask()
                        }
                    });

                    if (taskHandle != null)
                    {
                        AnyPlayersOnlineSinceLastBackup = (Server.Players.OnlinePlayers > 0);
                        LastBackupTime = timestamp;
                    }

                    return (taskHandle != null);
                }
            }

            return false;
        }
        
        public void Update()
        {
            if((DateTime.Now - LastBackupTime).TotalMinutes >= Server.ServerSettings.Backups.MinutesBetweenAutoBackups)
            {
                TriggerBackup();
                LastBackupTime = DateTime.Now;
            }
        }

        public List<BackupReceipt> LoadAllBackupReceipts()
        {
            List<BackupReceipt> returnList = new List<BackupReceipt>();

            DirectoryInfo dir = new DirectoryInfo(Server.ServerSettings.Backups.BackupsPath.Replace("[SERVER_PATH]", Server.ServerPath));

            foreach(var subdir in dir.GetDirectories())
            {
                BackupReceipt bp = BackupReceipt.Load(subdir.FullName);

                if(bp != null)
                {
                    returnList.Add(bp);
                }
            }

            return returnList;
        }

        public void DestroyBackup(BackupReceipt receipt)
        {
            string dirToDelete = Path.GetDirectoryName(receipt.FullPath);
            Directory.Delete(Path.GetDirectoryName(receipt.FullPath), true);
        }

        void OnPlayerJoined(ServerPlayers.Player player)
        {
            AnyPlayersOnlineSinceLastBackup = true;
        }

        void OnPlayerLeft(ServerPlayers.Player player)
        {
            if(Server.ServerSettings.Backups.BackupWorldOnAllPlayersLeave &&
                Server.Players.OnlinePlayers == 0)
            {
                TriggerBackup();
            }
        }
    }
}
