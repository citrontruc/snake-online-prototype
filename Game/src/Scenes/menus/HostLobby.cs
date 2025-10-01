using System.Numerics;
using Raylib_cs;

public class HostLobby : Menu
{
    #region Related objects
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private MainMenu _mainMenu => ServiceLocator.Get<MainMenu>();
    private OnlineLevel _onlineLevel => ServiceLocator.Get<OnlineLevel>();
    private Connection _gameConnection = new();
    #endregion

    private SnakeServer _snakeServer => ServiceLocator.Get<SnakeServer>();
    private int _numPlayers = 0;
    private bool _hasPlayers = false;
    private string _serverPassKey = "";

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
    }

    public override void Load()
    {
        _gameConnection = new();
        _snakeServer.SetConnection(_gameConnection);
        //_serverPassKey = CreatePassKey();
        LaunchServer();

        ResetOption();
        AddOption("Back to menu", ReturnToMainMenu);
        _selectedOption = 0;
    }

    public override void Unload() { }

    public void LaunchServer()
    {
        _snakeServer?.LaunchServer();
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

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (!_hasPlayers && _gameConnection.CheckIfHasPlayer())
        {
            AddOption("Start Game", ConfirmInformation);
        }
        _hasPlayers = _gameConnection.CheckIfHasPlayer();
    }

    #region Scene Transitions
    public void ConfirmInformation()
    {
        _onlineLevel.SetPlayerRole(OnlineLevel.PlayerRole.Server);
        _onlineLevel.SetConnection(_gameConnection);
        InitializeGame message = new(1);
        _gameConnection?.SendMessage(message);
        _sceneHandler.SetNewScene(_onlineLevel);
    }

    public void ReturnToMainMenu()
    {
        _sceneHandler.SetNewScene(_mainMenu);
    }
    #endregion

    /// <summary>
    /// TODO : draw the passkey of the game on the screen.
    /// </summary>
    public override void Draw()
    {
        base.Draw();
    }
}
