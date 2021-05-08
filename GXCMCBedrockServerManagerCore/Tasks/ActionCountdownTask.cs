using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    class ActionCountdownTask : ServerControlTask
    {
        public int CountdownSeconds { get; set; } = 15;

        public string CountdownMessage { get; set; } = "Countdown: [TIME]";

        DateTime EndTime;

        public void OnFinished(ServerInstance server)
        {
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            if(CountdownSeconds <= 0)
            {
                return TaskCompletionState.CompletedSuccess;
            }

            EndTime = DateTime.Now + TimeSpan.FromSeconds((double)CountdownSeconds);

            return TaskCompletionState.NotCompleted;
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            if(CountdownSeconds <= 0)
            {
                return TaskCompletionState.CompletedSuccess;
            }

            int secondsUntilCompletion = Math.Abs((int)((EndTime - DateTime.Now).TotalSeconds));

            if(secondsUntilCompletion < CountdownSeconds)
            {
                server.SendServerMessage(CountdownMessage.Replace("[TIME]", secondsUntilCompletion.ToString()));
                CountdownSeconds = secondsUntilCompletion;
            }

            return TaskCompletionState.NotCompleted;
        }
    }
}
