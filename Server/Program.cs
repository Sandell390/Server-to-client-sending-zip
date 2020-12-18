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
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

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


            if (text.Contains("send")) 
            {
                sendZip(socket , text);
            }
            else if(text.Contains("files"))
            {
                sendListFiles(socket);
            }
            else 
            {
                Console.WriteLine("Something is wrong, Did not get the right string from client");
            }


        }

        private static void sendListFiles(Socket socket)
        {
            byte[] data;

            string musicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Musik";

            List<string> listFiles = Directory.GetFiles(musicPath).Select(Path.GetFileName).ToList();

            string fullList = string.Empty;

            listFiles.ForEach(x => fullList += x + ";");

            data = Encoding.UTF8.GetBytes(fullList);

            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }

        private static void sendZip(Socket socket, string text)
        {
            byte[] data;

            int count = 0;

            string musicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Musik";
            string zipPath = AppDomain.CurrentDomain.BaseDirectory + $"\\{count}ZippedSongs.zip";

            int maxZip = 1024 * 5000;

            int fileLenght = 0;

            List<string> filesList = new List<string>();

            int previousPoint = 0;

            previousPoint = text.IndexOf(":");

            text = text.Replace(text, text.Remove(0, previousPoint + 1));

            for (int i = 0; i < text.Length; i++)
            {
                previousPoint = text.IndexOf(";");

                filesList.Add(text.Substring(0, previousPoint));

                text = text.Replace(text, text.Remove(0, previousPoint + 1));
            }


            filesList.ForEach(x => Console.WriteLine(x));
            
            List<string> path = new List<string>();

            filesList.ForEach(x => path.Add(musicPath + "\\" + x));

            Console.WriteLine(path.Count);

            List<string> tempFiles = new List<string>();

            List<string> allZips = new List<string>();

            int countEntities = 0;

            foreach (var item in path)
            {
                Console.WriteLine("Path: " + item);
                Console.WriteLine("Lenght: " + fileLenght);

                if (File.ReadAllBytes(item).Length  < maxZip) 
                {
                    if (fileLenght + File.ReadAllBytes(item).Length < maxZip) 
                    {
                        tempFiles.Add(item);
                        
                    }

                    if (fileLenght + File.ReadAllBytes(item).Length > maxZip || item == path[path.Count - 1])
                    {
                        var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);

                        foreach (var tempPath in tempFiles)
                        {
                            zip.CreateEntryFromFile(tempPath, Path.GetFileName(tempPath), CompressionLevel.Optimal);
                        }

                        zip.Dispose();

                        allZips.Add(zipPath);

                        count++;
                        tempFiles.Clear();
                        fileLenght = 0;

                        zipPath = AppDomain.CurrentDomain.BaseDirectory + $"\\{count}ZippedSongs.zip";
                    }

                    fileLenght += File.ReadAllBytes(item).Length;

                }
                else
                {
                    Console.WriteLine($"Too large file, name: {Directory.GetFiles(item).Select(Path.GetFileName)}");
                    continue;
                }

                

                Console.WriteLine("Enities: " + countEntities);
            }

            data = Encoding.UTF8.GetBytes(allZips.Count.ToString());

            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

            Console.WriteLine(allZips.Count + " zips sent");

            for (int i = 0; i < allZips.Count; i++)
            {

                data = File.ReadAllBytes(allZips[i]);

                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

                File.Delete(allZips[i]);

                Thread.Sleep(900);
            }

            allZips.Clear();
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
