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

        volatile string messageToSend;
        volatile string messageRecieved;

        volatile Command command;

        public Client()
        {
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            clientReceiver = new UdpClient(clientEP);
            clientSender = new UdpClient();

            messageRecieved = "wait";

            command = Command.wait;
        }

        void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSender.Send(data, serverEP);
            Console.WriteLine($"SEND: {message}");
        }

        string ReceiveMessage()
        {
            var result = clientReceiver.Receive(ref serverEP);
            messageRecieved = Encoding.UTF8.GetString(result);
            Console.WriteLine($"Recieved: {messageRecieved}");
            return messageRecieved;
        }

        void SendKey(char pressedKeyChar)
        {
            
            if (pressedKeyChar == (char)27)
            {
                SendMessage("3esc");
            }
            else
            {
                SendMessage("3"+pressedKeyChar.ToString());
            }
        }

        void RespondToKeyRequest(string messageFromServer)
        {
            char pressedKeyChar = Console.ReadKey().KeyChar;
            SendKey(pressedKeyChar);
        }

        public async void ClientMain()
        {
            Console.WriteLine("Started client...");

            while (true)
            {
                string recievedMessage = ReceiveMessage();
                switch (recievedMessage[0])
                {
                    case '1':
                        Console.WriteLine(recievedMessage.Substring(1));
                        break;
                    case '3':
                        RespondToKeyRequest(recievedMessage);
                        break;
                }
            }
        }
    }
}
