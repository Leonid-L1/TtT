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
    private List<GameObject> _targets;
    private int _currentTargetIndex = 0;

    private void Awake()
    {   
        _spell = GetComponent<SpellCast>();
        _rollHandler = GetComponent<RollHandler>();
        _actionAnimationController = GetComponent<ActionAnimationController>();
        _attackHandler = GetComponent<AttackHandler>();
    }

    private void OnEnable()
    {
        _attackHandler.TargetDied += SetNewTarget;
    }

    private void OnDisable()
    {
        _attackHandler.TargetDied -= SetNewTarget;
        _targets = null;
        _currentTargetIndex = 0;
    }

    public void SetTargetsList(List<GameObject> enemyList)
    {
        _targets = enemyList;
        SetTarget();
    }

    public void SetNewTarget()
    {
        if (_currentTargetIndex == _targets.Count - 1)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].GetComponent<Enemy>().IsAlive && _targets[i] != null)
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
            if (i > _currentTargetIndex && _targets[i].GetComponent<Enemy>().IsAlive)
            {
                _currentTargetIndex = i;
                break;
            }
        }
        SetTarget();
    }

    private void SetTarget()
    {
        _camera.SetTarget(_targets[_currentTargetIndex].transform);
        _actionAnimationController.SetTarget(_targets[_currentTargetIndex].transform);
        _attackHandler.SetTarget(_targets[_currentTargetIndex].GetComponent<Enemy>());
        _rollHandler.SetTarget(_targets[_currentTargetIndex].transform);
        _spell.SetTarget(_targets[_currentTargetIndex].transform);
    }
}
    
