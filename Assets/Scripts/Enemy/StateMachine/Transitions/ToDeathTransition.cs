using UnityEngine;
[RequireComponent(typeof(Enemy))]

public class ToDeathTransition : Transition
{
    private Enemy _enemy;

    private void OnEnable()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if(_enemy.Health <= 0)
        {
            IsReadyToTransit = true;
        }
    }
}

