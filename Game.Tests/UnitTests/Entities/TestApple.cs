[TestClass]
public class AppleTests
{
    private Grid _grid = new Grid(5, 5, 10, 0, 0);
    private EntityHandler _entityHandler => ServiceLocator.Get<EntityHandler>();

    [TestInitialize]
    public void Setup()
    {
        ServiceLocator.Reset();
        EntityHandler entityHandler = new EntityHandler();
        _grid = new Grid(5, 5, 10, 0, 0);
    }

    [TestMethod]
    public void Apple_ShouldRegisterWithEntityHandler()
    {
        var apple = new Apple(_grid, 5);
        var retrieved = _entityHandler.GetEntity(apple.GetID());

        Assert.AreEqual(apple, retrieved, "Apple should be registered in EntityHandler");
    }

    [TestMethod]
    public void Apple_ShouldOccupyCell_WhenActive()
    {
        var apple = new Apple(_grid, 5);
        var pos = apple.GetPosition();
        _grid.Update();

        Assert.AreEqual(apple.GetID(), _grid.Cells[pos.X, pos.Y]);
    }

    [TestMethod]
    public void Apple_ShouldFreeCell_WhenDisabled()
    {
        var apple = new Apple(_grid, 5);
        var pos = apple.GetPosition();

        apple.SetDisabled();

        Assert.AreEqual(0, _grid.Cells[pos.X, pos.Y]);
        Assert.AreEqual(Entity.EntityState.disabled, apple.GetState());
    }

    /// <summary>
    /// Tests that on reset, the apple is set active.
    /// Positions for elements are random, an apple can appear twice in a row in the same position.
    /// </summary>
    [TestMethod]
    public void Apple_ShouldReposition_WhenReset()
    {
        var apple = new Apple(_grid, 5);
        apple.Reset();

        Assert.AreEqual(Entity.EntityState.active, apple.GetState());
    }

    [TestMethod]
    public void Apple_ShouldDisable_WhenCollided()
    {
        var apple = new Apple(_grid, 5);

        // Collide with a dummy entity
        DummyEntity dummyEntity = new();
        apple.Collide(dummyEntity);

        Assert.AreEqual(Entity.EntityState.disabled, apple.GetState());
    }
}
