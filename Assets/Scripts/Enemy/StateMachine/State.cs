using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    protected Player Target { get; set; }

    public void Enter(Player target)
    {
        if(!enabled)
        {
            Target = target;
            enabled = true;
            
            foreach(var transition in _transitions)
            {
                transition.enabled = true;
                transition.SetTarget(Target);
            }
        }
    }

    public void Exit()
    {
        if (enabled)
        {
            foreach(var transition in _transitions)
            {
                transition.enabled = false;
                enabled = false;
            }
        }
    }

    public State TryGetNextState()
    {
        foreach(var transition in _transitions)
        {
            if (transition.IsReadyToTransit)
                return transition.TargetState;
        }
        return null;
    }
}

