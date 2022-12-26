using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public State TargetState => _targetState;

    protected Player Target { get; private set; }

    public bool IsReadyToTransit { get; protected set; }

    private void OnEnable()
    {
        IsReadyToTransit = false;
    }

    public void SetTarget(Player player)
    {
        Target = player;
    }
}


