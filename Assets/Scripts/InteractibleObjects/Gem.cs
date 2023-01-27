using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : InteractableObject
{
    [SerializeField] private MeshRenderer _renderer;

    private int _manaCount = 20;

    private void OnEnable()
    {
        if (_renderer.enabled == false)
            _renderer.enabled = true;
    }

    protected override void InteractWithPlayer(PlayerCollider playerCollider)
    {
        playerCollider.gameObject.GetComponentInParent<PlayerCollectibles>().IncreaseMana(_manaCount);

        _renderer.enabled = false;
    }
}
