/* A class to represent the main menu of our game */

using System.Numerics;
using Raylib_cs;

public class MainMenu : Menu
{
    #region Related objects
    private HostLobby _hostLobby => ServiceLocator.Get<HostLobby>();
    private JoinLobby _joinLobby => ServiceLocator.Get<JoinLobby>();
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    #endregion

    /// <summary>
    /// We initilialize our main menu.
    /// </summary>
    public MainMenu()
        : base("Snake Online!!!")
    {
        Vector2 titlePosition = new(_screenWidth / 2, _screenHeight / 3);
        Vector2 optionPosition = new(_screenWidth / 2, 2 * _screenHeight / 3);

        SetBackgroundcharacteristics(Color.Black);

        SetMenuTitleCharacteristics(
            "Snake Online!!!",
            titlePosition,
            _screenHeight / 10,
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

        AddOption("Create Online Game", HostLobby);
        AddOption("Join Online Game", JoinLobby);
        AddOption("Quit Game", CloseWindow);
        _selectedOption = 0;
        ServiceLocator.Register<MainMenu>(this);
    }

    public override void Load() { }

    public override void Unload() { }

    #region Actions to take on selecting options
    /// <summary>
    /// We add an action to execute when the user clicks on a button.
    /// </summary>
    private void CloseWindow()
    {
        Raylib.CloseWindow();
    }

    private void HostLobby()
    {
        _sceneHandler.SetNewScene(_hostLobby);
    }

    private void JoinLobby()
    {
        _sceneHandler.SetNewScene(_joinLobby);
    }
    #endregion
}
