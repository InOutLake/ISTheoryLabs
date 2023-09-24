using System;
using System.ComponentModel.Design;

namespace ISTheoryLab1
{
    class Program
    {
        public static void Main(string[] args)
        {
            string tmp = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Data.csv"; //path to config.ini
            List<Player> allPlayers = CSVEdtior.ReadPlayers(tmp);

            int inp = 5; //switch on key
            const string MESSAGE = "\nAvailable commands:\n1 - All Players;\n2 - Player by login\n3 - Add Player\n4 - Delete Player\n0 - Save and Exit\n\nEnter action code: ";
            string HEADER = "Login".PadRight(20) + "Password".PadRight(20) + "Race".PadRight(10) +
                            "Calss".PadRight(15) + "Guild".PadRight(25) + "LVL".PadRight(5) +
                            "Balance".PadRight(9) + "Admin".PadRight(5);
            while (inp > 0)
            {
                Console.WriteLine(MESSAGE);
                try
                {
                    inp = int.Parse(Console.ReadLine()); 
                    if (inp > 4 || inp < 0)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception e) 
                {
                    Console.WriteLine("Wrong input!");
                    inp = 5;
                }

                string login;

                switch (inp)
                {
                    case 1:
                        Console.WriteLine(HEADER);
                        foreach (Player p in allPlayers)
                        {
                            Console.WriteLine(p.ToString());
                        }
                        break;
                    case 2:
                        Console.Write("Enter Player's login: "); //
                        login = Console.ReadLine();
                        try
                        {
                            Player p = allPlayers.Find(x => x.Login == login);
                            Console.WriteLine($"{HEADER}\n{p.ToString()}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("There is no such player");
                        }
                        break;
                    case 3:
                        Console.Write("Login: ");
                        login = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = Console.ReadLine();
                        Console.Write("Race: ");
                        string race = Console.ReadLine();
                        Console.Write("Class: ");
                        string gClass = Console.ReadLine();
                        Console.Write("Guild: ");
                        string guild = Console.ReadLine();
                        int level = 0;
                        int balance = 0;
                        bool isAdmin = false;
                        Console.WriteLine("Adimin (0 - false, 1 - true:");
                        try
                        {
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    isAdmin = true; //ternar operator
                                    break;
                                case "2":
                                    isAdmin = false;
                                    break;
                            }
                            Player newPlayer = new Player(login, password, race, gClass, guild, level, balance, isAdmin);
                            allPlayers.Add(newPlayer);
                            Console.WriteLine($"Added new player:\n {newPlayer.ToString()}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Wrong input! Interrupting creation...");
                        }
                        break;
                    case 4:
                        Console.Write("Login: ");
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
            CSVEdtior.WritePlayers(tmp, allPlayers);
            Console.WriteLine("Thanks for using our player manager!"); //client requests a string and fill it in the gaps
        }
    }
}