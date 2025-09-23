namespace Game.Tests;

/// <summary>
/// A test class to test the elements in our utils class.
/// In our case, we mainly test how the Timer object works.
/// </summary>
[TestClass]
public class TestTimer
{
    [TestMethod]
    public void TestTimerOver()
    {
        float initialTime = 0.1f;
        Timer timer = new(initialTime, true);
        bool timerOver = timer.Update(initialTime);
        Assert.IsTrue(timerOver);
    }

    [TestMethod]
    public void TestTimerReset()
    {
        float initialTime = 1f;
        Timer timer = new(initialTime, true);
        timer.Update(initialTime);
        Assert.IsTrue((timer.GetTime() - (int)initialTime) == 0);
    }

    [TestMethod]
    public void TestTimerNotOver()
    {
        float initialTime = 0.1f;
        Timer timer = new(initialTime, true);
        bool timerOver = timer.Update(initialTime / 2);
        Assert.IsFalse(timerOver);
    }

    [TestMethod]
    public void TestTimerOverStoresValue()
    {
        float initialTime = 0.1f;
        Timer timer = new(initialTime, true);
        timer.Update(initialTime / 2);
        bool timerOver = timer.Update(initialTime / 2);
        Assert.IsTrue(timerOver);
    }
}
