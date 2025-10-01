using System.Net.WebSockets;
using System.Text;

public class Connection
{
    private List<WebSocket> _sockets = new();
    private MessageFactory _messageFactory => ServiceLocator.Get<MessageFactory>();
    private Queue<Message> _messageQueue = new();

    public bool CheckIfHasPlayer()
    {
        return _sockets.Count > 0;
    }

    public void AddConnection(WebSocket socket)
    {
        _sockets.Add(socket);
    }

    public bool CheckIfConnected()
    {
        return _sockets.Count() > 0;
    }

    public bool CheckIfHasMessage()
    {
        return _messageQueue.Count() > 0;
    }

    /// <summary>
    /// A method to disconnect all our sockets in order to reset our connection object.
    /// </summary>
    /// <returns></returns>
    public async Task Disconnect()
    {
        foreach (WebSocket socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Closing",
                    CancellationToken.None
                );
            }
            socket.Dispose();
        }
        _sockets = new();
    }

    /// <summary>
    /// A methode to send a message to all the other players
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns></returns>
    public async Task SendMessage(Message message)
    {
        byte[] data = Encoding.UTF8.GetBytes(_messageFactory.ToJson(message));
        foreach (WebSocket socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(
                    new ArraySegment<byte>(data, 0, data.Length),
                    WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }

    public void LoadMessage(string byteString)
    {
        Console.WriteLine(byteString);
        Message? messageValue = _messageFactory.FromJson(byteString);
        Console.WriteLine($"messageValue: {messageValue}");
        if (messageValue is not null)
        {
            _messageQueue.Enqueue(messageValue);
        }
    }

    public Message? ReadMessage()
    {
        if (CheckIfHasMessage())
        {
            return _messageQueue.Dequeue();
        }
        return null;
    }
}
