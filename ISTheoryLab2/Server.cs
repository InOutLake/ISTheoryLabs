using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ISTheoryLab1
{
    class Server
    {
        string tmp;
        List<Player> allPlayers;
        IPAddress ipAddress;
        int port;
        TcpListener listener;

        public Server()
        {
            this.Init();
        }

        public void Init()
        {
            tmp = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\config.ini")[0].Remove(0, 12);
            allPlayers = CSVEdtior.ReadPlayers(tmp);
            ipAddress = IPAddress.Parse("127.0.0.1");
            port = 12345;
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine("Waiting for client...");
        }
        public void SaveData()
        {
            CSVEdtior.WritePlayers(tmp, allPlayers);
        }
        public void OnClientClose()
        {
            SaveData();
        }

        public static void Main(string[] args)
        {

        }

        public string Respond(ConsoleKeyInfo command) //TODO many things
        {
            const string MESSAGE = "\nAvailable commands:\n1 - All Players;\n2 - Player by login\n3 - Add Player\n4 - Delete Player\n0 - Save and Exit\n\nEnter action code: ";
            string HEADER = "\nLogin".PadRight(20) + "Password".PadRight(20) + "Race".PadRight(10) +
                            "Calss".PadRight(15) + "Guild".PadRight(25) + "LVL".PadRight(5) +
                            "Balance".PadRight(9) + "Admin".PadRight(5) + "\n";

            string res = "";
            string login;

            switch (command.Key)
            {
                case ConsoleKey.D1:
                    res += HEADER;
                    foreach (Player p in allPlayers)
                    {
                        res += p.ToString() + "\n";
                    }
                    break;

                case ConsoleKey.D2:
                    Console.Write("\nEnter Player's login: "); //
                    login = Client.GetLineFromConsole();
                    Player p = allPlayers.Find(x => x.Login == login);
                    res += $"{HEADER}\n{p.ToString()}";
                    break;

                case ConsoleKey.D3:
                    Console.Write("\nLogin: ");
                    login = Client.GetLineFromConsole();
                    Console.Write("Password: ");
                    string password = Client.GetLineFromConsole();
                    Console.Write("Race: ");
                    string race = Client.GetLineFromConsole();
                    Console.Write("Class: ");
                    string gClass = Client.GetLineFromConsole();
                    Console.Write("Guild: ");
                    string guild = Client.GetLineFromConsole();
                    int level = 0;
                    int balance = 0;
                    bool isAdmin = false;
                    Console.WriteLine("Adimin (0 - false, 1 - true:");
                    try
                    {
                        isAdmin = Console.ReadLine() == "1" ? true : false;
                        Player newPlayer = new Player(login, password, race, gClass, guild, level, balance, isAdmin);
                        allPlayers.Add(newPlayer);
                        Console.WriteLine($"Added new player:\n {newPlayer.ToString()}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Wrong input! Interrupting creation...");
                    }
                    break;
                case ConsoleKey.D4:
                    Console.Write("\nLogin: ");
                    login = Console.ReadLine();
                    try
                    {
                        allPlayers.RemoveAll(x => x.Login == login);
                        Console.WriteLine($"{login}'s profile has been deleted >:D");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("There is no such player!");
                    }
                    break;
            }
        }
            return res;
        }
    }
}
