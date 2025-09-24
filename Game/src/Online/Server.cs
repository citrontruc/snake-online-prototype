using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class SnakeServer
{
    public static void LaunchServer()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Server started. Waiting for client...");

        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client connected!");

        NetworkStream stream = client.GetStream();

        // Run in a background thread to receive commands
        new Thread(() =>
        {
            byte[] buffer = new byte[256];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Client says: " + msg);
            }
        }).Start();

        // Send commands manually (for testing)
        while (true)
        {
            string cmd = Console.ReadLine() ?? "";
            byte[] data = Encoding.UTF8.GetBytes(cmd);
            stream.Write(data, 0, data.Length);
        }
    }
}
