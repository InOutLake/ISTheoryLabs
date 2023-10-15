using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player
    {
        string login;
        string password;
        string race;
        string gClass;
        string guild;
        int level;
        int balance;
        bool isAdmin;

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public string Race { get => race; set => race = value; }
        public string GClass { get => gClass; set => gClass = value; }
        public string Guild { get => guild; set => guild = value; }
        public int Level { get => level; set => level = value; }
        public int Balance { get => balance; set => balance = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }

        public Player(string login, string password, string race, string gClass, string guild, int level, int balance, bool isAdmin)
        {
            this.Login = login;
            this.Password = password;
            this.Race = race;
            this.GClass = gClass;
            this.Guild = guild;
            this.Level = level;
            this.Balance = balance;
            this.IsAdmin = isAdmin;
        }

        public string ToCSV()
        {
            string[] pms = { this.login, this.password, this.race, this.gClass, this.guild, this.level.ToString(), this.Balance.ToString(), this.isAdmin.ToString() };
            string res = string.Join(",", pms);
            return res;
        }

        override public string ToString()
        {
            return this.Login.PadRight(20) + this.Password.PadRight(20) + this.Race.PadRight(10) +
                   this.GClass.PadRight(15) + this.Guild.PadRight(25) + this.Level.ToString().PadRight(5) +
                   this.Balance.ToString().PadRight(9) + this.IsAdmin.ToString().PadRight(5);
        }
    }
}
