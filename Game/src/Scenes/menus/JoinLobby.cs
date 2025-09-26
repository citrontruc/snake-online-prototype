using System.Numerics;
using Raylib_cs;

public class JoinLobby : Menu
{

    #region Related objects
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private MainMenu _mainMenu => ServiceLocator.Get<MainMenu>();
    #endregion

    private string _serverIP = "";
    private string _serverPassword = "";
    private int _letterCount => _serverIP.Count();
    private Rectangle _textBox = new(_screenWidth/2.0f - 100, 180, _screenHeight / 2, 50);
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

        AddOption("Confirm", ConfirmInformation);
        AddOption("Back to menu", ReturnToMainMenu);
        _selectedOption = 0;

        ServiceLocator.Register<JoinLobby>(this);
    }

    public override void Load() { }

    public override void Unload() { }

    public void Update()
    {

    }
    
    public void ConfirmInformation()
    {

    }

    public void ReturnToMainMenu()
    {
        _sceneHandler.SetNewScene(_mainMenu);
    }


    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangleRec(_textBox, Color.Red);
    }

}
