using System.Net;
using System.Net.Sockets;


public class SnakeServer
{
    private DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private TcpListener _server;
    private Connection _serverConnection = new();

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

    public void ConnectToClient()
    {
        Console.WriteLine("Waiting for clients to connect...");
        TcpClient client = _server.AcceptTcpClient();
        Console.WriteLine("Client connected!");
        _serverConnection.AddConnection(client);
    }
}
