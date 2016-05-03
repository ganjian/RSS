using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public class ClientSocket
    {

        IPAddress ip;
        TcpClient Client;
        int serverPort;
        public ClientSocket()
        {
            string serverip = System.Configuration.ConfigurationManager.AppSettings["serverIP"];
            serverPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["serverPort"].ToString());
            ip = new IPAddress(ipparse(serverip));
            Client = new TcpClient();



        }


        public string start()
        {
            Client.Connect(ip, serverPort);
            string result = "成功连接服务器：" + Client.Client.RemoteEndPoint;

            return result;
        }

        public void stop()
        {
            Client.Close();
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
