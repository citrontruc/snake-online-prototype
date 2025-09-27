using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SnakeClient
{
    private static DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private NetworkStream? _stream;

    public SnakeClient()
    {
        ServiceLocator.Register<SnakeClient>(this);
    }

    public void Reset()
    {
        _stream = null;
    }

    public static void JoinServer(string serverIP)
    {
        TcpClient client = new TcpClient(serverIP, _dotNetVariables.ServerPort);
        NetworkStream _stream = client.GetStream();
    }

    public void SendStringMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        _stream?.Write(data, 0, data.Length);
    }
}
