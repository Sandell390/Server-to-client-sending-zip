using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;


namespace Client
{
    public static class Connection
    {

        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static int connectionCount = 0;

        public static string status { get; set; }

        public async static Task LoopConnectAsync() 
        {


            TextBox t = Application.OpenForms["Form1"].Controls["ConnectStatus"] as TextBox;

            while (!_clientSocket.Connected)
            {
                
                try
                {
                    connectionCount++;
                    await _clientSocket.ConnectAsync(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    status = $"Connecting... Attempts: {connectionCount}";
                    t.Text = status;
                }
                
            }
            status = $"Connected";
            t.Text = status;
        }

        public async static Task SendMessage() 
        {

            TextBox Xmlbox = Application.OpenForms["Form1"].Controls["XmlText"] as TextBox;

            if (!_clientSocket.Connected) 
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await LoopConnectAsync();
            }

            byte[] buffer = Encoding.ASCII.GetBytes("song");
            _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

            var buffer2 = new byte[1024 * 50000];


            int received = _clientSocket.Receive(buffer2, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer2, data, received);

            string zipPath = AppDomain.CurrentDomain.BaseDirectory + "\\ZippedSongs.zip";
            string extractPath = AppDomain.CurrentDomain.BaseDirectory + "\\Songs";

            //File.WriteAllBytes(extractPath, data);

            /*
            using (NetworkStream networkStream = new NetworkStream(_clientSocket)) 
            {
                using (FileStream fileStream = File.Create(zipPath))
                {
                    await networkStream.CopyToAsync(fileStream);
                    
                }
            }
            */

            using (var compressedFileStream = new MemoryStream(data))
            {
                using (FileStream stream = File.OpenWrite(zipPath)) 
                {
                   await compressedFileStream.CopyToAsync(stream);
                }
            }

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (var entry in archive.Entries)
                {
                    entry.ExtractToFile(extractPath + "\\" + entry.FullName, true);
                }
            }

            File.Delete(zipPath);

           _clientSocket.Close();

        }

        public class MyMessage
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
        }

    }
}
