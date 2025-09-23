/* An object to handle keyboard, gamepad and mouse inputs. */

/// <summary>
/// A class to make the synthesis of all the different inputs from the user.
/// </summary>
public class InputHandler
{
    public UserInput _userInput { get; private set; } = new();

    public InputHandler()
    {
        ServiceLocator.Register<InputHandler>(this);
    }

    #region Getters and Setters
    public UserInput GetUserInput()
    {
        return _userInput;
    }
    #endregion

    #region Update
    public void Update()
    {
        _userInput.MousePosition = MouseInputHandler.position;
        _userInput.LeftClickPress = MouseInputHandler.isButtonPressed(
            MouseInputHandler.Button.Left
        );
        _userInput.RightClickPress = MouseInputHandler.isButtonPressed(
            MouseInputHandler.Button.Right
        );
        _userInput.LeftClickHold = MouseInputHandler.isButtonDown(MouseInputHandler.Button.Left);
        _userInput.RightClickHold = MouseInputHandler.isButtonDown(MouseInputHandler.Button.Right);
        _userInput.LeftClickRelease = MouseInputHandler.isButtonReleased(
            MouseInputHandler.Button.Left
        );
        _userInput.RightClickRelease = MouseInputHandler.isButtonReleased(
            MouseInputHandler.Button.Right
        );

        // Position key variables (hold and release)
        _userInput.RightHold = KeyboardInputHandler.Keyboard.IsKeyDown(
            KeyboardInputHandler.Key.Right
        );
        _userInput.LeftHold = KeyboardInputHandler.Keyboard.IsKeyDown(
            KeyboardInputHandler.Key.Left
        );
        _userInput.UpHold = KeyboardInputHandler.Keyboard.IsKeyDown(KeyboardInputHandler.Key.Up);
        _userInput.DownHold = KeyboardInputHandler.Keyboard.IsKeyDown(
            KeyboardInputHandler.Key.Down
        );

        _userInput.RightRelease = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Right
        );
        _userInput.LeftRelease = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Left
        );
        _userInput.UpRelease = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Up
        );
        _userInput.DownRelease = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Down
        );

        // Other keys
        _userInput.Enter = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Enter
        );
        _userInput.Pause = KeyboardInputHandler.Keyboard.IsKeyReleased(
            KeyboardInputHandler.Key.Space
        );
    }
    #endregion
}
