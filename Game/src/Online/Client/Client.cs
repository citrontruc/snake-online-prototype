using System.Net.WebSockets;
using System.Text;

public class SnakeClient
{
    private static DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private ClientWebSocket? _ws;
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
        _ws?.Dispose();
        _ws = null;
    }

    public async Task JoinServer(string serverIP)
    {
        _ws = new ClientWebSocket();
        var uri = new Uri($"ws://{serverIP}:{_dotNetVariables.ServerPort}/");
        await _ws.ConnectAsync(uri, CancellationToken.None);
        Console.WriteLine("Connected to server via WebSocket!");
        _serverConnection.AddConnection(_ws);

        _ = ReceiveLoop();
    }

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024];
        while (_ws != null && _ws.State == WebSocketState.Open)
        {
            var result = await _ws.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await _ws.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Closed",
                    CancellationToken.None
                );
            }
            else
            {
                string byteString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _serverConnection.LoadMessage(byteString);
            }
        }
    }
}
