using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : InteractableObject
{
    private int _manaCount = 20;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider playerCollider))
        {
            collider.gameObject.GetComponentInParent<PlayerCollectibles>().IncreaseMana(_manaCount);

            gameObject.SetActive(false);
        }
    }
}
