using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    public class ServerTaskController
    {
        ServerInstance Server { get; set; }

        public class TaskCreationParams
        {
            public ServerControlTask Task { get; set; } = null;
            public TaskCreationParams OnSuccessTask { get; set; } = null;
            public TaskCreationParams OnFailTask { get; set; } = null;
            public DateTime TaskScheduleTime { get; set; } = new DateTime(0);
        }

        public class TaskHandle
        {
            public TaskCreationParams Params { get; internal set; }
            public bool IsRunning { get; internal set; } = false;
            public TaskCompletionState CompletionState { get; internal set; } = TaskCompletionState.NotCompleted;
        }

        List<TaskHandle> PendingTasks { get; set; } = new List<TaskHandle>();
        List<TaskHandle> TasksWaitingToRun { get; set; } = new List<TaskHandle>();
        TaskHandle ActiveTask { get; set; } = null;

        public bool AnyTasksRunning { get { return ActiveTask != null; } }

        object TasksLock = new object();

        public bool Initialise(ServerInstance parent)
        {
            Server = parent;

            return true;
        }

        public void Update()
        {
            lock (TasksLock)
            {
                List<TaskHandle> readyToStart = PendingTasks.FindAll(T => (T.Params.TaskScheduleTime <= DateTime.Now));
                TasksWaitingToRun.AddRange(readyToStart);
                PendingTasks.RemoveAll(T => readyToStart.Contains(T));

                if (ActiveTask == null && TasksWaitingToRun.Count > 0)
                {
                    ActiveTask = TasksWaitingToRun[0];
                    TasksWaitingToRun.RemoveAt(0);

                    ActiveTask.IsRunning = true;

                    ActiveTask.CompletionState = ActiveTask.Params.Task.OnStarted(Server);
                }
            }

            if(ActiveTask != null)
            {
                var result = ActiveTask.CompletionState == TaskCompletionState.NotCompleted ?
                    ActiveTask.Params.Task.OnUpdate(Server) :
                    ActiveTask.CompletionState;

                if (result != TaskCompletionState.NotCompleted)
                {
                    ActiveTask.Params.Task.OnFinished(Server);
                    ActiveTask.CompletionState = result;

                    ActiveTask.IsRunning = false;

                    if (ActiveTask.CompletionState == TaskCompletionState.CompletedSuccess)
                    {
                        if (ActiveTask.Params.OnSuccessTask != null)
                        {
                            QueueTask(ActiveTask.Params.OnSuccessTask);
                        }
                    }
                    else
                    {
                        if (ActiveTask.Params.OnFailTask != null)
                        {
                            QueueTask(ActiveTask.Params.OnFailTask);
                        }
                    }

                    ActiveTask = null;
                }
            }
        }

        public void CancelAllTasks()
        {
            lock(TasksLock)
            {
                PendingTasks.Clear();
                TasksWaitingToRun.Clear();
            }
        }

        public TaskHandle QueueTask(TaskCreationParams taskParams)
        {
            if (taskParams != null & taskParams.Task != null)
            {
                TaskHandle hndl = new TaskHandle()
                {
                    Params = taskParams
                };

                lock (TasksLock)
                {
                    if (taskParams.TaskScheduleTime == new DateTime(0))
                    {
                        TasksWaitingToRun.Add(hndl);
                    }
                    else
                    {
                        PendingTasks.Add(hndl);
                    }
                }

                return hndl;
            }

            return null;
        }
    }
}
