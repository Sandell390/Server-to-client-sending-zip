﻿using System;
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

            int count = 0;

            string musicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Musik";
            string zipPath = AppDomain.CurrentDomain.BaseDirectory + $"\\{count}ZippedSongs.zip";

            if (text == "song") 
            {
                int maxZip = 1024 * 5000;

                int fileLenght = 0;

                string[] path = Directory.GetFiles(musicPath);
                List<string> tempFiles = new List<string>();

                List<string> allZips = new List<string>();

                int countEntities = 0;

                foreach (var item in path)
                {
                    Console.WriteLine("Path: " + item);
                    Console.WriteLine("Lenght: " + fileLenght);
                    

                    if (File.ReadAllBytes(item).Length < maxZip)
                        fileLenght += File.ReadAllBytes(item).Length;
                    else
                    {
                        Console.WriteLine($"Too large file, name: {Directory.GetFiles(item).Select(Path.GetFileName)}");
                        continue;
                    }

                    tempFiles.Add(item);

                    if (fileLenght > maxZip || countEntities == path.Length - 1)
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

                    countEntities++;


                    Console.WriteLine("Enities: " + countEntities);
                }

                

                //ZipFile.CreateFromDirectory(musicPath, zipPath);

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

                /*
                using (ZipArchive Ziparchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    foreach (var entry in Ziparchive.Entries)
                    {
                        Console.WriteLine($"Zip File size/name: {entry.CompressedLength} / {entry.Name}");
                        fileLenght += int.Parse(entry.CompressedLength.ToString());
                    }
                }
                
                */

                
            }
            else 
            {
                data = Encoding.UTF8.GetBytes("Wrong");
            }

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
