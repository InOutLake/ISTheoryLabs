using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    public class Server
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

        string recieved;
        string toSend;

        public Server()
        {
            tmp = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\config.ini")[0].Remove(0, 12);
            allPlayers = CSVEdtior.ReadPlayers(tmp);
            serverEP = new IPEndPoint(IPADDRESS, SERVERPORT);
            clientEP = new IPEndPoint(IPADDRESS, CLIENTPORT);
            serverReceiver = new UdpClient(serverEP);
            serverSender = new UdpClient();
        }


        async Task SendMessageAsync()
        {
            while (true)
            {
                if (toSend != "wait")
                {
                    byte[] data = Encoding.UTF8.GetBytes(toSend);
                    await serverSender.SendAsync(data, clientEP);
                    Console.WriteLine($"SEND: {toSend}");
                    toSend = "wait";
                }
            }
        }

        async Task ReceiveMessageAsync()
        {
            while (true)
            {
                var result = await serverReceiver.ReceiveAsync();
                recieved = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine($"RECIEVED: {recieved}");
            }
        }

        public void sendWhenReady(string s)
        {
            bool isSent = false;
            while (!isSent)
            {
                toSend = s;
                isSent = true;
            }
        }
        public void WriteOnClient(string s)
        {
            sendWhenReady("w" + s);
        }

        public async Task<ConsoleKeyInfo> ReadKeyInfoFromClient()
        {
            UdpReceiveResult receivedData = new UdpReceiveResult();
            while (receivedData.Buffer == null) {
                try
                {
                    receivedData = await serverReceiver.ReceiveAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            char keyChar = Convert.ToChar(receivedData.Buffer[0]);
            Console.WriteLine($"Key {keyChar} has been received from client {clientEP}!");
            return new ConsoleKeyInfo(keyChar, (ConsoleKey)keyChar, false, false, false);
        }

        //public async Task SendPlayerByLogin()
        //{
        //    string login = await RequestStringWithMessage("Enter Player's login: ");
        //    Player p = allPlayers.Find(x => x.Login == login);
        //    await SendStringToClient($"{HEADER}\n{p.ToString()}");
        //}

        //public async Task AddNewPlayer(IPEndPoint clientEP)
        //{
        //    string login = await RequestStringWithMessage("\nLogin: ");
        //    string password = await RequestStringWithMessage("Password: ");
        //    string race = await RequestStringWithMessage("Race: ");
        //    string gClass = await RequestStringWithMessage("Class: ");
        //    string guild = await RequestStringWithMessage("Guild: ");
        //    int level = 0;
        //    int balance = 0;
        //    bool isAdmin = false;
        //
        //    try
        //    {
        //        isAdmin = await RequestStringWithMessage("Admin (0 - false, 1 - true):") == "1" ? true : false;
        //        Player newPlayer = new Player(login, password, race, gClass, guild, level, balance, isAdmin);
        //        allPlayers.Add(newPlayer);
        //        await SendStringToClient($"Added new player:\n {newPlayer.ToString()}");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Wrong input! Interrupting creation...");
        //        await SendStringToClient("Wrong input! Interrupting creation...");
        //    }
        //}
        //
        //public async Task RemovePlayerByLogin(IPEndPoint clientEP)
        //{
        //    string login = await RequestStringWithMessage("Enter Player's login: ");
        //    allPlayers.RemoveAll(x => x.Login == login);
        //    await SendStringToClient($"{login}'s profile has been deleted >:D");
        //}

        public void WriteCommandsInfo()
        {
            WriteOnClient(COMMANDSINFO);
        }

        public void WriteAllPlayers()
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
            Task.Run(ReceiveMessageAsync);
            Task.Run(SendMessageAsync);
            
            while (true)
            {
                WriteCommandsInfo();
                //ConsoleKeyInfo command = await RequestKey();
                //
                //switch (command.Key)
                //{
                //    case ConsoleKey.D1:
                //        await SendAllPlayers();
                //        break;
                //    case ConsoleKey.D2:
                //        await SendPlayerByLogin();
                //        break;
                //    case ConsoleKey.D3:
                //        await AddNewPlayer(clientEP);
                //        break;
                //    case ConsoleKey.D4:
                //        await RemovePlayerByLogin(clientEP);
                //        break;
                //    case ConsoleKey.Escape:
                //        OnClientClose();
                //        break;
                //}
            }
        }
    }
}
