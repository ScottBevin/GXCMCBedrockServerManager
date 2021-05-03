using GXCMCBedrockServerManager.Forms;
using GXCMCBedrockServerManager.Properties;
using GXCMCBedrockServerManager.Server;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        ServerManager ServerManager = new Server.ServerManager(); 

        public TrayAppContext()
        {
            if (ServerManager.Initialise() == false)
            {
                MessageBox.Show("Failed to initialise server manager");

                Application.Exit();
            }
            else
            {
                List<MenuItem> contextMenuItems = new List<MenuItem>();

                contextMenuItems.Add(new MenuItem("Add New Server", AddNewServer));
                contextMenuItems.Add(new MenuItem("-"));

                foreach(ServerInstance server in ServerManager.Instances)
                {
                    string name = server.ServerProperties != null ? server.ServerProperties.ServerName : "Unknown Server";

                    contextMenuItems.Add(new MenuItem($"{name} ({server.State})"));
                }

                contextMenuItems.Add(new MenuItem("-"));
                contextMenuItems.Add(new MenuItem("Exit", Exit));

                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.AppIcon,
                    ContextMenu = new ContextMenu(contextMenuItems.ToArray()),
                    Visible = true
                };
            }
        }

        void AddNewServer(object sender, EventArgs e)
        {
            AddNewServerForm newform = new AddNewServerForm();
            newform.ServerManager = ServerManager;
            newform.Show();
        }

        void Exit(object sender, EventArgs e)
        {
            if(trayIcon != null)
            {
                trayIcon.Visible = false;
            }

            Application.Exit();
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new TrayAppContext());
        }
    }
}
