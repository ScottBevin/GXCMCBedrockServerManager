using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Utils
{
    public class Logger
    {
        public struct LogEntry
        {
            public string Tag;
            public string Message;
            public DateTime TimeStamp;
        }

        public delegate void OnNewLogEntryDelegate(LogEntry entry);

        public static string LogFileName { get { return "ServerLog.txt"; } }

        public int LocalHistoryLinesKept { get; set; } = 1024;
        public List<LogEntry> LocalHistory { get; set; } = new List<LogEntry>();
        public OnNewLogEntryDelegate OnNewLogEntry { get; set; } = null;

        object OutputSyncGate = new object();

        public void BeginStreamingToFile(string path)
        {
        
        }

        public void StopStreamingToFile()
        {

        }

        public void LogChapterHeader(string msg)
        {
            PipeMessage("", "");
            PipeMessage("", "=============================================================================================================");
            PipeMessage("", msg);
            PipeMessage("", "=============================================================================================================");
            PipeMessage("", "");
        }

        public void LogSectionHeader(string msg)
        {
            PipeMessage("", "");
            PipeMessage("", $" -- [{msg}] --");
            PipeMessage("", "");
        }

        public void LogInfo(string msg)
        {
            PipeMessage("Info", msg);
        }

        public void LogWarning(string msg)
        {
            PipeMessage("Warning", msg);
        }

        public void LogError(string msg)
        {
            PipeMessage("Error", msg);

            ErrorHandler.RaiseErrorMessage("Error", msg);
        }

        void PipeMessage(string tag, string message)
        {
            LogEntry newEntry;
            newEntry.Tag = tag;
            newEntry.Message = message;
            newEntry.TimeStamp = DateTime.Now;

            lock (OutputSyncGate)
            {
                if (LocalHistory.Count > LocalHistoryLinesKept)
                {
                    LocalHistory.RemoveAt(0);
                }

                LocalHistory.Add(newEntry);

                if (OnNewLogEntry != null)
                {
                    OnNewLogEntry(newEntry);
                }
            }
        }
    }
}
