/* A connection object contains the list of all the connected sockets and a queue if all messages received.*/

using System.Net.WebSockets;
using System.Text;

public class Connection
{
    #region Connection variables
    private List<WebSocket> _sockets = new();
    private MessageFactory _messageFactory => ServiceLocator.Get<MessageFactory>();
    private Queue<Message> _messageQueue = new();
    #endregion

    #region Getters and Setters
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
    #endregion

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

    #region Message Handling
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
        Message? messageValue = _messageFactory.FromJson(byteString);
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
    #endregion
}
