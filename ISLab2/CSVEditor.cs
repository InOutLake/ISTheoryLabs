using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class CSVEdtior
    {
        public static List<Player> ReadPlayers(string path)
        {
            List<Player> players = new List<Player>();
            List<string> lines = new List<string>(File.ReadAllLines(path));
            lines.Remove(lines[0]);
            foreach (string line in lines)
            {
                try
                {
                    string[] parts = line.Split(',');
                    players.Add(new Player(parts[0], parts[1], parts[2], parts[3], parts[4],
                                            int.Parse(parts[5]), int.Parse(parts[6]),
                                            bool.Parse(parts[7])));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка чтения в строке:\n{line}\n{e}");
                }
            }
            return players;
        }
        public static void WritePlayers(string path, List<Player> players)
        {
            string toWrite = "Login,Password,Race,GClass,Guild,Level,Balance,IsAdmin\n";
            foreach (Player p in players)
            {
                toWrite += p.ToCSV() + "\n";
            }
            File.WriteAllText(path, toWrite);
        }
    }
}
