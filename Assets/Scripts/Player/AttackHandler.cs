using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AttackHandler : MonoBehaviour
{
    private const string FirstAttackTrigger = "First";
    private const string SecondAttackTrigger = "Second";
    private const string ThirdAttackTrigger = "Third";

    [SerializeField] private Button _attackButton;
    [SerializeField] private float _timeBetweenAttacks = 1.5f;
    [SerializeField] private List<float> _attackDurations;
    [SerializeField] private List<int> _damageValues;
    [SerializeField] private List<float> _timeBeforeTouch;
    [SerializeField] private float _distanceToAttack;

    public event UnityAction<float> Attacking;
    public event UnityAction TargetDied;

    private Enemy _target;
    private List<AttackType> _attacks;
    private Animator _animator;
    private float _elapsedTime;
    private bool _isWaitingForNextAttack = false;
    private bool _isAttacking = false;

    public bool IsAttacking => _isAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(Attack);
    }

    private void OnDisable()
    {
        _attackButton.onClick.RemoveListener(Attack);
    }

    private void Start()
    {
        _attacks = new List<AttackType>()
        {
          new AttackType(FirstAttackTrigger, _attackDurations[0],_timeBeforeTouch[0], _damageValues[0]),
          new AttackType(SecondAttackTrigger, _attackDurations[1],_timeBeforeTouch[1], _damageValues[1]),
          new AttackType(ThirdAttackTrigger,  _attackDurations[2],_timeBeforeTouch[2], _damageValues[2])
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

    public void SetTarget(Enemy target)
    {
        _target = target;
    }

    private void Attack()
    {
        if (_isAttacking == false)
        {
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
    }

    private float GetDistance()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        return distance;
    }

    private IEnumerator SetAttack(AttackType currentAttack)
    {
        _isWaitingForNextAttack = false;
        _isAttacking = true;
        _elapsedTime = 0;

        _animator.SetTrigger(currentAttack.Triggername);

        Attacking?.Invoke(currentAttack.Duration);

        yield return new WaitForSeconds(currentAttack.TimeBeforeTouch);

        float distance = GetDistance();

        if (distance <= _distanceToAttack)
        {
            _target.ApplyDamage(currentAttack.Damage);

            if (_target.IsAlive == false)
            {
                TargetDied?.Invoke();
            }
        }

        yield return new WaitForSeconds(currentAttack.Duration - currentAttack.TimeBeforeTouch);

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
    public int Damage { get; private set; }

    public AttackType(string triggerName, float duration, float timeBeforeTouch, int damage)
    {
        Triggername = triggerName;
        Duration = duration;
        TimeBeforeTouch = timeBeforeTouch;
        Damage = damage;
    }

    public void SetCompleteState(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }
}