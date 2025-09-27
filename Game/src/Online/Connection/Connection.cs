using System.Net.Sockets;
using System.Text;

public class Connection
{
    private List<TcpClient> _clients = new();
    private List<NetworkStream> _streams = new();

    public void AddConnection(TcpClient client)
    {
        _clients.Add(client);
        _streams.Add(client.GetStream());
    }

    public void Disconnect()
    {
        
    }

    public void SendMessage(Message message)
    {
        byte[] data = new byte[1024];
        data = Encoding.Default.GetBytes(MessageFactory.ToJson(message));
        foreach (NetworkStream _playerStream in _streams)
        {
            _playerStream.Write(data, 0, data.Length);
        }
    }

    public Message ReceiveMessage()
    {
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
