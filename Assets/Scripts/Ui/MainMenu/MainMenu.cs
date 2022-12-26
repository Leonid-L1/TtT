using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string _buttonFromScreenAnimationName = "MainButtonFromScreen";
    private const string _buttonToScreenAnimationName = "MainButtonToScreen";
    private const string _exitButtonFromScreenAnimationName = "ExitButtonFromScreen";
    private const string _exitButtonToScreenAnimationName = "ExitButtonToScreen";

    [SerializeField] private ExitButton _exitButton;
    [SerializeField] private ButtonAnimationController[] _leftSideButtons;
    [SerializeField] private LevelPanel _levelPanel;

    private bool _isButtonsOnScreen;

    public bool IsButtonsOnScreen => _isButtonsOnScreen;

    public void SetToLevelPanel()
    {
        SetButtons(false);
        _levelPanel.MoveToScreen();
    }

    public void ReturnToMainMenuFromLevelPanel()
    {
        _levelPanel.MoveFromScreen();
        SetButtons(true);
    }
    
    public void HideElements()
    {
        if (_isButtonsOnScreen)
        {
            SetButtons(false);
        }
        else if (_levelPanel.IsOnScreen)
        {
            _levelPanel.MoveFromScreen();
        }
    }

    public void SetButtons(bool isToScreen)
    {
        if (isToScreen)
        {   
            _exitButton.PlayAnimation(_exitButtonToScreenAnimationName);

            foreach (ButtonAnimationController button in _leftSideButtons)
                button.PlayAnimation(_buttonToScreenAnimationName);

            _isButtonsOnScreen = true;
        }
        else
        {
            _exitButton.PlayAnimation(_exitButtonFromScreenAnimationName);

            foreach (ButtonAnimationController button in _leftSideButtons)
                button.PlayAnimation(_buttonFromScreenAnimationName);

            _isButtonsOnScreen = false;
        }
    }
}
