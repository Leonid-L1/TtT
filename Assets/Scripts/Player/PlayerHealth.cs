using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AudioSource _hitSound;   
    
    private Player _player;
    private int _health;
    private int _maxHealth = 100;
    private float minPitchValue = 0.9f;
    private float maxPitchvalue = 1.1f;

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
        _hitSound.pitch = Random.Range(minPitchValue, maxPitchvalue);      
        _hitSound.Play();
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        HealthChanged?.Invoke(_maxHealth);
    }
}
