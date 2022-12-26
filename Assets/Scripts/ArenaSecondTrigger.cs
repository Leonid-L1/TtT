using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSecondTrigger : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();    
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider playerCollider))
        {           
            _gameController.SetActionPhaze();
            _collider.isTrigger = false;
        }
    }

    public void ResetCondition()
    {
        _collider.isTrigger = true;
    }
}
