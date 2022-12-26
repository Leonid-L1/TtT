using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(ActionAnimationController))]

public class ActionMovementKeyBoard : MonoBehaviour
{
    [SerializeField] private float _speed;

    private ActionAnimationController _animationController;
    private AttackHandler _attackHandler;
    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isMoving;
    private bool _isAbleToMove = true;

    public bool IsMoving => _isMoving;
    public float Horizontal => _horizontalInput;
    public float Vertical => _verticalInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _attackHandler = GetComponent<AttackHandler>();
        _animationController = GetComponent<ActionAnimationController>();
    }

    private void OnEnable()
    {
        _attackHandler.Attacking += SetDelay;
        _animationController.Rolling += SetDelay;
    }

    private void OnDisable()
    {
        _attackHandler.Attacking -= SetDelay;
        _animationController.Rolling -= SetDelay;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if (_isAbleToMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _horizontalInput = 0;
                _verticalInput = Vector3.forward.z;
                _isMoving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _horizontalInput = 0;
                _verticalInput = Vector3.back.z;
                _isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _verticalInput = 0;
                _horizontalInput = Vector3.left.x;
                _isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _verticalInput = 0;
                _horizontalInput = Vector3.right.x;
                _isMoving = true;
            }
            else
            {
                _horizontalInput = 0;
                _verticalInput = 0;
                _isMoving = false;
            }
        }
        else
        {
            _horizontalInput = 0;
            _verticalInput = 0;
        }
    }

    private void FixedUpdate()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        Vector3 velocity = _moveDirection * _speed;
        _rigidbody.velocity = velocity;
    }

    private void SetDelay(float delay)
    {
        StartCoroutine(DoDelay(delay));
    }

    private IEnumerator DoDelay(float delay)
    {
        _isAbleToMove = false;
        yield return new WaitForSeconds(delay);
        _isAbleToMove = true;
        yield break;
    }
}
