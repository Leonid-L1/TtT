using UnityEngine;

public class ToAttackTransition : Transition
{
    [SerializeField] private float _attackDistance = 1.5f;

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if(distance < _attackDistance)
        {
            IsReadyToTransit = true;
        }
    }
}