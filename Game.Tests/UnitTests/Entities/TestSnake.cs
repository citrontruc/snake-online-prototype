[TestClass]
public class SnakeTests
{
    private Grid _grid = new Grid(10, 10, 10, 0, 0);
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();

    [TestInitialize]
    public void Setup()
    {
        ServiceLocator.Reset();
        EntityHandler entityHandler = new EntityHandler();
        _grid = new Grid(10, 10, 10, 0, 0);
    }

    [TestMethod]
    public void Snake_ShouldRegisterWithEntityHandler()
    {
        var head = new CellCoordinates(3, 3);
        var snake = new Snake(head, _grid, 3);

        var retrieved = _entityHandler.GetEntity(snake.GetID());
        Assert.AreEqual(snake, retrieved, "Snake should be registered in EntityHandler");
    }

    [TestMethod]
    public void Snake_ShouldOccupyCells_OnCreation()
    {
        var head = new CellCoordinates(3, 3);
        var snake = new Snake(head, _grid, 3);
        _grid.Update();

        foreach (var cell in snake.SnakeBody)
        {
            Assert.AreEqual(
                snake.GetID(),
                _grid.Cells[cell.X, cell.Y],
                "Snake should occupy its body cells"
            );
        }
    }

    [TestMethod]
    public void Snake_ShouldMove_AndUpdateBody()
    {
        var head = new CellCoordinates(3, 3);
        var snake = new Snake(head, _grid, 3);
        _grid.Update();

        var oldTail = snake.tail;
        snake.Move();
        _grid.Update();

        Assert.AreEqual(
            3,
            snake.SnakeBody.Count,
            "Snake length should remain the same when not growing"
        );
        Assert.IsFalse(snake.SnakeBody.Contains(oldTail), "Old tail should be freed after moving");
        Assert.AreEqual(
            snake.GetID(),
            _grid.Cells[snake.head.X, snake.head.Y],
            "New head should occupy the grid cell"
        );
        Assert.AreEqual(0, _grid.Cells[oldTail.X, oldTail.Y], "Old tail cell should be freed");
    }

    [TestMethod]
    public void Snake_ShouldGrow_WhenEatingApple()
    {
        var head = new CellCoordinates(3, 3);
        var snake = new Snake(head, _grid, 3);

        var apple = new Apple(_grid, 5);
        apple.SetPosition(snake.head); // Force apple at head

        snake.Collide(apple);
        snake.Move();

        Assert.AreEqual(4, snake.SnakeBody.Count, "Snake should grow by 1 after eating an apple");
    }

    [TestMethod]
    public void Snake_ShouldNotReverseDirection()
    {
        var head = new CellCoordinates(3, 3);
        var snake = new Snake(head, _grid, 3);
        _grid.Update();

        var initialDirection = CellCoordinates.right;
        snake.ChangeDirection(CellCoordinates.left); // Try reversing

        snake.Move();
        _grid.Update();
        Assert.AreEqual(
            head + initialDirection,
            snake.head,
            "Snake should not move in the opposite direction"
        );
    }

    /// <summary>
    ///  Snake should be long enough to provoke collision and grid large enough for the snake.
    /// </summary>
    [TestMethod]
    public void Snake_ShouldDetectSelfCollision()
    {
        var head = new CellCoordinates(5, 5);
        var snake = new Snake(head, _grid, 5);
        _grid.Update();

        // Make snake collide with itself
        snake.ChangeDirection(CellCoordinates.up);
        snake.Move();
        _grid.Update();
        snake.ChangeDirection(CellCoordinates.left);
        snake.Move();
        _grid.Update();
        snake.ChangeDirection(CellCoordinates.down);
        snake.Move();
        _grid.Update();

        Assert.IsTrue(snake.IsCollidingWithItself(), "Snake should detect collision with itself");
    }
}
