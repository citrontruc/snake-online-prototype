/* An abstract class to define generic scenes. */

using Raylib_cs;

public abstract class Scene
{
    #region Display information
    protected static int _screenWidth = Raylib.GetScreenWidth();
    protected static int _screenHeight = Raylib.GetScreenHeight();
    #endregion

    /// <summary>
    /// Load any assets necessary for the scene.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Unload all the assets that were previously loaded.
    /// </summary>
    public abstract void Unload();

    /// <summary>
    /// Update the scene.
    /// </summary>
    /// <param name="deltaTime"> Time between frames</param>
    public abstract void Update(float deltaTime);

    public abstract void Draw();
}
