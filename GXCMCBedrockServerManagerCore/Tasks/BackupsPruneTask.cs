using System;
using System.Collections.Generic;
using System.IO;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    class BackupsPruneTask : ServerControlTask
    {
        public void OnFinished(ServerInstance server)
        {
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            List<ServerBackups.BackupReceipt> receipts = server.Backups.LoadAllBackupReceipts();

            if(receipts.Count > server.ServerSettings.Backups.BackupsToKeep)
            {
                receipts.Sort((L, R) => (L.BackupTime > R.BackupTime) ? 1 : -1);

                while(receipts.Count > server.ServerSettings.Backups.BackupsToKeep)
                {
                    server.Backups.DestroyBackup(receipts[0]);
                    receipts.RemoveAt(0);
                }
            }

            return TaskCompletionState.CompletedSuccess;
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            return TaskCompletionState.CompletedSuccess;
        }
    }
}
