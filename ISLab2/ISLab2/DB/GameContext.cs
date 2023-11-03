using System.Data.Entity;

namespace Server.DB
{
    public class GameContext : DbContext
    {
        public DbSet<Player>? Players { get; set; }
        public GameContext() : base("name=GameContext")
        {

        }
    }
}
