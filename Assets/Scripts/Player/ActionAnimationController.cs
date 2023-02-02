using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ActionMovement))]
[RequireComponent(typeof(RollHandler))]
[RequireComponent(typeof(Animator))]

public class ActionAnimationController : MonoBehaviour
{
    private const string HorizontalParameter = "Horizontal";
    private const string VerticalParameter = "Vertical";
    private const string IsMovingParameter = "IsMoving";    

    [SerializeField] private float _rollAnimationDuration;
    [SerializeField] private Transform _rotation;

    private float _rotationSpeed = 10;

    private RollHandler _rollHandler;
    private ActionMovement _movement;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Transform _currentTarget;

    private Quaternion _lookRotation;
    
    private void Start()
    {   
        _rollHandler = GetComponent<RollHandler>();       
        _animator = GetComponent<Animator>();
        _movement = GetComponent<ActionMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {   
        if(_movement != null && _movement.enabled)
        {
            _animator.SetFloat(HorizontalParameter, _movement.Horizontal);
            _animator.SetFloat(VerticalParameter, _movement.Vertical);
            _animator.SetBool(IsMovingParameter, _movement.IsMoving);
        }        

        if (_currentTarget != null)
        {
            Vector3 targetPosition = new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z);
            _lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
        }      
    }

    private void FixedUpdate()
    {  
        if (_currentTarget != null && !_rollHandler.IsRolling)
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, _lookRotation, _rotationSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger("ActionDeath");        
    }
}
