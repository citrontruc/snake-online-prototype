using System.Net;
using System.Net.WebSockets;
using System.Text;

public class SnakeServer
{
    private DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();
    private HttpListener _listener;
    private Connection _serverConnection = new();

    public SnakeServer()
    {
        ServiceLocator.Register<SnakeServer>(this);
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://+:{_dotNetVariables.ServerPort}/");;
    }

    public void SetConnection(Connection connection)
    {
        _serverConnection = connection;
    }

    public async Task LaunchServer()
    {
        _listener.Start();
        Console.WriteLine("Server started. Waiting for client...");

        while (true)
        {
            HttpListenerContext context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                Console.WriteLine("Client connected via WebSocket!");
                _serverConnection.AddConnection(wsContext.WebSocket);

                _ = HandleClient(wsContext.WebSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async Task HandleClient(WebSocket socket)
    {
        var buffer = new byte[1024];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
            else
            {
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received: " + msg);
            }
        }
    }
}
