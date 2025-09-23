/* An object to handle a menu scene in which the player can choose options. */

using System.Numerics;
using Raylib_cs;

public abstract class Menu : Scene
{
    #region Related objects
    private InputHandler _inputHandler => ServiceLocator.Get<InputHandler>();
    #endregion
    private Color _backGroundColor = Color.Black;

    #region Menu title Characteristics
    /// <summary>
    /// Title to display on our menu. Example : title of the game or "game over"
    /// </summary>
    public string Title { get; private set; }
    private Vector2 _menuTitlePosition = new(100, 100);
    private int _menuTitleSize = 10;
    private bool _menuTitleCentered = true;
    private Color _menuTitleColor = Color.White;
    #endregion

    #region Menu options characteristics
    /// <summary>
    /// List of all the options the user can choose in menu.
    /// </summary>
    private List<string> _listMenuOptions = new();
    private List<Action> _listMenuActions = new();
    private List<Vector2> _listOptionPositions = new();

    /// <summary>
    /// Indications on the position of the elements in the menu.
    /// </summary>
    private Vector2 _menuOptionPosition = new(100, 100);
    private int _menuOptionSize = 10;
    private Color _menuOptionColor = Color.White;
    private bool _menuOptionCentered = true;
    private int _menuOptionSpacing = 10;

    /// <summary>
    /// Variables containing the selected option and how it should be highlighted.
    /// </summary>
    private float _selectedScaleFactor = 1.2f;
    private Color _selectedOptionColor = Color.Red;
    protected int _selectedOption;
    #endregion

    public Menu(string title)
    {
        Title = title;
    }

    #region Get information
    public int GetLenOptions()
    {
        return _listMenuOptions.Count;
    }

    public List<string> GetOptionText()
    {
        return _listMenuOptions;
    }

    public List<Vector2> GetPositionOptions()
    {
        return _listOptionPositions;
    }

    public int GetOptionSize()
    {
        return _menuOptionSize;
    }
    #endregion

    #region Set Characteristics
    public void SetBackgroundcharacteristics(Color backGroundColor)
    {
        _backGroundColor = backGroundColor;
    }

    public void SetMenuTitleCharacteristics(
        string title,
        Vector2 menuTitlePosition,
        int menuTitleSize,
        Color menuTitleColor,
        bool menuTitleCentered
    )
    {
        Title = title;
        _menuTitlePosition = menuTitlePosition;
        _menuTitleSize = menuTitleSize;
        _menuTitleColor = menuTitleColor;
        _menuTitleCentered = menuTitleCentered;
    }

    public void SetMenuOptionCharacteristics(
        Vector2 menuOptionPosition,
        int menuOptionSize,
        Color menuOptionColor,
        bool menuOptionCentered,
        int menuOptionSpacing
    )
    {
        _menuOptionPosition = menuOptionPosition;
        _menuOptionSize = menuOptionSize;
        _menuOptionColor = menuOptionColor;
        _menuOptionCentered = menuOptionCentered;
        _menuOptionSpacing = menuOptionSpacing;
    }

    /// <summary>
    /// If the user selects a text entry, the entry needs to be highlighted.
    /// </summary>
    /// <param name="selectedScaleFactor">The higlighted entry is bigger by a scale factor.</param>
    /// <param name="selectedOptionColor">Colour of the highlighted entry.</param>
    public void SetSelectedOptionCharacteristics(
        float selectedScaleFactor,
        Color selectedOptionColor
    )
    {
        _selectedScaleFactor = selectedScaleFactor;
        _selectedOptionColor = selectedOptionColor;
    }
    #endregion

    #region Update
    public override void Update(float deltaTime)
    {
        UserInput userInput = _inputHandler.GetUserInput();
        if (userInput.UpRelease)
        {
            _selectedOption = Math.Clamp(_selectedOption - 1, 0, GetLenOptions() - 1);
        }
        if (userInput.DownRelease)
        {
            _selectedOption = Math.Clamp(_selectedOption + 1, 0, GetLenOptions() - 1);
        }
        if (userInput.Enter)
        {
            TakeAction(_selectedOption);
        }

        (bool mouseOnOption, int mouseSelectedOption) = CheckIfMouseOnOption(
            userInput.MousePosition
        );
        if (mouseOnOption)
        {
            _selectedOption = mouseSelectedOption;
            if (userInput.LeftClickPress)
            {
                TakeAction(_selectedOption);
            }
        }
    }

