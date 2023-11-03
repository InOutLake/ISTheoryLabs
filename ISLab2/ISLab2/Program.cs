using Server.DB;
using System.Reflection;
using System.Threading;

namespace Server
{
    class ServerProgram
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.ServerMain();
        }
    }
}
