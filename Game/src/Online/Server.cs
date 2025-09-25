using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SnakeServer
{
    private DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private TcpListener _server;
    private List<TcpClient> _clients = new();
    private ConcurrentDictionary<int, string> _latestInputs = new();

    public SnakeServer()
    {
        ServiceLocator.Register<SnakeServer>(this);
        _server = new TcpListener(IPAddress.Any, _dotNetVariables.ServerPort);
    }

    public void LaunchServer()
    {
        _server.Start();
        Console.WriteLine("Server started. Waiting for client...");

        TcpClient client = _server.AcceptTcpClient();
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

    public void ConnectToClient()
    {
        TcpClient client = _server.AcceptTcpClient();
        Console.WriteLine("Client connected!");
    }

    public void CheckClientConnection(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
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
    }
}
