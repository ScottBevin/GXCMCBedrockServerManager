using GXCMCBedrockServerManagerCore;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager.Forms
{
    public partial class ShutdownAppForm : Form
    {
        public ServerManager ServerManager { get; set; } = null;

        Thread WaitThread = null;

        public ShutdownAppForm()
        {
            InitializeComponent();
        }

        private void ClosingForm_Shown(object sender, EventArgs e)
        {
            WaitThread = new Thread(WaitForAllServersToClose);
            WaitThread.Start();

            ServerManager.ShutdownAllServers();
        }

        void WaitForAllServersToClose()
        {
            bool allClosed = false;

            while(!allClosed)
            {
                allClosed = true;

                foreach(ServerInstance inst in ServerManager.Instances)
                {
                    if(inst.State != ServerInstance.ServerState.NotInitialised &&
                        inst.State != ServerInstance.ServerState.Idle &&
                        inst.State != ServerInstance.ServerState.InitialisationError)
                    {
                        allClosed = false;
                    }
                }
            }

            BeginInvoke(new Action(ShutdownApp));
        }

        void ShutdownApp()
        {
            Application.Exit();
        }
    }
}
