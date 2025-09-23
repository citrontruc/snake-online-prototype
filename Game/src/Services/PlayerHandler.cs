/* Handles player control and player items. */

using System.Numerics;
using Raylib_cs;

public class PlayerHandler
{
    private Grid? _levelGrid;

    #region Related objects
    private Vector2 _playerPosition => _inputHandler.GetUserInput().MousePosition;
    private static InputHandler _inputHandler => ServiceLocator.Get<InputHandler>();
    #endregion

    #region State information
    private bool _pause = true;
    private Queue<DirectionBlock> _blockQueue = new();
    #endregion

    #region Draw properties
    private int _triangleSideLength;
    private Color _triangleColor;
    private CellCoordinates _playerBlockDirection = CellCoordinates.right;
    private Timer _blockTimer = new(.3f, true);
    private bool _blockVisible;
    #endregion

    public PlayerHandler(int triangleSideLength, Color triangleColor)
    {
        _triangleSideLength = triangleSideLength;
        _triangleColor = triangleColor;
        ServiceLocator.Register<PlayerHandler>(this);
    }

    #region Getter and setters
    public void SetGrid(Grid grid)
    {
        _levelGrid = grid;
    }

    public bool GetPause()
    {
        return _pause;
    }

    public int GetRemainingBlockCount()
    {
        return _blockQueue.Count;
    }

    public Queue<DirectionBlock> GetQueue()
    {
        return _blockQueue;
    }

    public void Reset()
    {
        _blockQueue = new();
        _pause = true;
    }
    #endregion

    #region Handle blockQueue
    public void FillQueue()
    {
        if (_levelGrid is not null)
        {
            DirectionBlock directionBlock = new(
                _playerBlockDirection,
                _triangleSideLength,
                CellCoordinates.zero,
                _levelGrid,
                this
            );
            AddToQueue(directionBlock);
        }
        else
        {
            throw new InvalidOperationException("No grid attached to the playerHandler!");
        }
    }

    public void AddToQueue(DirectionBlock block)
    {
        _blockQueue.Enqueue(block);
    }
    #endregion

    #region Update
    public void Update(float deltaTime)
    {
        UserInput userInput = _inputHandler.GetUserInput();
        UpdateDirection(userInput);
        _pause = userInput.Pause ? !_pause : _pause;
        if (userInput.LeftClickPress)
        {
            CreateBlock();
        }

        // Update timers
        bool timerOver = _blockTimer.Update(deltaTime);
        if (timerOver)
        {
            _blockVisible = !_blockVisible;
        }
    }

    private void UpdateDirection(UserInput userInput)
    {
        if (userInput.UpRelease)
        {
            _playerBlockDirection = CellCoordinates.up;
        }
        if (userInput.DownRelease)
        {
            _playerBlockDirection = CellCoordinates.down;
        }
        if (userInput.RightRelease)
        {
            _playerBlockDirection = CellCoordinates.right;
        }
        if (userInput.LeftRelease)
        {
            _playerBlockDirection = CellCoordinates.left;
        }
    }

    private void CreateBlock()
    {
        if (_blockQueue.Any() && _levelGrid is not null)
        {
            if (_levelGrid.CheckIfInGrid(_playerPosition))
            {
                CellCoordinates blockCell = _levelGrid.ToGrid(_playerPosition);
                if (_levelGrid.CheckIfEmptyCell(blockCell.X, blockCell.Y))
                {
                    DirectionBlock directionBlock = _blockQueue.Dequeue();
                    directionBlock.Place(blockCell, _playerBlockDirection);
                }
            }
        }
    }
    #endregion

    #region Draw
    public void Draw()
    {
        if (_blockVisible)
        {
            DrawTools.DrawFullTriangle(
                _playerBlockDirection,
                _playerPosition,
                _triangleSideLength,
                _triangleColor
            );
        }
    }
    #endregion
}
