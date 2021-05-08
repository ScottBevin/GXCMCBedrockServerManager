
namespace GXCMCBedrockServerManagerCore.Tasks
{
    public interface ServerControlTask
    {
        /*
         * Called when this task is going to run and start calling update
         * */
        void OnStarted(ServerInstance server);

        /**
         * Called once the task is running - Return true if finished, OnFinished() will be called.
         * */
        bool OnUpdate(ServerInstance server);

        /*
         * Called when the task has finished running
         * */
        void OnFinished(ServerInstance server);
    }
}
