/* First level of the game with two snakes and two fruits to grab.*/

using Raylib_cs;

public class Level1 : Level
{
    private int _appleCount = 0;

    #region Related objects
    private PlayerHandler _playerHandler => ServiceLocator.Get<PlayerHandler>();
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private GameOverMenu _gameOverMenu => ServiceLocator.Get<GameOverMenu>();
    #endregion

    #region Grid properties
    private static int _cellSize = 24;
    private static int _columns = 15;
    private static int _rows = 15;
    private static int _offsetX = (_screenWidth - _cellSize * _columns) / 2;
    private static int _offsetY = (_screenHeight - _cellSize * _rows) / 2;
    private Grid _level1Grid = new(_columns, _rows, _cellSize, _offsetX, _offsetY);
    #endregion

    #region  Update properties
    private GameState _currentState = GameState.play;
    private List<int> _appleIDList = new();
    private List<int> _snakeIDList = new();
    private Timer _gameOverTimer = new(10f, false);
    #endregion

    #region HUD Properties
    private static string _title = "Endless mode";
    private static int _titleFontSize = 20;
    private int _titleX = (_screenWidth - Raylib.MeasureText(_title, _titleFontSize)) / 2;
    private int _titleY = _screenHeight / 10;
    private Color _titleColor = Color.Red;

    private Color _hudColor = Color.White;
    private int _hudY = (_screenHeight + _offsetY + _cellSize * _rows) / 2;
    private static int _fontSize = 18;
    private static int _gapSize = 10;
    private static string _timerTitle = "Remaining Time:";
    private int _timerTitleX = _screenWidth / 4 - Raylib.MeasureText(_timerTitle, _fontSize) / 2;
    private static string _scoreTitle = "Apples Eaten:";
    private int _scoreTitleX = _screenWidth / 2 - Raylib.MeasureText(_scoreTitle, _fontSize) / 2;
    private static string _directionBlockTitle = "Direction blocks left:";
    private int _directionBlockTitleX =
        _screenWidth * 3 / 4 - Raylib.MeasureText(_directionBlockTitle, _fontSize) / 2;
    #endregion

    #region Draw properties
    private new Color _backGroundColor = Color.Black;
    #endregion


    public Level1()
    {
        ServiceLocator.Register<Level1>(this);
    }

    #region Initialization
    public override void Load()
    {
        _gameOverTimer.Reset();
        _currentState = GameState.pause;
        _level1Grid.Reset();
        _appleCount = 0;
        initializeSnake();
        initializeApple();
        initilializePlayer();
    }

    private void initilializePlayer()
    {
        _playerHandler.SetGrid(_level1Grid);
        for (int i = 0; i < 6; i++)
        {
            _playerHandler.FillQueue();
        }
    }

    private void initializeSnake()
    {
        _snakeIDList = new();
        CellCoordinates snakePosition = new(5, 5);
        CellCoordinates secondSnakePosition = new(5, 10);
        Snake snake = new(snakePosition, _level1Grid, 3);
        Snake secondSnake = new(secondSnakePosition, _level1Grid, 3);
        _snakeIDList.Add(snake.GetID());
        _snakeIDList.Add(secondSnake.GetID());
        _level1Grid.Update();
    }

    private void initializeApple()
    {
        _appleIDList = new();
        Apple apple = new(_level1Grid, _cellSize / 4);
        Apple secondapple = new(_level1Grid, _cellSize / 4);
        _appleIDList.Add(apple.GetID());
        _appleIDList.Add(secondapple.GetID());
        _level1Grid.Update();
    }
    #endregion

    #region Scene transitions
    public override void Unload()
    {
        _entityHandler.Reset();
        _playerHandler.Reset();
    }

    private void GameOver()
    {
        _sceneHandler.SetNewScene(_gameOverMenu);
    }
    #endregion

    #region Update
    public override void Update(float deltaTime)
    {
        _playerHandler.Update(deltaTime);
        bool pause = _playerHandler.GetPause();
        _currentState = pause ? GameState.pause : GameState.play;
        if (!pause)
        {
            _entityHandler.Update(deltaTime);
            CheckTimeOver(deltaTime);
        }
        _level1Grid?.Update();
        CheckSnake();
        CheckApple();
        if (_currentState == GameState.gameOver)
        {
            GameOver();
        }
    }

    private void CheckApple()
    {
        foreach (int appleID in _appleIDList)
        {
            if (_entityHandler.CheckIfActive(appleID))
            {
                return;
            }
        }
        _gameOverTimer.Reset();
        foreach (int appleID in _appleIDList)
        {
            _entityHandler.GetEntity(appleID).Reset();
            _appleCount++;
        }
    }

    private void CheckSnake()
    {
        foreach (int snakeID in _snakeIDList)
        {
            if (!_entityHandler.CheckIfActive(snakeID))
            {
                _currentState = GameState.gameOver;
            }
        }
    }

    private void CheckTimeOver(float deltaTime)
    {
        bool TimeOver = _gameOverTimer.Update(deltaTime);
        if (TimeOver)
        {
            GameOver();
        }
    }

    #endregion

    #region Draw
    public override void Draw()
    {
        DrawBackground();
        DrawGrid();
        DrawHud();
        _playerHandler?.Draw();
    }

    public void DrawBackground()
    {
        Raylib.ClearBackground(_backGroundColor);
    }

    public void DrawGrid()
    {
        _level1Grid?.Draw();
    }
    #endregion

    public void DrawHud()
    {
        Raylib.DrawText(_title, _titleX, _titleY, _titleFontSize, _titleColor);

        Raylib.DrawText(_timerTitle, _timerTitleX, _hudY, _fontSize, _hudColor);

        Raylib.DrawText(
            _gameOverTimer.GetTime().ToString(),
            _timerTitleX,
            _hudY + _fontSize + _gapSize,
            _fontSize,
            _hudColor
        );

        Raylib.DrawText(_scoreTitle, _scoreTitleX, _hudY, _fontSize, _hudColor);

        Raylib.DrawText(
            _appleCount.ToString(),
            _scoreTitleX,
            _hudY + _fontSize + _gapSize,
            _fontSize,
            _hudColor
        );

        Raylib.DrawText(_directionBlockTitle, _directionBlockTitleX, _hudY, _fontSize, _hudColor);

        Raylib.DrawText(
            _playerHandler.GetRemainingBlockCount().ToString(),
            _directionBlockTitleX,
            _hudY + _fontSize + _gapSize,
            _fontSize,
            _hudColor
        );
    }
}
