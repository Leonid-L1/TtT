using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class RunnerMovement : MonoBehaviour
{
    private const string JumpAnimationName = "Jump"; 

    [SerializeField] private SwipeListener _swipeListener;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _jumpLength;

    private const float LaneOffset = 3;
    private float laneChangeSpeed = 15;
    private Vector3 _targetPosition;
    private bool _isAbleToMove = true;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _swipeListener.SwipeComplete += OnSwipeComplete;
    }

    private void OnDisable()
    {
        _swipeListener.SwipeComplete -= OnSwipeComplete;
    }

    private void Start()
    {
        _targetPosition = transform.position;        
    }
    private void Update()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, laneChangeSpeed * Time.deltaTime);
    }

    public void RestartPosition()
    {
        _targetPosition.x = 0;
    }

    private void OnSwipeComplete(Vector2 direction)
    {
        if (_isAbleToMove)
        {
            if (direction == Vector2.up)
            {
                Jump();
            }
            else if (direction.x > 0 && transform.position.x != LaneOffset)
            {
                _targetPosition.x = GetTargetPosition(direction).x;
            }
            else if (direction.x < 0 && transform.position.x != -LaneOffset)
            {
                _targetPosition.x = GetTargetPosition(direction).x;
            }
        }       
    }

    private Vector3 GetTargetPosition(Vector2 direction)
    {
        return new Vector3(_targetPosition.x + (LaneOffset * direction.x), 0, transform.position.z);
    }

    private void Jump()
    {
        StartCoroutine(JumpAnimate());
    }

    private IEnumerator JumpAnimate()
    {
        _isAbleToMove = false;
        float elapsedTime = 0;
        float progress = 0;
        Vector3 startPosition = transform.position;

        _animator.Play(JumpAnimationName);

        while (progress < 1)
        {   
            elapsedTime+=Time.deltaTime;
            progress = elapsedTime / _jumpDuration;

            transform.position = startPosition + new Vector3(0, _jumpCurve.Evaluate(progress) * _jumpLength, 0);
            yield return null;
        }
        _isAbleToMove = true;
        yield break;
    }
}
