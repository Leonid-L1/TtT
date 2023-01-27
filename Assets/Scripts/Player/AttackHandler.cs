using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerWeapon))]

public class AttackHandler : MonoBehaviour
{
    private const string FirstAttackTrigger = "First";
    private const string SecondAttackTrigger = "Second";
    private const string ThirdAttackTrigger = "Third";

    [SerializeField] private PlayerWeaponCollider _weaponCollider;
    [SerializeField] private Button _attackButton;
    [SerializeField] private float _timeBetweenAttacks = 1.5f;
    [SerializeField] private List<float> _attackDurations;
    [SerializeField] private List<float> _timeBeforeTouch;
    [SerializeField] private float _distanceToAttack;

    public event UnityAction<float> Attacking;

    private List<AttackType> _attacks;
    private EnemyHealth _target;
    private PlayerWeapon _weaponHandler;
    private Animator _animator;
    private float _elapsedTime;
    private bool _isWaitingForNextAttack = false;
    private bool _isAttacking = false;

    public bool IsAttacking => _isAttacking;

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(Attack);
        _weaponCollider.EnemyTouched += DealDamage;
    }

    private void OnDisable()
    {
        _attackButton.onClick.RemoveListener(Attack);
        _weaponCollider.EnemyTouched -= DealDamage;
    }

    private void Start()
    {   
        _weaponHandler = GetComponent<PlayerWeapon>();
        _animator = GetComponent<Animator>();

        _attacks = new List<AttackType>()
        {
          new AttackType(FirstAttackTrigger, _attackDurations[0],_timeBeforeTouch[0]),
          new AttackType(SecondAttackTrigger, _attackDurations[1],_timeBeforeTouch[1]),
          new AttackType(ThirdAttackTrigger,  _attackDurations[2],_timeBeforeTouch[2])
        };
    }

    private void Update()
    {
        if (_isWaitingForNextAttack == false)
            return;

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _timeBetweenAttacks)
        {
            _elapsedTime = 0;
            _isWaitingForNextAttack = false;

            foreach (AttackType attack in _attacks)
            {
                attack.SetCompleteState(false);
            }
        }
    }

    public void SetTarget(EnemyHealth target)
    {
        _target = target;
    }

    public void SetWeaponColliderAsActive()
    {
        _weaponCollider.gameObject.SetActive(true);
    }

    public void SetWeaponColliderAsInactive()
    {
        _weaponCollider.gameObject.SetActive(false);
    }

    private void DealDamage(EnemyHealth enemy)
    {
        enemy.ApplyDamage(_weaponHandler.CurrentWeaponDamage);
    }

    private void Attack()
    {           
        if (_isAttacking)
            return;

        if (_isWaitingForNextAttack == false)
        {
            StartCoroutine(SetAttack(_attacks[0]));
        }
        else
        {
            if (_attacks[0].IsCompleted)
            {
                _attacks[0].SetCompleteState(false);
                StartCoroutine(SetAttack(_attacks[1]));
            }
            else if (_attacks[1].IsCompleted)
            {
                _attacks[1].SetCompleteState(false);
                StartCoroutine(SetAttack(_attacks[2]));
            }
            else if (_attacks[2].IsCompleted)
            {
                _attacks[2].SetCompleteState(false);
                StartCoroutine(SetAttack(_attacks[0]));
            }
        }
    }

    private IEnumerator SetAttack(AttackType currentAttack)
    {
        _isWaitingForNextAttack = false;
        _isAttacking = true;
        _elapsedTime = 0;

        _animator.SetTrigger(currentAttack.Triggername);

        Attacking?.Invoke(currentAttack.Duration);

        yield return new WaitForSeconds(currentAttack.Duration);

        _isWaitingForNextAttack = true;
        _isAttacking = false;
        currentAttack.SetCompleteState(true);

        yield break;
    }
}

public class AttackType
{
    public string Triggername { get; private set; }
    public string NextAttackTriggername { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public float Duration { get; private set; }
    public float TimeBeforeTouch { get; private set; }

    public AttackType(string triggerName, float duration, float timeBeforeTouch)
    {
        Triggername = triggerName;
        Duration = duration;
        TimeBeforeTouch = timeBeforeTouch;
    }

    public void SetCompleteState(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }
}