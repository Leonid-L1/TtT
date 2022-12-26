using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class InteractableObject : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _direction = new Vector3(0, 0, -1);
    private float _edgePosition = -50;
    private bool _isMoving = true;

    private void Update()
    {   
        if(_isMoving)
        transform.Translate(_direction * (_speed * Time.deltaTime));

        if (transform.position.z <= _edgePosition)        
            gameObject.SetActive(false);       
    } 

    public void SetMoveCondition(bool isMoveRequreid)
    {
        _isMoving = isMoveRequreid;
    }
}
