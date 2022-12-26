using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class InGamePanel : Panel
{
    [SerializeField] private Player _player;
    [SerializeField] private Bar _healthBar;
    [SerializeField] private Bar _manaBar;

    private void OnEnable() 
    {
        _player.HealthChanged += OnHealthChanged;   
        _player.ManaChanged += OnManaChanged;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnHealthChanged;
        _player.ManaChanged -= OnManaChanged;
    }

    private void Start()
    {
        _healthBar.SetMaxValue(_player.MaxHealth);
        _manaBar.SetMaxValue(_player.MaxMana);
    }

    private void OnHealthChanged(int targetValue)
    {
        _healthBar.ChangeValue(targetValue);
    }

    private void OnManaChanged(int targetValue)
    {
        _manaBar.ChangeValue(targetValue);
    }
}
