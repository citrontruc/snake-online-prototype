/* An object to get inputs from the mouse */

using System.Numerics;
using Raylib_cs;

public static class MouseInputHandler
{
    public enum Button
    {
        Left = MouseButton.Left,
        Right = MouseButton.Right,
        Middle = MouseButton.Middle,
    }

    public static Vector2 position => Raylib.GetMousePosition();
    public static float wheelMove => Raylib.GetMouseWheelMove();

    public static bool isButtonDown(Button button)
    {
        return Raylib.IsMouseButtonDown((MouseButton)button);
    }

    public static bool isButtonUp(Button button)
    {
        return Raylib.IsMouseButtonUp((MouseButton)button);
    }

    public static bool isButtonPressed(Button button)
    {
        return Raylib.IsMouseButtonPressed((MouseButton)button);
    }

    public static bool isButtonReleased(Button button)
    {
        return Raylib.IsMouseButtonReleased((MouseButton)button);
    }

    public static void setCursorVisible(bool visible)
    {
        if (visible)
            Raylib.ShowCursor();
        else
            Raylib.HideCursor();
    }

    public static void setCursorLocked(bool locked)
    {
        if (locked)
            Raylib.DisableCursor();
        else
            Raylib.EnableCursor();
    }

    public static bool isCursorLocked => Raylib.IsCursorOnScreen();
}
