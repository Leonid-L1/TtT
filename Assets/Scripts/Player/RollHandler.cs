using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ActionMovement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerTargetHandler))]
[RequireComponent(typeof(Player))]

public class RollHandler : MonoBehaviour
{
    private const string RollTrigger = "Roll";

    [SerializeField] private float _rollAnimationDuration;
    [SerializeField] private Transform _rotation;
    [SerializeField] private Button _rollButton; 

    public event UnityAction<float> Rolling;
    private float _speed = 10;
    private PlayerTargetHandler _targetHandler;
    private ActionMovement _movement;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Transform _enemy;
    private Player _player;

    private bool _isRolling = false;
    
    public bool IsRolling => _isRolling;   

    private void OnEnable()
    {
        _rollButton.onClick.AddListener(Roll);
    }

    private void OnDisable()
    {
        _rollButton.onClick.RemoveListener(Roll);
    }
    private void Start()
    {   
        _player = GetComponent<Player>();
        _targetHandler = GetComponent<PlayerTargetHandler>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<ActionMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform currentTarget)
    {
        _enemy = currentTarget;
    } 

    private void Roll()
    {
        if (!_isRolling)
            StartCoroutine(DoRoll());
    }

    private IEnumerator DoRoll()
    {
        _isRolling = true;
        _rollButton.interactable = false;
        SetRollDirection();
        _rigidbody.rotation = _rotation.rotation;
        Rolling?.Invoke(_rollAnimationDuration);
        _animator.SetTrigger(RollTrigger);

        _rigidbody.velocity = _rotation.forward.normalized * _speed;
        yield return new WaitForSeconds(_rollAnimationDuration);

        _rotation.localRotation = Quaternion.Euler(Vector3.zero);
        _rigidbody.rotation = _rotation.rotation;
        _isRolling = false;
        _rollButton.interactable = true;

        yield break;
    }

    private void SetRollDirection()
    {
        Vector3 enemyToPlayer = _enemy.position - transform.position;
        Vector3 directionFromTarget = enemyToPlayer.normalized;
        Vector3 rollTargetDirection = new Vector3(transform.position.x + directionFromTarget.x, 0, transform.position.z + directionFromTarget.z);

        if (Mathf.Abs(_movement.Horizontal) > Mathf.Abs(_movement.Vertical))
        {
            if (_movement.Horizontal > 0)
            {
                Vector3 directionRight = new Vector3(directionFromTarget.z, 0, -directionFromTarget.x);
                rollTargetDirection = new Vector3(transform.position.x + directionRight.x, 0, transform.position.z + directionRight.z);
            }
            else
            {
                Vector3 directionLeft = new Vector3(-directionFromTarget.z, 0, directionFromTarget.x);
                rollTargetDirection = new Vector3(transform.position.x + directionLeft.x, 0, transform.position.z + directionLeft.z);
            }
        }
        else
        {
            if (_movement.Vertical < 0)
            {
                rollTargetDirection = new Vector3(transform.position.x + -directionFromTarget.x, 0, transform.position.z + -directionFromTarget.z);
            }
        }
        _rotation.rotation = Quaternion.LookRotation(rollTargetDirection - transform.position);
    }
}
