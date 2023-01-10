using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour
{
    private Player _player;
    private int _health;
    private int _maxHealth = 100;

    public int Health => _health;
    public int MaxHealth => _maxHealth;

    public event UnityAction<int> HealthChanged;

    private void Start()
    {
        _player = GetComponent<Player>();
        ResetHealth();
    }

    public void ApplyDamage(int damage)
    {
        if (damage >= _health)
        {
            _health = 0;
            _player.SetDeath();
        }
        else
        {
            _health -= damage;
        }
        HealthChanged?.Invoke(_health);
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        HealthChanged?.Invoke(_health);
    }
}
