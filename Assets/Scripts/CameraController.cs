using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class CameraController : MonoBehaviour
{
    private const string _toMenuAnimationName = "ToMenu";
    private const string _toRunPositionAnimationName = "ToRunPosition";

    [SerializeField] private Player _player;
    [SerializeField] private CameraHolder _cameraHolder;

    private Animator _animator;

    public event UnityAction MovedToMainMenu;
    public event UnityAction MovedToRunPosition;
    public event UnityAction MovedToActionPosition;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
        /////////////////
    public void InvokeToMenuMovedEvent()
    {
        MovedToMainMenu?.Invoke();
    }

    public void InvokeMovedToRunPositionEvent()
    {
        MovedToRunPosition?.Invoke();
    }

    public void MoveToMenuPosition()
    {
        Restart();
        _animator.Play(_toMenuAnimationName);
    }

    public void MoveToRunnerPosition()
    {
        _animator.Play(_toRunPositionAnimationName);
    }
    /// ////////////////////
    public void Restart()
    {   
        _animator.enabled = true;
        transform.SetParent(null, false);
        _cameraHolder.enabled = false;
        _cameraHolder.ClearTarget();
        _cameraHolder.transform.position = _cameraHolder.StartPosition;
        _cameraHolder.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetActionState(Transform enemy)
    {
        _animator.enabled = false;
        transform.SetParent(_cameraHolder.transform, true);
        _cameraHolder.SetTarget(enemy);
        _cameraHolder.CalculateFirstPosition();
        _cameraHolder.enabled = true;
        MovedToActionPosition?.Invoke();
    }
}
