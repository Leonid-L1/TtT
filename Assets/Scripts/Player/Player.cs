using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SwipeListener))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(RunnerMovement))]
[RequireComponent(typeof(ActionMovement))]
[RequireComponent(typeof(ActionAnimationController))]
[RequireComponent (typeof(AttackHandler))]

public class Player : MonoBehaviour
{
    [SerializeField] GameController _gameController;
    [SerializeField] CameraHolder _camera;

    private int _coins;
    private int _mana;
    private int _health;

    public event UnityAction<int> HealthChanged;
    public event UnityAction<int> ManaChanged;
    public event UnityAction DeadOnRunnerPhaze;
    public event UnityAction DeadOnActionPhaze;

    private PlayerAnimationController _animationController;
    private RunnerMovement _runnerMovement;
    private SwipeListener _swipeListener;
    private ActionMovement _actionMovement;
    private ActionAnimationController _actionAnimationController;  
    private AttackHandler _attackHandler;

    private List<GameObject> _targets;
    private int _currentTargetIndex = 0;

    private int _maxHealth = 100;
    private int _maxMana = 100;

    public int MaxHealth => _maxHealth;
    public int MaxMana => _maxMana;


    private Rigidbody _rg;
    private void Awake()
    {   
        _rg = GetComponent<Rigidbody>();    
        _actionMovement = GetComponent<ActionMovement>();
        _animationController = GetComponent<PlayerAnimationController>();
        _swipeListener = GetComponent<SwipeListener>();
        _runnerMovement = GetComponent<RunnerMovement>();
        _actionAnimationController = GetComponent<ActionAnimationController>();
        _attackHandler = GetComponent<AttackHandler>();
    }

    private void Update()
    {
        Debug.Log("velocity" + _rg.angularVelocity);
        Debug.Log("drag" + _rg.angularDrag);
    }

    private void OnEnable()
    {
        _animationController.MovedToRunPosition += OnMovedToRunPosition;
        _attackHandler.TargetDied += SetNewTarget;
    }

    private void OnDisable()
    {
        _animationController.MovedToRunPosition -= OnMovedToRunPosition;
        _attackHandler.TargetDied -= SetNewTarget;
    }

    private void Start()
    {
        _health = _maxHealth;
        _mana = 0;
    }

    public void IncreaseCoinsCount()
    {
        _coins++;
    }

    public void ApplyDamage(int damage)
    {
        //Debug.Log("damage");

        if(damage >= _health)
        {
            _health = 0;
            SetDeath();
        }
        else
        {
            _health -= damage;
        }
        HealthChanged?.Invoke(_health);           
    }

    public void IncreaseMana(int mana)
    {   
        if(_maxMana < (_mana + mana))
        {
            _mana = _maxMana;   
        }
        else
        {
            _mana += mana;
        }
        ManaChanged?.Invoke(_mana);
    }

    public void Restart()
    {
        _health = _maxHealth;
        HealthChanged?.Invoke(_health);

        _mana = 0;
        ManaChanged?.Invoke(_mana);

        _animationController.PlayRestartAnimation();
        _runnerMovement.RestartPosition();

        SetActionControllersOff();
        SetRunnerControllersOn();
    }

    public void SetToMainMenu()
    {
        SetRunnerControllersOff();
        SetActionControllersOff();
        _animationController.MoveToMenuPosition();
        _health = _maxHealth;
        _mana = 0;
        HealthChanged?.Invoke(_health);
        ManaChanged?.Invoke(_mana);
    }

    public void SetToActionPhaze(List<GameObject> enemyList)
    {
        _targets = enemyList;
        
        SetRunnerControllersOff();
        SetActionControllersOn();
        SetTarget();
    }

    public void SetNewTarget()
    {   
        if(_currentTargetIndex == _targets.Count - 1)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].GetComponent<Enemy>().IsAlive  && _targets[i] != null)
                {
                    _currentTargetIndex = i;
                    SetTarget();
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < _targets.Count; i++)
        {
            if(i> _currentTargetIndex && _targets[i].GetComponent<Enemy>().IsAlive)
            {
                _currentTargetIndex = i;
                break;
            }
        }       
        SetTarget();
    }

    private void SetDeath()
    {
        if (_actionAnimationController.enabled)
        {
            DeadOnActionPhaze.Invoke();
            _actionAnimationController.PlayDeathAnimation();
            _actionAnimationController.SetTarget(null);
            _attackHandler.SetTarget(null);
        }
        else
        {
            DeadOnRunnerPhaze.Invoke();
            SetRunnerControllersOff();            
            _animationController.PlayDeathAnimation();
        }
    }

    private void SetTarget()
    {
        _camera.SetTarget(_targets[_currentTargetIndex].transform);
        _actionAnimationController.SetTarget(_targets[_currentTargetIndex].transform);
        _attackHandler.SetTarget(_targets[_currentTargetIndex].GetComponent<Enemy>());
    }

    public Enemy GetTarget(List<Enemy> targetList)
    {   
        Enemy target = null;

        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].IsAlive)
            {
                target =  targetList[i];
            }
        }

        return target;
    }   

    public void StartRun()
    {
        _animationController.StartRunPhazeAnimation();
    }

    private void OnMovedToRunPosition()
    {
        SetRunnerControllersOn();
    }

    private void SetRunnerControllersOn()
    {
        _swipeListener.enabled = true;
        _runnerMovement.enabled = true;
    }

    private void SetRunnerControllersOff()
    {
        _swipeListener.enabled = false;
        _runnerMovement.enabled = false;
    }

    private void SetActionControllersOn()
    {
        _animationController.SetActionAnimation();
        _actionMovement.enabled = true;      
        _attackHandler.enabled = true;
    }

    private void SetActionControllersOff()
    {
        _actionAnimationController.enabled = false;
        _actionMovement.enabled = false;
        _attackHandler.enabled = false;
    }
}
