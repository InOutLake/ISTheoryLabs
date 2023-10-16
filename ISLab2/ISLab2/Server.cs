using NLog;
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

        Logger logger;

        volatile string messageReceived;

        public Server()
        {
            tmp = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Dependencies\\config.ini")[0].Remove(0, 12);
            allPlayers = CSVEdtior.ReadPlayers(tmp);
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            serverReceiver = new UdpClient(serverEP);
            serverSender = new UdpClient();
            logger = NLog.LogManager.GetCurrentClassLogger();
            messageReceived = "wait";
        }

        void SendMessage(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            serverSender.SendAsync(data, clientEP);
            logger.Info($"SEND: {s}");
            //Console.WriteLine($"SEND: {s}");
        }

        async Task ReceiveMessageAsync()
        {
            while (true)
            {
                var result = await serverReceiver.ReceiveAsync();
                messageReceived = Encoding.UTF8.GetString(result.Buffer);
                logger.Info($"RECEIVED: {messageReceived}");
                //Console.WriteLine($"Recieved: {messageRecieved}");
            }
        }

        string RecieveKey()
        {
            while (messageReceived[0] != '3'){}
            return messageReceived.Substring(1);
        }

        string RecieveString()
        {
            while (messageReceived[0] != '2') {}
            return messageReceived.Substring(1);
        }

        void RequestStringFromClient(string message)
        {
            messageReceived = "wait";
            DisplayOnClient(message);
            SendMessage("2");
        }
        void RequestKeyFromClient(string message)
        {
            DisplayOnClient(message);
            SendMessage("3");
        }
        void DisplayOnClient(string message)
        {
            SendMessage("1" + message);
        }

        void SendAllPlayersInfo()
        {
            string stringToSend = "";
            foreach (Player p in allPlayers)
            {
                stringToSend += p.ToString() + "\n";
            }
            DisplayOnClient(stringToSend);
            messageReceived = "wait";
        }

        void SendPlayerInfoByLogin()
        {
            RequestStringFromClient("Enter player's login: ");
            string login = RecieveString();
            foreach(Player p in allPlayers)
            {
                if (p.Login == login) DisplayOnClient(p.ToString());
            }
        }

        void AddPlayer()
        {
            RequestStringFromClient("Enter login: ");
            string login = RecieveString();

            RequestStringFromClient("Enter Password: ");
            string password = RecieveString();

            RequestStringFromClient("Enter race: ");
            string race = RecieveString();

            RequestStringFromClient("Enter class: ");
            string gClass = RecieveString();

            RequestStringFromClient("Enter guild: ");
            string guild = RecieveString();
            int level = 0;
            int balance = 0;

            RequestStringFromClient("Is player admin? y - yes / n - no");
            bool isAdmin = RecieveString() == "y" ? true : false;

            Player newPlayer = new Player(login, password, race, gClass, guild, level, balance, isAdmin);
            allPlayers.Add(newPlayer);
            logger.Info("Added player: " + newPlayer.ToString());

            DisplayOnClient("Player has been successfully created!");
        }

        void DeletePlayerByLogin()
        {
            RequestStringFromClient("Enter player's login: ");
            string toDelete = RecieveString();
            if (allPlayers.RemoveAll(x => x.Login == toDelete) > 0)
            {
                DisplayOnClient($"Player with login {toDelete} has been removed");
                logger.Info("Removed player: " + toDelete);
            }
            else
            {
                DisplayOnClient($"Player with login {toDelete} was not found");
            }
        }

        public void ServerMain()
        {
            Console.WriteLine("Started server...");
            Task receiveTask = Task.Run(ReceiveMessageAsync);

            bool quit = false;
            while (!quit)
            {
                RequestKeyFromClient(COMMANDSINFO);
                string command = "wait";
                while (command == "wait")
                {
                    command = RecieveKey();
                };
                switch(messageReceived[1])
                {
                    case '1':
                        SendAllPlayersInfo();
                        break;
                    case '2':
                        SendPlayerInfoByLogin();
                        break;
                    case '3':
                        AddPlayer();
                        break;
                    case '4':
                        DeletePlayerByLogin();
                        break;
                    case '5':
                        CSVEdtior.WritePlayers(tmp, allPlayers);
                        logger.Info("Changes pushed");
                        break;
                    case '6':
                        quit = true;
                        break;
                }
            }
            CSVEdtior.WritePlayers(tmp, allPlayers);
            logger.Info("Shut down");
        }
    }
}
