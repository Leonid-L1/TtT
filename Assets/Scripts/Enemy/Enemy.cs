using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    private static string DeathTrigger = "Death";

    [SerializeField] private GameObject _pointer;
    [SerializeField] private GameObject _setAsTargetButton;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private StateMachine _stateMachine;
    private Animator _animator;

    private int _health = 100;
    private Player _target;
    private bool _isActiveTarget;

    public UnityAction<Enemy> EnemyDied;
    public int Health => _health;
    public Player Target => _target;
    public bool IsActiveTarget => _isActiveTarget;

    private void Awake()
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        _animator = GetComponent<Animator>();   
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnDisable()
    {
        if (Target != null)
            _target.DeadOnActionPhaze -= OnPlayerDied;
    }

    public void SetTarget(Player target)
    {
        _target = target;
        _stateMachine.SetTarget(target);
        _stateMachine.enabled = true;
        _target.DeadOnActionPhaze += OnPlayerDied;
    }

    public void SetAsActiveTarget(bool isActiveTarget)
    {
        _pointer.SetActive(isActiveTarget);
        _isActiveTarget = isActiveTarget;

        if (isActiveTarget)
        {
            _target.GetComponent<PlayerTargetHandler>().SetTarget(this);
            _setAsTargetButton.SetActive(false);
        }
        else
        {
            _setAsTargetButton.SetActive(true);
        }
    }

    public void Die() 
    {   
        _stateMachine.enabled = false;
        _pointer.SetActive(false);
        StopAllCoroutines();
        _animator.applyRootMotion = false;
        _animator.SetTrigger(DeathTrigger);
        _collider.enabled = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        EnemyDied?.Invoke(this);
    }

    private void OnPlayerDied()
    {
        StopAllCoroutines();
        _pointer.SetActive(false);
        _stateMachine.enabled = false;
        _animator.applyRootMotion = false;
        _collider.enabled = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }
}

