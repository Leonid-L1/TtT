using System.Collections;
using UnityEngine;

public class AttackState : State
{
    private const string AttackTrigger = "Attack";
    private const string PrepareTrigger = "Prepare";

    [SerializeField] private EnemyWeaponCollider _weaponCollider;
    [SerializeField] private float _attackDuration;
    [SerializeField] private int _attackDamage = 15;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private float _rotationSpeed = 5;
    private float _rotateTime = 0.45f;

    private bool _isAttacking = false;
    private bool _isWaitingForAttack = true;
    
    public bool IsAttacking => _isAttacking;

    private Coroutine _attackCoroutine;

    private void Awake()
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _weaponCollider.PlayerTouched += DealDamage;
        _isWaitingForAttack = true;
    }

    private void OnDisable()
    {
        _weaponCollider.PlayerTouched += DealDamage;

        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _animator.applyRootMotion = _isAttacking;

        if (_isWaitingForAttack)
            _attackCoroutine = StartCoroutine(DoAttack());
    }

    private void OnAnimatorMove()
    {
        if (IsAttacking == false)
            return;

        Vector3 deltaPoisition = _animator.deltaPosition;
        deltaPoisition.y = 0;
        Vector3 velocity = deltaPoisition / Time.deltaTime;
        _rigidbody.velocity = velocity;
    }

    public void SetWeaponColliderAsActive()
    {
        _weaponCollider.gameObject.SetActive(true);
    }

    public void SetWeaponColliderAsInactive()
    {
        _weaponCollider.gameObject.SetActive(false);
    }

    private void DealDamage()
    {
        Target.GetComponent<PlayerHealth>().ApplyDamage(_attackDamage);
    } 

    private IEnumerator DoAttack()
    {
        float elapsedTime = 0;

        _isWaitingForAttack = false;
        _isAttacking = false;
        _animator.SetTrigger(PrepareTrigger);

        while(elapsedTime < _rotateTime)
        {
            RotateToTarget();
            elapsedTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
        _animator.SetTrigger(AttackTrigger);
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDuration);

        _isWaitingForAttack = true;
        _isAttacking = false;

        yield break;
    }
    private void RotateToTarget()
    {
        Vector3 targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
        _rigidbody.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }
}
