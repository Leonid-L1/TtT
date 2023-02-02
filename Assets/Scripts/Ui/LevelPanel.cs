using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]

public class LevelPanel : Panel
{
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private Button _levelButton;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private TMP_Text _levelNumber;

    [SerializeField] private List<Image> _stars;
    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private Sprite _filledStar;

    private int _currentLevelResult;
    private int _currentLevel = 1;

    private void OnEnable()
    {
        _levelButton.onClick.AddListener(OnLevelButtonClick);
        _leftArrow.onClick.AddListener(OnLeftArrowClick);
        _rightArrow.onClick.AddListener(OnRightArrowClick);
    }

    private void OnDisable()
    {
        _levelButton.onClick.RemoveListener(OnLevelButtonClick);
        _leftArrow.onClick.RemoveListener(OnLeftArrowClick);
        _rightArrow.onClick.RemoveListener(OnRightArrowClick);
    }

    public override void MoveToScreen()
    {
        base.MoveToScreen();
        SetInfo();
    }

    public void SetInfo()
    {
        int maxStarsCount = 3;

        foreach (Image star in _stars)
            star.sprite = _emptyStar;

        _currentLevelResult = _levelHandler.GetResult(_currentLevel - 1);
        int emptyStarsCount = maxStarsCount - _currentLevelResult;

        _levelNumber.text = _currentLevel.ToString();

        for (int i = 0; i < _currentLevelResult; i++)
            _stars[i].sprite = _filledStar;
    }

    private void OnLevelButtonClick()
    {
        MoveFromScreen();
        _levelHandler.SetLevel(_currentLevel);
    }

    private void OnLeftArrowClick()
    {
        if(_currentLevel != 1)
        {
            _currentLevel--;
            SetInfo();
        }
    }

    private void OnRightArrowClick()
    {
        if (_currentLevel!=_levelHandler.Levels.Count && _levelHandler.Levels[_currentLevel - 1].IsCompleted)
        {
            _currentLevel++;
            SetInfo();
        }
    }
}
