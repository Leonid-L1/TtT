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

    private PlayerHealth _playerHealth;
    private PlayerCollectibles _playerMana;

    private void Awake()
    {   
        //_player.gameObject.GetComponent<PlayerHealth>();
        //_player.gameObject.GetComponent<PlayerCollectibles>();
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _playerMana = _player.GetComponent<PlayerCollectibles>();
    }
    private void OnEnable() 
    {
        _playerHealth.HealthChanged += OnHealthChanged;
        _playerMana.ManaCountChanged += OnManaChanged;
    }

    private void OnDisable()
    {
        _playerHealth.HealthChanged -= OnHealthChanged;
        _playerMana.ManaCountChanged -= OnManaChanged;
    }

    private void Start()
    {
        _healthBar.SetMaxValue(_playerHealth.MaxHealth);
        _manaBar.SetMaxValue(_playerMana.MaxMana);
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
