using GXCMCBedrockServerManagerCore;
using GXCMCBedrockServerManagerCore.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager.Forms
{
    public partial class ServerInstanceOverviewForm : Form
    {
        public ServerInstance ServerInstance { get; set; } = null;

        object OutputSync = new object();
        List<string> OutputLines = new List<string>();

        public ServerInstanceOverviewForm()
        {
            InitializeComponent();
        }

        private void ServerInstanceOverview_Shown(object sender, EventArgs e)
        {
            if(ServerInstance != null)
            {
                foreach (Logger.LogEntry entry in ServerInstance.Log.LocalHistory)
                {
                    OutputLines.Add(FormatLogEntryForOutput(entry));
                }

                ServerInstance.Log.OnNewLogEntry += OnNewLogEntry;
            }
        }

        private void ServerInstanceOverview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerInstance != null)
            {
                ServerInstance.Log.OnNewLogEntry -= OnNewLogEntry;
            }
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            ServerInstance.Start();
        }

        private void StopServerButton_Click(object sender, EventArgs e)
        {
            ServerInstance.Stop();
        }

        void OnNewLogEntry(Logger.LogEntry entry)
        {
            lock (OutputSync)
            {
                OutputLines.Add(FormatLogEntryForOutput(entry));
            }

            BeginInvoke(new Action(UpdateOutput));
        }

        string FormatLogEntryForOutput(Logger.LogEntry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Tag))
            {
                return $"{entry.TimeStamp.ToString()} {entry.Message}\r\n";
            }
            else
            {
                return $"{entry.TimeStamp.ToString()} [{entry.Tag}] - {entry.Message}\r\n";
            }
        }

        void UpdateOutput()
        {
            OutputTextBox.Text = "";

            lock (OutputSync)
            {
                foreach (string line in OutputLines)
                {
                    OutputTextBox.AppendText(line);
                }
            }
        }
    }
}
