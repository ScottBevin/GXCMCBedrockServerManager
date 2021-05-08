
namespace GXCMCBedrockServerManagerCore.Tasks
{
    public enum TaskCompletionState
    {
        NotCompleted,
        CompletedSuccess,
        CompletedFailure,
    }

    public interface ServerControlTask
    {
        /*
         * Called when this task is going to run and start calling update
         * */
        TaskCompletionState OnStarted(ServerInstance server);

        /**
         * Called once the task is running - Return true if finished, OnFinished() will be called.
         * */
        TaskCompletionState OnUpdate(ServerInstance server);

        /*
         * Called when the task has finished running
         * */
        void OnFinished(ServerInstance server);
    }
}
