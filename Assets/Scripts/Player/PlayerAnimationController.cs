using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(ActionAnimationController))]

public class PlayerAnimationController : MonoBehaviour
{
    private const string RunAnimation = "Run";
    private const string SitAnimation = "Sit";
    private const string ToActionTrigger = "ToAction";
    private const string RunnerDeathTrigger = "RunnerDeath";

    [SerializeField] private Transform _body;

    public UnityAction MovedToRunPosition;
    public UnityAction MovedToMenuPosition;

    private ActionAnimationController _acionAnimationController;
    private Animator _animator;

    private Vector3 _runnerPosition = new Vector3(0, 0, -26);
    private Vector3 _toMenuPosition = new Vector3(0, 0, -33.5f);
    private Quaternion _toMenuRotation = Quaternion.Euler(new Vector3(0,90,0));

    private float _toRunnerPositionDuration = 1f;
    private float _toMenuPositionDuration = 0.001f;

    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _acionAnimationController = GetComponent<ActionAnimationController>();
    }

    public void StartRunPhazeAnimation()
    {   
        StartCoroutine(DoMoveToRunPosition());
    }

    public void MoveToMenuPosition()
    {
        StartCoroutine(DoMoveToMenuPosition());
    }

    public void PlayRestartAnimation()
    {
        StartCoroutine(DoRestartAnimation());
    }

    public void SetActionAnimation()
    {
        _acionAnimationController.enabled = true;
        _animator.SetTrigger(ToActionTrigger);
        GetComponentInChildren<CapsuleCollider>().isTrigger = false;
    }

    public void PlayDeathAnimation()
    {
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;
        _animator.SetTrigger(RunnerDeathTrigger);
    }

    private IEnumerator DoMoveToRunPosition()
    {
        _animator.Play(RunAnimation);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        while (true)
        {
            transform.DOMove(_runnerPosition, _toRunnerPositionDuration);
            yield return new WaitForSeconds(_toRunnerPositionDuration);
            MovedToRunPosition?.Invoke();
            yield break;
        }
    }

    private IEnumerator DoMoveToMenuPosition()
    {
        _animator.Play(SitAnimation);
        transform.rotation = _toMenuRotation;

        while (true)
        {
            transform.DOMove(_toMenuPosition, _toMenuPositionDuration);
            yield return new WaitForSeconds(_toMenuPositionDuration);
            MovedToMenuPosition?.Invoke();
            yield break;
        }
    }

    private IEnumerator DoRestartAnimation()
    {
        while (true)
        {
            transform.DOMove(_toMenuPosition, _toMenuPositionDuration);
            yield return new WaitForSeconds(_toMenuPositionDuration);

            transform.DOMove(_runnerPosition, _toRunnerPositionDuration);
            _animator.Play(RunAnimation);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            yield break;
        }
    }
}
