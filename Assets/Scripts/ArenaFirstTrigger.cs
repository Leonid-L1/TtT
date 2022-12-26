using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaFirstTrigger : MonoBehaviour
{
    [SerializeField] private InteractableSpawner _spawner;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider playerCollider))
        {
            _spawner.SetSpawnCondition(false);
        }
    }
}
