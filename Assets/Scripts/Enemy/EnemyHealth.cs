using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Bar _healthBar;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private ToStunTransition _toStunTransition;
    [SerializeField] private AudioSource _hitSound;

    private float minPitchValue = 0.9f;
    private float maxPitchvalue = 1.1f;

    private Enemy _enemy;
    private int _health;

    private void Start()
    {
        _health = _maxHealth;
        _healthBar.SetMaxValue(_maxHealth);
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (!_healthBar.gameObject.activeSelf)
            return;

        if (_healthBar.Value <= 0)
            _healthBar.gameObject.SetActive(false);
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {           
            _enemy.Die();
        }
        else
        {
            _toStunTransition.SetAsReady();
        }
        _healthBar.gameObject.SetActive(true);
        _hitSound.pitch = Random.Range(minPitchValue, maxPitchvalue);
        _hitSound.Play();
        _healthBar.ChangeValue(_health);
    }
}
