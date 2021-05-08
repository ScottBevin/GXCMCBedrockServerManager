using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Tasks
{
    class ServerTaskController
    {
        ServerInstance Server { get; set; }

        public class TaskCreationParams
        {
            public ServerControlTask Task { get; set; } = null;
            public TaskCreationParams OnCompleteTask { get; set; } = null;
            public DateTime TaskScheduleTime { get; set; } = new DateTime(0);
        }

        public class TaskHandle
        {
            public TaskCreationParams Params { get; internal set; }
            public bool IsRunning { get; internal set; } = false;
            public bool IsComplete { get; internal set; } = false;
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

                    ActiveTask.Params.Task.OnStarted(Server);
                    ActiveTask.IsRunning = true;
                }
            }

            if(ActiveTask != null)
            {
                if(ActiveTask.Params.Task.OnUpdate(Server))
                {
                    ActiveTask.Params.Task.OnFinished(Server);
                    ActiveTask.IsRunning = false;
                    ActiveTask.IsComplete = true;

                    if(ActiveTask.Params.OnCompleteTask != null)
                    {
                        QueueTask(ActiveTask.Params.OnCompleteTask);
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

        public void QueueTask(TaskCreationParams taskParams)
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
            }
        }
    }
}
