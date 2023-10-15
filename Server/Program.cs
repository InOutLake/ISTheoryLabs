using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

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
