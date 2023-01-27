using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{
    [SerializeField] private MeshRenderer _renderer;

    private void OnEnable()
    {
        if (_renderer.enabled == false)
            _renderer.enabled = true;
    }

    protected override void InteractWithPlayer(PlayerCollider playerCollider)
    {
        playerCollider.gameObject.GetComponentInParent<PlayerCollectibles>().IncreaseCoinsCount();

        _renderer.enabled = false;
    }
}
