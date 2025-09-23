/* A class to represent the main menu of our game */

using System.Numerics;
using Raylib_cs;

public class MainMenu : Menu
{
    #region Related objects
    private Level1 _level1 => ServiceLocator.Get<Level1>();
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private Tutorial _tutorial => ServiceLocator.Get<Tutorial>();
    #endregion

    /// <summary>
    /// We initilialize our main menu.
    /// </summary>
    public MainMenu()
        : base("Twin Snakes")
    {
        Vector2 titlePosition = new(_screenWidth / 2, _screenHeight / 3);
        Vector2 optionPosition = new(_screenWidth / 2, 2 * _screenHeight / 3);

        SetBackgroundcharacteristics(Color.Black);

        SetMenuTitleCharacteristics(
            "Twin Snakes",
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

        AddOption("Play Game", LoadLevel);
        AddOption("Play Tutorial", LoadTutorial);
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

    private void LoadLevel()
    {
        _sceneHandler.SetNewScene(_level1);
    }

    public void LoadTutorial()
    {
        _sceneHandler.SetNewScene(_tutorial);
    }
    #endregion
}
