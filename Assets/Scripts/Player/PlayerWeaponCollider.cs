using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponCollider : MonoBehaviour
{
    public UnityAction<EnemyHealth> EnemyTouched;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out EnemyCollider enemy))
        {
            EnemyTouched?.Invoke(enemy.GetComponentInParent<EnemyHealth>());
        }
    }
}
