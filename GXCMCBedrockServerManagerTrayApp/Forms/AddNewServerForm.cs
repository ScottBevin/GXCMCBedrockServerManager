using System;
using System.Windows.Forms;
using System.IO;
using GXCMCBedrockServerManagerCore;
using GXCMCBedrockServerManagerCore.Files;

namespace GXCMCBedrockServerManager.Forms
{
    public partial class AddNewServerForm : Form
    {
        public ServerManager ServerManager { get; set; } = null;

        public AddNewServerForm()
        {
            InitializeComponent();
        }

        void ClearOutputBox()
        {
            OutputListBox.Items.Clear();
        }

        void AddOutputString(string s)
        {
            OutputListBox.Items.Add(s);
        }

        bool VerifyChosenServerPath()
        {
            bool errorsFound = false;

            string path = FolderBrowserDialog.SelectedPath;

            if (string.IsNullOrWhiteSpace(path))
            {
                AddOutputString("Invalid path");
                errorsFound = true;
            }

            AddOutputString($"Server Path: {path}");

            ServerPropertiesFile spf = ServerPropertiesFile.Load(path);

            if(spf == null || spf.FileHasErrors)
            {
                AddOutputString("Server.properties file has issues");
                errorsFound = true;
            }
            else
            {
                AddOutputString("Server.properties file is valid");
                AddOutputString($"Server: {spf.ServerName}");
            }

            if (File.Exists(Path.Combine(path, ServerManager.GlobalSettings.ServerExecutableFileName)))
            {
                AddOutputString("Server.exe found");
            }
            else
            {
                AddOutputString("Server.exe missing.");
                errorsFound = true;
            }

            return !errorsFound;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = FolderBrowserDialog.ShowDialog(this);

            if(result == DialogResult.OK)
            {
                ClearOutputBox();

                AddButton.Enabled = VerifyChosenServerPath();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if(ServerManager.AddNewServer(FolderBrowserDialog.SelectedPath))
            {
                Close();
            }
            else 
            {
                MessageBox.Show("Error adding server.");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
