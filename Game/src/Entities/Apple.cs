/* The apple entity. Snakes need to eat apples to grow and gain points.*/

using System.Numerics;
using Raylib_cs;

public class Apple : Entity
{
    #region Related objects
    readonly Grid _appleGrid;
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();
    #endregion

    #region Location properties
    private CellCoordinates _position;
    #endregion

    #region Draw properties
    private Color _color;
    private int _radius;
    #endregion

    public Apple(Grid grid, int radius, Color? color = null)
    {
        _entityID = _entityHandler.Register(this);
        _appleGrid = grid;
        _color = color ??= Color.Red;
        _radius = radius;
        RandomPosition();
        SetActive();
    }

    #region Getters and Setters
    public CellCoordinates GetPosition()
    {
        return _position;
    }

    public void SetPosition(CellCoordinates cell)
    {
        _position = cell;
    }

    public void SetActive()
    {
        _currentState = EntityState.active;
        _appleGrid.OccupyCell(_position, _entityID);
    }

    public void SetDisabled()
    {
        _currentState = EntityState.disabled;
        _appleGrid.FreeCell(_position);
    }
    #endregion

    #region On reset of object
    /// <summary>
    /// On reset of an apple, we choose a new location for it.
    /// </summary>
    public override void Reset()
    {
        RandomPosition();
        SetActive();
    }

    /// <summary>
    /// Generate a new position for our apple.
    /// An apple cannot appear on another object.
    /// </summary>
    public void RandomPosition()
    {
        (int column, int row) = _appleGrid.GetDimensions();
        bool validApplePosition = false;
        while (!validApplePosition)
        {
            int newAppleColumn = RandomGlobal.Next(column);
            int newAppleRow = RandomGlobal.Next(row);
            // In order to avoid situations where the player can't see an apple coming,
            // we avoid to make an apple appear in front of the player.
            if (
                _appleGrid.CheckIfEmptyCell(new(newAppleColumn, newAppleRow))
                && !_appleGrid.CheckIfNeumannNeighborhood(new(newAppleColumn, newAppleRow))
            )
            {
                validApplePosition = true;
                SetPosition(new(newAppleColumn, newAppleRow));
            }
        }
    }
    #endregion

    #region Actions and reactions
    public override void Collide(Entity entity)
    {
        SetDisabled();
    }

    public override void Update(float deltaTime)
    {
        return;
    }
    #endregion

    #region Draw
    public override void Draw()
    {
        int cellSize = _appleGrid.GetCellSize();
        Vector2 worldCoordinates = _appleGrid.ToWorld(_position);
        Raylib.DrawCircle(
            (int)worldCoordinates.X + cellSize / 2,
            (int)worldCoordinates.Y + cellSize / 2,
            _radius,
            _color
        );
    }
    #endregion
}
