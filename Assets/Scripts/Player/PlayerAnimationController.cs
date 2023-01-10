using System.Collections;
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

    private ActionAnimationController _actionAnimationController;
    private Animator _animator;

    private Vector3 _runnerPosition = new Vector3(0, 0, -26);
    private Vector3 _menuPosition = new Vector3(0, 0, -33.5f);
    private Quaternion _toMenuRotation = Quaternion.Euler(new Vector3(0,90,0));
    private float _toRunnerPositionDuration = 1f;
    private float _delayBeforeStartRun = 0.25f;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _actionAnimationController = GetComponent<ActionAnimationController>();
    }

    public void StartRunPhazeAnimation()
    {   
        StartCoroutine(DoMoveToRunPosition());
    }

    public void MoveToMenuPosition()
    {
        _animator.Play(SitAnimation);
        transform.rotation = _toMenuRotation;
        transform.position = _menuPosition;
    }

    public void Restart()
    {
        StartCoroutine(DoMoveToRunPosition());
    }

    public void SetActionAnimation()
    {   
        if(!_actionAnimationController.enabled)
            _actionAnimationController.enabled = true;

        _animator.SetTrigger(ToActionTrigger);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger(RunnerDeathTrigger);
    }

    private IEnumerator DoMoveToRunPosition()
    {
        transform.position = _menuPosition;
        _animator.Play(RunAnimation);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        yield return new WaitForSeconds(_delayBeforeStartRun);      

        while (true)
        {
            transform.DOMove(_runnerPosition, _toRunnerPositionDuration);
            yield return new WaitForSeconds(_toRunnerPositionDuration);
            MovedToRunPosition?.Invoke();
            yield break;
        }
    }
}
