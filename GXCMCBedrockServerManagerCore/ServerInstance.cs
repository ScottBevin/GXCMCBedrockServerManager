using GXCMCBedrockServerManagerCore.Utils;
using GXCMCBedrockServerManagerCore.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using GXCMCBedrockServerManagerCore.Tasks;

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

        public string ServerPath { get; private set; } = "";

        public ServerSettingsFile ServerSettings { get; private set; } = null;

        public ServerPropertiesFile ServerProperties { get; private set; } = null;

        public ServerState State { get; private set; } = ServerState.NotInitialised;

        public Logger Log { get; private set; } = new Logger();

        public class OutputHandler
        {
            public delegate void RegexMatchCallbackDelegate(Match regexMatch);

            public Regex RegexPattern { get; set; }
            public RegexMatchCallbackDelegate Callback { get; set; }
        }

        public ServerPlayers Players { get; private set; } = new ServerPlayers();

        ServerManager ServerManager { get; set; } = null;

        List<OutputHandler> OutputRegexCallbacks = new List<OutputHandler>();

        Process RunningServerProcess = null;

        ServerTaskController TaskController { get; set; } = new ServerTaskController();

        ServerTaskController.TaskHandle ShutDownTaskHndl { get; set; } = null;

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
            if (!File.Exists(executableName))
            {
                State = ServerState.InitialisationError;
                return;
            }

            if (!Players.Initialise(this))
            {
                State = ServerState.InitialisationError;
                return;
            }

            if(!TaskController.Initialise(this))
            {
                State = ServerState.InitialisationError;
                return;
            }

            OutputRegexCallbacks.Add(new OutputHandler() { RegexPattern = new Regex("Server started", RegexOptions.Multiline), Callback = OutputHandler_ServerStarted });
            OutputRegexCallbacks.Add(new OutputHandler() { RegexPattern = new Regex("Player connected.*$", RegexOptions.Multiline), Callback = OutputHandler_PlayerConnected });
            OutputRegexCallbacks.Add(new OutputHandler() { RegexPattern = new Regex("Player disconnected.*$", RegexOptions.Multiline), Callback = OutputHandler_PlayerDisconnected });

            State = ServerState.Idle;
        }

        public void Update()
        {
            TaskController.Update();

            switch(State)
            {
                case ServerState.Stopping:
                    {
                        if (ShutDownTaskHndl == null || ShutDownTaskHndl.IsComplete)
                        {
                            State = ServerState.Idle;
                            RunningServerProcess = null;
                            ShutDownTaskHndl = null;

                            Log.LogSectionHeader("Stopped");
                        }
                    } break;
            }
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

            RunningServerProcess.OutputDataReceived += new DataReceivedEventHandler(ProcessOutputHandler);
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

            ShutDownServerTask shutdownTask = new ShutDownServerTask();
            ShutDownTaskHndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
            {
                Task = shutdownTask
            });

            if (ShutDownTaskHndl == null)
            {
                Log.LogWarning("Failed to queue server shutdown task, aborting shutdown");
            }

            return true;
        }

        public void SendServerMessage(string message)
        {
            if(RunningServerProcess != null && State == ServerState.Running && !string.IsNullOrEmpty(message))
            {
                Log.LogInfo($"Sending server message: {message}");
                RunningServerProcess.StandardInput.WriteLine($"say {message}");
            }
        }

        public void RunCommand(string command)
        {
            if (RunningServerProcess != null && !string.IsNullOrEmpty(command))
            {
                Log.LogInfo($"Running Command: {command}");
                RunningServerProcess.StandardInput.WriteLine(command);
            }
        }

        public void RegisterOutputHandler(OutputHandler outputHndl)
        {
            OutputRegexCallbacks.Add(outputHndl);
        }

        public void UnregisterOutputHandler(OutputHandler outputHndl)
        {
            OutputRegexCallbacks.Remove(outputHndl);
        }

        #endregion

        #region Output Handling

        void ProcessOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                foreach (var item in OutputRegexCallbacks)
                {
                    Match match = item.RegexPattern.Match(outLine.Data);

                    if (match.Success)
                    {
                        item.Callback(match);
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
        void OutputHandler_PlayerConnected(Match match)
        {
            string s = match.ToString();

            int colonIdx = s.IndexOf(':') + 1;
            int commaIdx = s.IndexOf(',');

            s = s.Substring(colonIdx, commaIdx - colonIdx).Trim();

            Log.LogInfo($"Player Joined: {s}");

            ServerPlayers.Player player = Players.FindPlayerByName(s);

            if(player != null)
            {
                Players.NotifyPlayerJoinedServer(player);
            }
        }

        void OutputHandler_PlayerDisconnected(Match match)
        {
            string s = match.ToString();

            int colonIdx = s.IndexOf(':') + 1;
            int commaIdx = s.IndexOf(',');

            s = s.Substring(colonIdx, commaIdx - colonIdx).Trim();

            Log.LogInfo($"Player Left: {s}");

            ServerPlayers.Player player = Players.FindPlayerByName(s);

            if (player != null)
            {
                Players.NotifyPlayerLeftServer(player);
            }
        }

        #endregion
    }
}
