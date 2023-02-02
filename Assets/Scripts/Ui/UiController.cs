using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private CameraController _camera;
    [SerializeField] private ForestSpawnController _forestSpawnController;
    [SerializeField] private Button _logoButton;
    [SerializeField] private InGamePanel _inGamePanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private Panel _actionPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private LevelCompletePanel _levelCompletePanel;
    [SerializeField] private LevelPanel _levelPanel;
    [SerializeField] private PlayerAnimationController _player;

    private void OnEnable()
    {
        _player.MovedToRunPosition += OnPlayerMovedToRunPosition;
        _camera.MovedToActionPosition += _actionPanel.MoveToScreen;
        _camera.MovedToMainMenu += OnCameraSetToMainMenu;
        _logoButton?.onClick.AddListener(OnLogoButtonClick);
    }

    private void OnDisable()
    {
        _player.MovedToRunPosition -= OnPlayerMovedToRunPosition;
        _camera.MovedToActionPosition -= _actionPanel.MoveToScreen; 
        _camera.MovedToMainMenu -= OnCameraSetToMainMenu;
        _logoButton?.onClick.RemoveListener(OnLogoButtonClick);
    }

    public void SetMainMenu()
    {
        if (_actionPanel.IsOnScreen)
            _actionPanel.MoveFromScreen();

        if (_inGamePanel.IsOnScreen)
           _inGamePanel.MoveFromScreen();      
    }

    public void StartLevel()
    {
        _camera.MoveToRunnerPosition();
             
        _mainMenu.HideElements();
    }

    public void PauseGame()
    {
        _pausePanel.gameObject.SetActive(true);
        _pausePanel.MoveToScreen();
    }
    
    public void Restart()
    {
        _inGamePanel.gameObject.SetActive(false);

        if (_actionPanel.IsOnScreen)
            _actionPanel.MoveFromScreen();
    }

    public void OnLevelComplete(int newResult)
    {
        if (_inGamePanel.IsOnScreen)
            _inGamePanel.MoveFromScreen(); 

        if (_actionPanel.IsOnScreen)
            _actionPanel.MoveFromScreen();

        _levelCompletePanel.gameObject.SetActive(true);
        _levelCompletePanel.MoveToScreen();
        _levelCompletePanel.SetStars(newResult);
        _levelPanel.SetInfo();
    }

    private void OnPlayerMovedToRunPosition()
    {
        _inGamePanel.gameObject.SetActive(true);
        _inGamePanel.MoveToScreen();
    }
    private void OnCameraSetToMainMenu()
    {
        if (!_mainMenu.IsButtonsOnScreen)
            _mainMenu.SetButtons(true);
    }

    private void OnLogoButtonClick()
    {
        _forestSpawnController.ChangeMoveCondition(false);
        _camera.MoveToMenuPosition();
    }

    public void OnPlayerDeath()
    {
        _losePanel.MoveToScreen();

        if (_inGamePanel.IsOnScreen)
            _inGamePanel.MoveFromScreen();

        if (_actionPanel.IsOnScreen)
            _actionPanel.MoveFromScreen();
    }
}
