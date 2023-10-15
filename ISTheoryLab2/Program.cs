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

namespace Client
{
    class ClientProgram
    {
        public static void Main(string[] args)
        {
            Client client = new Client();
            client.ClientMain();
        }
    }
}
