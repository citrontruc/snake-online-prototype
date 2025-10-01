using System.Net.Sockets;
using System.Numerics;
using Raylib_cs;

public class JoinLobby : Menu
{
    #region Related objects
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private MainMenu _mainMenu => ServiceLocator.Get<MainMenu>();
    private SnakeClient _snakeClient => ServiceLocator.Get<SnakeClient>();
    private OnlineLevel _onlineLevel => ServiceLocator.Get<OnlineLevel>();
    private Connection _gameConnection = new();
    #endregion

    public enum ConnectionState
    {
        NoHost,
        WaitingForHost,
        HostFound,
    }

    private ConnectionState _state = ConnectionState.NoHost;
    private string _serverIP = "";
    private int MAXINPUTCHAR = 15;
    private string _serverPassword = "";
    private int _letterCount => _serverIP.Count();
    private Rectangle _textBox = new(
        (_screenWidth - Raylib.MeasureText("CONNECTING...", 50)) / 2.0f,
        _screenHeight / 2,
        Raylib.MeasureText("CONNECTING...", 50),
        50
    );

    public JoinLobby()
        : base("Looking for lobbies...")
    {
        Vector2 titlePosition = new(_screenWidth / 2, _screenHeight / 4);
        Vector2 optionPosition = new(_screenWidth / 2, 3 * _screenHeight / 4);

        SetBackgroundcharacteristics(Color.Black);

        SetMenuTitleCharacteristics(
            "Write server IP address",
            titlePosition,
            _screenHeight / 15,
            Color.White,
            true
        );

        SetMenuOptionCharacteristics(
            optionPosition,
            _screenHeight / 40,
            Color.White,
            true,
            _screenHeight / 50
        );

        SetSelectedOptionCharacteristics(1.2f, Color.Red);

        AddOption("Search for Game", SearchForGame);
        AddOption("Back to menu", ReturnToMainMenu);
        _selectedOption = 0;

        ServiceLocator.Register<JoinLobby>(this);
    }

    public override void Load()
    {
        _state = ConnectionState.NoHost;
        _gameConnection = new();
        _snakeClient.SetConnection(_gameConnection);
    }

    public override void Unload() { }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        int key;
        while ((key = Raylib.GetCharPressed()) > 0)
        {
            if ((key >= 32) && (key <= 125) && (_letterCount <= MAXINPUTCHAR))
            {
                _serverIP += (char)key;
            }
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            if (_letterCount > 0)
            {
                _serverIP = _serverIP.Remove(_letterCount - 1, 1);
            }
        }

        if (_state == ConnectionState.WaitingForHost)
        {
            bool newMessage = _gameConnection.CheckIfHasMessage();
            if (newMessage)
            {
                _state = ConnectionState.HostFound;
            }
        }

        if (_state == ConnectionState.HostFound)
        {
            ConfirmInformation();
        }
    }

    #region Scene Transitions
    public void SearchForGame()
    {
        try
        {
            _snakeClient?.JoinServer(_serverIP);
            _state = ConnectionState.WaitingForHost;
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}.");
            _state = ConnectionState.NoHost;
        }
    }

    public void ConfirmInformation()
    {
        _onlineLevel.SetPlayerRole(OnlineLevel.PlayerRole.Client);
        _onlineLevel.SetConnection(_gameConnection);
        _sceneHandler.SetNewScene(_onlineLevel);
    }

    public void ReturnToMainMenu()
    {
        _sceneHandler.SetNewScene(_mainMenu);
    }
    #endregion

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangleRec(_textBox, Color.Red);
        if (_state == ConnectionState.NoHost)
        {
            Raylib.DrawText(
                _serverIP,
                (int)_textBox.X,
                (int)_textBox.Y,
                (int)_textBox.Height,
                Color.White
            );
        }
        if (_state == ConnectionState.WaitingForHost)
        {
            Raylib.DrawText(
                "CONNECTING...",
                (int)_textBox.X,
                (int)_textBox.Y,
                (int)_textBox.Height,
                Color.White
            );
        }
    }
}
