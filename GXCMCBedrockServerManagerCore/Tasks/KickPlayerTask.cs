
namespace GXCMCBedrockServerManagerCore.Tasks
{
    class KickPlayerTask : ServerControlTask
    {
        public ServerPlayers.Player PlayerToKick { get; set; }
        public string Reason { get; set; }

        public void OnFinished(ServerInstance server)
        {
        }

        public TaskCompletionState OnStarted(ServerInstance server)
        {
            return TaskCompletionState.NotCompleted;
        }

        public TaskCompletionState OnUpdate(ServerInstance server)
        {
            server.Players.KickPlayer(PlayerToKick, Reason);

            return TaskCompletionState.CompletedSuccess;
        }
    }
}
