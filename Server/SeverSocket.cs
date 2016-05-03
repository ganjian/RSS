using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
namespace Server
{
    public class SeverSocket
    {

        IPAddress ip;
        TcpListener listener;
        public SeverSocket()
        {
            string serverip = System.Configuration.ConfigurationManager.AppSettings["serverIP"];
            int serverPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["serverPort"].ToString());
            ip = new IPAddress(ipparse(serverip));
            listener = new TcpListener(ip, serverPort);



        }


        public void start()
        {
            listener.Start();
          //  listener.AcceptSocket();
        }

        public void stop()
        {
            listener.Stop();
        }


        public byte[] ipparse(string ip)
        {
            var result = new byte[4];

            var iparray = ip.Split('.');

            for (int i = 0; i < 4; i++)
            {
                result[i] = byte.Parse(iparray[i]);
            }
            return result;
        }

    }
}
