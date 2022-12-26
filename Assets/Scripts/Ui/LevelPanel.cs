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

    public UnityAction LevelChosen;

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
        SetText();
    }
    
    private void OnLevelButtonClick()
    {
        LevelChosen?.Invoke();
        MoveFromScreen();
        _levelHandler.SetLevel(_currentLevel);
    }

    private void OnLeftArrowClick()
    {
        if(_currentLevel != 1)
        {
            _currentLevel--;
            SetText();
        }
    }

    private void OnRightArrowClick()
    {
        //if (_levelHandler.Levels[_currentLevel + 1].IsCompleted)
        //{
        //    _currentLevel++;
        //    SetText();
        //}
    }

    private void SetText()
    {
        _levelNumber.text = _currentLevel.ToString();
    }
}
