/* An item the user can place on the playing field in order to make the snake move.*/

using System.Numerics;
using Raylib_cs;

public class DirectionBlock : Entity
{
    #region Related objects
    readonly Grid _blockGrid;
    readonly PlayerHandler _playerHandler;
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();
    #endregion

    #region Main Properties
    private CellCoordinates _direction;
    private CellCoordinates _position;
    #endregion

    #region Draw properties
    private int _triangleSideLength;
    private Color _triangleColor = Color.SkyBlue;
    #endregion

    public DirectionBlock(
        CellCoordinates direction,
        int triangleSideLength,
        CellCoordinates position,
        Grid grid,
        PlayerHandler playerHandler
    )
    {
        _direction = direction;
        _triangleSideLength = triangleSideLength;
        _entityID = _entityHandler.Register(this);
        _position = position;
        _blockGrid = grid;
        _playerHandler = playerHandler;
        _currentState = EntityState.disabled;
    }

    #region Getters & Setters
    public void SetPosition(CellCoordinates position)
    {
        _position = position;
    }

    public void SetDirection(CellCoordinates direction)
    {
        _direction = direction;
    }

    public CellCoordinates GetPosition()
    {
        return _position;
    }

    public CellCoordinates GetDirection()
    {
        return _direction;
    }

    public void SetDisabled()
    {
        _currentState = EntityState.disabled;
        _blockGrid.FreeCell(_position);
        _playerHandler.AddToQueue(this);
    }
    #endregion

    #region Place on grid
    public void Place(CellCoordinates position, CellCoordinates direction)
    {
        bool cellIsEmpty = _blockGrid.CheckIfEmptyCell(position.X, position.Y);
        if (cellIsEmpty)
        {
            SetPosition(position);
            SetDirection(direction);
            _currentState = EntityState.active;
            _blockGrid.OccupyCell(position, _entityID);
        }
    }
    #endregion

    #region On reset of object
    public override void Reset()
    {
        if (_currentState != EntityState.disabled)
        {
            SetDisabled();
        }
    }
    #endregion

    #region Actions and reactions
    public override void Update(float deltaTime)
    {
        return;
    }

    public override void Collide(Entity entity)
    {
        SetDisabled();
    }
    #endregion

    #region Draw
    public override void Draw()
    {
        Vector2 worldPosition = _blockGrid.ToWorld(_position);
        int cellsize = _blockGrid.GetCellSize();
        worldPosition.X += cellsize / 2;
        worldPosition.Y += cellsize / 2;
        DrawTools.DrawFullTriangle(_direction, worldPosition, _triangleSideLength, _triangleColor);
    }
    #endregion
}
