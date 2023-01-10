using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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

    //public UnityAction LevelChosen;

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

    private void Start()
    {
        SetInfo();
    }

    public void SetInfo()
    {
        foreach (Image star in _stars)
            star.sprite = _emptyStar;

        int maxStarsCount = 3;
        _currentLevelResult = _levelHandler.GetResult(_currentLevel - 1);
        int emptyStarsCount = maxStarsCount - _currentLevelResult;

        _levelNumber.text = _currentLevel.ToString();

        for (int i = 0; i < _currentLevelResult; i++)
        {
            _stars[i].sprite = _filledStar;
        }
    }

    private void OnLevelButtonClick()
    {
        //LevelChosen?.Invoke();
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
        if (_levelHandler.Levels[_currentLevel - 1].IsCompleted)
        {
            _currentLevel++;
            SetInfo();
        }
    }

    
}
