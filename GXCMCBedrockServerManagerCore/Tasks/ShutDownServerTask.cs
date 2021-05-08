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

        public void OnStarted(ServerInstance server)
        {
            QuitOutputHandler = new ServerInstance.OutputHandler()
            {
                RegexPattern = new Regex("Quit correctly", RegexOptions.Multiline),
                Callback = OutputHandler_ServerQuit
            };

            server.RegisterOutputHandler(QuitOutputHandler);

            server.RunCommand("STOP");
        }

        void OutputHandler_ServerQuit(Match match)
        {
            ShutdownComplete = true;
        }

        public bool OnUpdate(ServerInstance server)
        {
            return ShutdownComplete;
        }
    }
}
