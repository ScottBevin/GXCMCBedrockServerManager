using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    public class SaveBackupTask : ServerControlTask
    {
        public enum SaveState
        {
            Beginning,
            WaitingOnSaveQuery,
            ProcessingSaveQuery,
            CopyingFiles,
            Resuming,
            Finished,
        }

        public string TargetDirectory { get; set; }
        public ServerBackups.BackupReceipt Receipt { get; set; }

        SaveState State = SaveState.Beginning;
        DateTime LastSaveQueryTime { get; set; } = new DateTime(0);
        
        ServerInstance.OutputHandler SaveHoldConfirmedOuputHandler { get; set; }
        ServerInstance.OutputHandler SaveQuerySuccessOutputHandler { get; set; }
        ServerInstance.OutputHandler SaveResumeConfirmedOuputHandler { get; set; }

        public void OnFinished(ServerInstance server)
        {
            server.UnregisterOutputHandler(SaveHoldConfirmedOuputHandler);
            server.UnregisterOutputHandler(SaveQuerySuccessOutputHandler);
            server.UnregisterOutputHandler(SaveResumeConfirmedOuputHandler);
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            SaveHoldConfirmedOuputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Saving...", RegexOptions.Multiline),
                Callback = OutputHandler_SaveHoldConfirmed
            };

            SaveQuerySuccessOutputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Data saved. Files are now ready to be copied.", RegexOptions.Multiline),
                Callback = OutputHandler_SaveQuerySuccess
            };

            SaveResumeConfirmedOuputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Changes to the level are resumed.", RegexOptions.Multiline),
                Callback = OutputHandler_SaveResumeConfirmed
            };

            server.RegisterOutputHandler(SaveHoldConfirmedOuputHandler);
            server.RegisterOutputHandler(SaveQuerySuccessOutputHandler);
            server.RegisterOutputHandler(SaveResumeConfirmedOuputHandler);

            server.SendServerMessage(server.ServerSettings.Backups.BackupBeginMessage);

            server.RunCommand("Save Hold");

            return TaskCompletionState.NotCompleted;
        }

        void CopyBackupFiles(ServerInstance server)
        {
            Directory.CreateDirectory(TargetDirectory);

            // Save the receipt now, all copied ok flags are false, this is so that an issue during the backup won't leave
            // us with a populated folder with no receipt, we will save it again later with full information
            Receipt.Save(TargetDirectory);

            for (int i = 0; i < Receipt.Files.Count; ++i)
            {
                var file = Receipt.Files[i];

                string fullSourcePath = Path.Combine(
                    server.ServerPath,
                    "worlds",
                    file.Path);

                string fullDestPath = Path.Combine(
                    TargetDirectory,
                    file.Path);

                if(Receipt.IsCompressed)
                {
                    fullDestPath = Path.Combine(
                    TargetDirectory,
                    "ToCompress",
                    file.Path);
                }

                try
                {
                    FileStream fileStream = File.Open(fullSourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    if(fileStream.Length < file.Size)
                    {
                        server.Log.LogWarning($"Backup file is too small: {fullSourcePath} ({fileStream.Length} vs {file.Size}");
                        continue;
                    }

                    Byte[] bytes = new Byte[file.Size];
                    int bytesRead = fileStream.Read(bytes, 0, file.Size);

                    if (bytesRead != file.Size)
                    {
                        server.Log.LogWarning($"Failed to read file that requires backup: {fullSourcePath}");
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(fullDestPath));
                    File.WriteAllBytes(fullDestPath, bytes);
                    server.Log.LogInfo($"Copied {fullSourcePath} to {fullDestPath}, {bytes.Length} bytes.");

                    file.CopiedOK = true;
                }
                catch(Exception e)
                {
                    server.Log.LogWarning($"Error copying backup file: {file.Path}, error = {e.Message}");
                }
            }

            if(Receipt.IsCompressed)
            {
                string dirToCompress = Path.Combine(
                    TargetDirectory,
                    "ToCompress");

                string compressedPath = Path.Combine(
                    TargetDirectory,
                    "Backup.zip");

                using (var archive = ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(dirToCompress);
                    archive.SaveTo(compressedPath, CompressionType.Deflate);
                }

                Directory.Delete(dirToCompress, true);
            }

            Receipt.Save(TargetDirectory);
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            switch(State)
            {
                case SaveState.WaitingOnSaveQuery:
                    {
                        if((DateTime.Now - LastSaveQueryTime).TotalSeconds > 5)
                        {
                            LastSaveQueryTime = DateTime.Now;

                            server.RunCommand("Save Query");
                        }
                    } break;

                case SaveState.CopyingFiles:
                    {
                        CopyBackupFiles(server);

                        State = SaveState.Resuming;
                        server.RunCommand("Save Resume");
                    } break;

            }

            if(State == SaveState.Finished)
            {
                server.SendServerMessage(server.ServerSettings.Backups.BackupEndMessage);

                return TaskCompletionState.CompletedSuccess;
            }

            return TaskCompletionState.NotCompleted;
        }

        void OutputHandler_SaveHoldConfirmed(ServerInstance server, string output)
        {
            State = SaveState.WaitingOnSaveQuery;
        }

        void OutputHandler_SaveQuerySuccess(ServerInstance server, string output)
        {
            if (State == SaveState.WaitingOnSaveQuery)
            {
                State = SaveState.ProcessingSaveQuery;

                server.RedirectNextOutput(OutputHandler_SaveFileList);
            }
        }

        void OutputHandler_SaveFileList(ServerInstance server, string output)
        {
            string[] entries = output.Split(',');

            foreach(string entry in entries)
            {
                string[] items = entry.Trim().Split(':');

                if(items.Length == 2)
                {
                    Receipt.Files.Add(new ServerBackups.BackupReceipt.BackupFile()
                    {
                        Path = items[0],
                        Size = int.Parse(items[1])
                    });
                }
            }

            State = SaveState.CopyingFiles;
        }

        void OutputHandler_SaveResumeConfirmed(ServerInstance server, string output)
        {
            State = SaveState.Finished;
        }
    }
}
