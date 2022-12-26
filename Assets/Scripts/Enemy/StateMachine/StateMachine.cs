using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;

    private Player _target;
    private State _currentState;

    //public State CurrentState => _currentState;

    private void OnEnable()
    {
        Reset(_firstState);
    }

    private void OnDisable()
    {
        ExitState();
    }

    private void FixedUpdate()
    {
        if( _currentState == null)
        {
            return;
        }
        State nextState = _currentState.TryGetNextState();

        if (nextState != null)
            Transit(nextState);        
    }

    public void SetTarget(Player target)
    {   
        _target = target;
    }

    private void Reset(State startState)
    {
        _currentState = startState;

        if( _currentState != null)
        {
            _currentState.Enter(_target);
        }
    }

    private void ExitState()
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = null;
    }

    private void Transit(State nextState)
    {
        if( _currentState != null)
            _currentState.Exit();
        
       _currentState = nextState;

        if (_currentState != null)
            _currentState.Enter(_target);     
    }
}

