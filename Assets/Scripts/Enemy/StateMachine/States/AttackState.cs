using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private const string FirstAttackName = "FirstAttack";
    private const string PrepareTrigger = "Prepare";

    [SerializeField] private float _attackDuration;
    [SerializeField] private float _distanceModifier;
    [SerializeField] private int _firstAttackDamage = 10;
    [SerializeField] private int _secondAttackDamage = 15;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private float _rotationSpeed = 5;
    private float _rotateTime = 0.45f;

    private bool _isAttacking = false;
    private bool _isWaitingForAttack = true;

    private float _delayBeforeFirstAttack = 1.6f;
    private float _delayBeforeSecondAttack = 0.8f;

    private float _distanceToAttack = 2.5f;
    
    public bool IsAttacking => _isAttacking;

    private Coroutine _attackCoroutine;

    private void Awake()
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _isWaitingForAttack = true;
    }

    private void OnDisable()
    {   
        if(_attackCoroutine != null)
        StopCoroutine(_attackCoroutine);

        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (_isWaitingForAttack)
        {
            _attackCoroutine = StartCoroutine(DoAttack());
        }
    }

    private void OnAnimatorMove()
    {
        if (IsAttacking == false)
        {
            return;
        }
        Vector3 deltaPoisition = _animator.deltaPosition;
        deltaPoisition.y = 0;
        Vector3 velocity = deltaPoisition / Time.deltaTime;
        _rigidbody.velocity = velocity;
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
        _rigidbody.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    private float GetDistance()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        return distance;
    }

    private IEnumerator DoAttack()
    {
        _isWaitingForAttack = false;

        _isAttacking = false;
        _animator.SetTrigger(PrepareTrigger);

        float elapsedTime = 0;

        while(elapsedTime < _rotateTime)
        {
            RotateToTarget();
            elapsedTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
        elapsedTime = 0;

        _animator.SetTrigger(FirstAttackName);
        _isAttacking = true;
        _animator.applyRootMotion = _isAttacking;

        yield return new WaitForSeconds(_delayBeforeFirstAttack);
        float distance = GetDistance();

        if(distance <= _distanceToAttack)
        {
            Target.GetComponent<PlayerHealth>().ApplyDamage(_firstAttackDamage);
        }
        yield return new WaitForSeconds(_delayBeforeSecondAttack);
        distance = GetDistance();

        if (distance <= _distanceToAttack)
        {
            Target.GetComponent<PlayerHealth>().ApplyDamage(_secondAttackDamage);
        }
        yield return new WaitForSeconds(_attackDuration - _delayBeforeFirstAttack - _delayBeforeSecondAttack);

        _isAttacking = false;
        _animator.applyRootMotion = _isAttacking;

        _isWaitingForAttack = true;

        yield break;
    }
}
