namespace Server.DB.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Race { get; set; }
        public string? GClass { get; set; }
        public string? Guild { get; set; }
        public int Level { get; set; }
        public int Balance { get; set; }
        public bool IsAdmin { get; set; }
    }
}
