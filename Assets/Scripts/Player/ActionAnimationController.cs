using System.Collections;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ActionMovement))]

public class ActionAnimationController : MonoBehaviour
{
    private const string HorizontalParameter = "Horizontal";
    private const string VerticalParameter = "Vertical";
    private const string IsMovingParameter = "IsMoving";
    private const string RollTrigger = "Roll";    

    [SerializeField] private float _rollAnimationDuration;
    [SerializeField] private Transform _rotation;

    public event UnityAction<float> Rolling;

    private float _rotationSpeed = 10;
    private bool _isRotationLockedOnTarget = true;
    private bool _isAbleToRoll = true;

    private ActionMovement _movement;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Transform _currentTarget;

    private Quaternion _rotationRight = Quaternion.Euler(new Vector3(0,90,0));
    private Quaternion _rotationLeft = Quaternion.Euler(new Vector3(0,-90,0));
    private Quaternion _rotationBack = Quaternion.Euler(new Vector3(0, 180, 0));
    private Quaternion _lookRotation;
    
    private void Awake()
    {   
        _animator = GetComponent<Animator>();
        _movement = GetComponent<ActionMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _animator.SetFloat(HorizontalParameter, _movement.Horizontal);
        _animator.SetFloat(VerticalParameter, _movement.Vertical);
        _animator.SetBool(IsMovingParameter, _movement.IsMoving);

        if (_currentTarget != null)
        {
            Vector3 targetPosition = new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z);
            _lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
        }      
    }
    private void FixedUpdate()
    {  
        if (_currentTarget != null && _isRotationLockedOnTarget)
        {
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, _lookRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        if (_animator.applyRootMotion == false)
            return;

        Vector3 deltaPoisition = _animator.deltaPosition;
        deltaPoisition.y = 0;
        Vector3 velocity = deltaPoisition / Time.deltaTime;
        _rigidbody.velocity = velocity;
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
    }

    public void Roll()
    {
        if (_isAbleToRoll)
            StartCoroutine(DoRoll());
    }

    public void PlayDeathAnimation()
    {
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;
        _animator.SetTrigger("ActionDeath");        
    }

    private void SetRollDirection()
    {
        if (Mathf.Abs(_movement.Horizontal) > Mathf.Abs(_movement.Vertical))
        {
            if (_movement.Horizontal > 0)
            {
                _rotation.localRotation = _rotationRight;
            }
            else
            {
                _rotation.localRotation = _rotationLeft;
            }
        }
        else
        {
            if (_movement.Vertical < 0)
            {
                _rotation.localRotation = _rotationBack;
            }
        }
    }

    private IEnumerator DoRoll()
    {
        _isRotationLockedOnTarget = false;
        SetRollDirection();
        _rigidbody.rotation = _rotation.rotation;
        Rolling?.Invoke(_rollAnimationDuration);
        _isAbleToRoll = false;
        _animator.SetTrigger(RollTrigger);
        _animator.applyRootMotion = true;

        yield return new WaitForSeconds(_rollAnimationDuration);

        _animator.applyRootMotion = false;
        _rotation.localRotation = Quaternion.Euler(Vector3.zero);
        _rigidbody.rotation = _rotation.rotation;

        _isAbleToRoll = true;
        _isRotationLockedOnTarget = true;

        yield break;
    }
}
