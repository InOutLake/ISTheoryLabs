using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Diagnostics.Metrics;

namespace Server
{
    public class Server
    {
        string COMMANDSINFO = "\nAvailable commands:\n1 - All Players;\n2 - Player by login\n3 - Add Player\n4 - Delete Player\n0 - Save and Exit\n\nEnter action code: ";
        string HEADER = "\nLogin".PadRight(20) + "Password".PadRight(20) + "Race".PadRight(10) +
                        "Calss".PadRight(15) + "Guild".PadRight(25) + "LVL".PadRight(5) +
                        "Balance".PadRight(9) + "Admin".PadRight(5) + "\n"; //q place to store

        string tmp;
        List<Player> allPlayers;
        IPAddress ipAddress;
        int port;
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;

        public void Init()
        {
            tmp = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\config.ini")[0].Remove(0, 12);
            allPlayers = CSVEdtior.ReadPlayers(tmp);
            ipAddress = IPAddress.Any;
            port = 12345;
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            client = new TcpClient();
            Console.WriteLine("Waiting for client...");
        }
        public void SaveData()
        {
            CSVEdtior.WritePlayers(tmp, allPlayers);
        }
        public void OnClientClose()
        {
            SaveData();
            Console.WriteLine("Client disconnected... waiting for connection...");
            client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected... process to main func");
            stream = client.GetStream();
        }
        public void ShutDown()
        {
            SaveData();
        }

        public Server()
        {
            this.Init();
        }

        public ConsoleKeyInfo ReadKeyInfoFromStream()
        {
            byte[] data = new byte[4096];
            int bytesRead = stream.Read(data, 0, data.Length);
            return Serializer.DeserializeConsoleKeyInfo(data);
        }

        public string ReadStringFromStream()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        public void SendCommandsInfo()
        {
            SendStringToClient(COMMANDSINFO);
        }

        public void SendStringToClient(string s)
        {
            byte[] responseBytes = Encoding.ASCII.GetBytes(s);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        public void SendAllPlayers()
        {
            string send = "";
            foreach (Player p in allPlayers)
            {
                send += p.ToString() + "\n";
            }
            SendStringToClient(send);
        }
        public void SendPlayerByLogin()
        {
            SendStringToClient("Enter Player's login: ");
            string login = ReadStringFromStream();
            Player p = allPlayers.Find(x => x.Login == login);
            SendStringToClient($"{HEADER}\n{p.ToString()}");
        }

        public void AddNewPlayer()
        {
            SendStringToClient("\nLogin: ");
            string login = ReadStringFromStream();
            SendStringToClient("Password: ");
            string password = ReadStringFromStream();
            SendStringToClient("Race: ");
            string race = ReadStringFromStream();
            SendStringToClient("Class: ");
            string gClass = ReadStringFromStream();
            SendStringToClient("Guild: ");
            string guild = ReadStringFromStream();
            int level = 0;
            int balance = 0;
            bool isAdmin = false;
            SendStringToClient("Adimin (0 - false, 1 - true:");
            try
            {
                isAdmin = ReadStringFromStream() == "1" ? true : false;
                Player newPlayer = new Player(login, password, race, gClass, guild, level, balance, isAdmin);
                allPlayers.Add(newPlayer);
                SendStringToClient($"Added new player:\n {newPlayer.ToString()}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong input! Interrupting creation...");
            }
        }


        public void RemovePlayerByLogin()
        {
            SendStringToClient("Enter Player's login: ");
            string login = ReadStringFromStream();
            allPlayers.RemoveAll(x => x.Login == login);
            SendStringToClient($"{login}'s profile has been deleted >:D");
        }

        public void Main()
        {
            ConsoleKeyInfo command = new ConsoleKeyInfo();
            bool shutDown = false;
            Console.WriteLine("Client connected... process to main processing");

            while (!shutDown)
            {
                if (client.Connected)
                {
                    SendCommandsInfo();
                    command = ReadKeyInfoFromStream();
                    switch (command.Key)
                    {
                        case ConsoleKey.D1:
                            SendAllPlayers();
                            break;
                        case ConsoleKey.D2:
                            SendPlayerByLogin();
                            break;
                        case ConsoleKey.D3:
                            AddNewPlayer();
                            break;
                        case ConsoleKey.D4:
                            RemovePlayerByLogin();
                            break;
                        case ConsoleKey.Escape:
                            OnClientClose();
                            break;
                    }
                }
                else
                {
                    OnClientClose();
                }
            }
            SaveData();
        }
    }
}
