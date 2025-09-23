namespace Game.Tests;

[TestClass]
public class TestCell
{
    [TestMethod]
    public void Constructor_ShouldAssignValuesCorrectly()
    {
        var coord = new CellCoordinates(3, 4);

        Assert.AreEqual(3, coord.X);
        Assert.AreEqual(4, coord.Y);
    }

    [TestMethod]
    public void StaticProperties_ShouldReturnExpectedValues()
    {
        Assert.AreEqual(new CellCoordinates(0, 0), CellCoordinates.zero);
        Assert.AreEqual(new CellCoordinates(-1, 0), CellCoordinates.left);
        Assert.AreEqual(new CellCoordinates(1, 0), CellCoordinates.right);
        Assert.AreEqual(new CellCoordinates(0, -1), CellCoordinates.up);
        Assert.AreEqual(new CellCoordinates(0, 1), CellCoordinates.down);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(0)]
    [DataRow(-1)]
    public void TestMultiplicationSymmetry(int k)
    {
        CellCoordinates testCell = new(1, 2);
        Assert.IsTrue(testCell * k == k * testCell);
    }

    [TestMethod]
    public void OperatorPlus_ShouldAddCoordinates()
    {
        var a = new CellCoordinates(2, 3);
        var b = new CellCoordinates(1, -1);

        var result = a + b;

        Assert.AreEqual(new CellCoordinates(3, 2), result);
    }

    [TestMethod]
    public void OperatorMinus_ShouldSubtractCoordinates()
    {
        var a = new CellCoordinates(5, 2);
        var b = new CellCoordinates(1, 3);

        var result = a - b;

        Assert.AreEqual(new CellCoordinates(4, -1), result);
    }

    [TestMethod]
    public void UnaryMinus_ShouldNegateCoordinates()
    {
        var a = new CellCoordinates(3, -2);

        var result = -a;

        Assert.AreEqual(new CellCoordinates(-3, 2), result);
    }

    [TestMethod]
    public void OperatorMultiply_CellByCell_ShouldMultiplyElementWise()
    {
        var a = new CellCoordinates(2, 3);
        var b = new CellCoordinates(4, -1);

        var result = a * b;

        Assert.AreEqual(new CellCoordinates(8, -3), result);
    }

    [TestMethod]
    public void OperatorMultiply_ByScalar_ShouldScaleCoordinates()
    {
        var a = new CellCoordinates(2, -3);

        var result1 = a * 3;
        var result2 = 3 * a;

        Assert.AreEqual(new CellCoordinates(6, -9), result1);
        Assert.AreEqual(new CellCoordinates(6, -9), result2);
    }

    [TestMethod]
    public void EqualityOperator_ShouldReturnTrue_WhenSameCoordinates()
    {
        var a = new CellCoordinates(2, 3);
        var b = new CellCoordinates(2, 3);

        Assert.IsTrue(a == b);
        Assert.IsFalse(a != b);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void EqualityOperator_ShouldReturnFalse_WhenDifferentCoordinates()
    {
        var a = new CellCoordinates(2, 3);
        var b = new CellCoordinates(3, 2);

        Assert.IsFalse(a == b);
        Assert.IsTrue(a != b);
    }

    [TestMethod]
    public void GetHashCode_ShouldBeEqual_ForSameCoordinates()
    {
        var a = new CellCoordinates(4, 5);
        var b = new CellCoordinates(4, 5);

        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Equals_ShouldReturnFalse_ForDifferentType()
    {
        var coord = new CellCoordinates(1, 2);

        Assert.IsFalse(coord.Equals("not a coordinate"));
    }
}
