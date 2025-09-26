/* Handles player control and player items. */

using System.Numerics;
using Raylib_cs;

public class PlayerHandler
{
    private Grid? _levelGrid;

    #region Related objects
    private static InputHandler _inputHandler => ServiceLocator.Get<InputHandler>();
    #endregion

    #region Draw properties
    private CellCoordinates _playerDirection = CellCoordinates.right;
    #endregion

    public PlayerHandler()
    {
        ServiceLocator.Register<PlayerHandler>(this);
    }

    #region Getter and setters
    public void SetGrid(Grid grid)
    {
        _levelGrid = grid;
    }
    public CellCoordinates GetPlayerDirection()
    {
        return _playerDirection;
    }

    public void Reset()
    {
        _levelGrid = null;
        _playerDirection = CellCoordinates.right;
    }
    #endregion

    #region Update
    public void Update(float deltaTime)
    {
        UserInput userInput = _inputHandler.GetUserInput();
        UpdateDirection(userInput);
    }

    private void UpdateDirection(UserInput userInput)
    {
        if (userInput.UpRelease)
        {
            _playerDirection = CellCoordinates.up;
        }
        if (userInput.DownRelease)
        {
            _playerDirection = CellCoordinates.down;
        }
        if (userInput.RightRelease)
        {
            _playerDirection = CellCoordinates.right;
        }
        if (userInput.LeftRelease)
        {
            _playerDirection = CellCoordinates.left;
        }
    }
    #endregion

    #region Draw
    public void Draw()
    {
    }
    #endregion
}
