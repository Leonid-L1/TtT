using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(ActionAnimationController))]

public class ActionMovement : MonoBehaviour
{   
    [SerializeField] private MovementButtonHandler _upButton;
    [SerializeField] private MovementButtonHandler _downButton;
    [SerializeField] private MovementButtonHandler _leftButton;
    [SerializeField] private MovementButtonHandler _rightButton;

    [SerializeField] private float _speed;
    [SerializeField] private float _interpolatingSpeed;

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
        _rigidbody.velocity = Vector3.zero;
        _attackHandler.Attacking += SetDelay;
        _animationController.Rolling += SetDelay;
    }

    private void OnDisable()
    {
        _attackHandler.Attacking -= SetDelay;
        _animationController.Rolling -= SetDelay;
        //_rigidbody.rotation = Quaternion.Euler(Vector3.zero);       
    }
    
    private void FixedUpdate()
    {
        if (_isAbleToMove == false)
            return;

        if (_upButton.IsHold)
        {
            _horizontalInput = 0;
            _verticalInput = InterpolateInput(_verticalInput, Vector3.forward.z);
            _isMoving = true;
        }
        else if (_downButton.IsHold)
        {
            _horizontalInput = 0;
            _verticalInput = InterpolateInput(_verticalInput, Vector3.back.z);
            _isMoving = true;
        }
        else if (_leftButton.IsHold)
        {
            _verticalInput = 0;
            _horizontalInput = InterpolateInput(_horizontalInput, Vector3.left.x);
            _isMoving = true;
        }
        else if (_rightButton.IsHold)
        {
            _verticalInput = 0;
            _horizontalInput = InterpolateInput(_horizontalInput, Vector3.right.x);
            _isMoving = true;
        }
        else
        {
            _horizontalInput = 0;
            _verticalInput = 0;
            _isMoving = false;
        }
        Move();
    }

    private void Move()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        Vector3 velocity = _moveDirection * _speed;

        _rigidbody.velocity = velocity;
    }

    private float InterpolateInput(float input, float target)
    {
        float newAxis = Mathf.MoveTowards(input, target, _interpolatingSpeed * Time.deltaTime);

        return newAxis;
    }

    private void SetDelay(float delay)
    {
        _rigidbody.velocity = Vector3.zero;
        _isMoving = false;
        StartCoroutine(DoDelay(delay));
    }

    private IEnumerator DoDelay(float delay)
    {
        _isAbleToMove = false;
        yield return new WaitForSeconds(delay);
        _isAbleToMove = true;
        _rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        yield break;
    }
}
