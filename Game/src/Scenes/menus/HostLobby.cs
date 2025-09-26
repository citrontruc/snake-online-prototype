using System.Numerics;
using Raylib_cs;

public class HostLobby : Menu
{
    #region Related objects
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private MainMenu _mainMenu => ServiceLocator.Get<MainMenu>();
    private OnlineLevel _onlineLevel => ServiceLocator.Get<OnlineLevel>();
    #endregion

    private SnakeServer _snakeServer => ServiceLocator.Get<SnakeServer>();
    private int _numPlayers = 0;
    private string _serverPassKey = "";
    int playerNumber = 0;
    private Rectangle _textBox = new(_screenWidth / 2.0f - 100, 180, _screenHeight / 2, 50);

    public HostLobby()
        : base("Waiting for other players...")
    {
        ServiceLocator.Register<HostLobby>(this);

        Vector2 titlePosition = new(_screenWidth / 2, _screenHeight / 4);
        Vector2 optionPosition = new(_screenWidth / 2, 3 * _screenHeight / 4);

        SetBackgroundcharacteristics(Color.Black);

        SetMenuTitleCharacteristics(
            "Waiting for other players...",
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

        AddOption("Start Game", ConfirmInformation);
        AddOption("Back to menu", ReturnToMainMenu);
        _selectedOption = 0;
    }

    public override void Load()
    {
        _serverPassKey = CreatePassKey();
        //LaunchServer();
        //_snakeServer.CreateClientThreads();
    }

    public override void Unload() { }

    public void LaunchServer()
    {
        _snakeServer.LaunchServer();
        for (int i = 1; i < playerNumber; i++)
        {
            _snakeServer.ConnectToClient();
        }
    }

    private string CreatePassKey()
    {
        string passKey = "";
        for (int i = 0; i < 4; i++)
        {
            passKey += RandomGlobal.Next(9).ToString();
        }
        return passKey;
    }

    public override void Update(float deltaTime) { }

    #region Scene Transitions
    public void ConfirmInformation()
    {
        _onlineLevel.SetPlayerRole(OnlineLevel.PlayerRole.Server);
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
    }
}
