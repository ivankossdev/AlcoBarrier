﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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

        public async static Task AsyncServer()
        {
            Int32 port = 10500;
            IPAddress localAddr = IPAddress.Parse("192.168.0.204");

            TcpListener server = new TcpListener(localAddr, port);
            server.Start();

            Byte[] bytes = new Byte[256];
            String data = null;

            while (true)
            {
                try
                {
                    Console.Write("Waiting for a connection... ");

                    using (var client = await server.AcceptTcpClientAsync())
                    {
                        Console.WriteLine("Connected!");

                        data = null;

                        NetworkStream stream = client.GetStream();

                        int i;

                        while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine($"Received: {data}");
                        }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine($"Lost connection ");
                }
            }
        }

    }
}
