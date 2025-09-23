using System.Numerics;

[TestClass]
public class GridTests
{
    private Grid CreateDefaultGrid()
    {
        return new Grid(columns: 5, rows: 5, cellSize: 10, offsetX: 0, offsetY: 0);
    }

    [TestMethod]
    public void Constructor_ShouldInitializeCellsCorrectly()
    {
        var grid = CreateDefaultGrid();

        Assert.AreEqual(10, grid.CellSize);
        Assert.AreEqual(5, grid.Columns);
        Assert.AreEqual(5, grid.Rows);
        Assert.AreEqual(0, grid.OffsetX);
        Assert.AreEqual(0, grid.OffsetY);

        Assert.AreEqual(0, grid.Cells[0, 0]);
    }

    [TestMethod]
    public void GetOffset_ShouldReturnCorrectTuple()
    {
        var grid = new Grid(5, 5, 10, 3, 4);

        var result = grid.GetOffset();

        Assert.AreEqual((3, 4), result);
    }

    [TestMethod]
    public void GetDimensions_ShouldReturnCorrectSize()
    {
        var grid = new Grid(7, 9, 10, 0, 0);

        var result = grid.GetDimensions();

        Assert.AreEqual((7, 9), result);
    }

    [TestMethod]
    public void CheckIfEmptyCell_ShouldReturnTrueForNewCell()
    {
        var grid = CreateDefaultGrid();

        Assert.IsTrue(grid.CheckIfEmptyCell(2, 2));
    }

    [TestMethod]
    public void FreeCell_ShouldClearOccupiedCell()
    {
        var grid = CreateDefaultGrid();
        grid.Cells[2, 2] = 42;

        grid.FreeCell(new CellCoordinates(2, 2));

        Assert.IsTrue(grid.CheckIfEmptyCell(2, 2));
    }

    [TestMethod]
    public void OccupyCell_ShouldStoreInOccupancyDict()
    {
        var grid = CreateDefaultGrid();
        var coord = new CellCoordinates(1, 1);

        grid.OccupyCell(coord, 99);

        grid.Update();

        Assert.AreEqual(99, grid.Cells[1, 1]);
    }

    [TestMethod]
    [DataRow(0, 0, true)]
    [DataRow(4, 4, true)]
    [DataRow(5, 5, false)]
    [DataRow(-1, 0, false)]
    public void CheckIfInGrid_IntCoordinates_ShouldReturnExpected(int x, int y, bool expected)
    {
        var grid = CreateDefaultGrid();

        Assert.AreEqual(expected, grid.CheckIfInGrid(x, y));
    }

    [TestMethod]
    [DataRow(5, 5, true)]
    [DataRow(60, 0, false)]
    public void CheckIfInGrid_Vector2_ShouldReturnExpected(float x, float y, bool expected)
    {
        var grid = CreateDefaultGrid();

        Assert.AreEqual(expected, grid.CheckIfInGrid(new Vector2(x, y)));
    }

    [TestMethod]
    public void ToWorld_ShouldConvertToCorrectVector()
    {
        var grid = CreateDefaultGrid();

        Vector2 pos = grid.ToWorld(2, 3);

        Assert.AreEqual(new Vector2(20, 30), pos);
    }

    [TestMethod]
    public void ToGrid_ShouldConvertBackToCellCoordinates()
    {
        var grid = CreateDefaultGrid();

        CellCoordinates coords = grid.ToGrid(new Vector2(25, 35));

        Assert.AreEqual(2, coords.X);
        Assert.AreEqual(3, coords.Y);
    }

    [TestMethod]
    public void ToWorld_ShouldThrow_WhenOutOfGrid()
    {
        var grid = CreateDefaultGrid();

        Assert.ThrowsExactly<ArgumentException>(() => grid.ToWorld(10, 10));
    }

    [TestMethod]
    public void CheckIfNeumannNeighborhood_ShouldReturnFalse_WhenAllEmpty()
    {
        var grid = CreateDefaultGrid();
        var coord = new CellCoordinates(2, 2);

        Assert.IsFalse(grid.CheckIfNeumannNeighborhood(coord));
    }

    [TestMethod]
    public void CheckIfMooreNeighborhood_ShouldReturnTrue_WhenNeighborOccupied()
    {
        var grid = CreateDefaultGrid();
        grid.Cells[2, 3] = 1; // neighbor occupied
        var coord = new CellCoordinates(2, 2);

        Assert.IsTrue(grid.CheckIfMooreNeighborhood(coord));
    }

    [TestMethod]
    public void Update_ShouldResolveCollisionUsingEntityHandler()
    {
        ServiceLocator.Reset();
        MockEntityHandler mockEntityHandler = new();
        var grid = CreateDefaultGrid();
        var coord = new CellCoordinates(1, 1);

        grid.Cells[1, 1] = 5; // already occupied
        grid.OccupyCell(coord, 10);
        grid.Update();

        Assert.AreEqual(10, grid.Cells[1, 1]);
    }
}
