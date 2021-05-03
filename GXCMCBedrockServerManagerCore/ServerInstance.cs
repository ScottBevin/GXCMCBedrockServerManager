using GXCMCBedrockServerManagerCore.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace GXCMCBedrockServerManagerCore
{
    public class ServerInstance
    {
        public enum ServerState
        {
            NotInitialised,
            InitialisationError,
            Idle,
            Starting,
            Running,
            Stopping,
        }

        string ServerPath { get; set; } = "";

        ServerManager ServerManager { get; set; } = null;

        public ServerSettingsFile ServerSettings { get; private set; } = null;

        public ServerPropertiesFile ServerProperties { get; private set; } = null;

        public ServerState State { get; private set; } = ServerState.NotInitialised;

        Process RunningServerProcess = null;

        public Logger Log { get; private set; } = new Logger();

        public void Initialise(string path, ServerManager serverMgr)
        {
            ServerPath = path;
            ServerManager = serverMgr;

            ServerSettings = ServerSettingsFile.Load(path, serverMgr.GlobalSettings.ServerSettingsFileName);
            ServerProperties = ServerPropertiesFile.Load(path);

            if (ServerSettings == null || ServerProperties == null)
            {
                State = ServerState.InitialisationError;

                return;
            }

            string executableName = Path.Combine(ServerPath, serverMgr.GlobalSettings.ServerExecutableFileName);
            if(!File.Exists(executableName))
            {
                State = ServerState.InitialisationError;

                return;
            }

            State = ServerState.Idle;
        }

        public bool Start()
        {
            Log.BeginStreamingToFile(ServerPath);

            if (State != ServerState.Idle)
            {
                Log.LogError($"Server start requested when not idle, Current State = {State}");

                return false;
            }

            Log.LogChapterHeader($"Starting Server - {DateTime.Now}");
            Log.LogSectionHeader("Initialising");

            string executableName = Path.Combine(ServerPath, ServerManager.GlobalSettings.ServerExecutableFileName);
            if (!File.Exists(executableName))
            {
                Log.LogError($"Cannot start server - {ServerManager.GlobalSettings.ServerExecutableFileName} is missing.");

                return false;
            }

            RunningServerProcess = new Process();

            RunningServerProcess.StartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = !ServerSettings.ShowOutputConsoleWindow,

                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,

                FileName = executableName
            };

            RunningServerProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            RunningServerProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

            RunningServerProcess.Start();

            RunningServerProcess.BeginOutputReadLine();
            RunningServerProcess.BeginErrorReadLine();

            State = ServerState.Running;
            return true;
        }

        public bool Stop()
        {
            if (State != ServerState.Running)
            {
                Log.LogError($"Server stop requested when not running, Current State = {State}");

                return false;
            }

            if (RunningServerProcess == null)
            {
                Log.LogError($"Server stop requested while running with no process.");

                State = ServerState.Idle;
                return true;
            }

            RunningServerProcess.StandardInput.WriteLine("STOP");

            State = ServerState.Idle;
            return true;
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Log.LogInfo(outLine.Data);
        }

        void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Log.LogError(outLine.Data);
        }
    }
}
