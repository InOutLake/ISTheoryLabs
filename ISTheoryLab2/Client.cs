using System;
using System.ComponentModel.Design;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;


namespace Client
{
    class Client
    {
        TcpClient client;
        NetworkStream stream;
        public Client()
        {
            Init();
        }
        public void Init()
        {
            client = new TcpClient();
            client.Connect("localhost", 8080);
            stream = client.GetStream();
        }
        public string readStringFromStream()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
        public void SendKeyInfoToServer(ConsoleKeyInfo keyInfo)
        {
            byte[] data = Serializer.SerializeConsoleKeyInfo(keyInfo);
            stream.Write(data, 0, data.Length);
        }
        public void SendStringToServer(string s)
        {
            byte[] sBytes = Encoding.ASCII.GetBytes(s);
            stream.Write(sBytes, 0, sBytes.Length);
        }

        public string getRespondType()
        {
            return readStringFromStream();
        }

        public void respondOnStringRequest() //TODO add input message show
        {
            Console.WriteLine(readStringFromStream());
            SendStringToServer(Console.ReadLine());
        }
        public void respondOnKeyInfoRequest()
        {
            SendKeyInfoToServer(Console.ReadKey());
        }
        public void Main(string[] args)
        {
            string respondType;
            while (true)
            {
                respondType = getRespondType();
                if (respondType == "key")
                {
                    respondOnKeyInfoRequest();
                }
                else
                {
                    respondOnStringRequest();
                }
            }
        }
    }
}