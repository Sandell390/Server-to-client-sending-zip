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
            await Connection.LoopConnectAsync();

            ConnectStatus.Text = Connection.status;
        }

        private async void GetXmlButton_Click(object sender, EventArgs e)
        {
            await Connection.SendMessage();

            
        }

        private async void ConnectionChecker_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
