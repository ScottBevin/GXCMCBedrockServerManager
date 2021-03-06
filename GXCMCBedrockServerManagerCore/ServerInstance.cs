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

        public ServerTaskController TaskController { get; private set; } = new ServerTaskController();

        public ServerPlayers Players { get; private set; } = new ServerPlayers();

        public ServerBackups Backups { get; private set; } = new ServerBackups();

        ServerManager ServerManager { get; set; } = null;
        public class OutputHandler
        {
            public delegate void CallbackDelegate(ServerInstance server, string output);

            public Regex RegexPattern { get; set; }
            public CallbackDelegate Callback { get; set; }
        }

        List<OutputHandler> OutputHandlers = new List<OutputHandler>();
        OutputHandler.CallbackDelegate RedirectNextOutputCallback = null;

        Process RunningServerProcess = null;

        ServerTaskController.TaskHandle StartupTaskHndl { get; set; } = null;
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

            if(!Backups.Initialise(this))
            {
                State = ServerState.InitialisationError;
                return;
            }

            State = ServerState.Idle;
        }

        public void Update()
        {
            TaskController.Update();
            Backups.Update();

            switch(State)
            {
                case ServerState.Starting:
                    {
                        if(StartupTaskHndl == null || StartupTaskHndl.CompletionState == TaskCompletionState.CompletedFailure)
                        {
                            State = ServerState.Idle;
                            RunningServerProcess.Kill();
                            RunningServerProcess = null;
                        }
                        else if(StartupTaskHndl.CompletionState == TaskCompletionState.CompletedSuccess)
                        {
                            State = ServerState.Running;
                            StartupTaskHndl = null;
                        }

                    } break;

                case ServerState.Stopping:
                    {
                        if (ShutDownTaskHndl == null || ShutDownTaskHndl.CompletionState != TaskCompletionState.NotCompleted)
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
            if (State == ServerState.Idle && RunningServerProcess == null && StartupTaskHndl == null)
            {
                StartupTaskHndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
                {
                    Task = new ServerStartupTask()
                });

                if (StartupTaskHndl == null)
                {
                    Log.LogWarning("Failed to queue server start task, aborting startup");
                    State = ServerState.Idle;

                    return false;
                }

                return true;
            }

            return false;
        }

        public bool Stop(int countdownTimer)
        {
            if (State != ServerState.Running)
            {
                Log.LogWarning($"Server stop requested when not running, Current State = {State}");

                return false;
            }

            if (ShutDownTaskHndl != null)
            {
                Log.LogWarning("Server shutdown already requested");

                return false;
            }

            if(countdownTimer > 0)
            {
                var hndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
                {
                    Task = new ActionCountdownTask()
                    {
                        CountdownSeconds = countdownTimer,
                        CountdownMessage = "Server shutdown in [TIME]"
                    },

                    OnSuccessTask = new ServerTaskController.TaskCreationParams()
                    {
                        Task = new RequestServerStopTask()
                    }
                });
            }
            else
            {
                ShutDownTaskHndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
                {
                    Task = new ShutDownServerTask()
                });

                if (ShutDownTaskHndl == null)
                {
                    Log.LogWarning("Failed to queue server shutdown task, aborting shutdown");
                }
            }

            return true;
        }

        public bool Restart(int countdownTimer)
        {
            if (State != ServerState.Running)
            {
                Log.LogWarning($"Server restart requested when not running, Current State = {State}");

                return false;
            }

            Log.LogInfo("Server restart requested.");

            if (ShutDownTaskHndl != null)
            {
                Log.LogWarning("Server shutdown already requested");

                return false;
            }

            if (countdownTimer > 0)
            {
                var hndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
                {
                    Task = new ActionCountdownTask()
                    {
                        CountdownSeconds = countdownTimer,
                        CountdownMessage = "Server restart in [TIME]"
                    },

                    OnSuccessTask = new ServerTaskController.TaskCreationParams()
                    {
                        Task = new RequestServerRestartTask()
                    }
                });
            }
            else
            {
                ShutDownTaskHndl = TaskController.QueueTask(new ServerTaskController.TaskCreationParams()
                {
                    Task = new ShutDownServerTask(),
                    OnSuccessTask = new ServerTaskController.TaskCreationParams()
                    {
                        Task = new RequestServerStartTask()
                    }
                });

                if (ShutDownTaskHndl == null)
                {
                    Log.LogWarning("Failed to queue server shutdown task, aborting restart");
                }
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
            OutputHandlers.Add(outputHndl);
        }

        public void UnregisterOutputHandler(OutputHandler outputHndl)
        {
            OutputHandlers.Remove(outputHndl);
        }

        public void RedirectNextOutput(OutputHandler.CallbackDelegate callback)
        {
            RedirectNextOutputCallback = callback;
        }

        #endregion

        internal bool Start_Internal()
        {
            if (State == ServerState.Idle && RunningServerProcess == null)
            {
                Log.BeginStreamingToFile(ServerPath);

                if (State != ServerState.Idle)
                {
                    Log.LogError($"Server start requested when not idle, Current State = {State}");

                    return false;
                }

                Log.LogChapterHeader($"Starting Server - {DateTime.Now}");
                Log.LogSectionHeader("Initialising");

                State = ServerState.Starting;

                RunningServerProcess = new Process();

                string executableName = Path.Combine(ServerPath, ServerManager.GlobalSettings.ServerExecutableFileName);
                if (!File.Exists(executableName))
                {
                    Log.LogError($"Cannot start server - {ServerManager.GlobalSettings.ServerExecutableFileName} is missing.");

                    return false;
                }

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

            return false;
        }

        internal bool Stop_Internal()
        {
            if (State != ServerState.Running)
            {
                return false;
            }

            Log.LogSectionHeader("Stopping");

            State = ServerState.Stopping;

            return true;
        }


        #region Output Handling

        void ProcessOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                if (RedirectNextOutputCallback != null)
                {
                    RedirectNextOutputCallback(this, outLine.Data);
                    RedirectNextOutputCallback = null;
                }
                else
                {
                    foreach (var item in OutputHandlers)
                    {
                        Match match = item.RegexPattern.Match(outLine.Data);

                        if (match.Success)
                        {
                            item.Callback(this, match.Value);
                        }
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

        #endregion
    }
}
