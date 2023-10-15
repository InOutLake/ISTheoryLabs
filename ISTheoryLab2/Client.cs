using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        UdpClient clientReciever;
        UdpClient clientSender;
        IPEndPoint serverEP;
        IPEndPoint clientEP;
        const int CLIENTPORT = 8081;
        const int SERVERPORT = 8080;
        IPAddress IPADDRESS = IPAddress.Parse("127.0.0.1");

        string recieved;
        string toSend;
        string responseType;



        async Task SendMessageAsync()
        {
            while (true)
            {
                if (toSend != "wait")
                {
                    byte[] data = Encoding.UTF8.GetBytes(toSend);
                    await clientSender.SendAsync(data, serverEP);
                    toSend = "wait";
                }
            }
        }

        async Task ReceiveMessageAsync()
        {
            while (true)
            {
                string result = Encoding.UTF8.GetString((await clientReciever.ReceiveAsync()).Buffer);
            }
        }

        string getResponceType(char b)
        {
            switch (b) 
            {
                case 'w':
                    return "write";
                case 's':
                    return "strng";
                case 'k':
                    return "key";
            }
            return "wait";
        }

        public Client()
        {
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            clientReciever = new UdpClient(clientEP);
            clientSender = new UdpClient();
        }

        public async Task<string> ReadStringFromServer()
        {
            byte[] receivedData = clientReciever.Receive(ref clientEP);
            return Encoding.ASCII.GetString(receivedData);
        }

        public async Task SendStringToServer(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            clientSender.Send(data, data.Length, serverEP);
        }

        public void ClientMain()
        {
            Task.Run(ReceiveMessageAsync);
            Task.Run(SendMessageAsync);

            while (true)
            {
                if (responseType.Equals("key"))
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    toSend = keyInfo.KeyChar.ToString();
                    responseType = "wait";
                }
                else if (responseType == "string")
                {
                    toSend = Console.ReadLine();
                    responseType = "wait";
                }
                else if (responseType == "write")
                {
                    Console.WriteLine(recieved);
                    responseType = "wait";
                }
            }
        }
    }
}
