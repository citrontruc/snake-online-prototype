/* Handles scene creations and transitions */

public class SceneHandler
{
    #region Properties
    private Scene _currentScene;
    private Scene? _nextScene;
    #endregion

    public SceneHandler(Scene firstScene)
    {
        _currentScene = firstScene;
        _currentScene.Load();
        ServiceLocator.Register<SceneHandler>(this);
    }

    #region Handle the arrival of a new scene
    public void SetNewScene(Scene nextScene)
    {
        if (_nextScene != null)
        {
            throw new InvalidOperationException("A transition is already set");
        }
        _nextScene = nextScene;
    }

    public void LoadNewScene()
    {
        _currentScene?.Unload();
        if (_nextScene is not null)
        {
            _currentScene = _nextScene;
            _nextScene = null;
            _currentScene.Load();
        }
    }
    #endregion

    #region Update Scenes
    public void Update(float deltaTime)
    {
        if (_nextScene != null)
        {
            LoadNewScene();
        }
        _currentScene.Update(deltaTime);
    }

    public void Draw()
    {
        _currentScene.Draw();
    }
    #endregion
}
