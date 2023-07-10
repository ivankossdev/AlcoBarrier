using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace AlcoBarrier
{
    static class TcpServer
    {
        public static void Server()
        {
            TcpListener server = null;
            try
            {
                
                Int32 port = 10500;
                IPAddress localAddr = IPAddress.Parse("192.168.0.204");
                
                server = new TcpListener(localAddr, port);
                server.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine($"Received: {data}");
                    }
                }
            }

            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }
        }

        public static async Task StartListener(int port)
        {
            var tcpListener = TcpListener.Create(port);
            tcpListener.Start();
            for (; ; )
            {
                Console.WriteLine("[Server] waiting for clients...");
                using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                {
                    try
                    {
                        Console.WriteLine("[Server] Client has connected");
                        using (var networkStream = tcpClient.GetStream())
                        using (var reader = new StreamReader(networkStream))
                        using (var writer = new StreamWriter(networkStream) { AutoFlush = true })
                        {
                            var buffer = new byte[4096];
                            Console.WriteLine("[Server] Reading from client");
                            var request = await reader.ReadLineAsync();
                            string.Format(string.Format("[Server] Client wrote '{0}'", request));

                            await writer.WriteLineAsync($"[Server] to Client {request}");
                            //for (int i = 0; i < 5; i++)
                            //{
                            //    await writer.WriteLineAsync("I am the server! HAHAHA!");
                            //    Console.WriteLine("[Server] Response has been written");
                            //    await Task.Delay(TimeSpan.FromSeconds(1));
                            //}
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[Server] client connection lost");
                    }
                }
            }
        }
    }
}
