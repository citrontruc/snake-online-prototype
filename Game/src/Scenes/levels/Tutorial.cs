/* A tutorial level to let the User familiarize himself with the controls. */

using Raylib_cs;

public class Tutorial : Level
{
    #region Related objects
    private PlayerHandler _playerHandler => ServiceLocator.Get<PlayerHandler>();
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();
    private SceneHandler _sceneHandler => ServiceLocator.Get<SceneHandler>();
    private GameOverMenu _gameOverMenu => ServiceLocator.Get<GameOverMenu>();
    private Level1 _level1 => ServiceLocator.Get<Level1>();
    #endregion

    #region Grid properties
    private static int _cellSize = 24;
    private static int _columns = 10;
    private static int _rows = 10;
    private static int _offsetX = _screenWidth / 10;
    private static int _offsetY = (_screenHeight - _cellSize * _columns) / 2;
    private Grid _tutorialGrid = new(_columns, _rows, _cellSize, _offsetX, _offsetY);
    #endregion

    #region HUD Properties
    private static string _title = "Tutorial";
    private static int _titleFontSize = 20;
    private int _titleX =
        _offsetX + (_cellSize * _columns - Raylib.MeasureText(_title, _titleFontSize)) / 2;
    private int _titleY = _screenHeight / 10;
    private Color _titleColor = Color.Red;
    private int _hudPositionX = 2 * _offsetX + _columns * _cellSize;
    private int _maxX = _screenWidth * 9 / 10; // Avoid to write too far on the right.
    private int _hudPositionY = _screenHeight / 10;
    private int _fontSize = 18;
    private int _gapSize = 4;
    private List<string> _instructions =
    [
        "Your task is to make the snake eat apples.",
        "In order to do so, place Direction blocks in its path to change the direction he's heading in.",
        "Choose the direction of the block to place with the arrow keys and place the block by clicking on a cell with the mouse.",
        "If you need time to place your blocks, press space to pause / restart the game.",
        "This is a tutorial level, so you have no constraints on time or the number of Directions block to put.",
        "Press space to start the level. The tutorial is over when you eat 6 apples.",
    ];
    #endregion

    #region  Update properties
    private GameState _currentState = GameState.play;
    private List<int> _appleIDList = new();
    private List<int> _snakeIDList = new();
    private Timer _gameOverTimer = new(3600f, false);
    private int _appleCounter = 0;
    private int _appleObjective = 3;
    #endregion


    #region Draw properties
    private new Color _backGroundColor = Color.Black;
    #endregion


    public Tutorial()
    {
        ServiceLocator.Register<Tutorial>(this);
    }

    #region Initialization
    public override void Load()
    {
        _gameOverTimer.Reset();
        _currentState = GameState.pause;
        _tutorialGrid.Reset();
        _appleCounter = 0;
        initializeSnake();
        initializeApple();
        initilializePlayer();
    }

    private void initilializePlayer()
    {
        _playerHandler.SetGrid(_tutorialGrid);
        for (int i = 0; i < 60; i++)
        {
            _playerHandler.FillQueue();
        }
    }

    private void initializeSnake()
    {
        _snakeIDList = new();
        CellCoordinates snakePosition = new(5, 5);
        Snake snake = new(snakePosition, _tutorialGrid, 3);
        _snakeIDList.Add(snake.GetID());
        _tutorialGrid.Update();
    }

    private void initializeApple()
    {
        _appleIDList = new();
        Apple apple = new(_tutorialGrid, _cellSize / 4);
        Apple secondapple = new(_tutorialGrid, _cellSize / 4);
        _appleIDList.Add(apple.GetID());
        _appleIDList.Add(secondapple.GetID());
        _tutorialGrid.Update();
    }
    #endregion

    #region Scene transitions
    public override void Unload()
    {
        _entityHandler.Reset();
        _playerHandler.Reset();
        _tutorialGrid.Reset();
    }

    private void GameOver()
    {
        _sceneHandler.SetNewScene(_gameOverMenu);
    }

    private void NextLevel()
    {
        _sceneHandler.SetNewScene(_level1);
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
        _tutorialGrid?.Update();
        CheckSnake();
        CheckApple();
        if (_currentState == GameState.gameOver)
        {
            GameOver();
        }
        if (_currentState == GameState.complete)
        {
            NextLevel();
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
        foreach (int appleID in _appleIDList)
        {
            _entityHandler.GetEntity(appleID).Reset();
        }
        _gameOverTimer.Reset();
        _appleCounter++;
        if (_appleCounter >= _appleObjective)
        {
            _currentState = GameState.complete;
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
        DrawHUD();
        _playerHandler?.Draw();
    }

    public void DrawBackground()
    {
        Raylib.ClearBackground(_backGroundColor);
    }

    public void DrawGrid()
    {
        _tutorialGrid?.Draw();
    }

    public void DrawHUD()
    {
        Raylib.DrawText(_title, _titleX, _titleY, _titleFontSize, _titleColor);

        int numSentence = 0;
        string sentence = "";
        foreach (string instructionSentence in _instructions)
        {
            foreach (char c in instructionSentence)
            {
                sentence += c;
                if (Raylib.MeasureText(sentence, _fontSize) > _maxX - _hudPositionX)
                {
                    Raylib.DrawText(
                        sentence,
                        _hudPositionX,
                        _hudPositionY + numSentence * _fontSize + (numSentence - 1) * _gapSize,
                        _fontSize,
                        Color.White
                    );
                    numSentence++;
                    sentence = "";
                }
            }
            Raylib.DrawText(
                sentence,
                _hudPositionX,
                _hudPositionY + numSentence * _fontSize + (numSentence - 1) * _gapSize,
                _fontSize,
                Color.White
            );
            numSentence++;
            numSentence++;
            sentence = "";
        }
    }
    #endregion
}
