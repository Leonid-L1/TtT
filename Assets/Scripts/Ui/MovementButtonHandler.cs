using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isHold = false;

    public bool IsHold => _isHold;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _isHold = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        _isHold = false;
    }

}
