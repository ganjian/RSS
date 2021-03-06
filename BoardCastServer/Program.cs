﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BoardCastServer
{
    class Program
    {
        private static Socket sock;
        private static IPEndPoint iep1;
        private static byte[] data;
        static void Main(string[] args)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);
            //255.255.255.255
            iep1 =
            new IPEndPoint(IPAddress.Broadcast, 9050);

            string hostname = Dns.GetHostName();
            data = Encoding.ASCII.GetBytes(hostname);

            sock.SetSocketOption(SocketOptionLevel.Socket,
            SocketOptionName.Broadcast, 1);

            Thread t = new Thread(BroadcastMessage);
            t.Start();
            //sock.Close();

            Console.ReadKey();

        }

        private static void BroadcastMessage()
        {
            while (true)
            {
                sock.SendTo(data, iep1);
                Thread.Sleep(2000);
            }

        }
    }
}
