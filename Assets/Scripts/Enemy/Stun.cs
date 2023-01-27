using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stun : MonoBehaviour
{
    [SerializeField] private float stunDuration = 0.6f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private StateMachine _stateMachine;

    private const string StunTrigger = "Hit";
    private Coroutine _stun;

    public event UnityAction StunEnded;

    private void Start()
    {   
        _stateMachine = GetComponent<StateMachine>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (_animator.applyRootMotion == false)
            return;

        Vector3 deltaPoisition = _animator.deltaPosition;
        deltaPoisition.y = 0;
        Vector3 velocity = deltaPoisition / Time.deltaTime;
        _rigidbody.velocity = velocity;
    }

    public void SetStun()
    {
        if( _stun == null)
        {
            _stun = StartCoroutine(DoStun());
        }
        else
        {
            StopCoroutine(DoStun());
            _stun = StartCoroutine(DoStun());
        }
    }

    private IEnumerator DoStun()
    {
        _animator.SetTrigger(StunTrigger);
        _animator.applyRootMotion = true;
        _stateMachine.enabled = false;
        yield return new WaitForSeconds(stunDuration);

        _animator.applyRootMotion = false;
        StunEnded?.Invoke();
        _stateMachine.enabled = true;
        yield break;
    }
}
