using UnityEngine;

public class MoveState : State
{
    private static string Walk = "ToWalk";

    [SerializeField] private float _speed;
    [SerializeField] private AttackState _attackState;
    [SerializeField] private float _interpolateSpeed = 2;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _rotationSpeed = 5;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.SetTrigger(Walk);
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {   
        if (Target != null)
        {   
            Vector3 targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);

            Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, lookRotation, _rotationSpeed * Time.deltaTime);

            Vector3 velocity = transform.forward * _speed;
            _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, velocity, _interpolateSpeed * Time.deltaTime);
        }     
    }
}

