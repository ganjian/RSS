using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Server
{
    public class SeverSocket
    {
        private TcpClient client;
        private NetworkStream streamToClient;
        private const int BufferSize = 8192;
        private byte[] buffer;
        private RequestHandler handler;
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

            TcpClient client = listener.AcceptTcpClient();
            streamToClient = client.GetStream();


            buffer = new byte[BufferSize];

            handler = new RequestHandler();

            AsyncCallback callback = new AsyncCallback(ReadComplete);

            streamToClient.BeginRead(buffer, 0, BufferSize, callback, null);
        }

        public void stop()
        {
            listener.Stop();
        }


        public void sendMessage(string msg)
        {
            var client = listener.AcceptTcpClient();
            var networkstream = client.GetStream();

            var bytes = Encoding.Unicode.GetBytes(msg);

            networkstream.Write(bytes, 0, bytes.Length);

        }


        public void SendMessage(string msg)
        {
            msg = string.Format("[length={0}]{1}", msg.Length, msg);
            for (int i = 0; i < 2; i++)
            {
                byte[] temp = Encoding.Unicode.GetBytes(msg);

                try
                {
                    streamToServer.Write(temp, 0, temp.Length);
                    Console.WriteLine("Sent:{0}", msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
            AsyncCallback callback = new AsyncCallback(ReadComplete);
            streamToServer.BeginRead(buffer, 0, BufferSize, callback, null);
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



        private void ReadComplete(IAsyncResult ar)
        {
            int byteRead = 0;

            try
            {
                string readmsg = Console.ReadLine();


                byteRead = streamToClient.EndRead(ar);
                if (byteRead == 0)
                {
                    Console.WriteLine("Client offline");
                    return;
                }
                string msg = Encoding.Unicode.GetString(buffer, 0, byteRead);

                Array.Clear(buffer, 0, buffer.Length);

                string[] msgArray = handler.GetActualString(msg);

                foreach (string m in msgArray)
                {
                    //Console.WriteLine("Received:{0}[{1}bytes]", m, byteRead);
                    //string back = m.ToUpper();
                    string back = readmsg;
                    byte[] temp = Encoding.Unicode.GetBytes(back);
                    streamToClient.Write(temp, 0, temp.Length);
                    streamToClient.Flush();
                    Console.WriteLine("Sent:{0}", back);

                }


                AsyncCallback callback = new AsyncCallback(ReadComplete);
                streamToClient.BeginRead(buffer, 0, BufferSize, callback, null);


            }
            catch (Exception ex)
            {
                if (streamToClient != null)
                {
                    streamToClient.Dispose();

                }

                client.Close();
                Console.WriteLine(ex.Message);
            }


        }
    }



    public class RequestHandler
    {

        private string temp = string.Empty;
        public string[] GetActualString(string input)
        {
            return GetActualString(input, null);

        }
        private string[] GetActualString(string input, List<string> outputlist)
        {
            if (outputlist == null)
            {
                outputlist = new List<string>();
            }
            if (!string.IsNullOrEmpty(temp))
            {
                input = temp + input;
            }
            string output = "";

            string pattern = @"(?<=^\[length=)(\d+)(?=\])";

            int length;

            if (Regex.IsMatch(input, pattern))
            {
                Match m = Regex.Match(input, pattern);
                length = Convert.ToInt32(m.Groups[0].Value);
                int startIndex = input.IndexOf(']') + 1;

                output = input.Substring(startIndex);

                if (output.Length == length)
                {
                    outputlist.Add(output); temp = "";
                }
                else if (output.Length < length)
                {
                    temp = input;
                }
                else if (output.Length > length)
                {
                    output = output.Substring(0, length);
                    outputlist.Add(output);
                    temp = "";

                    input = input.Substring(startIndex + length);
                    GetActualString(input, outputlist);
                }

            }
            else
            {
                temp = input;
            }
            return outputlist.ToArray();

        }
    }
}
