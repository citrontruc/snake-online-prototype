/* A class to represent the game over screen menu of our game */

using System.Numerics;
using Raylib_cs;

public class GameOverMenu : Menu
{
    #region Related objects
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private MainMenu _mainMenu => ServiceLocator.Get<MainMenu>();
    private Level1 _level1 => ServiceLocator.Get<Level1>();
    #endregion

    /// <summary>
    /// We initilialize our game over screen.
    /// </summary>
    public GameOverMenu()
        : base("Main Menu")
    {
        Vector2 titlePosition = new(_screenWidth / 2, _screenHeight / 3);
        Vector2 optionPosition = new(_screenWidth / 2, 2 * _screenHeight / 3);

        SetBackgroundcharacteristics(Color.Black);

        SetMenuTitleCharacteristics(
            "Game Over",
            titlePosition,
            _screenHeight / 10,
            Color.Red,
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

        AddOption("Retry Game", LoadLevel);
        AddOption("Back to Main Menu", LoadMainMenu);
        AddOption("Quit Game", CloseWindow);
        _selectedOption = 0;
        ServiceLocator.Register<GameOverMenu>(this);
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

    private void LoadMainMenu()
    {
        _sceneHandler.SetNewScene(_mainMenu);
    }

    private void LoadLevel()
    {
        _sceneHandler.SetNewScene(_level1);
    }
    #endregion
}
