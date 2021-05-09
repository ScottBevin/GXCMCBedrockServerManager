
namespace GXCMCBedrockServerManager.Forms
{
    partial class ServerInstanceOverviewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TriggerBackupButton = new System.Windows.Forms.Button();
            this.RestartTimerNumberInput = new System.Windows.Forms.NumericUpDown();
            this.StopTimerNumberInput = new System.Windows.Forms.NumericUpDown();
            this.RestartButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SendCommandButton = new System.Windows.Forms.Button();
            this.SendCommandTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.SendMessageTextBox = new System.Windows.Forms.TextBox();
            this.StopServerButton = new System.Windows.Forms.Button();
            this.StartServerButton = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.KickPlayerButton = new System.Windows.Forms.Button();
            this.KickMessageTextBox = new System.Windows.Forms.TextBox();
            this.PromotePlayerButton = new System.Windows.Forms.Button();
            this.DemotePlayerButton = new System.Windows.Forms.Button();
            this.BanPlayerButton = new System.Windows.Forms.Button();
            this.UnbanPlayerButton = new System.Windows.Forms.Button();
            this.AddPlayerToWhitelistButton = new System.Windows.Forms.Button();
            this.RemovePlayerFromWhitelistButton = new System.Windows.Forms.Button();
            this.PlayerPermissionsTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.IsOnlineTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.IsBannedTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.IsWhiteListedTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.LastJoinedTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.FirstJoinedTextBos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PlayerXUIDTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PlayerNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RevertPlayersChangesButton = new System.Windows.Forms.Button();
            this.SavePlayersButton = new System.Windows.Forms.Button();
            this.AddNewPlayerButton = new System.Windows.Forms.Button();
            this.AddNewPlayerTextBox = new System.Windows.Forms.TextBox();
            this.PlayerFilterTextBox = new System.Windows.Forms.TextBox();
            this.PlayersListBox = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RestartTimerNumberInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTimerNumberInput)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1631, 901);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TriggerBackupButton);
            this.tabPage1.Controls.Add(this.RestartTimerNumberInput);
            this.tabPage1.Controls.Add(this.StopTimerNumberInput);
            this.tabPage1.Controls.Add(this.RestartButton);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.SendCommandButton);
            this.tabPage1.Controls.Add(this.SendCommandTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.SendMessageButton);
            this.tabPage1.Controls.Add(this.SendMessageTextBox);
            this.tabPage1.Controls.Add(this.StopServerButton);
            this.tabPage1.Controls.Add(this.StartServerButton);
            this.tabPage1.Controls.Add(this.OutputTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1623, 875);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TriggerBackupButton
            // 
            this.TriggerBackupButton.Location = new System.Drawing.Point(654, 492);
            this.TriggerBackupButton.Name = "TriggerBackupButton";
            this.TriggerBackupButton.Size = new System.Drawing.Size(126, 23);
            this.TriggerBackupButton.TabIndex = 21;
            this.TriggerBackupButton.Text = "Trigger Backup";
            this.TriggerBackupButton.UseVisualStyleBackColor = true;
            this.TriggerBackupButton.Click += new System.EventHandler(this.TriggerBackupButton_Click);
            // 
            // RestartTimerNumberInput
            // 
            this.RestartTimerNumberInput.Location = new System.Drawing.Point(654, 411);
            this.RestartTimerNumberInput.Name = "RestartTimerNumberInput";
            this.RestartTimerNumberInput.Size = new System.Drawing.Size(46, 20);
            this.RestartTimerNumberInput.TabIndex = 20;
            // 
            // StopTimerNumberInput
            // 
            this.StopTimerNumberInput.Location = new System.Drawing.Point(521, 411);
            this.StopTimerNumberInput.Name = "StopTimerNumberInput";
            this.StopTimerNumberInput.Size = new System.Drawing.Size(46, 20);
            this.StopTimerNumberInput.TabIndex = 19;
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(706, 408);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(75, 23);
            this.RestartButton.TabIndex = 18;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 463);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Send Command";
            // 
            // SendCommandButton
            // 
            this.SendCommandButton.Location = new System.Drawing.Point(706, 463);
            this.SendCommandButton.Name = "SendCommandButton";
            this.SendCommandButton.Size = new System.Drawing.Size(75, 23);
            this.SendCommandButton.TabIndex = 16;
            this.SendCommandButton.Text = "Send";
            this.SendCommandButton.UseVisualStyleBackColor = true;
            this.SendCommandButton.Click += new System.EventHandler(this.SendCommandButton_Click);
            // 
            // SendCommandTextBox
            // 
            this.SendCommandTextBox.Location = new System.Drawing.Point(90, 463);
            this.SendCommandTextBox.Name = "SendCommandTextBox";
            this.SendCommandTextBox.Size = new System.Drawing.Size(610, 20);
            this.SendCommandTextBox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 437);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Send Message";
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(706, 437);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.SendMessageButton.TabIndex = 13;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // SendMessageTextBox
            // 
            this.SendMessageTextBox.Location = new System.Drawing.Point(90, 437);
            this.SendMessageTextBox.Name = "SendMessageTextBox";
            this.SendMessageTextBox.Size = new System.Drawing.Size(610, 20);
            this.SendMessageTextBox.TabIndex = 12;
            // 
            // StopServerButton
            // 
            this.StopServerButton.Location = new System.Drawing.Point(573, 408);
            this.StopServerButton.Name = "StopServerButton";
            this.StopServerButton.Size = new System.Drawing.Size(75, 23);
            this.StopServerButton.TabIndex = 11;
            this.StopServerButton.Text = "Stop";
            this.StopServerButton.UseVisualStyleBackColor = true;
            this.StopServerButton.Click += new System.EventHandler(this.StopServerButton_Click);
            // 
            // StartServerButton
            // 
            this.StartServerButton.Location = new System.Drawing.Point(440, 408);
            this.StartServerButton.Name = "StartServerButton";
            this.StartServerButton.Size = new System.Drawing.Size(75, 23);
            this.StartServerButton.TabIndex = 10;
            this.StartServerButton.Text = "Start";
            this.StartServerButton.UseVisualStyleBackColor = true;
            this.StartServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(6, 6);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(775, 394);
            this.OutputTextBox.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.KickPlayerButton);
            this.tabPage2.Controls.Add(this.KickMessageTextBox);
            this.tabPage2.Controls.Add(this.PromotePlayerButton);
            this.tabPage2.Controls.Add(this.DemotePlayerButton);
            this.tabPage2.Controls.Add(this.BanPlayerButton);
            this.tabPage2.Controls.Add(this.UnbanPlayerButton);
            this.tabPage2.Controls.Add(this.AddPlayerToWhitelistButton);
            this.tabPage2.Controls.Add(this.RemovePlayerFromWhitelistButton);
            this.tabPage2.Controls.Add(this.PlayerPermissionsTextBox);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.IsOnlineTextBox);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.IsBannedTextBox);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.IsWhiteListedTextBox);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.LastJoinedTextBox);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.FirstJoinedTextBos);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.PlayerXUIDTextBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.PlayerNameTextBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.PlayerFilterTextBox);
            this.tabPage2.Controls.Add(this.PlayersListBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1623, 875);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Players";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // KickPlayerButton
            // 
            this.KickPlayerButton.Location = new System.Drawing.Point(479, 370);
            this.KickPlayerButton.Name = "KickPlayerButton";
            this.KickPlayerButton.Size = new System.Drawing.Size(164, 23);
            this.KickPlayerButton.TabIndex = 27;
            this.KickPlayerButton.Text = "Kick Player";
            this.KickPlayerButton.UseVisualStyleBackColor = true;
            this.KickPlayerButton.Click += new System.EventHandler(this.KickPlayerButton_Click);
            // 
            // KickMessageTextBox
            // 
            this.KickMessageTextBox.Location = new System.Drawing.Point(294, 344);
            this.KickMessageTextBox.Name = "KickMessageTextBox";
            this.KickMessageTextBox.Size = new System.Drawing.Size(349, 20);
            this.KickMessageTextBox.TabIndex = 26;
            // 
            // PromotePlayerButton
            // 
            this.PromotePlayerButton.Location = new System.Drawing.Point(294, 314);
            this.PromotePlayerButton.Name = "PromotePlayerButton";
            this.PromotePlayerButton.Size = new System.Drawing.Size(167, 23);
            this.PromotePlayerButton.TabIndex = 25;
            this.PromotePlayerButton.Text = "Promote Player";
            this.PromotePlayerButton.UseVisualStyleBackColor = true;
            this.PromotePlayerButton.Click += new System.EventHandler(this.PromotePlayerButton_Click);
            // 
            // DemotePlayerButton
            // 
            this.DemotePlayerButton.Location = new System.Drawing.Point(479, 314);
            this.DemotePlayerButton.Name = "DemotePlayerButton";
            this.DemotePlayerButton.Size = new System.Drawing.Size(164, 23);
            this.DemotePlayerButton.TabIndex = 24;
            this.DemotePlayerButton.Text = "Demote Player";
            this.DemotePlayerButton.UseVisualStyleBackColor = true;
            this.DemotePlayerButton.Click += new System.EventHandler(this.DemotePlayerButton_Click);
            // 
            // BanPlayerButton
            // 
            this.BanPlayerButton.Location = new System.Drawing.Point(294, 285);
            this.BanPlayerButton.Name = "BanPlayerButton";
            this.BanPlayerButton.Size = new System.Drawing.Size(167, 23);
            this.BanPlayerButton.TabIndex = 23;
            this.BanPlayerButton.Text = "Ban Player";
            this.BanPlayerButton.UseVisualStyleBackColor = true;
            this.BanPlayerButton.Click += new System.EventHandler(this.BanPlayerButton_Click);
            // 
            // UnbanPlayerButton
            // 
            this.UnbanPlayerButton.Location = new System.Drawing.Point(479, 285);
            this.UnbanPlayerButton.Name = "UnbanPlayerButton";
            this.UnbanPlayerButton.Size = new System.Drawing.Size(164, 23);
            this.UnbanPlayerButton.TabIndex = 22;
            this.UnbanPlayerButton.Text = "Unban Player";
            this.UnbanPlayerButton.UseVisualStyleBackColor = true;
            this.UnbanPlayerButton.Click += new System.EventHandler(this.UnbanPlayerButton_Click);
            // 
            // AddPlayerToWhitelistButton
            // 
            this.AddPlayerToWhitelistButton.Location = new System.Drawing.Point(294, 256);
            this.AddPlayerToWhitelistButton.Name = "AddPlayerToWhitelistButton";
            this.AddPlayerToWhitelistButton.Size = new System.Drawing.Size(167, 23);
            this.AddPlayerToWhitelistButton.TabIndex = 21;
            this.AddPlayerToWhitelistButton.Text = "Add To Whitelist";
            this.AddPlayerToWhitelistButton.UseVisualStyleBackColor = true;
            this.AddPlayerToWhitelistButton.Click += new System.EventHandler(this.AddPlayerToWhitelistButton_Click);
            // 
            // RemovePlayerFromWhitelistButton
            // 
            this.RemovePlayerFromWhitelistButton.Location = new System.Drawing.Point(479, 256);
            this.RemovePlayerFromWhitelistButton.Name = "RemovePlayerFromWhitelistButton";
            this.RemovePlayerFromWhitelistButton.Size = new System.Drawing.Size(164, 23);
            this.RemovePlayerFromWhitelistButton.TabIndex = 20;
            this.RemovePlayerFromWhitelistButton.Text = "Remove From Whitelist";
            this.RemovePlayerFromWhitelistButton.UseVisualStyleBackColor = true;
            this.RemovePlayerFromWhitelistButton.Click += new System.EventHandler(this.RemovePlayerFromWhitelistButton_Click);
            // 
            // PlayerPermissionsTextBox
            // 
            this.PlayerPermissionsTextBox.Location = new System.Drawing.Point(405, 215);
            this.PlayerPermissionsTextBox.Name = "PlayerPermissionsTextBox";
            this.PlayerPermissionsTextBox.ReadOnly = true;
            this.PlayerPermissionsTextBox.Size = new System.Drawing.Size(238, 20);
            this.PlayerPermissionsTextBox.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(291, 215);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Permissions";
            // 
            // IsOnlineTextBox
            // 
            this.IsOnlineTextBox.Location = new System.Drawing.Point(405, 189);
            this.IsOnlineTextBox.Name = "IsOnlineTextBox";
            this.IsOnlineTextBox.ReadOnly = true;
            this.IsOnlineTextBox.Size = new System.Drawing.Size(238, 20);
            this.IsOnlineTextBox.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(291, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Is Online";
            // 
            // IsBannedTextBox
            // 
            this.IsBannedTextBox.Location = new System.Drawing.Point(405, 163);
            this.IsBannedTextBox.Name = "IsBannedTextBox";
            this.IsBannedTextBox.ReadOnly = true;
            this.IsBannedTextBox.Size = new System.Drawing.Size(238, 20);
            this.IsBannedTextBox.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(291, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Is Banned";
            // 
            // IsWhiteListedTextBox
            // 
            this.IsWhiteListedTextBox.Location = new System.Drawing.Point(405, 137);
            this.IsWhiteListedTextBox.Name = "IsWhiteListedTextBox";
            this.IsWhiteListedTextBox.ReadOnly = true;
            this.IsWhiteListedTextBox.Size = new System.Drawing.Size(238, 20);
            this.IsWhiteListedTextBox.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(291, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Is whitelisted";
            // 
            // LastJoinedTextBox
            // 
            this.LastJoinedTextBox.Location = new System.Drawing.Point(405, 111);
            this.LastJoinedTextBox.Name = "LastJoinedTextBox";
            this.LastJoinedTextBox.ReadOnly = true;
            this.LastJoinedTextBox.Size = new System.Drawing.Size(238, 20);
            this.LastJoinedTextBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(291, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Last Joined";
            // 
            // FirstJoinedTextBos
            // 
            this.FirstJoinedTextBos.Location = new System.Drawing.Point(405, 85);
            this.FirstJoinedTextBos.Name = "FirstJoinedTextBos";
            this.FirstJoinedTextBos.ReadOnly = true;
            this.FirstJoinedTextBos.Size = new System.Drawing.Size(238, 20);
            this.FirstJoinedTextBos.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(291, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "First Joined";
            // 
            // PlayerXUIDTextBox
            // 
            this.PlayerXUIDTextBox.Location = new System.Drawing.Point(405, 59);
            this.PlayerXUIDTextBox.Name = "PlayerXUIDTextBox";
            this.PlayerXUIDTextBox.ReadOnly = true;
            this.PlayerXUIDTextBox.Size = new System.Drawing.Size(238, 20);
            this.PlayerXUIDTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(291, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "XUID";
            // 
            // PlayerNameTextBox
            // 
            this.PlayerNameTextBox.Location = new System.Drawing.Point(405, 33);
            this.PlayerNameTextBox.Name = "PlayerNameTextBox";
            this.PlayerNameTextBox.ReadOnly = true;
            this.PlayerNameTextBox.Size = new System.Drawing.Size(238, 20);
            this.PlayerNameTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(291, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RevertPlayersChangesButton);
            this.groupBox1.Controls.Add(this.SavePlayersButton);
            this.groupBox1.Controls.Add(this.AddNewPlayerButton);
            this.groupBox1.Controls.Add(this.AddNewPlayerTextBox);
            this.groupBox1.Location = new System.Drawing.Point(7, 785);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1610, 84);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Utils";
            // 
            // RevertPlayersChangesButton
            // 
            this.RevertPlayersChangesButton.Location = new System.Drawing.Point(88, 47);
            this.RevertPlayersChangesButton.Name = "RevertPlayersChangesButton";
            this.RevertPlayersChangesButton.Size = new System.Drawing.Size(75, 23);
            this.RevertPlayersChangesButton.TabIndex = 3;
            this.RevertPlayersChangesButton.Text = "Revert";
            this.RevertPlayersChangesButton.UseVisualStyleBackColor = true;
            this.RevertPlayersChangesButton.Click += new System.EventHandler(this.RevertPlayersChangesButton_Click);
            // 
            // SavePlayersButton
            // 
            this.SavePlayersButton.Location = new System.Drawing.Point(7, 47);
            this.SavePlayersButton.Name = "SavePlayersButton";
            this.SavePlayersButton.Size = new System.Drawing.Size(75, 23);
            this.SavePlayersButton.TabIndex = 2;
            this.SavePlayersButton.Text = "Save";
            this.SavePlayersButton.UseVisualStyleBackColor = true;
            this.SavePlayersButton.Click += new System.EventHandler(this.SavePlayersButton_Click);
            // 
            // AddNewPlayerButton
            // 
            this.AddNewPlayerButton.Location = new System.Drawing.Point(284, 20);
            this.AddNewPlayerButton.Name = "AddNewPlayerButton";
            this.AddNewPlayerButton.Size = new System.Drawing.Size(103, 23);
            this.AddNewPlayerButton.TabIndex = 1;
            this.AddNewPlayerButton.Text = "Add New Player";
            this.AddNewPlayerButton.UseVisualStyleBackColor = true;
            this.AddNewPlayerButton.Click += new System.EventHandler(this.AddNewPlayerButton_Click);
            // 
            // AddNewPlayerTextBox
            // 
            this.AddNewPlayerTextBox.Location = new System.Drawing.Point(7, 20);
            this.AddNewPlayerTextBox.Name = "AddNewPlayerTextBox";
            this.AddNewPlayerTextBox.Size = new System.Drawing.Size(270, 20);
            this.AddNewPlayerTextBox.TabIndex = 0;
            // 
            // PlayerFilterTextBox
            // 
            this.PlayerFilterTextBox.Location = new System.Drawing.Point(7, 7);
            this.PlayerFilterTextBox.Name = "PlayerFilterTextBox";
            this.PlayerFilterTextBox.Size = new System.Drawing.Size(277, 20);
            this.PlayerFilterTextBox.TabIndex = 1;
            this.PlayerFilterTextBox.TextChanged += new System.EventHandler(this.PlayerFilterTextBox_TextChanged);
            // 
            // PlayersListBox
            // 
            this.PlayersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PlayersListBox.FormattingEnabled = true;
            this.PlayersListBox.Location = new System.Drawing.Point(7, 33);
            this.PlayersListBox.Name = "PlayersListBox";
            this.PlayersListBox.Size = new System.Drawing.Size(277, 745);
            this.PlayersListBox.TabIndex = 0;
            this.PlayersListBox.SelectedIndexChanged += new System.EventHandler(this.PlayersListBox_SelectedIndexChanged);
            // 
            // ServerInstanceOverviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1655, 925);
            this.Controls.Add(this.tabControl1);
            this.Name = "ServerInstanceOverviewForm";
            this.Text = "ServerInstanceOverview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerInstanceOverview_FormClosing);
            this.Shown += new System.EventHandler(this.ServerInstanceOverview_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RestartTimerNumberInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTimerNumberInput)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SendCommandButton;
        private System.Windows.Forms.TextBox SendCommandTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.TextBox SendMessageTextBox;
        private System.Windows.Forms.Button StopServerButton;
        private System.Windows.Forms.Button StartServerButton;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PlayerFilterTextBox;
        private System.Windows.Forms.ListBox PlayersListBox;
        private System.Windows.Forms.Button AddNewPlayerButton;
        private System.Windows.Forms.TextBox AddNewPlayerTextBox;
        private System.Windows.Forms.Button RevertPlayersChangesButton;
        private System.Windows.Forms.Button SavePlayersButton;
        private System.Windows.Forms.TextBox PlayerXUIDTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PlayerNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button KickPlayerButton;
        private System.Windows.Forms.TextBox KickMessageTextBox;
        private System.Windows.Forms.Button PromotePlayerButton;
        private System.Windows.Forms.Button DemotePlayerButton;
        private System.Windows.Forms.Button BanPlayerButton;
        private System.Windows.Forms.Button UnbanPlayerButton;
        private System.Windows.Forms.Button AddPlayerToWhitelistButton;
        private System.Windows.Forms.Button RemovePlayerFromWhitelistButton;
        private System.Windows.Forms.TextBox PlayerPermissionsTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox IsOnlineTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox IsBannedTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox IsWhiteListedTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox LastJoinedTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox FirstJoinedTextBos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.NumericUpDown RestartTimerNumberInput;
        private System.Windows.Forms.NumericUpDown StopTimerNumberInput;
        private System.Windows.Forms.Button TriggerBackupButton;
    }
}