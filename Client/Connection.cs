using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Client
{
    public static class Connection
    {

        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static int connectionCount = 1;

        static ListBox listBox = Application.OpenForms["Form1"].Controls["fileBox"] as ListBox;

        
        public static string status { get; set; }

        public async static Task LoopConnectAsync() 
        {
            TextBox ipText = Application.OpenForms["Form1"].Controls["ipServerText"] as TextBox;
            TextBox ConnectStatus = Application.OpenForms["Form1"].Controls["ConnectStatus"] as TextBox;

            IPAddress server;

            if (!IPAddress.TryParse(ipText.Text, out server))
            {
                MessageBox.Show("Du skrev ikke en gyldig ip adresse\n\rSkriv ip adressen på din server du vil forbinde til");
                return;
            }

            status = $"Connecting... Attempts: {connectionCount}";
            ConnectStatus.Text = status;

            while (!_clientSocket.Connected)
            {
                
                try
                {
                    connectionCount++;
                    await _clientSocket.ConnectAsync(server, 100);
                }
                catch (SocketException)
                {
                    status = $"Connecting... Attempts: {connectionCount}";
                    ConnectStatus.Text = status;
                }
                
            }
            status = $"Connected";
            ConnectStatus.Text = status;
        }

        public async static Task SendMessage(string message) 
        {

            if (!_clientSocket.Connected) 
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await LoopConnectAsync();
            }

            

            switch (message)
            {
                case "send":
                    await GetZipFiles();
                    break;

                case "files":
                    await ListOverFiles();
                    break;

                default:
                    break;
            }

        }

        async static Task GetZipFiles() 
        {
            if (listBox.SelectedItems.Count < 1)
            {
                MessageBox.Show("Du har ikke valgt nogle filer\n\rVælg en eller filer som der skal downloades");
                return;
            }

            List<string> listFiles = (from string s in listBox.SelectedItems select s).ToList();

            string fullList = string.Empty;

            listFiles.ForEach(x => fullList += x + ";");

            var data = Encoding.UTF8.GetBytes(fullList);

            byte[] buffer = Encoding.ASCII.GetBytes("send:" + fullList);
            _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

            data = await Task.Run(() => receivedBytes(1024 * 5000));

            int numberOfZips = int.Parse(Encoding.UTF8.GetString(data));

            for (int i = 0; i < numberOfZips; i++)
            {
                if (!_clientSocket.Connected)
                {
                    _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    await LoopConnectAsync();
                }

                data = await Task.Run(() => receivedBytes(1024 * 5000));

                string zipPath = AppDomain.CurrentDomain.BaseDirectory + $"\\{i}ZippedSongs.zip";
                string extractPath = AppDomain.CurrentDomain.BaseDirectory + "\\Songs";

                MessageBox.Show("Creating Zip");

                string domain = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                IdentityReference sid = null;
                string owner = null;

                

                using (var compressedFileStream = new MemoryStream(data))
                {
                    using (FileStream stream = File.Create(zipPath))
                    {
                        FileSecurity fSecurity = File.GetAccessControl(zipPath);
                        fSecurity.AddAccessRule(new FileSystemAccessRule(domain, FileSystemRights.FullControl, AccessControlType.Allow));
                        sid = fSecurity.GetOwner(typeof(SecurityIdentifier));
                        NTAccount ntAccount = sid.Translate(typeof(NTAccount)) as NTAccount;
                        owner = ntAccount.Value;

                        File.SetAttributes(zipPath, FileAttributes.Normal);
                        await compressedFileStream.CopyToAsync(stream);
                    }
                }
                MessageBox.Show("Zip created, Extract entries from zip");
                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    foreach (var entry in archive.Entries)
                    {
                        await Task.Run(() => entry.ExtractToFile(extractPath + "\\" + entry.FullName, true));
                    }
                }

                File.Delete(zipPath);
            }

            _clientSocket.Close();

        }

        async static Task ListOverFiles() 
        {
            byte[] buffer = Encoding.ASCII.GetBytes("files");
            _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

            if (!_clientSocket.Connected)
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await LoopConnectAsync();
            }

            var data = await Task.Run(() => receivedBytes(1024 * 5000));

            string fullStringFiles = Encoding.UTF8.GetString(data);


            List<string> filesList = new List<string>();

            int previousPoint = 0;

            for (int i = 0; i < fullStringFiles.Length; i++)
            {
                previousPoint = fullStringFiles.IndexOf(";");

                filesList.Add(fullStringFiles.Substring(0,previousPoint));

                fullStringFiles = fullStringFiles.Replace(fullStringFiles, fullStringFiles.Remove(0, previousPoint + 1));
            }

            listBox.SelectionMode = SelectionMode.MultiExtended;

            listBox.Items.Clear();

            listBox.Items.AddRange(filesList.ToArray());

            _clientSocket.Close();
        }


        private static byte[] receivedBytes(int bufferSize) 
        {
            var buffer = new byte[bufferSize];

            int received = _clientSocket.Receive(buffer, SocketFlags.None);

            var data = new byte[received];
            Array.Copy(buffer, data, received);

            return data;
        }
    }
}
