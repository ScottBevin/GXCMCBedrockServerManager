using System.Collections.Generic;

namespace GXCMCBedrockServerManager.Server
{
    public class ServerManager
    {
        public GlobalSettingsFile GlobalSettings { get; private set; } = null;

        public List<ServerInstance> Instances { get; private set; } = new List<ServerInstance>();

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

            return true;
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
    }
}
