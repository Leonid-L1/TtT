using UnityEngine;

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
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _playerMana = _player.GetComponent<PlayerCollectibles>();
    }
    private void OnEnable() 
    {
        _healthBar.SetMaxValue(_playerHealth.MaxHealth);
        _manaBar.SetMaxValue(_playerMana.MaxMana);
        _healthBar.ChangeValue(_playerHealth.MaxHealth);

        _playerHealth.HealthChanged += OnHealthChanged;
        _playerMana.ManaCountChanged += OnManaChanged;
    }

    private void OnDisable()
    {
        _playerHealth.HealthChanged -= OnHealthChanged;
        _playerMana.ManaCountChanged -= OnManaChanged;
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
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
