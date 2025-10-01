/**/

using System.Net;
using System.Net.WebSockets;
using System.Text;

public class SnakeServer
{
    #region Connection parameters
    private Connection _serverConnection = new();
    private HttpListener _listener;
    #endregion

    private DotNetVariables _dotNetVariables => ServiceLocator.Get<DotNetVariables>();

    public SnakeServer()
    {
        ServiceLocator.Register<SnakeServer>(this);
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://+:{_dotNetVariables.ServerPort}/");
        ;
    }

    #region Getters and setters
    public void SetConnection(Connection connection)
    {
        _serverConnection = connection;
    }
    #endregion

    #region Launch the server and launch message receive loop.
    /// <summary>
    /// We launch our server which will connect in the background and unpack messages.
    /// Note: we should do a verification of who tries to connect.
    /// </summary>
    /// <returns> Async task that recovers message while loop is active. </returns>
    public async Task LaunchServer()
    {
        _listener.Start();

        while (true)
        {
            HttpListenerContext context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
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
            var result = await socket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(
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
    #endregion
}
