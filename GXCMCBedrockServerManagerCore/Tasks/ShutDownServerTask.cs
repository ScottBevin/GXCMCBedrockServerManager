using System;
using System.Text.RegularExpressions;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    public class ShutDownServerTask : ServerControlTask
    {
        ServerInstance.OutputHandler QuitOutputHandler { get; set; }

        bool ShutdownComplete { get; set; } = false;

        public void OnFinished(ServerInstance server)
        {
            server.UnregisterOutputHandler(QuitOutputHandler);
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            if(!server.Stop_Internal())
            {
                return TaskCompletionState.CompletedFailure;
            }

            QuitOutputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Quit correctly", RegexOptions.Multiline),
                Callback = OutputHandler_ServerQuit
            };

            server.RegisterOutputHandler(QuitOutputHandler);

            server.RunCommand("STOP");

            return TaskCompletionState.NotCompleted;
        }

        void OutputHandler_ServerQuit(Match match)
        {
            ShutdownComplete = true;
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            if (ShutdownComplete)
            {
                return TaskCompletionState.CompletedSuccess;
            }

            return TaskCompletionState.NotCompleted;
        }
    }
}
