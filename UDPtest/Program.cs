//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//
//class Program
//{
//    static void Main()
//    {
//        UdpClient client = new UdpClient();
//        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
//
//        string message = "Hello, server!";
//        byte[] data = Encoding.ASCII.GetBytes(message);
//        Thread.Sleep(3000);
//        // Send the message to the server
//        client.Send(data, data.Length, serverEP);
//        Console.WriteLine("Message sent to server");
//
//        // Receive the response from the server
//        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
//        byte[] response = client.Receive(ref remoteEP);
//        string responseMessage = Encoding.ASCII.GetString(response);
//        Console.WriteLine($"Received response from {remoteEP}: {responseMessage}");
//
//        client.Close();
//    }
//}


using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

static class Program
{
    public static void Main(String[] args)
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEP = new IPEndPoint(ip, 8080);
        IPEndPoint clientEP = new IPEndPoint(ip, 8081);
        Thread.Sleep(2000);
        UdpClient client = new UdpClient(clientEP);
        Console.WriteLine("Client is on");

        string message = "Signal from client";
        byte[] mBytes = Encoding.ASCII.GetBytes(message);
        client.Send(mBytes, mBytes.Length, serverEP);
        Console.WriteLine("Signal sent");

    }
}