using GXCMCBedrockServerManager.Forms;
using GXCMCBedrockServerManager.Properties;
using System;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public TrayAppContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Add New Server", AddNewServer),
                    new MenuItem("Exit", Exit)
            }),
                Visible = true
            };
        }

        void AddNewServer(object sender, EventArgs e)
        {
            AddNewServerForm newform = new AddNewServerForm();
            DialogResult result = newform.ShowDialog();

            int test = 1;
            ++test;
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

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
