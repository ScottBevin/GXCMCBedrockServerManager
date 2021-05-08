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

                this.Text = ServerInstance.ServerProperties.ServerName;

                UpdatePlayersList();
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
            ServerInstance.Stop((int)StopTimerNumberInput.Value);
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

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(SendMessageTextBox.Text))
            {
                ServerInstance.SendServerMessage(SendMessageTextBox.Text);
                SendMessageTextBox.Text = "";
            }
        }

        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SendCommandTextBox.Text))
            {
                ServerInstance.RunCommand(SendCommandTextBox.Text);
                SendCommandTextBox.Text = "";
            }
        }

        private void UpdatePlayersList()
        {
            string selectedName = PlayersListBox.SelectedItem as string;

            PlayersListBox.Items.Clear();

            foreach(var player in ServerInstance.Players.Players)
            {
                if(string.IsNullOrEmpty(PlayerFilterTextBox.Text) || player.Name.Contains(PlayerFilterTextBox.Text))
                {
                    PlayersListBox.Items.Add(player.Name);
                }
            }

            if(!string.IsNullOrEmpty(selectedName))
            {
                int index = PlayersListBox.Items.IndexOf(selectedName);

                if(index >= 0)
                {
                    PlayersListBox.SelectedIndex = index;
                }
                else
                {
                    PlayersListBox.SelectedIndex = -1;
                }
            }
        }

        private void AddNewPlayerButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AddNewPlayerTextBox.Text))
            {
                ServerInstance.Players.FindOrRegisterNewPlayer(AddNewPlayerTextBox.Text);
                AddNewPlayerTextBox.Text = "";

                UpdatePlayersList();
            }
        }

        private void PlayerFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdatePlayersList();
        }

        private void SavePlayersButton_Click(object sender, EventArgs e)
        {
            ServerInstance.Players.SaveAndNotifyServer();
        }

        private void RevertPlayersChangesButton_Click(object sender, EventArgs e)
        {
            ServerInstance.Players.Reload();

            UpdatePlayersList();
        }

        void UpdateSelectedPlayerInfo()
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                PlayerNameTextBox.Text = selected.Name;
                PlayerXUIDTextBox.Text = selected.XUID;
                FirstJoinedTextBos.Text = selected.FirstJoined.ToString();
                LastJoinedTextBox.Text = selected.LastJoined.ToString();
                IsWhiteListedTextBox.Text = selected.IsWhitelisted ? "Yes" : "No";
                IsBannedTextBox.Text = selected.IsBanned ? "Yes" : "No";
                IsOnlineTextBox.Text = selected.IsOnline ? "Yes" : "No";
                PlayerPermissionsTextBox.Text = ServerEnumHelpers.ServerPermissionToString(selected.Permissions);
            }
        }

        private void PlayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedPlayerInfo();
        }

        private void AddPlayerToWhitelistButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                selected.IsWhitelisted = true;

                ServerInstance.Players.SaveAndNotifyServer();

                UpdateSelectedPlayerInfo();
            }
        }

        private void RemovePlayerFromWhitelistButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                selected.IsWhitelisted = false;

                ServerInstance.Players.SaveAndNotifyServer();

                UpdateSelectedPlayerInfo();
            }
        }

        private void BanPlayerButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                ServerInstance.Players.BanPlayer(selected);

                UpdateSelectedPlayerInfo();
            }
        }

        private void UnbanPlayerButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                selected.IsBanned = false;

                ServerInstance.Players.SaveAndNotifyServer();

                UpdateSelectedPlayerInfo();
            }
        }

        private void PromotePlayerButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                ServerPermissions current = selected.Permissions;

                if(current == ServerPermissions.Default)
                {
                    current = ServerInstance.ServerProperties.DefaultPlayerPermissionLevel;
                }

                if(current == ServerPermissions.Visitor)
                {
                    selected.Permissions = ServerPermissions.Member;
                }
                else if (current == ServerPermissions.Member)
                {
                    selected.Permissions = ServerPermissions.Operator;
                }

                ServerInstance.Players.SaveAndNotifyServer();

                UpdateSelectedPlayerInfo();
            }
        }

        private void DemotePlayerButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                ServerPermissions current = selected.Permissions;

                if (current == ServerPermissions.Default)
                {
                    current = ServerInstance.ServerProperties.DefaultPlayerPermissionLevel;
                }

                if (current == ServerPermissions.Operator)
                {
                    selected.Permissions = ServerPermissions.Member;
                }
                else if (current == ServerPermissions.Member)
                {
                    selected.Permissions = ServerPermissions.Visitor;
                }

                ServerInstance.Players.SaveAndNotifyServer();

                UpdateSelectedPlayerInfo();
            }
        }

        private void KickPlayerButton_Click(object sender, EventArgs e)
        {
            ServerPlayers.Player selected = ServerInstance.Players.FindPlayerByName(PlayersListBox.SelectedItem as string);

            if (selected != null)
            {
                ServerInstance.Players.KickPlayer(selected, KickMessageTextBox.Text);

                UpdateSelectedPlayerInfo();
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ServerInstance.Restart((int)RestartTimerNumberInput.Value);
        }
    }
}
