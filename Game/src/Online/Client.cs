using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class SnakeClient
{
    public static void JoinServer()
    {
        Console.Write("Enter server IP: ");
        string serverIp = Console.ReadLine() ?? "";

        TcpClient client = new TcpClient(serverIp, 5000);
        Console.WriteLine("Connected to server!");

        NetworkStream stream = client.GetStream();

        // Background thread to receive commands
        new Thread(() =>
        {
            byte[] buffer = new byte[256];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Server says: " + msg);
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
