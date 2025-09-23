/* An object to handle a level scene in which the player can evolve. */

using Raylib_cs;

public abstract class Level : Scene
{
    protected Color _backGroundColor;

    protected enum GameState
    {
        play,
        pause,
        gameOver,
        complete,
    }

    public Level() { }

    #region Getters and Setters
    public void SetBackgroundcharacteristics(Color backGroundColor)
    {
        _backGroundColor = backGroundColor;
    }
    #endregion

    #region Update
    public override void Update(float deltaTime) { }
    #endregion

    #region Draw functions
    /// <summary>
    /// We let the entity handler handle drawing.
    /// </summary>
    public override void Draw() { }
    #endregion
}
