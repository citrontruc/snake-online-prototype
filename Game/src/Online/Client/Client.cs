using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SnakeClient
{
    private static DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private NetworkStream? _stream;
    private Connection _serverConnection = new();

    public SnakeClient()
    {
        ServiceLocator.Register<SnakeClient>(this);
    }

    public void SetConnection(Connection connection)
    {
        _serverConnection = connection;
    }

    public void Reset()
    {
        _stream = null;
    }

    public void JoinServer(string serverIP)
    {
        TcpClient client = new TcpClient(serverIP, _dotNetVariables.ServerPort);
        _serverConnection.AddConnection(client);
    }
}
