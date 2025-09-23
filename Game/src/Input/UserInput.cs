/* Class to store our user input. */

using System.Numerics;

public class UserInput
{
    #region Mouse actions
    public Vector2 MousePosition { get; set; }
    public bool LeftClickPress { get; set; }
    public bool LeftClickHold { get; set; }
    public bool LeftClickRelease { get; set; }
    public bool RightClickPress { get; set; }
    public bool RightClickHold { get; set; }
    public bool RightClickRelease { get; set; }
    #endregion

    #region Keyboard Actions
    /// <summary>
    /// Mouse keys
    /// </summary>
    public bool UpHold { get; set; }
    public bool DownHold { get; set; }
    public bool LeftHold { get; set; }
    public bool RightHold { get; set; }

    public bool UpRelease { get; set; }
    public bool DownRelease { get; set; }
    public bool LeftRelease { get; set; }
    public bool RightRelease { get; set; }

    /// <summary>
    /// Other Keys
    /// </summary>
    public bool Enter { get; set; }
    public bool Pause { get; set; }
    #endregion
}
