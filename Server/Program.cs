using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO.Compression;

namespace Server
{


    class Program
    {
        private static byte[] _buffer = new byte[1024];

        private static List<Socket> _clientSockets = new List<Socket>();

        private static Socket _server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SetupServer();
            Console.ReadLine();

        }

        private static void SetupServer() 
        {
            Console.WriteLine("Setting server up");
            _server.Bind(new IPEndPoint(IPAddress.Any, 100));
            _server.Listen(5);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);

            Console.WriteLine("Making Json File");
            JsonHandler.FirstRead();
        }

        

        private static void AcceptCallback(IAsyncResult AR) 
        {
            Socket socket = _server.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client Connected");
            socket.BeginReceive(_buffer,0,_buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        } 

        private static void ReceiveCallback(IAsyncResult AR) 
        {
            Socket socket = (Socket)AR.AsyncState;

            int received = socket.EndReceive(AR);
            byte[] databuf = new byte[received];
            Array.Copy(_buffer, databuf, received);

            string text = Encoding.UTF8.GetString(databuf);
            Console.WriteLine("Text received:" + text);

            byte[] data;

            string musicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Musik";
            string zipPath = AppDomain.CurrentDomain.BaseDirectory + "\\ZippedSongs.zip";

            if (text == "song") 
            {
                ZipFile.CreateFromDirectory(musicPath, zipPath);


                data = File.ReadAllBytes(zipPath);

                
                int max = 1024 * 50000;

                byte[] temp;

                int fileLenght = 0;

                using (ZipArchive Ziparchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    foreach (var entry in Ziparchive.Entries)
                    {
                        if (fileLenght > max) 
                        {

                        }
                        Console.WriteLine($"Zip File size/name: {entry.CompressedLength} / {entry.Name}");
                        fileLenght += int.Parse(entry.CompressedLength.ToString());
                    }
                }
                
                File.Delete(zipPath);
            }
            else 
            {
                data = Encoding.UTF8.GetBytes("Wrong");
            }

            
            socket.BeginSend(data,0,data.Length,SocketFlags.None, new AsyncCallback(SendCallback), socket);
            //socket.Close();

            
        }

        private static void SendCallback(IAsyncResult AR) 
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

    }

    public class MyMessage
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
}
