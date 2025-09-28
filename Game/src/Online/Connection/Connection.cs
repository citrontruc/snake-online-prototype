using System.Net.Sockets;
using System.Text;

public class Connection
{
    private List<TcpClient> _clients = new();
    private List<NetworkStream> _streams = new();
    private MessageFactory _messageFactory => ServiceLocator.Get<MessageFactory>();

    public bool CheckIfHasPlayer()
    {
        return _clients.Count() > 0;
    }

    public void AddConnection(TcpClient client)
    {
        _clients.Add(client);
        _streams.Add(client.GetStream());
    }

    public void Disconnect()
    {
        foreach (NetworkStream stream in _streams)
        {
            stream.Close();
        }
        foreach (TcpClient client in _clients)
        {
            client.Close();
        }
        _clients = new();
        _streams = new();
    }

    public void SendMessage(Message message)
    {
        byte[] data = new byte[1024];
        data = Encoding.UTF8.GetBytes(_messageFactory.ToJson(message));
        foreach (NetworkStream _playerStream in _streams)
        {
            _playerStream.Write(data, 0, data.Length);
        }
    }

    public bool CheckIfNewMessage()
    {
        foreach (NetworkStream _playerStream in _streams)
        {
            if (_playerStream.DataAvailable)
                return true;
        }
        return false;
    }

    public async Task<List<Message>> ReceiveMessage()
    {
        List<Message> messageList = new();
        foreach (NetworkStream _playerStream in _streams)
        {
            if (!_playerStream.DataAvailable)
                continue; // skip streams with no data

            byte[] buffer = new byte[1024];
            int bytesRead = await _playerStream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string byteString = Encoding.UTF8.GetString(buffer);
                Message? messageValue = _messageFactory.FromJson(byteString);
                if (!(messageValue is null))
                {
                    messageList.Add(messageValue);
                }
            }
        }
        return messageList;
    }
}