    public (bool, int) CheckIfMouseOnOption(Vector2 mousePosition)
    {
        for (int i = 0; i < _listOptionPositions.Count; i++)
        {
            if (
                mousePosition.X > _listOptionPositions[i].X
                && mousePosition.X
                    < _listOptionPositions[i].X
                        + Raylib.MeasureText(_listMenuOptions[i], _menuOptionSize)
                && mousePosition.Y > _listOptionPositions[i].Y
                && mousePosition.Y < _listOptionPositions[i].Y + _menuOptionSize
            )
            {
                return (true, i);
            }
        }
        return (false, -1);
    }

    #endregion

    #region Draw functions
    /// <summary>
    /// We draw each of the elements of the menu independently.
    /// </summary>
    override public void Draw()
    {
        DrawBackground();
        DrawTitle();
        DrawOptions();
    }

    public void DrawBackground()
    {
        Raylib.ClearBackground(_backGroundColor);
    }

    public void DrawTitle()
    {
        int x_position = (int)_menuTitlePosition.X;
        int y_position = (int)_menuTitlePosition.Y;
        if (_menuTitleCentered)
        {
            int textWidth = Raylib.MeasureText(Title, _menuTitleSize);
            x_position -= textWidth / 2;
            y_position -= _menuTitleSize / 2;
        }
        Raylib.DrawText(Title, x_position, y_position, _menuTitleSize, _menuTitleColor);
    }

    public void DrawOptions()
    {
        int lenOptions = _listMenuOptions.Count;
        if (lenOptions > 0)
        {
            for (int i = 0; i < lenOptions; i++)
            {
                if (_selectedOption == i)
                {
                    // Draw highlighted option in big text.
                    Raylib.DrawText(
                        _listMenuOptions[i],
                        (int)_listOptionPositions[i].X,
                        (int)_listOptionPositions[i].Y,
                        (int)(_menuOptionSize * _selectedScaleFactor),
                        _selectedOptionColor
                    );
                }
                else
                {
                    Raylib.DrawText(
                        _listMenuOptions[i],
                        (int)_listOptionPositions[i].X,
                        (int)_listOptionPositions[i].Y,
                        _menuOptionSize,
                        _menuOptionColor
                    );
                }
            }
        }
    }
    #endregion

    #region Options
    public void AddOption(string optionName, Action action)
    {
        _listMenuOptions.Add(optionName);
        _listMenuActions.Add(action);
        RecomputePositionOfOptions();
    }

    public void RecomputePositionOfOptions()
    {
        _listOptionPositions = new();
        int lenOptions = _listMenuOptions.Count;
        int xPosition = (int)_menuOptionPosition.X;
        int yPosition = (int)_menuOptionPosition.Y;
        int xPositionText = xPosition;
        int yPositionText = yPosition;
        for (int i = 0; i < lenOptions; i++)
        {
            string option = _listMenuOptions[i];
            if (_menuOptionCentered)
            {
                int textWidth = Raylib.MeasureText(option, _menuOptionSize);
                xPositionText = xPosition - textWidth / 2;
                yPositionText =
                    yPosition
                    - (lenOptions / 2 - i) * _menuOptionSize
                    - (lenOptions / 2 - 1 - i) * _menuOptionSpacing;
            }
            else
            {
                yPositionText = yPosition + _menuOptionSize + _menuOptionSpacing;
            }
            Vector2 vectorPosition = new(xPositionText, yPositionText);
            _listOptionPositions.Add(vectorPosition);
        }
    }

    public void TakeAction(int ActionIndicator)
    {
        _listMenuActions[ActionIndicator]();
    }
    #endregion
}
