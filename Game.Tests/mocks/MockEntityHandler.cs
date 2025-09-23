/// <summary>
/// Service to mock an entityHandler.
/// Returns default values for entities.
/// </summary>
public class MockEntityHandler : EntityHandler
{
    public MockEntityHandler()
    {
        ServiceLocator.Unregister<EntityHandler>();
        ServiceLocator.Register<EntityHandler>(this);
    }

    public override int EvaluateCollision(int id1, int id2)
    {
        return id1;
    }
}
