using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GXCMCBedrockServerManagerCore.Files;

namespace GXCMCBedrockServerManagerCore
{
    public class ServerManager
    {
        public GlobalSettingsFile GlobalSettings { get; private set; } = null;

        public List<ServerInstance> Instances { get; private set; } = new List<ServerInstance>();
        object InstanceLock = new object();

        bool bShuttingDown = false;
        Thread UpdateThread = null;

        public bool Initialise()
        {
            GlobalSettings = GlobalSettingsFile.Load();

            if(GlobalSettings == null)
            {
                return false;
            }

            foreach(string path in GlobalSettings.ServerLocations)
            {
                RegisterExistingServer(path);
            }

            // Setup and launch the update thread
            ThreadStart updateThreadStart = UpdateThreadLoop;
            UpdateThread = new Thread(updateThreadStart);
            UpdateThread.Start();

            return true;
        }

        void UpdateThreadLoop()
        {
            Stopwatch timer = new Stopwatch();

            timer.Start();

            while (!bShuttingDown)
            {
                TimeSpan currentTime = timer.Elapsed;

                lock(InstanceLock)
                {
                    foreach(ServerInstance inst in Instances)
                    {
                        inst.Update();
                    }
                }

                double sleepTime = GlobalSettings.ServerUpdateTickRate - (double)(((timer.Elapsed - currentTime).Milliseconds) * 0.001);

                if(sleepTime > 0)
                {
                    Thread.Sleep((int)(sleepTime * 1000));
                }
            }

            timer.Stop();
        }

        public void Shutdown()
        {
            bShuttingDown = true;

            UpdateThread.Join();
            UpdateThread = null;
        }

        /*
         * Attempts to initialise and add a new server setup at the specified path
         * */
        public bool AddNewServer(string path)
        {
            // Attempt to load settings, this will create a default if this isn't an existing install being re-added
            // if that succeedes then register this path and load it via the existing server path

            ServerSettingsFile serverSettings = ServerSettingsFile.Load(path, GlobalSettings.ServerSettingsFileName);

            if(serverSettings != null)
            {
                GlobalSettings.ServerLocations.Add(path);

                if(GlobalSettings.Save())
                {
                    RegisterExistingServer(path);

                    return true;
                }
            }

            return false;
        }

        void RegisterExistingServer(string path)
        {
            ServerInstance server = new ServerInstance();
            Instances.Add(server);

            server.Initialise(path, this);
        }

        public void ShutdownAllServers()
        {
            foreach (ServerInstance inst in Instances)
            {
                inst.Stop();
            }
        }
    }
}
