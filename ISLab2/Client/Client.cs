using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        const int CLIENTPORT = 8081;
        const int SERVERPORT = 8080;
        IPAddress IPADDRESS = IPAddress.Parse("127.0.0.1");

        IPEndPoint serverEP;
        IPEndPoint clientEP;
        UdpClient clientReceiver;
        UdpClient clientSender;

        volatile string messageRecieved;
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

        void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSender.Send(data, serverEP);
            logger.Info("SEND" + message);
            //Console.WriteLine($"SEND: {message}");
        }

        string ReceiveMessage()
        {
            var result = clientReceiver.Receive(ref clientEP);
            messageRecieved = Encoding.UTF8.GetString(result);
            logger.Info("RECEIVED" + messageRecieved);
            //Console.WriteLine($"Recieved: {messageRecieved}");
            return messageRecieved;
        }

        void SendKey(char pressedKeyChar)
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

        void RespondToKeyRequest()
        {
            char pressedKeyChar = Console.ReadKey().KeyChar;
            SendKey(pressedKeyChar);
        }

        void RespondToStringRequest()
        {
            SendMessage('2' + Console.ReadLine());
        }

        public async void ClientMain()
        {
            Console.WriteLine("Started client...");
            
            bool quit = false;
            while (!quit)
            {
                string recievedMessage = ReceiveMessage();
                switch (recievedMessage[0])
                {
                    case '1':
                        Console.WriteLine(recievedMessage.Substring(1));
                        break;
                    case '2':
                        RespondToStringRequest();
                        break;
                    case '3':
                        RespondToKeyRequest();
                        break;
                }
            }
        }
    }
}
