//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//
//class Program
//{
//    static void Main()
//    {
//        UdpClient server = new UdpClient(8080);
//        Console.WriteLine("Server is running...");
//
//        IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
//        byte[] data = server.Receive(ref clientEP);
//        string message = Encoding.ASCII.GetString(data);
//        Console.WriteLine($"Received message from {clientEP}: {message}");
//
//        // Send a response back to the client
//        byte[] response = Encoding.ASCII.GetBytes("Message received");
//        server.Send(response, response.Length, clientEP);
//    }
//}

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

static class Program
{
    public static void Main(String[] args)
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEP = new IPEndPoint(ip, 8080);
        IPEndPoint clientEP = new IPEndPoint(ip, 8081);

        UdpClient server = new UdpClient(serverEP);
        Console.WriteLine("Server is on");

        byte[] message = server.Receive(ref serverEP);
        Console.WriteLine(Encoding.ASCII.GetString(message));
        Console.ReadKey();
    }
}
