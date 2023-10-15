using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        string COMMANDSINFO = "\nAvailable commands:\n1 - All Players;\n2 - Player by login\n3 - Add Player\n4 - Delete Player\n0 - Save and Exit\n\nEnter action code: ";
        string HEADER = "\nLogin".PadRight(20) + "Password".PadRight(20) + "Race".PadRight(10) +
                        "Calss".PadRight(15) + "Guild".PadRight(25) + "LVL".PadRight(5) +
                        "Balance".PadRight(9) + "Admin".PadRight(5) + "\n";

        const int CLIENTPORT = 8081;
        const int SERVERPORT = 8080;
        IPAddress IPADDRESS = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEP;
        IPEndPoint clientEP;

        string tmp;
        List<Player> allPlayers;
        UdpClient serverReceiver;
        UdpClient serverSender;

        volatile string messageRecieved;

        public Server()
        {
            tmp = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Dependencies\\config.ini")[0].Remove(0, 12);
            allPlayers = CSVEdtior.ReadPlayers(tmp);
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            serverReceiver = new UdpClient(serverEP);
            serverSender = new UdpClient();
            messageRecieved = "wait";
        }

        void SendMessage(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            serverSender.SendAsync(data, clientEP);
            Console.WriteLine($"SEND: {s}");
        }

        async Task ReceiveMessageAsync()
        {
            while (true)
            {
                var result = await serverReceiver.ReceiveAsync();
                messageRecieved = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine($"Recieved: {messageRecieved}");
            }
        }

        string RecieveKey()
        {
            if (messageRecieved[0] != '3'){ return "wait"; }
            messageRecieved = "wait";
            return messageRecieved.Substring(1);
        }

        void RequestStringFromClient(string message)
        {
            SendMessage("2" +  message);
        }
        void RequestKeyFromClient(string message)
        {
            WriteOnClient(message);
            SendMessage("3");
        }
        void WriteOnClient(string message)
        {
            SendMessage("1" + message);
        }


        void SendCommandsInfo()
        {
            SendMessage(COMMANDSINFO);
        }

        void SendAllPlayersInfo()
        {
            string stringToSend = "";
            foreach (Player p in allPlayers)
            {
                stringToSend += p.ToString() + "\n";
            }
            WriteOnClient(stringToSend);
        }

        public void ServerMain()
        {
            Console.WriteLine("Started server...");
            Task receiveTask = Task.Run(ReceiveMessageAsync);

            bool quit = false;
            while (!quit)
            {
                RequestKeyFromClient(COMMANDSINFO);
                //string command = "wait";
                //while (command == "wait")
                //{
                //    command = RecieveKey();
                //};
                Console.ReadLine();
                switch(messageRecieved[0])
                {
                    case '1':
                        SendAllPlayersInfo();
                        break;
                    case '5':
                        break;
                }
            }
        }
    }
}
