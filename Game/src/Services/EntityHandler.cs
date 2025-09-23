/* An object to verify interactions between different objects. */

public class EntityHandler
{
    private int _entityID = 0;
    private Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();

    public EntityHandler()
    {
        ServiceLocator.Register<EntityHandler>(this);
    }

    public int Register(Entity entity)
    {
        _entityID += 1;
        _entities.Add(_entityID, entity);
        return _entityID;
    }

    #region Getters and Setters
    public Entity GetEntity(int entityId)
    {
        return _entities[entityId];
    }

    public bool CheckIfActive(int entityId)
    {
        return _entities[entityId].GetState() == Entity.EntityState.active;
    }

    public void Reset()
    {
        _entityID = 0;
        _entities.Clear();
    }
    #endregion

    #region Update and reaction of objects
    /// <summary>
    /// Method to update all the entities in our entity handler.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        foreach (KeyValuePair<int, Entity> entity in _entities)
        {
            UpdateEntity(deltaTime, entity.Value);
        }
    }

    public void UpdateEntity(float deltaTime, Entity entity)
    {
        if (entity.GetState() == Entity.EntityState.active)
        {
            entity.Update(deltaTime);
        }
    }

    /// <summary>
    /// Only one entity can stay in a cell.
    /// We run the collision methods and return which entity is still alive after collision.
    /// If we wanted to do something more complicated, we would need three steps:
    ///     - A step to ask for movement.
    ///     - A step to evaluate the feasability of movements depending on collisions.
    ///     - A step to actually move the entities.
    /// </summary>
    /// <param name="index1"> EntityID of the first entity.</param>
    /// <param name="index2"> EntityID of the second entity.</param>
    /// <returns> The id off the entity </returns>
    /// <exception cref="InvalidOperationException"> After collision, both our entities are still alive. </exception>
    virtual public int EvaluateCollision(int index1, int index2)
    {
        Console.WriteLine("Collision");
        Entity entity1 = _entities[index1];
        Entity entity2 = _entities[index2];
        entity1.Collide(entity2);
        entity2.Collide(entity1);
        if (
            entity1.GetState() == Entity.EntityState.active
            || entity1.GetState() == Entity.EntityState.disabled
        )
        {
            return index1;
        }
        if (
            entity1.GetState() == Entity.EntityState.disabled
            || entity1.GetState() == Entity.EntityState.active
        )
        {
            return index2;
        }
        if (
            entity1.GetState() == Entity.EntityState.active
            || entity1.GetState() == Entity.EntityState.active
        )
        {
            throw new InvalidOperationException("Can't put two entities in the same Cell.");
        }
        return 0;
    }
    #endregion

    #region Draw
    public void Draw()
    {
        foreach (KeyValuePair<int, Entity> entity in _entities)
        {
            DrawEntity(entity.Value);
        }
    }

    public void DrawEntity(Entity entity)
    {
        if (entity.GetState() == Entity.EntityState.active)
        {
            entity.Draw();
        }
    }
    #endregion
}
