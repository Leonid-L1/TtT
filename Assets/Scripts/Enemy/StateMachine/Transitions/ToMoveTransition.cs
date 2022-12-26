using UnityEngine;

public class ToMoveTransition : Transition
{
    [SerializeField] private float _requiredDistance = 1.5f;
    [SerializeField] private AttackState _attackState;
    
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if(distance > _requiredDistance && _attackState.IsAttacking == false)
        {            
            IsReadyToTransit = true;
        }
    }
}