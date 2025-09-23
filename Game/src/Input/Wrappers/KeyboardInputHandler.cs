/* An object to get all keyboard inputs */

using Raylib_cs;

public static class KeyboardInputHandler
{
    public enum Key
    {
        Space = KeyboardKey.Space,
        Enter = KeyboardKey.Enter,
        Back = KeyboardKey.Back,
        Left = KeyboardKey.Left,
        Right = KeyboardKey.Right,
        Up = KeyboardKey.Up,
        Down = KeyboardKey.Down,
    }

    public static class Keyboard
    {
        public static bool IsKeyDown(Key key)
        {
            return Raylib.IsKeyDown((KeyboardKey)key);
        }

        public static bool IsKeyUp(Key key)
        {
            return Raylib.IsKeyUp((KeyboardKey)key);
        }

        public static bool IsKeyPressed(Key key)
        {
            return Raylib.IsKeyPressed((KeyboardKey)key);
        }

        public static bool IsKeyReleased(Key key)
        {
            return Raylib.IsKeyReleased((KeyboardKey)key);
        }

        public static bool IsKeyPressedRepeat(Key key)
        {
            return Raylib.IsKeyPressedRepeat((KeyboardKey)key);
        }

        public static int GetKeyPressed()
        {
            return Raylib.GetKeyPressed();
        }

        public static int GetCharPressed()
        {
            return Raylib.GetCharPressed();
        }

        public static void SetExitKey(Key key)
        {
            Raylib.SetExitKey((KeyboardKey)key);
        }
    }
}
