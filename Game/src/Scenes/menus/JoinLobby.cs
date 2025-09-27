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

    private string _serverIP = "";
    private string _serverPassword = "";
    private int _letterCount => _serverIP.Count();
    private Rectangle _textBox = new(_screenWidth / 2.0f - 100, 180, _screenHeight / 2, 50);

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
        _gameConnection = new();
    }

    public override void Unload() { }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    #region Scene Transitions
    public void SearchForGame()
    {
        // Tenter de se connecter. Si succès, on se lance.
        //_snakeClient
        ConfirmInformation();
    }

    public void ConfirmInformation()
    {
        _onlineLevel.SetPlayerRole(OnlineLevel.PlayerRole.Client);
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
