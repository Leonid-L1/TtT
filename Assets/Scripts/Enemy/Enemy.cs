using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private static string DeathTrigger = "Death";

    [SerializeField] private float _deathAnimationDuration = 2.4f;

    private Collider _collider;
    private StateMachine _stateMachine;
    private Stun _stunHandler;
    private Animator _animator;

    private float _timeBeforeDestroy = 2;
    private int _health = 100;
    private Player _target;
    private bool _isAlive = true;

    public bool IsAlive => _isAlive;
    public int Health => _health;
    public Player Target => _target;

    private void Awake()
    {   
        _collider = GetComponentInChildren<CapsuleCollider>();
        _animator = GetComponent<Animator>();   
        _stunHandler = GetComponent<Stun>();
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stunHandler.StunEnded += TurnOnStateMachine;       
    }

    private void OnDisable()
    {
        _stunHandler.StunEnded -= TurnOnStateMachine;
        if(Target != null)
            _target.DeadOnActionPhaze -= OnPlayerDead;
    }

    public void SetTarget(Player target)
    {
        _target = target;
        _target.DeadOnActionPhaze += OnPlayerDead;
        _stateMachine.SetTarget(target);
        TurnOnStateMachine();
    }

    public void ApplyDamage(int damage)
    {   
        if (_isAlive == false)
            return;

        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _stunHandler.SetStun();
            _stateMachine.enabled = false;
        }       
    }

    private void TurnOnStateMachine()
    {
        _stateMachine.enabled = true;
    }  

    private void Die() 
    {   
        _isAlive = false;
        _stateMachine.enabled = false;
        _animator.SetTrigger(DeathTrigger);
        _collider.enabled = false;
        Destroy(gameObject, _deathAnimationDuration + _timeBeforeDestroy);
    }

    private void OnPlayerDead()
    {
        Destroy(gameObject);
    }
}

