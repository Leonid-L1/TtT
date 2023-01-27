using UnityEngine;

public class ArenaFirstTrigger : MonoBehaviour
{
    [SerializeField] private InteractableSpawner _spawner;
    [SerializeField] private BackgroundMusicHandler _musicHandler;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider playerCollider))
        {
            _spawner.SetSpawnCondition(false);
            _musicHandler.SetToActionPhaze();
        }
    }
}
