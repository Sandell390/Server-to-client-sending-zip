using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;

            await Connection.LoopConnectAsync();

            ConnectStatus.Text = Connection.status;

            ConnectButton.Enabled = true;
            DownloadFiles.Enabled = true;
            GetFiles.Enabled = true;
        }

        private async void DownloadFiles_Click(object sender, EventArgs e)
        {
            await Connection.SendMessage("send"); 
        }

        private async void GetFiles_Click(object sender, EventArgs e)
        {
            await Connection.SendMessage("files");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ipServerText_TextChanged(object sender, EventArgs e)
        {
            ConnectButton.Enabled = true;
        }
    }
}
