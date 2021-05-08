using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    class StartServerTask : ServerControlTask
    {
        ServerInstance.OutputHandler StartupOutputHandler { get; set; }

        bool StartupComplete { get; set; } = false;

        public void OnFinished(ServerInstance server)
        {
            server.UnregisterOutputHandler(StartupOutputHandler);
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            if(!server.Start_Internal())
            {
                return TaskCompletionState.CompletedFailure;
            }

            StartupOutputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Server started", RegexOptions.Multiline),
                Callback = OutputHandler_ServerStarted
            };

            server.RegisterOutputHandler(StartupOutputHandler);

            return TaskCompletionState.NotCompleted;
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            if(StartupComplete)
            {
                return TaskCompletionState.CompletedSuccess;
            }

            return TaskCompletionState.NotCompleted;
        }

        void OutputHandler_ServerStarted(Match match)
        {
            StartupComplete = true;
        }
    }
}
