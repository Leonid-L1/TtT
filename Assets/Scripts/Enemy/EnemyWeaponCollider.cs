using UnityEngine;
using UnityEngine.Events;

public class EnemyWeaponCollider : MonoBehaviour
{
    public UnityAction PlayerTouched;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCollider player))
        {
            PlayerTouched?.Invoke();
        }
    }
}
