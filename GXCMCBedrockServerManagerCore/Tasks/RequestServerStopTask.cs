using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    class RequestServerStopTask : ServerControlTask
    {
        public void OnFinished(ServerInstance server)
        {
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            return TaskCompletionState.NotCompleted;    
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            // todo, we could add retry/timeout here.
            if(server.Stop(0))
            {
                return TaskCompletionState.CompletedSuccess;
            }

            return TaskCompletionState.CompletedFailure;
        }
    }
}
