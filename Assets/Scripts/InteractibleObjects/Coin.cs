using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{      
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider playerCollider))
        {
            collider.gameObject.GetComponentInParent<PlayerCollectibles>().IncreaseCoinsCount();

            gameObject.SetActive(false);
        }   
    }
}
