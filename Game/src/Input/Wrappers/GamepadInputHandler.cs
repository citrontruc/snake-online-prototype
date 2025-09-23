/* An object to get all gamepad inputs */

using Raylib_cs;

public static class GamepadInputHandler
{
    public enum Button
    {
        // Face buttons
        DPadUp = GamepadButton.LeftFaceUp,
        DPadRight = GamepadButton.RightFaceUp,
        DPadDown = GamepadButton.LeftFaceDown,
        DPadLeft = GamepadButton.LeftFaceLeft,

        ButtonDown = GamepadButton.RightFaceDown,
        ButtonRight = GamepadButton.RightFaceRight,
        ButtonLeft = GamepadButton.RightFaceLeft,
        ButtonUp = GamepadButton.RightFaceUp,

        // Shoulders / Triggers
        LeftBumper = GamepadButton.LeftTrigger1,
        RightBumper = GamepadButton.RightTrigger1,
        LeftTrigger = GamepadButton.LeftTrigger2,
        RightTrigger = GamepadButton.RightTrigger2,

        // Special
        Select = GamepadButton.MiddleLeft,
        Start = GamepadButton.MiddleRight,

        // Sticks
        LeftStick = GamepadButton.LeftThumb,
        RightStick = GamepadButton.RightThumb,
    }

    public enum Axis
    {
        LeftX = GamepadAxis.LeftX,
        LeftY = GamepadAxis.LeftY,
        RightX = GamepadAxis.RightX,
        RightY = GamepadAxis.RightY,
        LeftTrigger = GamepadAxis.LeftTrigger,
        RightTrigger = GamepadAxis.RightTrigger,
    }

    public class Gamepad
    {
        private readonly int _index;

        public Gamepad(int _index)
        {
            this._index = _index;
        }

        public bool IsAvailable()
        {
            return Raylib.IsGamepadAvailable(_index);
        }

        public bool IsButtonDown(GamepadButton button)
        {
            return Raylib.IsGamepadButtonDown(_index, button);
        }

        public bool IsButtonUp(GamepadButton button)
        {
            return Raylib.IsGamepadButtonUp(_index, button);
        }

        public bool IsButtonPressed(GamepadButton button)
        {
            return Raylib.IsGamepadButtonPressed(_index, button);
        }

        public bool IsButtonReleased(GamepadButton button)
        {
            return Raylib.IsGamepadButtonReleased(_index, button);
        }

        public float GetAxisMovement(GamepadAxis axis)
        {
            return Raylib.GetGamepadAxisMovement(_index, axis);
        }

        public int GetAxisCount()
        {
            return Raylib.GetGamepadAxisCount(_index);
        }
    }
}
