using Microsoft.EntityFrameworkCore;
using Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class DataBaseConnector
    {
        //public static void TransferDB(string path)
        //{
        //    List<Player> allPlayers = ReadPlayers(path);
        //    using (GameContext db = new GameContext())
        //    {
        //        var players = db.Players.Where(x => x.PlayerID > 0);
        //        foreach (Player p in players)
        //        {
        //            db.Players.Remove(p);
        //        }
        //        foreach (Player p in allPlayers)
        //        {
        //            db.Players.Add(p);
        //        }
        //        db.SaveChanges();
        //    }
        //}
        //public static List<Player> ReadPlayers(string path)
        //{
        //    List<Player> players = new List<Player>();
        //    List<string> lines = new List<string>(File.ReadAllLines(path));
        //    lines.Remove(lines[0]);
        //    foreach (string line in lines)
        //    {
        //        try
        //        {
        //            string[] parts = line.Split(',');
        //            players.Add(new Player(int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4],
        //                                    parts[5], int.Parse(parts[6]),
        //                                    int.Parse(parts[7]), bool.Parse(parts[8])));
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine($"Ошибка чтения в строке:\n{line}\n{e}");
        //        }
        //    }
        //    return players;
        //}
        public static List<Player> ReadPlayers()
        {
            using (GameContext db = new GameContext())
            {
                var allPlayers = db.Players;
                return allPlayers.ToList();
            }
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
        public static void WritePlayers(List<Player> allPlayers)
        {
            using (GameContext db = new GameContext())
            { 
                db.Players.ExecuteDelete();
                db.Players.AddRange(allPlayers);
                db.SaveChanges();
            }
        }
        public static void AddPlayer(Player p)
        {
            using (GameContext db = new GameContext())
            {
                db.Players.Add(p);
                db.SaveChanges();
            }
        }
        public static void RemovePlayer(int id)
        {
            using (GameContext db = new GameContext())
            {
                db.Players.Remove(db.Players.Find(id));
                db.SaveChanges();
            }
        }
        public static Player ReadPlayer(int id)
        {
            using (GameContext db = new GameContext())
            {
                return db.Players.Find(id);
            }
        }
    }
}
