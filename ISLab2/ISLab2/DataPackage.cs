using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal struct DataPackage
    {
        Commands command;
        string body;

        public DataPackage(string s)
        {
            command = (Commands)int.Parse(s[0].ToString());
            body = s[1..];
        }
    }
}
