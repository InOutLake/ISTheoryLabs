using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client
{
    public static class Serializer
    {

        // Serialize a ConsoleKeyInfo object into a byte array
        public static byte[] SerializeConsoleKeyInfo(ConsoleKeyInfo consoleKeyInfo)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, consoleKeyInfo);
                return memoryStream.ToArray();
            }
        }

        // Deserialize a byte array back into a ConsoleKeyInfo object
        public static ConsoleKeyInfo DeserializeConsoleKeyInfo(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                return (ConsoleKeyInfo)formatter.Deserialize(memoryStream);
            }
        }
    }
}
