using GXCMCBedrockServerManagerCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
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

        delegate void RegexMatchCallbackDelegate(Match regexMatch);
        List<Tuple<Regex, RegexMatchCallbackDelegate>> OutputRegexCallbacks = new List<Tuple<Regex, RegexMatchCallbackDelegate>>();

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

            OutputRegexCallbacks.Add(new Tuple<Regex, RegexMatchCallbackDelegate>(new Regex("Server started", RegexOptions.Multiline), OutputHandler_ServerStarted));
            OutputRegexCallbacks.Add(new Tuple<Regex, RegexMatchCallbackDelegate>(new Regex("Quit correctly", RegexOptions.Multiline), OutputHandler_ServerQuit));

            State = ServerState.Idle;
        }

        #region Public Interface

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

            State = ServerState.Starting;

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

            return true;
        }

        public bool Stop()
        {
            // If we are starting up we need to block and wait...
            while(State == ServerState.Starting)
            {

            }

            if (State != ServerState.Running)
            {
                Log.LogWarning($"Server stop requested when not running, Current State = {State}");

                return false;
            }

            Log.LogSectionHeader("Stopping");

            State = ServerState.Stopping;

            if (RunningServerProcess == null)
            {
                Log.LogError($"Server stop requested while running with no process.");

                State = ServerState.Idle;
                return true;
            }

            RunningServerProcess.StandardInput.WriteLine("STOP");

            return true;
        }

        public void SendServerMessage(string message)
        {
            if(RunningServerProcess != null)
            {
                Log.LogInfo($"Sending server message: {message}");
                RunningServerProcess.StandardInput.WriteLine($"say {message}");
            }
        }

        #endregion

        #region Output Handling

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                foreach (var item in OutputRegexCallbacks)
                {
                    Match match = item.Item1.Match(outLine.Data);

                    if (match.Success)
                    {
                        item.Item2(match);
                    }
                }

                Log.LogInfo(outLine.Data);
            }
        }

        void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                Log.LogError(outLine.Data);
            }
        }

        void OutputHandler_ServerStarted(Match match)
        {
            if (State != ServerState.Starting)
            {
                Log.LogError($"Server start output found, but server not starting. Current State = {State}");
            }

            Log.LogSectionHeader("Running");

            State = ServerState.Running;
        }

        void OutputHandler_ServerQuit(Match match)
        {
            if (State != ServerState.Stopping)
            {
                Log.LogError($"Server stop output found, but server not stopping. Current State = {State}");
            }

            State = ServerState.Idle;
            RunningServerProcess = null;

            Log.LogSectionHeader("Stopped");
        }

        #endregion
    }
}
