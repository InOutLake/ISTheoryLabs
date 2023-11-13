using NLog;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        const int CLIENTPORT = 8081;
        const int SERVERPORT = 8080;
        IPAddress IPADDRESS = IPAddress.Parse("127.0.0.1");

        IPEndPoint serverEP;
        IPEndPoint clientEP;
        UdpClient clientReceiver;
        UdpClient clientSender;

        public volatile string messageRecieved;
        Logger logger;


        public Client()
        {
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            clientReceiver = new UdpClient(clientEP);
            clientSender = new UdpClient();

            messageRecieved = "wait";

            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSender.Send(data, serverEP);
            logger.Info("SEND " + message);
            //Console.WriteLine($"SEND: {message}");
        }

        public string ReceiveMessage()
        {
            var result = clientReceiver.Receive(ref clientEP);
            messageRecieved = Encoding.UTF8.GetString(result);
            logger.Info("RECEIVED " + messageRecieved);
            //Console.WriteLine($"Recieved: {messageRecieved}");
            return messageRecieved;
        }

        public void SendKey(char pressedKeyChar)
        {
            
            if (pressedKeyChar == (char)27)
            {
                SendMessage("36");
            }
            else
            {
                SendMessage("3"+pressedKeyChar.ToString());
            }
        }

        public void RespondToKeyRequest()
        {
            char pressedKeyChar = Console.ReadKey().KeyChar;
            SendKey(pressedKeyChar);
        }

        public void RespondToStringRequest()
        {
            SendMessage('2' + Console.ReadLine());
        }

        public void ClientMain()
        {
            Console.WriteLine("Started client...");
            
            bool quit = false;
            while (!quit)
            {
                string recievedMessage = ReceiveMessage();
                switch (recievedMessage[0])
                {
                    case '1':
                        Console.WriteLine("\n" + recievedMessage.Substring(1));
                        break;
                    case '2':
                        RespondToStringRequest();
                        break;
                    case '3':
                        RespondToKeyRequest();
                        break;
                    case '4':
                        quit = true;
                        break;
                }
            }
        }
    }
}
