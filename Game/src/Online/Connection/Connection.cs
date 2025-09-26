using System.Net.Sockets;
using System.Text;

public abstract class Connection
{
    protected TcpClient tcpClient;
    protected NetworkStream stream;

    public abstract void Connect(string host, int port);
    public abstract void Disconnect();

    public void SendMessage(Message message)
    {
        byte[] data = new byte[1024];
        data = Encoding.Default.GetBytes(MessageFactory.ToJson(message));
        stream.Write(data, 0, data.Length);
    }

    public Message ReceiveMessage()
    {
        // Simplified example
        byte[] buffer = new byte[1024];
        string byteString = Encoding.Default.GetString(buffer);
        Message? messageValue = MessageFactory.FromJson(byteString);
        if (!(messageValue is null))
        {
            return messageValue;
        }
        throw new Exception("Decoding message failed");
    }
}
