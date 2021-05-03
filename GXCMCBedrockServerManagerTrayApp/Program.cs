using GXCMCBedrockServerManager.Forms;
using GXCMCBedrockServerManager.Properties;
using GXCMCBedrockServerManagerCore;
using GXCMCBedrockServerManagerCore.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        ServerManager ServerManager = new ServerManager(); 

        public TrayAppContext()
        {
            ErrorHandler.OnErrorMessage = OnErrorMessage;
            ErrorHandler.OnErrorRequestHandle = OnErrorRequestHandle;

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

                    MenuItem newItem = new MenuItem($"{name} ({server.State})", ShowServerInstanceOverview);
                    newItem.Tag = server;

                    contextMenuItems.Add(newItem);
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

        void OnErrorMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }

        bool OnErrorRequestHandle(string title, string message)
        {
            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }

            return false;
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

            ShutdownAppForm newform = new ShutdownAppForm();
            newform.ServerManager = ServerManager;
            newform.Show();
        }

        void ShowServerInstanceOverview(object sender, EventArgs e)
        {
            ServerInstanceOverviewForm newform = new ServerInstanceOverviewForm();
            newform.ServerInstance = (sender as MenuItem).Tag as ServerInstance;
            newform.Show();
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
