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
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerCollectibles))]
[RequireComponent(typeof(PlayerTargetHandler))]
[RequireComponent(typeof(RollHandler))]
[RequireComponent(typeof(SpellCast))]

public class Player : MonoBehaviour
{
    [SerializeField] GameController _gameController;
    [SerializeField] CameraHolder _camera;

    public event UnityAction DeadOnRunnerPhaze;
    public event UnityAction DeadOnActionPhaze;

    private ActionAnimationController _actionAnimationController;  
    private PlayerAnimationController _animationController;
    private PlayerTargetHandler _targetHandler;
    private PlayerCollectibles _collectibles;
    private RunnerMovement _runnerMovement;
    private ActionMovement _actionMovement;
    private SwipeListener _swipeListener;
    private AttackHandler _attackHandler;
    private PlayerHealth _healthHandler;
    private RollHandler _rollHandler;
    private SpellCast _spellCast;

    private void OnEnable()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        _animationController.MovedToRunPosition += SetRunnerControllersOn;
    }

    private void OnDisable()
    {
        _animationController.MovedToRunPosition -= SetRunnerControllersOn;
    }

    private void Start()
    {   
        _rollHandler = GetComponent<RollHandler>();
        _spellCast = GetComponent<SpellCast>();
        _actionAnimationController = GetComponent<ActionAnimationController>();
        _targetHandler = GetComponent<PlayerTargetHandler>();
        _collectibles = GetComponent<PlayerCollectibles>();
        _runnerMovement = GetComponent<RunnerMovement>();
        _actionMovement = GetComponent<ActionMovement>();
        _healthHandler = GetComponent<PlayerHealth>();
        _swipeListener = GetComponent<SwipeListener>();
        _attackHandler = GetComponent<AttackHandler>();
    }

    public void Restart()
    {
        SetRunnerControllersOff();
        SetActionControllersOff();

        _animationController.Restart();
        _healthHandler.ResetHealth();
        _collectibles.ResetMana();
    }

    public void SetToMainMenu()
    {
        _animationController.MoveToMenuPosition();
        _healthHandler.ResetHealth();
        SetRunnerControllersOff();
        SetActionControllersOff();
        _collectibles.ResetMana();
    }

    public void SetToActionPhaze(List<GameObject> enemyList)
    {
        GetComponentInChildren<CapsuleCollider>().isTrigger = false;
        _targetHandler.SetTargetsList(enemyList);          
        SetRunnerControllersOff();
        SetActionControllersOn();
    } 

    public void SetDeath()
    {
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;

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

    public void StartRun()
    {
        _animationController.StartRunPhazeAnimation();
    }

    private void SetRunnerControllersOn()
    {
        if (!_swipeListener.enabled)
            _swipeListener.enabled = true;

        if (!_runnerMovement.enabled)
            _runnerMovement.enabled = true;
    }

    private void SetRunnerControllersOff()
    {
        if(_swipeListener.enabled)
            _swipeListener.enabled = false;

        if(_runnerMovement.enabled)
            _runnerMovement.enabled = false;
    }

    private void SetActionControllersOn()
    {   
        _animationController.SetActionAnimation();

        if (!_targetHandler.enabled)
            _targetHandler.enabled = true;

        if (!_actionMovement.enabled)
            _actionMovement.enabled = true;    
        
        if(!_attackHandler.enabled)
            _attackHandler.enabled = true;

        if(!_rollHandler.enabled)
            _rollHandler.enabled = true;

        if (!_spellCast.enabled)
            _spellCast.enabled = true;
        
    }

    private void SetActionControllersOff()
    {   
        if( _actionAnimationController.enabled)
            _actionAnimationController.enabled = false;

        if( _actionMovement.enabled)
            _actionMovement.enabled = false;

        if( _attackHandler.enabled)
            _attackHandler.enabled = false;

        if (_rollHandler.enabled)
            _rollHandler.enabled = false;

        if (_spellCast.enabled)
            _spellCast.enabled = false;

        if (_targetHandler.enabled)
            _targetHandler.enabled = false;
    }
}
