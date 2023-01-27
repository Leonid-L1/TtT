using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    [SerializeField] private float stunDuration = 0.6f;

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private const string StunTrigger = "Hit";
    private Coroutine _stunCoroutine;

    private bool _isStunned;

    public bool IsStunned => _isStunned;

    private void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        //_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetStun();
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

    private void SetStun()
    {
        if (_stunCoroutine == null)
        {
            _stunCoroutine = StartCoroutine(DoStun());
        }
        else
        {
            StopCoroutine(DoStun());
            _stunCoroutine = StartCoroutine(DoStun());
        }
    }

    private IEnumerator DoStun()
    {
        _animator.SetTrigger(StunTrigger);
        _isStunned = true;
        _animator.applyRootMotion = true;
        yield return new WaitForSeconds(stunDuration);

        _animator.applyRootMotion = false;
        _isStunned = false;
        yield break;
    }

}
