using UnityEngine;
using UnityEngine.Events;

public class SwipeListener : MonoBehaviour
{
    private const float _swipeDistance = 200;
    private Vector2 _startTouch;
    private Vector2 _swipeDelta;
    private bool _isTouchMoving;

    private bool _isSwipeLeft;
    private bool _isSwipeRight;

    public event UnityAction<Vector2> SwipeComplete;
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTouch = GetTouchPosition();
            _isTouchMoving = true;
        }
        else if (Input.GetMouseButtonUp(0) && _isTouchMoving)
        {
            _isTouchMoving = false;
        }

        _swipeDelta = Vector2.zero;

        if (_isTouchMoving && Input.GetMouseButton(0))
        {
            _swipeDelta = GetTouchPosition() - _startTouch;
        }

        if(_swipeDelta.magnitude >= _swipeDistance)
        {   
            if(Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                _isSwipeLeft = _swipeDelta.x < 0;
                _isSwipeRight = _swipeDelta.x > 0;
                SendSwipe();
            }
            else
            {
                if(_swipeDelta.y > 0)
                {
                    SendSwipe();
                }
            }
        }
    }

    private Vector2 GetTouchPosition()
    {
        return (Vector2)Input.mousePosition;
    }

    private bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }

    private void SendSwipe()
    {
        if (_isSwipeLeft)
        {
            SwipeComplete?.Invoke(Vector2.left);
        }
        else if (_isSwipeRight)
        {
            SwipeComplete?.Invoke(Vector2.right);
        }
        else
        {
            SwipeComplete?.Invoke(Vector2.up);
        }
        Reset();
    }

    private void Reset()
    {
        _startTouch = _swipeDelta = Vector2.zero;
        _isTouchMoving = _isSwipeLeft = _isSwipeRight = false;
    }
}
