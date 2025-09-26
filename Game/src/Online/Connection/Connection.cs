using System.Net.Sockets;
using System.Text;

public abstract class Connection
{
    protected TcpClient tcpClient;
    protected NetworkStream stream;

    public abstract void Connect(string host, int port);
    public abstract void Disconnect();

    public void SendMessage(IMessage message)
    {
        byte[] data = message.Serialize();
        stream.Write(data, 0, data.Length);
    }

    public IMessage ReceiveMessage()
    {
        // Simplified example
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        return MessageFactory.FromBytes(buffer, bytesRead);
    }
}
