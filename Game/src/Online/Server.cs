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
    private int _maxPlayers = 3;
    private int _currentPlayers = 0;

    public SnakeServer()
    {
        ServiceLocator.Register<SnakeServer>(this);
        _server = new TcpListener(IPAddress.Any, _dotNetVariables.ServerPort);
    }

    public void LaunchServer()
    {
        _server.Start();
        Console.WriteLine("Server started. Waiting for client...");
    }

    /// <summary>
    /// We accept a client connection and check that the client has the right session name.
    /// If the session name is incorrect, we refuse the connection.
    /// </summary>
    public void ConnectToClient()
    {
        TcpClient client = _server.AcceptTcpClient();
        Console.WriteLine("Client connected!");
        NetworkStream stream = client.GetStream();

        // A new client must always specify the session name before connecting.
        byte[] buffer = new byte[256];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        // We accept clients with the correct session name and refuse other clients.
        if (msg == _dotNetVariables.SessionName)
        {
            if (_currentPlayers + 1 < _maxPlayers)
            {
                Console.WriteLine("✅ Correct session name, client accepted");
                _clients.Add(client);
                _currentPlayers++;
            }
            else
            {
                Console.WriteLine("Too many players");
                client.Close();  
            }
        }
        else
        {
            Console.WriteLine(msg);
            Console.WriteLine(_dotNetVariables.SessionName);
            Console.WriteLine("❌ Wrong session name, closing connection");
            client.Close();
        }
    }

    public void CreateClientThreads()
    {
        foreach (TcpClient client in _clients)
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

        // Send commands manually (for testing)
        while (true)
        {
            foreach (var kvp in _latestInputs)
            {
                int player = kvp.Key;
                string message = kvp.Value;
                Console.WriteLine($"{player.ToString()} {message}");
            }
            Thread.Sleep(100);
        }
    }
}
