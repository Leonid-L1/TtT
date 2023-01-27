using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionAnimationController))]
[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(RollHandler))]
[RequireComponent(typeof(SpellCast))]

public class PlayerTargetHandler : MonoBehaviour
{
    [SerializeField] CameraHolder _camera;

    private SpellCast _spell;
    private ActionAnimationController _actionAnimationController;
    private AttackHandler _attackHandler;
    private RollHandler _rollHandler;
    private List<Enemy> _targets;
    
    private int _currentTargetIndex = 0;

    private void Awake()
    {   
        _spell = GetComponent<SpellCast>();
        _rollHandler = GetComponent<RollHandler>();
        _actionAnimationController = GetComponent<ActionAnimationController>();
        _attackHandler = GetComponent<AttackHandler>();
    }

    private void OnDisable()
    {
        _targets = null;
        _currentTargetIndex = 0;
    }

    public void SetTargetsList(List<Enemy> enemyList)
    {
        _targets = enemyList;

        foreach(var target in _targets)
        {
            target.EnemyDied += OnEnemyDied;
        }
        _targets[_currentTargetIndex].SetAsActiveTarget(true);
    }

    public void SetNewTarget()
    {
        if (_targets.Count <= 0)
            return;

        if(_currentTargetIndex >= _targets.Count)
        {
            _currentTargetIndex = 0;
            _targets[_currentTargetIndex].SetAsActiveTarget(true);
            return;
        }
        
        _targets[_currentTargetIndex].SetAsActiveTarget(false); //мне нужно что бы после смерти его выключало?
        _currentTargetIndex++;

        if (_currentTargetIndex == _targets.Count)
            _currentTargetIndex = 0;

        _targets[_currentTargetIndex].SetAsActiveTarget(true);
    }

    public void SetTarget(Enemy enemy)
    {       
        _camera.SetTarget(enemy.transform);
        _actionAnimationController.SetTarget(enemy.transform);
        _attackHandler.SetTarget(enemy.GetComponent<EnemyHealth>());
        _rollHandler.SetTarget(enemy.transform);
        _spell.SetTarget(enemy.transform);

        if(enemy != _targets[_currentTargetIndex])
        {
            _targets[_currentTargetIndex].SetAsActiveTarget(false);

            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == enemy)
                {
                    _currentTargetIndex = i;
                }
            }
        }   
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _targets.Remove(enemy);

        if (enemy.IsActiveTarget)
            SetNewTarget();

        enemy.EnemyDied -= OnEnemyDied;
    }
}
    
