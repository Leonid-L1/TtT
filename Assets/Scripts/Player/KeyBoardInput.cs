using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ActionMovement))]

public class KeyBoardInput : MonoBehaviour
{
    private ActionMovement _movement;

    private float _horizontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _movement = GetComponent<ActionMovement>();   
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _verticalInput = Vector3.up.y;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _verticalInput = Vector3.down.y;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _horizontalInput = Vector3.left.x;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _horizontalInput = Vector3.right.x;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _horizontalInput = 0;
            _verticalInput = 0;
        }
    }
}
