using System;
using System.Collections.Generic;

namespace GXCMCBedrockServerManager.Server
{
    public class ServerInstance
    {
        public enum ServerState
        {
            NotInitialised,
            InitialisationError,
            Idle,
            Running,
        }

        string ServerPath { get; set; } = "";

        public ServerSettingsFile ServerSettings { get; private set; } = null;

        public ServerPropertiesFile ServerProperties { get; private set; } = null;

        public ServerState State { get; private set; } = ServerState.NotInitialised;

        public void Initialise(string path, ServerManager serverMgr)
        {
            ServerPath = path;
            ServerSettings = ServerSettingsFile.Load(path, serverMgr.GlobalSettings.ServerSettingsFileName);
            ServerProperties = ServerPropertiesFile.Load(path);

            if (ServerSettings == null || ServerProperties == null)
            {
                State = ServerState.InitialisationError;

                return;
            }

            State = ServerState.Idle;
        }
    }
}
