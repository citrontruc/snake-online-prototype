/* An object to link together all the different element of the game. Static!*/

using Raylib_cs;

public static class GameHandler
{
    #region Related classes
    private static InputHandler? _inputHandler;
    private static SceneHandler? _sceneHandler;
    private static EntityHandler? _entityHandler;
    #endregion

    #region Display information
    private static readonly int _screenHeight = 600;
    private static readonly int _screenWidth = 800;
    private static readonly int _targetFPS = 60;
    #endregion

    #region Initialization
    public static void Initiliaze()
    {
        InitializeWindow();
        InitiliazeServices();
    }

    private static void InitializeWindow()
    {
        Raylib.SetTargetFPS(_targetFPS);
        Raylib.InitWindow(_screenWidth, _screenHeight, "Snake");
    }

    private static void InitiliazeServices()
    {
        Level1 level1 = new();
        OnlineLevel onlineLevel = new();
        MainMenu mainMenu = new();
        GameOverMenu gameOverMenu = new();
        PlayerHandler playerHandler = new();
        DotNetVariables dotnetVariables = new();
        HostLobby hostLobby = new();
        JoinLobby joinLobby = new();
        SnakeServer snakeServer = new();
        _inputHandler = new();
        _sceneHandler = new(mainMenu);
        _entityHandler = new();
    }
    #endregion

    #region Execution
    public static void RunGame()
    {
        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();
            _inputHandler?.Update();
            _sceneHandler?.Update(dt);
            Draw();
        }
        Raylib.CloseWindow();
    }

    public static void Draw()
    {
        Raylib.BeginDrawing();
        _sceneHandler?.Draw();
        _entityHandler?.Draw();
        Raylib.EndDrawing();
    }
    #endregion
}
